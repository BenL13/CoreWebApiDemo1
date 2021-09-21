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
using Microsoft.Azure.Documents.Client;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using Unity;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
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
            DocumentClient docClient = new DocumentClient(new Uri(Configuration["DBEndpoint"]), Configuration["Key"]);
            container.RegisterType<ICreateDBInstance, CreateDBInstance>();
            container.RegisterType<IDocumentDBCollectionRepository, DocumentDBCollection>();
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
            var key = "This is my first Test Key";
            //services.AddAuthentication(x =>
            //{
            //    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            //    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            //}).AddJwtBearer(x =>
            //{
            //    x.RequireHttpsMetadata = false;
            //    x.SaveToken = true;
            //    x.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
            //    {
            //        ValidateIssuerSigningKey = true,
            //        ValidateIssuer = false,
            //        ValidateAudience = false,
            //        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(key))
            //    };
            //});

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = "Bearer";
            });
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })

            // Adding Jwt Bearer  
            .AddJwtBearer(options =>
            {
                options.Authority = "https://localhost:44376";
                options.SaveToken = true;
                options.RequireHttpsMetadata = false;
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidAudience = "https://localhost:44376/api/family/",
                    ValidIssuer = Configuration["ClientID"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["ClientSecret"]))
                };
            });

            services.AddSingleton<IJwtAuth>(new Auth(key));
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
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        
    }
}
