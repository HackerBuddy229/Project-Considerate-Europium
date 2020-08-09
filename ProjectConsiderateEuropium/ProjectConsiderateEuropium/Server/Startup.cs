using System;
using System.Text;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using ProjectConsiderateEuropium.Server.Authentication.Services;
using ProjectConsiderateEuropium.Server.Controllers;
using ProjectConsiderateEuropium.Server.Data;
using ProjectConsiderateEuropium.Server.Data.Models;
using ProjectConsiderateEuropium.Server.External;
using ProjectConsiderateEuropium.Server.services;
using ProjectConsiderateEuropium.Server.services.AlternativeProduct;
using ProjectConsiderateEuropium.Server.services.ApplicationUser;
using ProjectConsiderateEuropium.Server.services.ZeroProduct;

namespace ProjectConsiderateEuropium.Server
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {

            //ef core
            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseInMemoryDatabase("InMemoryDb");
                options.ConfigureWarnings(x => x.Ignore(InMemoryEventId.TransactionIgnoredWarning));
            });

            //identity
            services.AddIdentityCore<ApplicationIdentityUser>(o =>
                {
                    o.Password.RequireDigit = false;
                    o.Password.RequireNonAlphanumeric = false;
                    o.Password.RequireUppercase = false;
                    o.Password.RequiredLength = 4;

                    o.User.RequireUniqueEmail = true;
                })
                .AddEntityFrameworkStores<ApplicationDbContext>();


            //auth

            var validAudience = Configuration.GetSection("Jwt").GetValue<string>("ValidAudience");
            var validIssuer = Configuration.GetSection("Jwt").GetValue<string>("ValidIssuer");

            var secret = Configuration["Jwt:AuthenticationSigningKey"];
            var bytes = Encoding.UTF8.GetBytes(secret);
            var key = new SymmetricSecurityKey(bytes);


            services.AddAuthentication("jwt")
                .AddJwtBearer("jwt", o =>
                {
                    o.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidAudience = validAudience,
                        ValidIssuer = validIssuer,
                        IssuerSigningKey = key
                    };
                });

            
            
            //dependency injection
            services.AddHttpContextAccessor();
            services.AddSingleton<IUriService>(o =>
            {
                var accessor = o.GetRequiredService<IHttpContextAccessor>();
                var request = accessor.HttpContext.Request;
                var uri = string.Concat(request.Scheme, "://", request.Host.ToUriComponent());
                return new UriService(uri);
            });

            services.AddScoped<EFCoreTestingService>();

            //data services
            services.AddScoped<IAlternativeService, AlternativeService>();
            services.AddScoped<IAlternativeGetterService, AlternativeGetterService>();
            services.AddScoped<IAlternativeCreationService, AlternativeCreationService>();

            services.AddScoped<IProductGetterService, ProductGetterService>();
            services.AddScoped<IProductCreationService, ProductCreationService>();
            services.AddScoped<IProductService, ProductService>();

            //identityServices
            services.AddScoped<IUserManagementService, UserManagementService>();
            services.AddScoped<IUserRegistrationService, UserRegistrationService>();

            //authentication services
            services.AddScoped<ITokenCreationService, TokenCreationService>();
            services.AddScoped<ITokenValidationService, TokenValidationService>();
            services.AddScoped<ITokenService, TokenService>();

            services.AddScoped<IAuthenticationService, AuthenticationService>();

            //external
            services.AddSingleton<IJwtConfiguration, JwtConfiguration>();

            //base
            services.AddControllersWithViews();


            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "Project Considerate Europium",
                    Description = "description goes here",
                    Contact = new OpenApiContact
                    {
                        Name = "Rasmus Bengtsson",
                        Email = "Rasmusb229@gmail.com",
                        Url = new Uri("https://www.Beryllium.tech")
                    }
                });

                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    Description = "Input your Bearer token to access parts of this API"
                });
            });

            services.AddRazorPages();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {

            app.UseSwagger();//TODO: remove before publish maybe...

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseWebAssemblyDebugging();
            }
            else
            {
                app.UseExceptionHandler("/Error");
            }

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Project Considerate Europium V1");
            });


            app.UseBlazorFrameworkFiles();
            app.UseStaticFiles();

            app.UseRouting();

            //auth
            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
                endpoints.MapControllers();
                endpoints.MapFallbackToFile("index.html");
            });
        }
    }
}
