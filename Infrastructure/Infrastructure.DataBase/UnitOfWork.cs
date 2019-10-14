using Core.DomainModel;
using Core.DomainModel.Entities;
using Core.DomainService;
using System;
using System.Threading.Tasks;

namespace Infrastructure.DataBase
{
    public class UnitOfWork : IUnitOfWork
    {

        #region Properties

        private string _transactionName;

        private ManufacturingDbContext _dbContext;

        #endregion /Properties

        #region Constructors

        public UnitOfWork(ManufacturingDbContext dbContext)
        {
            this._dbContext = dbContext;
        }

        #endregion /Constructors

        #region Destructors

        ~UnitOfWork()
        {
            Dispose();
        }

        #endregion /Destructors

        #region Methods

        public string GetTransactionName()
        {
            return this._transactionName;
        }

        public bool HasTransaction()
        {
            return !string.IsNullOrEmpty(this._transactionName);
        }

        public void BeginTransaction(string transactionName)
        {
            if (string.IsNullOrEmpty(this._transactionName))
            {
                this._transactionName = transactionName;
            }
        }

        public async Task Commit()
        {
            if (string.IsNullOrEmpty(this._transactionName))
            {
                throw new CustomException(ExceptionKey.NoActiveTransaction);
            }
            await this._dbContext.SaveChangesAsync();
            this._transactionName = string.Empty;
        }

        public void RollBack()
        {
        }

        public void Dispose()
        {
            if (this._dbContext != null)
            {
                this._dbContext.Dispose();
            }
            GC.SuppressFinalize(this);
        }

        #endregion /Methods

    }
}
