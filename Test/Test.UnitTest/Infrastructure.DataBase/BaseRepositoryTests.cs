using Core.DomainModel.Entities;
using Infrastructure.DataBase.Repository;
using NUnit.Framework;
using System.Threading.Tasks;

namespace Test.UnitTest.Infrastructure.DataBase
{
    public abstract class BaseRepositoryTests<TEntity, TKey> : BaseReadOnlyRepositoryTests<TEntity, TKey>
        where TEntity : BaseEntity
    {

        #region Properties

        protected new BaseRepository<TEntity, TKey> Repository
        {
            get => base.Repository as BaseRepository<TEntity, TKey>;
        }

        #endregion /Properties

        #region Consructors

        public BaseRepositoryTests()
        {
        }

        #endregion /Consructors

        #region Methods  

        [Test]
        public async Task InsertAsync_ReturnsOK()
        {
            // Arrange
            var entity = this.Entity;
            base.DataBaseContextMock.Setup(q => q.AddAsync(entity, default)).Verifiable();

            //Act
            await this.Repository.InsertAsync(entity);

            // Assert
            this.DataBaseContextMock.Verify(q => q.AddAsync(entity, default), "error in calling the correct method");
        }

        #endregion /Methods

    }
}
