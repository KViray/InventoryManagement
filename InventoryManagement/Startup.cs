using CloudinaryDotNet;
using InventoryManagement.Context;
using InventoryManagement.Features;
using InventoryManagement.Features.Attendances.Services;
using InventoryManagement.Features.Employees.Services;
using InventoryManagement.Features.Images.Services;
using InventoryManagement.Features.Inventories.Services;
using InventoryManagement.Features.ItemHistories.Services;
using InventoryManagement.Features.Items.Services;
using InventoryManagement.Features.Leaves.Services;
using InventoryManagement.Features.Logins.Services;
using InventoryManagement.Features.Salaries.Services;
using MediatR;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace InventoryManagement
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
            var securityKey = Configuration.GetValue<string>("JwtToken:SecurityKey"); ;
            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(securityKey));

            var cloudName = Configuration.GetValue<string>("CloudinaryAccountSettings:CloudName");
            var apiKey = Configuration.GetValue<string>("CloudinaryAccountSettings:ApiKey");
            var apiSecret = Configuration.GetValue<string>("CloudinaryAccountSettings:ApiSecret");

            services.AddDbContext<InventoryDbContext>(op => op.UseSqlServer(Configuration.GetConnectionString("InventoryManagementDB")));
            services.AddSingleton(new Cloudinary(new Account(cloudName, apiKey, apiSecret)));
            services.AddScoped<IAttendanceService, AttendanceService>();
            services.AddScoped<IEmployeeService, EmployeeService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IItemService, ItemService>();
            services.AddScoped<IInventoryService, InventoryService>();
            services.AddScoped<IImageService, ImageService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<ILeavesService, LeavesService>();
            services.AddScoped<IItemHistoryService, ItemHistoryService>();
            services.AddScoped<ISalaryService, SalaryService>();
            services.AddSingleton<Functions>();
            services.AddMediatR(typeof(Startup));
            services.AddControllers();
            services.AddAuthentication(auth =>
            {
                auth.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                auth.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.RequireHttpsMetadata = false;
                    options.SaveToken = true;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateAudience = false,
                        ValidateIssuerSigningKey = true,
                        ValidateIssuer = false,
                        IssuerSigningKey = symmetricSecurityKey
                    };
                    options.Events = new JwtBearerEvents
                    {
                        OnMessageReceived = context =>
                        {
                            context.Token = context.Request.Cookies["Jwt-Token"];
                            return Task.CompletedTask;
                        }
                    };
                });
            
            services.AddSwaggerGen();

            JsonConvert.DefaultSettings = () => new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();


            app.UseSwagger();
            app.UseSwaggerUI(s =>
            {
                s.SwaggerEndpoint("/swagger/v1/swagger.json", "Inventory Management");
                s.RoutePrefix = string.Empty;

            }
            );
            app.UseAuthentication();
            app.UseRouting();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
