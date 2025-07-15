using Compliance_Dtos.Agencies;
using Compliance_Dtos.Regulator;
using Compliance_Dtos.Regulator;
using Compliance_Services.JWT;
using Dapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Globalization;
using System.Text;
using System.Text.Json;


var builder = WebApplication.CreateBuilder(args);


// 🔐 JWT Authentication Setup
var conn = builder.Configuration.GetConnectionString("DefaultConnection")
           ?? throw new InvalidOperationException("Connection string 'DefaultConnection' is missing.");


builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(opts =>
    {
        opts.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])),
            ValidateIssuer = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidateAudience = true,
            ValidAudience = builder.Configuration["Jwt:Audience"],
            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero
        };
        opts.Events = new JwtBearerEvents
        {
            OnChallenge = context =>
            {
                context.HandleResponse(); // Suppress the default response

                context.Response.StatusCode = 401;
                context.Response.ContentType = "application/json";

                var result = JsonSerializer.Serialize(new
                {
                    success = false,
                    message = "Unauthorized"
                });

                return context.Response.WriteAsync(result);
            }

        };
    });

// 🧱 Dependency Injection
builder.Services.AddScoped<IJwtTokenService, JwtTokenService>();
//builder.Services.AddScoped<IAuditedFinancialRepository, AuditedFinancialRepository>();
//builder.Services.AddScoped<IAuditedFinancialService, AuditedFinancialService>();

SqlMapper.SetTypeMap(
    typeof(RegulatorAddDto),

    new CustomPropertyTypeMap(
        typeof(RegulatorAddDto),
        (type, columnName) =>
        {
            // Remove 'rg_' prefix
            if (columnName.StartsWith("rg_"))
                columnName = columnName.Substring(3);

            // Convert snake_case to PascalCase
            string pascalCaseName = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(
                columnName.Replace("_", " ")
            ).Replace(" ", "");

            // Find matching property
            return type.GetProperties()
                       .FirstOrDefault(prop =>
                           prop.Name.Equals(pascalCaseName, StringComparison.OrdinalIgnoreCase));
        }
    )
);

SqlMapper.SetTypeMap(
    typeof(AgencyAddDto),
    new CustomPropertyTypeMap(
        typeof(AgencyAddDto),
        (type, columnName) =>
        {
            // Remove 'rg_' prefix
            if (columnName.StartsWith("ag_"))
                columnName = columnName.Substring(3);

            // Convert snake_case to PascalCase
            string pascalCaseName = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(
                columnName.Replace("_", " ")
            ).Replace(" ", "");

            // Find matching property
            return type.GetProperties()
                       .FirstOrDefault(prop =>
                           prop.Name.Equals(pascalCaseName, StringComparison.OrdinalIgnoreCase));
        }
    )
);






// Call the RegisterTypes method to register your custom services
Compliance_Services.RegisterAllServices.RegisterTypes(builder.Services);
Compliance_Repository.RegisterAllRepositories.RegisterTypes(builder.Services);

Compliance_Services.RegisterAllServices.RegisterTypes(builder.Services);
Compliance_Repository.RegisterAllRepositories.RegisterTypes(builder.Services);
// Add services to the container.

builder.Services.AddHttpContextAccessor();

builder.Services.AddControllers();

//builder.Services.AddControllers(options =>
//{
//    var policy = new AuthorizationPolicyBuilder()
//        .RequireAuthenticatedUser()
//        .Build();
//    options.Filters.Add(new AuthorizeFilter(policy)); // Global [Authorize]
//});

// ✅ Add Swagger with JWT Bearer support
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "PayGCompliance API", Version = "v1" });

    // ✅ Add JWT Bearer token UI support
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. Example: `Bearer {token}`",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" }
            },
            Array.Empty<string>()
        }
    });
});
var app = builder.Build();

// 🔐 Enable Swagger UI
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();


// 🛡️ Custom Middleware for Error Handling

// 🔐 Enable Authentication & Authorization
app.UseAuthentication();  // <<< Missing in your original code!
app.UseAuthorization();

app.MapControllers();

app.Run();
