using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using DemoCityApi.Business;
using DemoCityApi.Business.Interfaces;
using DemoCityApi.Business.Managers;
using DemoCityApi.Data;
using DemoCityApi.RestServices;
using DemoCityApi.RestServices.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace DemoCityApi
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
            string t = Configuration.GetConnectionString("DefaultConnection");
            services.AddDbContext<CitiesContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            RegisterDependencies(services);

            services.AddControllers();
            services.AddSwaggerGen();
        }

        private static void RegisterDependencies(IServiceCollection services)
        {
            services.AddScoped<HttpMessageHandler, HttpClientHandler>();
            services.AddScoped<IHttpClientProvider, HttpClientProvider>();
            services.AddScoped<ICityManager, CityManager>();
            services.AddScoped<ICountryInfoService, CountryInfoService>();
            services.AddScoped<IWeatherService, WeatherService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseSwagger();
            app.UseSwaggerUI(swaggerui => {
                swaggerui.SwaggerEndpoint("/swagger/v1/swagger.json", "City Weather Api");
            });
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
