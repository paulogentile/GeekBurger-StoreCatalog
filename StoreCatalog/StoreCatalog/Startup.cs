﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using GeekBurger.StoreCatalog.Controllers;
using GeekBurger.StoreCatalog.Helper;
using GeekBurger.StoreCatalog.Repository;
using GeekBurger.StoreCatalog.Repository.Interfaces;
using GeekBurger.StoreCatalog.Service;
using GeekBurger.StoreCatalog.Service.GetProductChanged;
using GeekBurger.StoreCatalog.Service.GetProductionAreaChanged;
using GeekBurger.StoreCatalog.Service.UserWithLessOffer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.PlatformAbstractions;
using Swashbuckle.AspNetCore.Swagger;

namespace GeekBurger.StoreCatalog
{
    public class Startup
    {
        public static IConfiguration Configuration { get; set; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvcCore()
                .AddFormatterMappings()
                .AddJsonFormatters()
                .AddCors();

            services.AddMvc();

            // Configurando o serviço de documentação do Swagger
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1",
                    new Info
                    {
                        Title = "Geek Burguer StoreCatalog",
                        Version = "v1",
                        Description = "API que retorna os produtos da loja local.",
                        Contact = new Contact
                        {
                            Name = "Paulo Gentile",
                            Url = "https://github.com/paulogentile"
                        }
                    });
            });

            services.AddAutoMapper();


            // Configura Banco de Dados em Memoria

            services.AddDbContext<StoreCatalogContext>(o => o.UseSqlServer("Server=tcp:geekburguer.database.windows.net,1433;Initial Catalog=GeekBurger;Persist Security Info=False;User ID=StoreCatalog;Password=qwer1234!;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;"));

            services.AddScoped<IStoreCatalogRepository, StoreCatalogRepository>();

            services.AddScoped<ILogService, LogService>();
            services.AddScoped<IStoreCatalogReadyService, StoreCatalogReadyService>();
            services.AddScoped<IGetProducts, GetProducts>();
            services.AddScoped<IGetProductions, GetProductions>();
            services.AddScoped<IUserWithLessOffer, UserWithLessOffer>();
            services.AddScoped<IAppInnit, AppInnit>();

            services.AddSingleton<IHealthCheck, HealthCheck>();
            services.AddSingleton<IGetProductChangedService, GetProductChangedService>();
            services.AddSingleton<IGetProductionAreaChangedService, GetProductionAreaChangedService>();

            //services.AddPollyPolicies();
            services.AddScoped<IAppInnit, AppInnit>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IAppInnit appInnit)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            //context.Seed();

            app.UseMvc();

            // Ativando middlewares para uso do Swagger
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json",
                    "Geek Burguer StoreCatalog");
            });

            appInnit.run();
        }
    }
}
