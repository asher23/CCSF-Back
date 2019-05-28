using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using FreshmanCSForum.API.Data;
using FreshmanCSForum.API.Data.Interfaces;
using FreshmanCSForum.API.Helpers;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
// using FreshmanCSForum.API.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.CookiePolicy;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Driver;

namespace FreshmanCSForum.API
{
  public class Startup
  {
    public Startup(IConfiguration configuration)
    {
      Configuration = configuration;
    }

    public IConfiguration Configuration { get; }
    readonly string MyAllowSpecificOrigins = "_myAllowSpecificOrigins";


    // This method gets called by the runtime. Use this method to add services to the container.
    public void ConfigureServices(IServiceCollection services)
    {
      services.Configure<CookiePolicyOptions>(options =>
      {
        options.MinimumSameSitePolicy = SameSiteMode.None;
        options.HttpOnly = HttpOnlyPolicy.None;
        options.Secure = CookieSecurePolicy.None;
      });
      services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
      services.AddSingleton<MyMongoDatabase>(new MyMongoDatabase(Configuration.GetConnectionString("MongoDB")));

      services.AddScoped<IGuidesService, MongoGuidesService>();
      services.AddScoped<IUsersService, MongoUsersService>();
      services.AddScoped<ICodeLabsService, MongoCodeLabsService>();
      services.AddScoped<IAuthService, MongoAuthService>();
      services.AddScoped<ICommentsService, MongoCommentsService>();
      services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
        .AddCookie(options =>
        {
          options.Cookie.HttpOnly = true;
          options.Cookie.SecurePolicy = CookieSecurePolicy.None;
          options.Cookie.SameSite = SameSiteMode.None;
          // options.Cookie.SameSite = SameSiteMode.None;
          options.LoginPath = "/";
          options.Events.OnRedirectToLogin = (context) =>
          {
            context.Response.StatusCode = 401;
            return Task.CompletedTask;
          };
        });      // .AddJwtBearer(options =>
                 // {
                 //   options.TokenValidationParameters = new TokenValidationParameters
                 //   {
                 //     ValidateIssuerSigningKey = false,
                 //     IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Configuration.GetSection("AppSettings:Token").Value)),
                 //     ValidateIssuer = false,
                 //     ValidateAudience = false
                 //   };
                 // });

      services.AddCors(options =>
      {
        options.AddPolicy(MyAllowSpecificOrigins,
                builder =>
                {
                  builder.WithOrigins("http://localhost:3000", "http://www.contoso.com");
                  builder.AllowCredentials();
                  builder.AllowAnyHeader();
                  builder.AllowAnyMethod();
                });
      });
      services.Configure<CloudinarySettings>(Configuration.GetSection("CloudinarySettings"));
      services.AddAutoMapper();
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IHostingEnvironment env)
    {
      if (env.IsDevelopment())
      {
        app.UseDeveloperExceptionPage();
      }
      else
      {
        // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
        // app.UseHsts();
      }

      // app.UseHttpsRedirection();
      // app.UseCors(x => x.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader().AllowCredentials());
      app.UseCors(MyAllowSpecificOrigins);
      app.UseCookiePolicy();
      app.UseAuthentication();
      app.UseMvc();
    }
  }
}
