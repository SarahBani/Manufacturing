using Core.DomainModel.Entities;
using Core.DomainService.Repository;

namespace Infrastructure.DataBase.Repository
{
    public class OrderRepository : BaseRepository<Order, long>, IOrderRepository
    {

        #region Constructors

        public OrderRepository(ManufacturingDbContext dbContext)
            : base(dbContext)
        {

        }

        #endregion /Constructors

    }
}
