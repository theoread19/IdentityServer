using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityServer
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
/*            services.AddControllers();
            var builder = services.AddIdentityServer()
            .AddDeveloperSigningCredential()        //This is for dev only scenarios when you don’t have a certificate to use.
            .AddInMemoryApiScopes(Config.ApiScopes)
            .AddInMemoryIdentityResources(Config.IdentityResources)
            .AddInMemoryClients(Config.Clients)
            .AddTestUsers(Config.GetUser());

            builder.AddDeveloperSigningCredential();*/


            services.AddControllers();
            services.AddIdentityServer(options =>
            {
                options.Events.RaiseSuccessEvents = true;
                options.Events.RaiseFailureEvents = true;
                options.Events.RaiseErrorEvents = true;
            })
                 .AddInMemoryIdentityResources(Config.IdentityResources)
                 .AddInMemoryApiResources(Config.GetApiResources())
                 .AddInMemoryApiScopes(Config.ApiScopes)
                 .AddInMemoryClients(Config.Clients)
                 .AddDeveloperSigningCredential()
                 .AddTestUsers(Config.GetUser());
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseIdentityServer();

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
