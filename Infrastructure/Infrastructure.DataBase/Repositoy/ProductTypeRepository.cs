using Core.DomainModel.Entities;
using Core.DomainService.Repository;

namespace Infrastructure.DataBase.Repository
{
    public class ProductTypeRepository : BaseRepository<ProductType, byte>, IProductTypeRepository
    {

        #region Constructors

        public ProductTypeRepository(ManufacturingDbContext dbContext)
            : base(dbContext)
        {
        }

        #endregion /Constructors

    }
}
