using Core.DomainModel;
using Core.DomainModel.Entities;
using Core.DomainService;
using Core.DomainService.Repository;
using System;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace Core.ApplicationService.Implementation
{
    public abstract class BaseService<TRepository, TEntity, TKey> : BaseReadOnlyService<TRepository, TEntity, TKey>
        where TRepository : IBaseRepository<TEntity, TKey>
        where TEntity : BaseEntity
    {

        #region Properties

        protected string MethodName
        {
            get
            {
                StackFrame frame = new StackTrace().GetFrame(1);
                return frame.GetMethod().Name;
            }
        }

        #endregion /Properties

        #region Constructors

        protected BaseService(IEntityService entityService)
            : base(entityService)
        {
        }

        #endregion /Constructors

        #region Methods

        [MethodImpl(MethodImplOptions.NoInlining)]
        protected async Task<TransactionResult> GetTransactionResultAsync(Action action)
        {
            BeginTransaction();
            action();
            return await CommitTransactionAsync();
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        protected void BeginTransaction()
        {
            if (!base.EntityService.UnitOfWork.HasTransaction())
            {
                base.EntityService.UnitOfWork.BeginTransaction(GetCallerMethod());
            }
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        protected async Task<TransactionResult> CommitTransactionAsync(object content = null)
        {
            if (base.EntityService.UnitOfWork.GetTransactionName().Equals(GetCallerMethod()))
            {
                await base.EntityService.UnitOfWork.Commit();
            }
            return new TransactionResult(content);
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private string GetCallerMethod()
        {
            int intSkipFrames = 0;
            while (new StackFrame(intSkipFrames).GetMethod().GetMethodImplementationFlags() == MethodImplAttributes.NoInlining ||
                   new StackFrame(intSkipFrames).GetMethod().GetRealMethod() == null ||
                   !new StackFrame(intSkipFrames).GetMethod().GetBaseDeclaringType().IsAssignedGenericType(typeof(BaseService<,,>))) // This line is added for preventing mistakes in tests
            {
                intSkipFrames++;
            }
            var method = new StackTrace(new StackFrame(intSkipFrames)).GetFrame(0).GetMethod().GetRealMethod();
            return method.ReflectedType.Name + "." + method.Name;
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        protected TransactionResult GetTransactionException(Exception exception)
        {
            base.EntityService.UnitOfWork.RollBack();
            if (exception is CustomException)
            {
                return new TransactionResult(exception as CustomException);
            }
            else
            {
                return new TransactionResult(new CustomException(exception));
            }
        }

        public virtual async Task<TransactionResult> InsertAsync(TEntity entity)
        {
            try
            {
                return await GetTransactionResultAsync(() => base.Repository.InsertAsync(entity));
            }
            catch (Exception ex)
            {
                return GetTransactionException(ex);
            }
        }

        #endregion /Methods

    }
}