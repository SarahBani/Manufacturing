using Core.DomainModel.Entities;
using System.Collections.Generic;

namespace Core.ApplicationService.Contracts
{
    public interface IProductTypeService
    {

        IList<ProductType> GetAll();

    }
}
