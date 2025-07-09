using Compliance_Dtos;
using Compliance_Services.JWT;
using Dapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;


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
    });

// 🧱 Dependency Injection
builder.Services.AddScoped<IJwtTokenService, JwtTokenService>();

SqlMapper.SetTypeMap(
    typeof(RegulatorDto),
    new CustomPropertyTypeMap(
        typeof(RegulatorDto),
        (type, columnName) => type.GetProperties()
            .FirstOrDefault(prop =>
                prop.Name.Equals(columnName.Replace("_", ""), StringComparison.OrdinalIgnoreCase)
            )
    )
);


// Call the RegisterTypes method to register your custom services
Compliance_Services.RegisterAllServices.RegisterTypes(builder.Services);
Compliance_Repository.RegisterAllRepositories.RegisterTypes(builder.Services);
// Add services to the container.



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
