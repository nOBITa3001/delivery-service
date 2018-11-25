using DS.DataAccess;
using DS.DomainModel;
using DS.Handlers;
using DS.Infrastructure.Web.Filters;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Swashbuckle.AspNetCore.Swagger;
using System;

namespace DS.API
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public ILoggerFactory LoggerFactory { get; }

        public Startup(IConfiguration configuration, ILoggerFactory loggerFactory)
        {
            Configuration = configuration;
            LoggerFactory = loggerFactory;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            HandlersDiConfig.RegisterAllDependencies(services);
            DataAccessDiConfig.RegisterRepositories(services);
            DomainModelDiConfig.RegisterFactories(services);

            LoggerFactory.AddSentry();

            services
                .AddMvc(AddCustomFilters(LoggerFactory))
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            services.AddApiVersioning();
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc
                (
                    "v1",
                    new Info { Version = "v1", Title = "Delivery Service API" }
                );
            });
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
                app.UseDeveloperExceptionPage();
            else
                app.UseHsts();

            app.UseHttpsRedirection();

            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "Delivery Service API V1");
            });

            app.UseMvc();
        }

        private Action<MvcOptions> AddCustomFilters(ILoggerFactory loggerFactory)
        {
            return options =>
            {
                options.Filters.Add(new GlobalExceptionFilter(loggerFactory));
            };
        }
    }
}
