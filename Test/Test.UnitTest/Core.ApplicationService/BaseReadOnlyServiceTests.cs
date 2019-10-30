using Core.ApplicationService;
using Core.ApplicationService.Implementation;
using Core.DomainModel.Entities;
using Core.DomainService.Repository;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Test.Common;

namespace Test.UnitTest.Core.ApplicationService
{
    public abstract class BaseReadOnlyServiceTests<TRepository, TEntity, TKey>
        where TRepository : class, IBaseReadOnlyRepository<TEntity, TKey>
        where TEntity : BaseEntity
    {

        #region Properties

        protected Mock<IEntityService> EntityServiceMock { get; private set; }

        protected Mock<TRepository> RepositoryMock { get; private set; }

        protected BaseReadOnlyService<TRepository, TEntity, TKey> Service { get; set; }

        protected abstract TEntity Entity { get; }

        protected abstract IList<TEntity> EntityList { get; }

        #endregion /Properties

        #region Consructors

        public BaseReadOnlyServiceTests()
        {
            this.RepositoryMock = new Mock<TRepository>();
            SetEntityServiceMock();
        }

        #endregion /Consructors

        #region Methods    

        private void SetEntityServiceMock()
        {
            this.EntityServiceMock = new Mock<IEntityService>();
            this.EntityServiceMock.Setup(q => q.GetRepository<TEntity, TKey>()).Returns(this.RepositoryMock.Object);
        }

        [OneTimeSetUp]
        public abstract void Setup();

        protected void SetService<T>() where T : BaseReadOnlyService<TRepository, TEntity, TKey>
        {
            this.Service = (T)Activator.CreateInstance(typeof(T), this.EntityServiceMock.Object);
        }

        #region GetByIdAsync

        [Test]
        public async Task GetByIdAsync_ReturnsOK()
        {
            // Arrange
            TKey id = TestHelper.GetId<TKey>(5);
            var entity = this.Entity;
            this.RepositoryMock.Setup(q => q.GetByIdAsync(id, default)).ReturnsAsync(entity);

            //Act
            var result = await this.Service.GetByIdAsync(id);

            // Assert
            this.RepositoryMock.Verify(q => q.GetByIdAsync(id, default),
                "error in calling the correct method");  // Verifies that Repository.GetByIdAsync was called
            Assert.AreEqual(entity, result, "error in returning correct entity");
        }

        [Test]
        public async Task GetByIdAsync_IdIs0_ReturnsNull()
        {
            // Arrange
            TKey id = TestHelper.GetId<TKey>(0);
            TEntity entity = null;
            this.RepositoryMock.Setup(q => q.GetByIdAsync(id, default)).ReturnsAsync(entity);

            //Act
            var result = await this.Service.GetByIdAsync(id);

            // Assert
            this.RepositoryMock.Verify(q => q.GetByIdAsync(id, default),
                "error in calling the correct method");  // Verifies that Repository.GetByIdAsync was called
            Assert.IsNull(result, "error in returning null entity");
        }

        #endregion /GetByIdAsync

        #region GetAll

        [Test]
        public void GetAll_ReturnsOK()
        {
            // Arrange
            var entityList = this.EntityList;
            this.RepositoryMock.Setup(q => q.GetQueryable()).Returns(entityList.AsQueryable());

            //Act
            var result = this.Service.GetAll();

            // Assert            
            this.RepositoryMock.Verify(q => q.GetQueryable(),
                "error in calling the correct method");  // Verifies that Repository.GetQueryableAsync was called
            Assert.AreEqual(entityList, result, "error in returning correct entities");
        }

        #endregion /GetAll

        #endregion /Methods

    }
}
