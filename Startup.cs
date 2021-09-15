using CoreWebApiDemo1.Controllers;
using CoreWebApiDemo1.IRepository;
using CoreWebApiDemo1.Models;
using CoreWebApiDemo1.Repository;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using Unity;
namespace CoreWebApiDemo1
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        public async void ConfigureContainer(IUnityContainer container)
        {
            
            container.RegisterType<ICreateDBInstance, CreateDBInstance>();
            var instance = container.Resolve<GetKeyVaultSecret>();
            await instance.DocumentDBInstance(Configuration["DBEndpoint"], Configuration["Key"]);
        }
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddControllers(options => options.Filters.Add(typeof(HttpResponseExceptionFilterAttribute)));
            services.AddMvcCore()
                .AddControllersAsServices();
            services.AddOptions();
            services.Configure<EnvironmentConfig>(this.Configuration.GetSection("MySettings"));
            
            services.AddSingleton<IGetKeyVaultSecret, GetKeyVaultSecret>();
            services.AddApplicationInsightsTelemetry(Configuration["APPINSIGHTS_CONNECTIONSTRING"]);

        }
        
        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            
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
