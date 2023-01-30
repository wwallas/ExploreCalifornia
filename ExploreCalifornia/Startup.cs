﻿using ExploreCalifornia.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExploreCalifornia
{
    public class Startup
    {
        private readonly IConfiguration configuration;

        public Startup(IConfiguration Development) 
        {
            this.configuration = Development;
        }
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddTransient<FormattingService>();
            services.AddTransient<FeatureToggles>(x => new FeatureToggles { 
                DeveloperExceptions=configuration.GetValue<bool>("FeatureToggles:DeveloperExceptions")
            });

            services.AddDbContext<BlogDataContext>(options =>
            {
                var connectionString = configuration.GetConnectionString("BlogDataContext");
                options.UseSqlServer(connectionString);
            });


            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, 
            IHostingEnvironment env,
            FeatureToggles features)
        {
            app.UseExceptionHandler("/error.html");

            if (env.IsDevelopment()) 
            {
                app.UseDeveloperExceptionPage();
            }

            //if (features.DeveloperExceptions)
            //{
            //    app.UseDeveloperExceptionPage();
            //}

            //app.Use(async (context, next) =>
            //{
            //    if (context.Request.Path.Value.Contains("invalid"))
            //        throw new Exception("Error");
            //    await next();
            //});

            app.UseMvc(routes => 
            {
                routes.MapRoute("Default","{controller=Home}/{action=Index}/{id?}");
            });

            app.UseFileServer();
            //app.Use(async (context, next) =>
            //{
            //    if (context.Request.Path.Value.StartsWith("/hello")) 
            //    {
            //        await context.Response.WriteAsync("Hello World William!");
            //    }
                
            //    await next();
            //});

            //app.Run(async (context) =>
            //{
            //    await context.Response.WriteAsync("How are you!");
            //});
        }
    }
}