using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Http.Headers;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TabOrganizer___WebApp.Authentication;
using TabOrganizer___WebApp.Models;

namespace TabOrganizer___WebApp
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

            services.AddAuthentication(o => {
                o.DefaultScheme = SchemesNamesConst.TokenAuthenticationDefaultScheme;
            })
            .AddScheme<TokenAuthenticationOptions, TokenAuthenticationHandler>(SchemesNamesConst.TokenAuthenticationDefaultScheme, o => { });
            
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddSession();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
           // else
            //{
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            //}
            app.UseCors(x => x.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseSession();


            app.Use(async (context, next) =>
            {
                await next();

                if (context.Response.StatusCode == 401 || context.Response.StatusCode == 404 || context.Response.StatusCode == 400)
                {

                    //var newPath = new PathString("/Home/Error");
                    //var originalPath = context.Request.Path;
                    //var originalQueryString = context.Request.QueryString;
                    //context.Features.Set<IStatusCodeReExecuteFeature>(new StatusCodeReExecuteFeature()
                    //{
                    //    OriginalPathBase = context.Request.PathBase.Value,
                    //    OriginalPath = originalPath.Value,
                    //    OriginalQueryString = originalQueryString.HasValue ? originalQueryString.Value : null,
                    //});

                    //// An endpoint may have already been set. Since we're going to re-invoke the middleware pipeline we need to reset
                    //// the endpoint and route values to ensure things are re-calculated.
                    //context.SetEndpoint(endpoint: null);
                    //var routeValuesFeature = context.Features.Get<IRouteValuesFeature>();
                    //routeValuesFeature?.RouteValues?.Clear();

                    //context.Request.Path = newPath;
                    //try
                    //{
                    //    await next();
                    //}
                    //finally
                    //{
                    //    context.Request.QueryString = originalQueryString;
                    //    context.Request.Path = originalPath;
                    //    context.Features.Set<IStatusCodeReExecuteFeature>(null);
                    //}

                    // which policy failed? need to inform consumer which requirement was not met
                    //context.Response.ContentType = "text/plain";

                    throw new Exception($"Status Code: {context.Response.StatusCode}");

                    // await context.Response.WriteAsync($"Status Code: {context.Response.StatusCode}");
                    //await next();
                }

            });

            //Add JWToken to all incoming HTTP Request Header
            app.Use(async (context, next) =>
            {
                var JWToken = context.Session.GetString("JWToken");
                if (!string.IsNullOrEmpty(JWToken))
                {
                    context.Request.Headers.Add("Authorization", JWToken);
                }
                await next();
            });

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(name: "website",
            pattern: "website/{*containerId}",
            defaults: new { controller = "Website", action = "GetWebsites" });

                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
