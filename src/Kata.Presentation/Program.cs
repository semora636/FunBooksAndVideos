using FluentValidation;
using FluentValidation.AspNetCore;
using HealthChecks.UI.Client;
using Kata.BusinessLogic;
using Kata.Presentation.Authentication;
using Kata.Presentation.Middlewares;
using Kata.Presentation.Validators;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Reflection;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<JwtTokenService>();

// Add services to the container.
//builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
//    .AddMicrosoftIdentityWebApi(builder.Configuration.GetSection("AzureAd"));

// Configure simple JWT Authentication for demo purposes
builder.Services.AddAuthentication(option =>
{
    option.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    option.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"] ?? string.Empty)),
    };
});

builder.Services.AddAuthorization();

string? connectionString = builder.Configuration.GetConnectionString("KataConnection");

if (string.IsNullOrEmpty(connectionString))
{
    connectionString = "Server=.;Database=FunBooksAndVideos;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True";
}

builder.Services.RegisterBusinessLogicRepositories(connectionString);

builder.Services.AddScoped<IJwtTokenService, JwtTokenService>();

builder.Services.AddControllers(options => options.SuppressAsyncSuffixInActionNames = false);

// Register MediatR
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddValidatorsFromAssemblyContaining<BookValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<CustomerValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<MembershipProductValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<PurchaseOrderValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<OrderItemValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<VideoValidator>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(option =>
{
    option.SwaggerDoc("v1", new OpenApiInfo { Title = "Kata API", Version = "v1" });
    option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter a valid token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "Bearer"
    });
    option.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type=ReferenceType.SecurityScheme,
                    Id="Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

builder.Services.AddHealthChecks()
    .AddSqlServer(connectionString);

var app = builder.Build();

app.MapHealthChecks("/health",
    new HealthCheckOptions
    {
        ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
    })
    .RequireAuthorization();

app.UseMiddleware<ExceptionMiddleware>();

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
