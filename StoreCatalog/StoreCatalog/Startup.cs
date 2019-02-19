using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using GeekBurger.StoreCatalog.Helper;
using GeekBurger.StoreCatalog.Repository;
using GeekBurger.StoreCatalog.Repository.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
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
            services.AddDbContext<StoreCatalogContext>(o => o.UseInMemoryDatabase("geekburger-storecatalog"));

            services.AddScoped<IStoreCatalogRepository, StoreCatalogRepository>();

            services.AddScoped<IAppInnit, AppInnit>();
            services.AddScoped<IGetProducts, GetProducts>();

            services.AddSingleton<IHealthCheck, HealthCheck>();
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
