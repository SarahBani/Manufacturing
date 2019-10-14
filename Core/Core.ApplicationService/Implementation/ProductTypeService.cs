using Core.ApplicationService.Contracts;
using Core.DomainModel.Entities;
using Core.DomainService.Repository;

namespace Core.ApplicationService.Implementation
{
    public class ProductTypeService : BaseReadOnlyService<IProductTypeRepository, ProductType, byte>, IProductTypeService
    {

        #region Constructors

        public ProductTypeService(IEntityService entityService)
            : base(entityService)
        {
        }

        #endregion /Constructors

    }
}
