using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Petshop.Core;
using Petshop.Core.ApplicationService;
using Petshop.Core.ApplicationService.Implementation;
using Petshop.Core.DomainService;
using Petshop.Core.DomainService.Implementation;
using Petshop.Core.Entity;
using Petshop.Infrastructure.Data;
using Petshop.Infrastructure.SQL;
using Petshop.Infrastructure.SQL.Repositories;

namespace Petshop.RestAPI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration; //
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            FakeDB.InitData();
          
            services.AddDbContext<PetshopDBContext>(
                opt =>
                {
                    opt.UseSqlite("Data Source=Petshop.db");
                });

            services.AddScoped<IPetRepository, PetSQLRepository>(); // remove sql if needed
            services.AddScoped<IPetService, PetService>();
            services.AddScoped<IOwnerRepository, OwnerSQLRepository>();// remove sql if needed
            services.AddScoped<IOwnerService, OwnerService>();
            services.AddScoped<IPetTypeRepository, PetTypeSQLRepository>();// remove sql if needed
            services.AddScoped<IPetTypeService, PetTypeService>();
            services.AddCors(options =>
             options.AddDefaultPolicy(
                 builder =>
                 {
                     builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
                 })
         );
            services.AddControllers()
    .AddNewtonsoftJson(options =>
    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
);
            services.AddSwaggerGen();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            });
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                using (var scope = app.ApplicationServices.CreateScope())

                        {
                    var ctx = scope.ServiceProvider.GetService<PetshopDBContext>();
                    ctx.Database.EnsureDeleted();
                    ctx.Database.EnsureCreated();
                    var petRepository = scope.ServiceProvider.GetService<IPetRepository>();
                    var petTypeRepository = scope.ServiceProvider.GetService<IPetTypeRepository>();
                    var ownerRepository = scope.ServiceProvider.GetService<IOwnerRepository>();
                    new DBInitializer(petRepository, ownerRepository, petTypeRepository).InitData();


                        }
            }
            

            app.UseHttpsRedirection();

            app.UseRouting();
            app.UseCors();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
