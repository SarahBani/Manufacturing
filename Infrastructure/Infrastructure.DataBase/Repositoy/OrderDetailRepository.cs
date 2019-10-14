using Core.DomainModel.Entities;
using Core.DomainService.Repository;

namespace Infrastructure.DataBase.Repository
{
    public class OrderDetailRepository : BaseRepository<OrderDetail, long>, IOrderDetailRepository
    {

        #region Constructors

        public OrderDetailRepository(ManufacturingDbContext dbContext)
            : base(dbContext)
        {

        }

        #endregion /Constructors

    }
}
