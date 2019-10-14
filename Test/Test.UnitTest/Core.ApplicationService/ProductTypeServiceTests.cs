using Core.ApplicationService.Implementation;
using Core.DomainModel.Entities;
using Core.DomainService.Repository;
using NUnit.Framework;
using System.Collections.Generic;
using Test.Common.Models;

namespace Test.UnitTest.Core.ApplicationService
{
    [TestFixture]
    public class ProductTypeServiceTests : BaseReadOnlyServiceTests<IProductTypeRepository, ProductType, byte>
    {

        #region Properties

        protected new ProductTypeService Service
        {
            get => base.Service as ProductTypeService;
        }

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

        public ProductTypeServiceTests()
            : base()
        {
        }

        #endregion /Constructors

        #region Methods

        [OneTimeSetUp]
        public override void Setup()
        {
            base.SetService<ProductTypeService>();
        }       

        #endregion /Methods

    }
}
