using Core.DomainModel.Entities;
using Infrastructure.DataBase.Repository;
using Microsoft.EntityFrameworkCore;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Test.Common;

namespace Test.UnitTest.Infrastructure.DataBase
{
    public abstract class BaseReadOnlyRepositoryTests<TEntity, TKey>
        where TEntity : BaseEntity
    {

        #region Properties

        protected Mock<ManufacturingDbContext> DataBaseContextMock { get; private set; }

        protected BaseReadOnlyRepository<TEntity, TKey> Repository { get; private set; }

        protected abstract TEntity Entity { get; }

        protected abstract IList<TEntity> EntityList { get; }

        #endregion /Properties

        #region Consructors

        public BaseReadOnlyRepositoryTests()
        {
            SetDataBaseContextMock();
        }

        #endregion /Consructors

        #region Methods  

        [OneTimeSetUp]
        public abstract void Setup();

        private void SetDataBaseContextMock()
        {
            var options = new DbContextOptions<ManufacturingDbContext>();
            this.DataBaseContextMock = new Mock<ManufacturingDbContext>(options);
        }

        protected void SetRepository<T>() where T : BaseReadOnlyRepository<TEntity, TKey>
        {
            this.Repository = (T)Activator.CreateInstance(typeof(T), this.DataBaseContextMock.Object);
        }

        protected Mock<DbSet<TEntity>> GetDbSetMock()
        {
            var dbSetMock = new Mock<DbSet<TEntity>>();
            var entityList = this.EntityList.AsQueryable();
            dbSetMock.As<IQueryable<TEntity>>().Setup(q => q.Provider).Returns(entityList.Provider);
            dbSetMock.As<IQueryable<TEntity>>().Setup(q => q.Expression).Returns(entityList.Expression);
            dbSetMock.As<IQueryable<TEntity>>().Setup(q => q.ElementType).Returns(entityList.ElementType);
            dbSetMock.As<IQueryable<TEntity>>().Setup(q => q.GetEnumerator()).Returns(entityList.GetEnumerator());
            return dbSetMock;
        }

        protected abstract Expression<Func<TEntity, bool>> GetKeyFilter(TKey id);

        #region GetByIdAsync

        [Test]
        public async Task GetByIdAsync_ReturnsOK()
        {
            // Arrange
            TKey id = TestHelper.GetId<TKey>(5);
            var entity = this.Entity;
            this.DataBaseContextMock.Setup(q => q.Set<TEntity>().FindAsync(id)).ReturnsAsync(entity);

            //Act
            var result = await this.Repository.GetByIdAsync(id);

            // Assert
            this.DataBaseContextMock.Verify(q => q.Set<TEntity>().FindAsync(id),
                "error in calling the correct method"); // Verifies that DBContext.Set<TEntity>().FindAsync was called
            Assert.AreEqual(entity, result, "error in returning correct entity");
        }

        [Test]
        public async Task GetByIdAsync_IdIs0_ReturnsNull()
        {
            // Arrange
            TKey id = TestHelper.GetId<TKey>(0);
            TEntity entity = null;
            this.DataBaseContextMock.Setup(q => q.Set<TEntity>().FindAsync(id)).ReturnsAsync(entity);

            //Act
            var result = await this.Repository.GetByIdAsync(id);

            // Assert
            this.DataBaseContextMock.Verify(q => q.Set<TEntity>().FindAsync(id),
                "error in calling the correct method");  // Verifies that DBContext.Set<TEntity>().FindAsync was called
            Assert.IsNull(result, "error in returning null entity");
        }

        #endregion /GetByIdAsync

        #region GetSingle    

        [Test]
        public void GetSingle_ReturnsOK()
        {
            // Arrange
            TKey id = TestHelper.GetId<TKey>(3);
            var entity = this.EntityList.AsQueryable().Single(GetKeyFilter(id));
            var dbSetMock = GetDbSetMock();
            this.DataBaseContextMock.Setup(q => q.Set<TEntity>()).Returns(dbSetMock.Object);

            //Act
            var result = this.Repository.GetSingle(GetKeyFilter(id));

            // Assert
            this.DataBaseContextMock.Verify(q => q.Set<TEntity>(), "error in calling the correct method");
            TestHelper.AreEqualEntities(entity, result, "error in returning correct entity");
        }

        [Test]
        public void GetSingle_IdIsInvalid_ReturnsNull()
        {
            // Arrange
            TEntity entity = null;
            TKey id = TestHelper.GetId<TKey>(0);
            var dbSetMock = GetDbSetMock();
            this.DataBaseContextMock.Setup(q => q.Set<TEntity>()).Returns(dbSetMock.Object);

            //Act
            var result = this.Repository.GetSingle(GetKeyFilter(id));

            // Assert
            this.DataBaseContextMock.Verify(q => q.Set<TEntity>(), "error in calling the correct method");
            Assert.AreEqual(entity, result, "error in returning correct entity");
        }

        #endregion /GetSingle

        #region GetQueryable

        [Test]
        public void GetQueryableAsync_ReturnsOK()
        {
            // Arrange
            var entityList = this.EntityList.AsQueryable();
            var dbSetMock = GetDbSetMock();
            this.DataBaseContextMock.Setup(q => q.Set<TEntity>()).Returns(dbSetMock.Object);

            //Act
            var result = this.Repository.GetQueryable();

            // Assert            
            this.DataBaseContextMock.Verify(q => q.Set<TEntity>(), "error in calling the correct method");
            TestHelper.AreEqualEntities(entityList, result, "error in returning correct entities");
        }

        #endregion /GetQueryable

        #region GetEnumerable

        [Test]
        public void GetEnumerable_ReturnsOK()
        {
            // Arrange
            var entityList = this.EntityList.AsEnumerable();
            var dbSetMock = GetDbSetMock();
            this.DataBaseContextMock.Setup(q => q.Set<TEntity>()).Returns(dbSetMock.Object);
            Expression<Func<TEntity, bool>> filter = q => true;

            //Act
            var result = this.Repository.GetEnumerable(filter);

            // Assert            
            Assert.IsInstanceOf<IEnumerable<TEntity>>(result, "error in returning correct result type");
            this.DataBaseContextMock.Verify(q => q.Set<TEntity>(), "error in calling the correct method");
            TestHelper.AreEqualEntities(entityList, result, "error in returning correct entities");
        }

        [Test]
        public void GetEnumerable_FilterByKey_ReturnsOK()
        {
            // Arrange
            TKey id = TestHelper.GetId<TKey>(3);
            var entity = this.EntityList.AsQueryable().Single(GetKeyFilter(id));
            var dbSetMock = GetDbSetMock();
            this.DataBaseContextMock.Setup(q => q.Set<TEntity>()).Returns(dbSetMock.Object);

            //Act
            var result = this.Repository.GetEnumerable(GetKeyFilter(id));

            // Assert            
            Assert.IsInstanceOf<IEnumerable<TEntity>>(result, "error in returning correct result type");
            this.DataBaseContextMock.Verify(q => q.Set<TEntity>(), "error in calling the correct method");
            TestHelper.AreEqualEntities(entity, result.First(), "error in returning correct entities");
        }

        [Test]
        public void GetEnumerable_FilterByInvalidKey_ReturnsNull()
        {
            // Arrange
            TKey id = TestHelper.GetId<TKey>(0);
            var entity = this.EntityList.AsQueryable().Where(GetKeyFilter(id));
            var dbSetMock = GetDbSetMock();
            this.DataBaseContextMock.Setup(q => q.Set<TEntity>()).Returns(dbSetMock.Object);

            //Act
            var result = this.Repository.GetEnumerable(GetKeyFilter(id));

            // Assert            
            Assert.IsInstanceOf<IEnumerable<TEntity>>(result, "error in returning correct result type");
            this.DataBaseContextMock.Verify(q => q.Set<TEntity>(), "error in calling the correct method");
            TestHelper.AreEqualEntities(entity, result, "error in returning correct entities");
        }

        #endregion /GetEnumerable

        #endregion /Methods  

    }
}
