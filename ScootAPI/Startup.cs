using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using ScootAPI.Models;
using ScootAPI.Repositories;
using ScootAPI.Services;

namespace ScootAPI
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
            services.AddControllers();

            var sqlConnectionString = Configuration["PostgreSqlConnectionString"];

            services.AddDbContext<PostgreSQLContext>(options => options.UseNpgsql(sqlConnectionString));

            services.AddControllersWithViews().AddNewtonsoftJson(options =>
                options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);

            services.AddScoped<IScootersRepository, ScootersRepository>();
            services.AddScoped<IZonesRepository, ZonesRepository>();
            services.AddScoped<IZonesService, ZonesService>();
            services.AddScoped<IScootersService, ScootersService>();
            services.AddScoped<IAmqpService, AmqpService>();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "ScootAPI", Version = "v1" });
            });

            services.AddSession();

            services.AddStackExchangeRedisCache(options => {
                options.Configuration = Configuration.GetSection("Redis")["ConnectionString"];
                options.InstanceName = "ScootersAPIRedis";
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "ScootAPI v1"));
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
