using Application;
using Application.Common.Interfaces;
using FluentValidation.AspNetCore;
using Infrastructure;
using Infrastructure.Persistence;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NSwag;
using NSwag.Generation.Processors.Security;
using Serilog;
using System.Linq;
using Web.DocumentProcessors;
using Web.Filters;
using Web.Services;
using Slack.Options;
using Hangfire;
using MediatR;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Application.Common.Options;
using Web.Options;
using Microsoft.EntityFrameworkCore;
using Application.Common.Hangfire;
using Application.Common.SignalR.Hub;

namespace Web
{
  public class Startup
  {
    public Startup(IConfiguration configuration, IWebHostEnvironment environment)
    {
      Configuration = configuration;
      Environment = environment;
    }

    public IConfiguration Configuration { get; }

    public IWebHostEnvironment Environment { get; }
    // This method gets called by the runtime. Use this method to add services to the container.
    // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
    public void ConfigureServices(IServiceCollection services)
    {
      services.Configure<SlackOptions>(Configuration.GetSection(SlackOptions.SlackOptionsToken));
      services.Configure<TokenOptions>(Configuration.GetSection(TokenOptions.Tokens));
      services.Configure<CorsOptions>(Configuration.GetSection(CorsOptions.Cors));
      services.Configure<RedirectOptions>(Configuration.GetSection(RedirectOptions.Redirect));

      var corsOptions = Configuration.GetSection(CorsOptions.Cors).Get<CorsOptions>();

      services.AddCors(options =>
      {
        options.AddPolicy("AllowSecure",
                  builder =>
                  {
                    builder.WithOrigins(corsOptions.Origins);
                    builder.AllowAnyHeader();
                    builder.AllowAnyMethod();
                    builder.AllowCredentials();
                  });
      });


      services.AddApplication();
      services.AddInfrastructure(Configuration, Environment);

      services.AddHttpContextAccessor();

      services.AddHealthChecks()
          .AddDbContextCheck<ApplicationDbContext>();

      services.AddControllers(options =>
                 options.Filters.Add<ApiExceptionFilterAttribute>())
          .AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<IApplicationDbContext>())
          .AddNewtonsoftJson();

      // Customise default API behaviour
      services.Configure<ApiBehaviorOptions>(options =>
      {
        options.SuppressModelStateInvalidFilter = true;
      });

      services.AddOpenApiDocument(configure =>
      {
        configure.Title = "Backend API";
        configure.AddSecurity("JWT", Enumerable.Empty<string>(), new OpenApiSecurityScheme
        {
          Type = OpenApiSecuritySchemeType.ApiKey,
          Name = "Authorization",
          In = OpenApiSecurityApiKeyLocation.Header,
          Description = "Bearer {your JWT token}."
        });

        configure.OperationProcessors.Add(new AspNetCoreOperationSecurityScopeProcessor("JWT"));
        configure.DocumentProcessors.Add(new CustomDocumentProcessor());
      });

      services.AddScoped<ICurrentUserService, CurrentUserService>();
      services.AddScoped<IAuthorizationService, AuthorizationService>();
      services.AddScoped<ITokenService, TokenService>();

      services.AddHangfire(connString: Configuration.GetConnectionString("DefaultConnection"));

      var tokenOptions = Configuration.GetSection(TokenOptions.Tokens).Get<TokenOptions>();
      var key = Encoding.ASCII.GetBytes(tokenOptions.Secret);
      services.AddAuthentication(x =>
        {
          x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
          x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(x =>
        {
          x.RequireHttpsMetadata = false;
          x.SaveToken = true;
          x.TokenValidationParameters = new TokenValidationParameters
          {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(key),
            ValidateIssuer = false,
            ValidateAudience = false
          };

          x.Events = new JwtBearerEvents
          {
            OnMessageReceived = context =>
            {
              var path = context.HttpContext.Request.Path;
              if (!path.HasValue || !(path.Value.StartsWith("/hubs/"))) {
                return System.Threading.Tasks.Task.CompletedTask;
              }

              var accessToken = context.Request.Query["access_token"];
              if (string.IsNullOrEmpty(accessToken)) {
                return System.Threading.Tasks.Task.CompletedTask;
              }

              var accessTokenStr = accessToken.ToString();
              if(!accessTokenStr.StartsWith("bearer ", true, null)) {
                return System.Threading.Tasks.Task.CompletedTask;
              }

              var bearerToken = accessTokenStr.Substring(7);
              context.Token = bearerToken;

              return System.Threading.Tasks.Task.CompletedTask;
            }
          };
        });

        services.AddSignalR();
    }


    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IMediator mediator, ApplicationDbContext context)
    {
      if (env.IsDevelopment())
      {
        app.UseDeveloperExceptionPage();
      }
      else
      {
        app.UseExceptionHandler("/Error");
        // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
        app.UseHsts();

        using (context)
        {
          context.Database.Migrate();
        }
      }

      //TODO Handle cors
      app.UseCors("AllowSecure");

      app.UseSerilogRequestLogging();
      app.UseHealthChecks("/health");
      app.UseHttpsRedirection();
      app.UseStaticFiles();
      app.UseSwaggerUi3(settings =>
      {
        settings.Path = "/swagger";
        settings.DocumentPath = "/swagger/specification.json";
      });

      app.UseRouting();

      app.UseAuthentication();
      app.UseAuthorization();

      app.UseHangfireDashboard("/hangfire", new DashboardOptions
      {
        Authorization = new [] { new HangfireAuthorizationFilter() }
      });

      app.UseEndpoints(endpoints =>
      {
        endpoints.MapControllerRoute(
                  name: "default",
                  pattern: "{controller}/{action=Index}/{id?}");

        endpoints.MapHub<PrizeHub>("/hubs/prize");
        endpoints.MapHangfireDashboard();
      });

      mediator.SetupHangfireJobs(env.IsDevelopment());
    }
  }
}
