using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Persistance.Repositories;
using Persistance.Repositories.Kvietimai;
using Persistance.Repositories.PakviestiTreneriai;
using Persistance.Repositories.PrasymaiPakeistRole;
using Persistance.Repositories.Pratymai;
using Persistance.Repositories.PratymuSkaicius;
using Persistance.Repositories.Role;
using Persistance.Repositories.Statistika;
using Persistance.Repositories.Treniruote;
using Persistance.Repositories.Vartotojai;
using Persistance.Repositories.Vartotojas;

namespace ISporta
{
    public class Startup
    {
        readonly string MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddTransient<ISqlClient, SqlClient>(provider =>

                 new SqlClient(
                     "Server=localhost;Initial Catalog=ISportDatabase;MultipleActiveResultSets=true;User Id=admin;Password=admin"));
            services.AddSingleton<IRoleRepo, RoleRepo>();//kas karta kai new
            services.AddSingleton<IVartotojasRepo, VartotojasRepo>();
            services.AddSingleton<IKvietimaiRepo, KvietimaiRepo>();
            services.AddSingleton<ITreniruoteRepo, TreniruoteRepo>();
            services.AddSingleton<IPakviestiTreneriaiRepo, PakviestiTreneriaiRepo>();
            services.AddSingleton<IPrasymaiPakeistRoleRepo, PrasymaiPakeistRoleRepo>();
            services.AddSingleton<IVartotojaiRepo, VartotojaiRepo>();
            services.AddSingleton<IPratymaiRepo, PratymaiRepo>();
            services.AddSingleton<IStatistikaRepo, StatistikaRepo>();
            services.AddSingleton<IPratymuSkaiciusRepo, PratymuSkaiciusRepo>();
            services.AddSwaggerGen();

            services.AddCors(options => options.AddDefaultPolicy(builder => builder
             .AllowAnyHeader()
             .AllowAnyMethod()
             .AllowAnyOrigin()));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseSwagger();

            app.UseSwaggerUI(c =>

            {

                 c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");

            });

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseCors();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
