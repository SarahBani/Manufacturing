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
    public class ProductTypeRepositoryTests : BaseReadOnlyRepositoryTests<ProductType, byte>
    {

        #region Properties

        protected override ProductType Entity
        {
            get => new ProductTypeModel().Entity;
        }

        protected override IList<ProductType> EntityList
        {
            get => new ProductTypeModel().EntityList;
        }


        #endregion /Properties

        #region Constructors

        public ProductTypeRepositoryTests()
            : base()
        {
        }

        #endregion /Constructors

        #region Methods

        [OneTimeSetUp]
        public override void Setup()
        {
            base.SetRepository<ProductTypeRepository>();
        }

        protected override Expression<Func<ProductType, bool>> GetKeyFilter(byte id)
        {
            return q => q.ProductTypeID.Equals(id);
        }

        #endregion /Methods

    }
}
