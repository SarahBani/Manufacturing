using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;

namespace Test.IntegrationTest
{
    public class APIWebApplicationFactory : WebApplicationFactory<WebAPI.Startup>
    {

        #region Methods

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            /// It is not the operational database with essential data, 
            /// so I don't change the configuration for connection string
            /// or making database in memory
            builder.UseEnvironment("IntegrationTest");
            base.ConfigureWebHost(builder);
        }

        #endregion /Methods

    }
}
