using E_commerce.Core.Entities;
using E_commerce.Infrastructure.Database;
using E_commerce.Infrastructure.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<EcomDbContext>(e => e.
UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddIdentity<AppUser, IdentityRole>(p =>
{ 
    p.Password.RequireDigit= true;
    p.Password.RequireUppercase = true;
    p.Password.RequiredLength = 6;

}). AddEntityFrameworkStores<EcomDbContext>()
    .AddDefaultTokenProviders();



builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
//swagger

builder.Services.AddSwaggerGen(e =>
{ e.
SwaggerDoc("v1", new OpenApiInfo
{
    Title = "EcommDemo API",
    Version = "v1"
});
e.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
{
    Name = "Authorization",
    Type = SecuritySchemeType.Http,
    Scheme = "Bearer",
    BearerFormat = "JWT",
    In = ParameterLocation.Header,
    Description = "Enter: **Bearer {your JWT token}**"
});

e.AddSecurityRequirement(new OpenApiSecurityRequirement
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

// JWT Auth
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ClockSkew = TimeSpan.FromMinutes(5),
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!))
    };
});




builder.Services.AddOpenApi();

var app = builder.Build();

// seeding some identity
using (var scope = app.Services.CreateScope())
{
    await IdentitySeeder.SeedRolesAdminAsync(scope.ServiceProvider);
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    //  app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();

}


app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();


app.Run();
