using Core.DomainModel.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Core.ApplicationService.Contracts
{
    public interface IProductTypeService 
    {

        Task<IList<ProductType>> GetAllAsync();

    }
}
