using Challenge.API.Filters;
using Challenge.Application;
using Challenge.Application.Services.Cache.Redis;
using Challenge.Application.Services.Localization;
using Challenge.Core.Security.Encryption;
using Challenge.Core.Security.Hash;
using Challenge.Helpers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Challenge.API
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
            services.AddControllers(config =>
            {
                config.Filters.Add(typeof(GlobalExceptionFilter));
            }).AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.PropertyNamingPolicy = null;
            });

            services.AddMessageHandlers();
            services.AddApplicationServices();
            services.ConfigureIOptions(Configuration);
            services.ConfigureJWTAuthentication(Configuration);
            services.ConfigureSwagger();
            services.AddScoped<ILocalizationService, LocalizationService>();
            services.AddScoped<IRedisService, RedisService>();
            services.AddScoped<IHasher, Hasher>();
            services.AddScoped<IEncryption, Encryption>();

            services.AddDistributedRedisCache(options =>
            {
                options.Configuration = Configuration.GetConnectionString("Redis");
                options.InstanceName = "Challenge-";
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger();
            app.UseSwaggerUI(c => { c.SwaggerEndpoint("/swagger/v1.0/swagger.json", "API Version (V-1.0)"); });
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
