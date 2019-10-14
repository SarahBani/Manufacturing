using Core.DomainModel.Entities;
using Core.DomainServices;
using DependencyInversion.Injector;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace WebAPI
{
    public class Startup
    {

        #region Properties

        public IConfiguration Configuration { get; }

        #endregion /Properties

        #region Constructors

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        #endregion /Constructors

        #region Mehtods

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            string contentRootPath = services.BuildServiceProvider().GetService<IHostingEnvironment>().ContentRootPath;
            /// It is not the operational database with essential data, 
            /// so I don't change the connection string or make database in memory
            string connectionString = GetConnectionString(this.Configuration, contentRootPath);
            services.AddDbContext<ManufacturingDbContext>(options => options.UseSqlServer(connectionString));
            services.SetInjection();
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2)
                .ConfigureApiBehaviorOptions(options =>
                {
                    options.SuppressConsumesConstraintForFormFileParameters = true;
                    options.SuppressInferBindingSourcesForParameters = true;
                    options.SuppressModelStateInvalidFilter = true;
                    options.SuppressMapClientErrors = true;
                });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseMvc();
        }

        private string GetConnectionString(IConfiguration config, string contentRootPath)
        {
            string connectionString = Utility.GetConnectionString(config);
            connectionString = connectionString.Replace("%CONTENTROOTPATH%", contentRootPath);
            return connectionString;
        }

        #endregion /Mehtods

    }
}
