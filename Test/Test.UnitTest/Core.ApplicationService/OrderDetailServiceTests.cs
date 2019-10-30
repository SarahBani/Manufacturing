using Core.ApplicationService.Implementation;
using Core.DomainModel.Entities;
using Core.DomainService.Repository;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Test.Common.Models;

namespace Test.UnitTest.Core.ApplicationService
{
    [TestFixture]
    public class OrderDetailServiceTests : BaseServiceTests<IOrderDetailRepository, OrderDetail, long>
    {

        #region Properties

        protected new OrderDetailService Service
        {
            get => base.Service as OrderDetailService;
        }

        protected override OrderDetail Entity
        {
            get => new OrderDetailModel().Entity;
        }

        protected override IList<OrderDetail> EntityList
        {
            get => new OrderDetailModel().EntityList;
        }

        #endregion /Properties

        #region Constructors

        public OrderDetailServiceTests()
            : base()
        {
        }

        #endregion /Constructors

        #region Methods

        [OneTimeSetUp]
        public override void Setup()
        {
            base.SetService<OrderDetailService>();
        }

        public void GetListByBankId_ReturnsOK()
        {
            // Arrange
            var entityList = this.EntityList;
            int orderID = 5;
            var expression = It.IsAny<Expression<Func<OrderDetail, bool>>>();
            base.RepositoryMock.Setup(q => q.GetEnumerable(expression)).Returns(entityList);

            //Act
            var result = this.Service.GetListByOrderID(orderID);

            // Assert
            base.RepositoryMock.Verify(q => q.GetEnumerable(expression), "error in calling the correct method");  // Verifies that Repository.GetEnumerableAsync was called
            Assert.AreEqual(entityList, result, "error in returning correct entities");
        }

        public void GetListByBankId_BankIdIsInvalid_ReturnsNoItem()
        {
            // Arrange
            var entityList = new List<OrderDetail>();
            int bankId = -1;
            var expression = It.IsAny<Expression<Func<OrderDetail, bool>>>();
            base.RepositoryMock.Setup(q => q.GetEnumerable(expression)).Returns(entityList);

            //Act
            var result = this.Service.GetListByOrderID(bankId);

            // Assert
            base.RepositoryMock.Verify(q => q.GetEnumerable(expression), "error in calling the correct method");  // Verifies that Repository.GetEnumerableAsync was called
            Assert.AreEqual(entityList, result, "error in returning correct entities");
        }

        #endregion /Methods

    }
}
