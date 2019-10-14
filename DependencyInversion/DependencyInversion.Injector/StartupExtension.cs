using Core.ApplicationService;
using Core.ApplicationService.Contracts;
using Core.ApplicationService.Implementation;
using Core.DomainModel.Entities;
using Core.DomainService;
using Core.DomainService.Repository;
using Infrastructure.DataBase;
using Infrastructure.DataBase.Repository;
using Microsoft.Extensions.DependencyInjection;

namespace DependencyInversion.Injector
{
    public static class StartupExtension
    {
        public static IServiceCollection SetInjection(this IServiceCollection services)
        {
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IEntityService, EntityService>();

            services.AddScoped<IBaseRepository<Order, long>, OrderRepository>();
            services.AddScoped<IBaseRepository<OrderDetail, long>, OrderDetailRepository>();
            services.AddScoped<IBaseReadOnlyRepository<ProductType, byte>, ProductTypeRepository>();

            services.AddScoped<IOrderService, OrderService>();
            services.AddScoped<IOrderDetailService, OrderDetailService>();
            services.AddScoped<IProductTypeService, ProductTypeService>();

            return services;
        }

    }
}
