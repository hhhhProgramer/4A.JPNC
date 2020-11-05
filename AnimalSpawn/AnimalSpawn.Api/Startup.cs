using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AnimalSpawn.Application.Repositories;
using AnimalSpawn.Application.Services;
using AnimalSpawn.Domain.Interfaces;
using AnimalSpawn.Infraestructure.Data;
using AnimalSpawn.Infraestructure.Repositories;
using AutoMapper;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using AnimalSpawn.Infraestructure.Filters;


namespace AnimalSpawn.Api
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
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            services.AddControllers();
            services.AddDbContext<AnimalSpawnContext>(options =>
            options.UseSqlServer(Configuration.GetConnectionString("AnimalSpawnConnection"))
            );
            services.AddMvc().AddFluentValidation(options =>
            options.RegisterValidatorsFromAssemblies(AppDomain.CurrentDomain.GetAssemblies()));
            services.AddControllers(options =>
            options.Filters.Add<GlobalExceptionFilter>());
            services.AddTransient<IAnimalService, AnimalService>();
            services.AddScoped(typeof(IRepository<>), typeof(SQLRepository<>));
            services.AddTransient<IUnitOfWork, UnitOfWork>();


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
