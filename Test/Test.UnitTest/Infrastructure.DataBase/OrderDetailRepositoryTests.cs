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
    public class OrderDetailRepositoryTests : BaseRepositoryTests<OrderDetail, long>
    {

        #region Properties

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

        public OrderDetailRepositoryTests()
            : base()
        {
        }

        #endregion /Constructors

        #region Methods

        [OneTimeSetUp]
        public override void Setup()
        {
            base.SetRepository<OrderDetailRepository>();
        }

        protected override Expression<Func<OrderDetail, bool>> GetKeyFilter(long id)
        {
            return q => q.OrderDetailID.Equals(id);
        }

        #endregion /Methods

    }
}
