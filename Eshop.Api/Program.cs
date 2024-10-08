using eshop.DataAccess.Data;
using eshop.DataAccess.Services.Repo;
using eshop.DataAccess.Services.UnitOfWork;
using Eshop.Api.Controllers;
using Eshop.DataAccess.DataShaping;
using Eshop.DataAccess.Services;
using Eshop.DataAccess.Services.Auth;
using Eshop.DataAccess.Services.Links;
using Eshop.DataAccess.Services.Middleware;
using Eshop.DataAccess.Services.ModelService;
using Eshop.DataAccess.Services.Repo;
using Eshop.Models;
using Eshop.Models.Models;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.RateLimiting;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddHttpContextAccessor();
Log.Logger = new LoggerConfiguration()
                 .MinimumLevel
                 .Information()
                 .WriteTo.File("Logs/log-.txt", rollingInterval: RollingInterval.Day)
                 .CreateLogger();
builder.Logging.AddSerilog();

builder.Services.AddControllers().AddNewtonsoftJson()
                .AddJsonOptions(op => 
                op.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));

builder.Services.AddFluentValidationAutoValidation()
                .AddFluentValidationClientsideAdapters();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Ecommerce Api",
        Description = "Shopping Cart Api Asp.Net Core 8",
        Version = "v1"
    });
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "JWT Authorization header using the Bearer scheme."
    });
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
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
    options.UseInlineDefinitionsForEnums();
});


builder.Services.AddDbContext<AppDbContext>(op =>
                 op.UseSqlServer(builder.Configuration.GetConnectionString("constr")));
builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<AppDbContext>()
                .AddDefaultTokenProviders();

builder.Services.Configure<JWT>(builder.Configuration.GetSection("JWT"));
builder.Services.AddAuthentication(op =>
{
    op.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    op.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
    .AddJwtBearer(op =>
    {
        op.RequireHttpsMetadata = false;
        op.SaveToken = false;
        op.TokenValidationParameters = new TokenValidationParameters()
        {
            ValidateActor = true,
            ValidateIssuerSigningKey = true,
            RequireExpirationTime = true,
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidIssuer = builder.Configuration["JWT:Issuer"],
            ValidAudience = builder.Configuration["JWT:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Key"])),
            ClockSkew = TimeSpan.Zero
        };

    });

builder.Services.AddCors(op => op.AddPolicy("Eshop-UI", cfg =>
{
    cfg.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin().WithExposedHeaders("X-Pagination");
}));
Log.Logger = new LoggerConfiguration().MinimumLevel
                 .Information()
                 .WriteTo.File("Logging/log.txt", rollingInterval: RollingInterval.Day)
                 .CreateLogger();

builder.Services.AddMediatR(options =>
{
    options.RegisterServicesFromAssemblies(typeof(Program).Assembly);
});

builder.Services.AddRateLimiter(op =>
{
    op.RejectionStatusCode = StatusCodes.Status429TooManyRequests;
    op.AddFixedWindowLimiter("Fixed", op =>
    {
        op.Window = TimeSpan.FromSeconds(12);
        op.PermitLimit = 4;
        op.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
        op.QueueLimit = 2;
    }).RejectionStatusCode = StatusCodes.Status429TooManyRequests;
});

builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddAuthorization();

builder.Services.AddAutoMapper(typeof(MapperConfig));

builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<IOrderDetailRepository, OrderDetailRepository>(); 
builder.Services.AddScoped<ISupplierRepository, SupplierRepository>();
builder.Services.AddScoped<ICartRepository, CartRepository>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IUsersRepository, UsersRepository>();
builder.Services.AddScoped<ITransactionRepository, TransactionRepository>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped(typeof(IDataShaper<>), (typeof(DataShaper<>)));
builder.Services.AddScoped<ILinksService, LinksService>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<ICartService, CartService>();
builder.Services.AddApiVersioning(op =>
{
    op.ReportApiVersions = true;
    op.AssumeDefaultVersionWhenUnspecified = true;
    op.DefaultApiVersion = new ApiVersion(1,0);
    op.Conventions.Controller<OrderController>().HasApiVersion(new ApiVersion(1, 0)); 
    op.Conventions.Controller<OrderV2Controller>().HasDeprecatedApiVersion(new ApiVersion(2, 0));
    op.ApiVersionReader = new HeaderApiVersionReader("api-version");
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "EshopApi V1");
        c.SwaggerEndpoint("/swagger/v2/swagger.json", "EshopApi V2");
    });
}

app.UseExceptionHandler();
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseRateLimiter();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.UseCors("Eshop-UI");


app.Run();
