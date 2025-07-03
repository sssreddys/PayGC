using Compliance_Dtos;
using Compliance_Repository.User;
using Dapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

var conn = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddSingleton(conn);

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

builder.Services.AddScoped<IUserRepository>(_ => new UserRepository(conn));
// Call the RegisterTypes method to register your custom services
Compliance_Services.Register.RegisterTypes(builder.Services);
Compliance_Repository.Register.RegisterTypes(builder.Services);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
