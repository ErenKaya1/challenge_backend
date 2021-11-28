using System;
using System.Runtime.InteropServices;
using Challenge.Application;
using Challenge.Application.Services.Cache.Redis;
using Challenge.Application.Services.CurrentUser;
using Challenge.Application.Services.Localization;
using Challenge.Helpers;
using Hangfire;
using Hangfire.Mongo;
using Hangfire.Storage;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace Challenge.Hangfire
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
            services.AddControllersWithViews();

            services.AddMessageHandlers();
            services.AddApplicationServices();
            services.ConfigureIOptions(Configuration);
            services.AddScoped<ILocalizationService, LocalizationService>();
            services.AddScoped<IRedisService, RedisService>();
            services.AddScoped<ICurrentUserService, CurrentUserService>();

            services.AddDistributedRedisCache(options =>
            {
                options.Configuration = Configuration.GetConnectionString("Redis");
                options.InstanceName = "Challenge-";
            });

            services.AddHangfireServer();

            services.AddHangfire(config => config
                .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
                .UseSimpleAssemblyNameTypeSerializer()
                .UseRecommendedSerializerSettings()
                .UseMongoStorage(Configuration.GetConnectionString("Hangfire")));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseSerilogRequestLogging();
            app.UseHangfireDashboard();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });

            using (var connection = JobStorage.Current.GetConnection())
                foreach (var recurringJob in connection.GetRecurringJobs())
                    RecurringJob.RemoveIfExists(recurringJob.Id);

            TimeZoneInfo tz = null;
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                tz = TimeZoneInfo.FindSystemTimeZoneById("Turkey Standard Time");
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                tz = TimeZoneInfo.FindSystemTimeZoneById("Europe/Istanbul");

            // RecurringJob.AddOrUpdate<BirthDateJob>("BirthDateJob", x => x.ExecuteJob(), CronTimes.EVERY_DAY_AT_12, tz);
        }
    }
}