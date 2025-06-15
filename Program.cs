using InventoryManagementSystem.BussinessService.Implemantations;
using InventoryManagementSystem.BussinessService.Interface;
using InventoryManagementSystem.Data.Context;
using InventoryManagementSystem.DataService.Implementations;
using InventoryManagementSystem.DataService.Interface;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using Serilog;
using FluentValidation.AspNetCore;
using FluentValidation;
using InventoryManagementSystem.Validations;
using InventoryManagementSystem.Common.Helper;

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information() 
    .MinimumLevel.Override("InventoryManagementSystem", Serilog.Events.LogEventLevel.Debug) 
    .MinimumLevel.Override("Microsoft", Serilog.Events.LogEventLevel.Warning) 
    .MinimumLevel.Override("System", Serilog.Events.LogEventLevel.Warning) 
    .MinimumLevel.Override("Microsoft.EntityFrameworkCore", Serilog.Events.LogEventLevel.Warning) 
    .Enrich.FromLogContext()
    .WriteTo.Console(
        outputTemplate: "[{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz}] [{Level:u3}] {Message:lj}{NewLine}{Exception}")
    .WriteTo.File(
        path: "Logs/log-.txt",
        rollingInterval: RollingInterval.Day,
        outputTemplate: "[{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz}] [{Level:u3}] {Message:lj}{NewLine}{Exception}")
    .CreateLogger();

try
{
    Log.Information("Starting up the application...");

    var builder = WebApplication.CreateBuilder(args);

    builder.Host.UseSerilog();

    builder.Services.AddDbContext<ApplicationDbContext>(options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

    builder.Services.AddScoped<IUserService, UserService>();
    builder.Services.AddScoped<IUserDataService, UserDataService>();
    builder.Services.AddScoped<IProductService, ProductService>();
    builder.Services.AddScoped<IProductDataService, ProductDataService>();
    builder.Services.AddScoped<ICategoryService, CategoryService>();
    builder.Services.AddScoped<ICategoryDataService, CategoryDataService>();
    builder.Services.AddScoped<IStocksMovementService, StocksMovementService>();
    builder.Services.AddScoped<IStocksMovementDataService, StocksMovementDataService>();
    builder.Services.AddScoped<IStockPurchaseOrderService, StockPurchaseOrderService>();
    builder.Services.AddScoped<IStockPurchaseOrderDataService, StockPurchaseOrderDataService>();
    builder.Services.AddScoped<IVendorService, VendorService>();
    builder.Services.AddScoped<IVendorDataService, VendorDataService>();
    builder.Services.AddScoped<IOrderService, OrderService>();
    builder.Services.AddScoped<IOrderDataService, OrderDataService>();
    builder.Services.AddScoped<IEmailService, EmailService>();

    builder.Services.AddHostedService<LowStockCheckService>();

    builder.Services.AddControllers();
    builder.Services.AddFluentValidationAutoValidation();
    builder.Services.AddValidatorsFromAssemblyContaining<UserRegisterRequestValidator>();
    builder.Services.AddValidatorsFromAssemblyContaining<UserLoginRequestValidator>();
    builder.Services.AddValidatorsFromAssemblyContaining<AddProductRequestValidator>();
    builder.Services.AddValidatorsFromAssemblyContaining<CustomerOrderRequestValidator>();
    builder.Services.AddValidatorsFromAssemblyContaining<CustomerOrderItemRequestValidator>();
    builder.Services.AddValidatorsFromAssemblyContaining<ReturnOrderItemsRequestValidator>();
    builder.Services.AddValidatorsFromAssemblyContaining<StockPurchaseOrderItemRequestValidator>();
    builder.Services.AddValidatorsFromAssemblyContaining<StockPurchaseOrderPartialReceiveRequestValidator>();
    builder.Services.AddValidatorsFromAssemblyContaining<StockPurchaseOrderRequestValidator>();
    builder.Services.AddValidatorsFromAssemblyContaining<UpdateVendorRequestValidator>();
    builder.Services.AddValidatorsFromAssemblyContaining<VendorRequestValidator>();

    builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = builder.Configuration["Jwt:Issuer"],
                ValidAudience = builder.Configuration["Jwt:Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
            };
        });

    builder.Services.AddSwaggerGen(c =>
    {
        c.SwaggerDoc("v1", new OpenApiInfo { Title = "InventoryManagementSystem", Version = "v1" });

        c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
        {
            Description = "JWT Authorization header using the Bearer scheme.",
            Name = "Authorization",
            In = ParameterLocation.Header,
            Type = SecuritySchemeType.Http,
            Scheme = "bearer",
            BearerFormat = "JWT"
        });

        c.AddSecurityRequirement(new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    }
                },
                Array.Empty<string>()
            }
        });
    });

    builder.Services.AddAuthorization();
    builder.Services.AddEndpointsApiExplorer();

    var app = builder.Build();

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseHttpsRedirection();
    app.UseAuthentication();
    app.UseAuthorization();
    app.MapControllers();

    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Application failed to start");
}
finally
{
    Log.CloseAndFlush();
}