using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SpaServices.Webpack;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SportsStore.Models;
using Newtonsoft.Json;

namespace SportsStore
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
      services.AddDbContext<DataContext>(options =>
      {
        options.UseSqlServer(Configuration["Data:Products:ConnectionString"]);
      });
      services.AddMvc()
      .AddJsonOptions(option => {
        option.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Serialize;
        option.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
      })
      .SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

      services.AddDistributedSqlServerCache(options => {
        options.ConnectionString = Configuration["Data:Products:ConnectionString"];
        options.SchemaName = "dbo";
        options.TableName = "SessionData";
      });

      services.AddSession(options => {
        options.Cookie.Name = "SportsStore.Session";
        options.IdleTimeout = TimeSpan.FromHours(48);
        options.Cookie.HttpOnly = false;
      });
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IHostingEnvironment env, DataContext context)
    {

      app.UseDeveloperExceptionPage();
      app.UseWebpackDevMiddleware(new WebpackDevMiddlewareOptions()
      {
        HotModuleReplacement = true
      });

      //if (env.IsDevelopment())
      //{
      //    app.UseDeveloperExceptionPage();
      //}
      //else
      //{
      //    app.UseExceptionHandler("/Home/Error");
      //    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
      //    app.UseHsts();
      //}

      //app.UseHttpsRedirection();

      app.UseStaticFiles();
      app.UseSession();
      app.UseMvc(routes =>
      {
        routes.MapRoute(
                  name: "default",
                  template: "{controller=Home}/{action=Index}/{id?}");

        routes.MapSpaFallbackRoute("angular-fallback", new {controller = "Home", action = "Index"});
      });

      SeedData.SeedDatabase(context);
    }
  }
}
