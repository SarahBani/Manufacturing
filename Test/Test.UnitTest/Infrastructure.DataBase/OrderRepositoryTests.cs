using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Core.DomainModel.Entities;
using Infrastructure.DataBase.Repository;
using NUnit.Framework;
using Test.Common.Models;

namespace Test.UnitTest.Infrastructure.DataBase
{
    [TestFixture]
    public class OrderRepositoryTests : BaseRepositoryTests<Order, long>
    {

        #region Properties

        protected override Order Entity
        {
            get => new OrderModel().Entity;
        }

        protected override IList<Order> EntityList
        {
            get => new OrderModel().EntityList;
        }


        #endregion /Properties

        #region Constructors

        public OrderRepositoryTests()
            : base()
        {
        }

        #endregion /Constructors

        #region Methods

        [OneTimeSetUp]
        public override void Setup()
        {
            base.SetRepository<OrderRepository>();
        }

        protected override Expression<Func<Order, bool>> GetKeyFilter(long id)
        {
            return q => q.OrderID.Equals(id);
        }

        #endregion /Methods

    }
}
