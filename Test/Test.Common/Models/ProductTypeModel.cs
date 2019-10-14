using Core.DomainModel.Entities;
using System.Collections.Generic;

namespace Test.Common.Models
{
    public class ProductTypeModel : BaseModel<ProductType>
    {

        public ProductType Entity
        {
            get => new ProductType()
            {
                ProductTypeID = 4,
                Name = "Cards",
                Width = 4.7f,
                StackedCount = 1
            };
        }

        public IList<ProductType> EntityList
        {
            get => new List<ProductType>
                {
                    new ProductType()
                    {
                        ProductTypeID = 1,
                        Name = "PhotoBook",
                        Width = 19,
                        StackedCount = 1
                    },
                   new ProductType()
                    {
                        ProductTypeID = 2,
                        Name = "Calendar",
                        Width = 10,
                        StackedCount = 1
                    },
                   new ProductType()
                    {
                        ProductTypeID = 3,
                        Name = "Canvas",
                        Width = 16,
                        StackedCount = 1
                    },
                    new ProductType()
                    {
                        ProductTypeID = 4,
                        Name = "Cards",
                        Width = 4.7f,
                        StackedCount = 1
                    },
                    new ProductType()
                    {
                        ProductTypeID = 5,
                        Name = "Mug",
                        Width =94,
                        StackedCount = 4
                    }
                };
        }

    }
}
