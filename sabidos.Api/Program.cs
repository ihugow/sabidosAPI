using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Security.Claims;
using sabidos.Domain.Interfaces;
using sabidos.Infrastructure.Repositories;
using sabidos.Application.Services;
using Supabase;

var builder = WebApplication.CreateBuilder(args);

// Configurando a autenticação JWT
var jwtSecret = builder.Configuration["Supabase:JwtSecret"];
var key = Encoding.UTF8.GetBytes(jwtSecret!); // Supabase usa UTF8 para o secret

builder.Services.AddAuthentication(x =>
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
        ValidateIssuer = false, // Supabase issuer varia, melhor manter false por enquanto
        ValidateAudience = true,
        ValidAudience = "authenticated", // Valor padrão do Supabase
        ValidateLifetime = true
    };
});

// Configurar o Swagger para aceitar o Token nos testes
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "sabidos.Api",
        Version = "v1"
    });

    // Adiciona o campo de "Authorize" no Swagger
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Por favor, insira o token JWT com 'Bearer ' antes dele",
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        BearerFormat = "JWT",
        Scheme = "Bearer"
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

// 1. Pegar as configurações do appsettings.json
var supabaseUrl = builder.Configuration["Supabase:Url"];
var supabaseKey = builder.Configuration["Supabase:Key"];

// 2. Registrar o Cliente do Supabase (Singleton - um para a API toda)
builder.Services.AddScoped(_ =>
    new Supabase.Client(supabaseUrl!, supabaseKey!, new SupabaseOptions
    {
        AutoConnectRealtime = true
    }));

// 3. Registrar nossas camadas (Injeção de Dependência)
builder.Services.AddScoped<IFlashcardRepository, SupabaseFlashcardRepository>();
builder.Services.AddScoped<FlashcardService>();

builder.Services.AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "sabidos.Api v1");
        c.RoutePrefix = string.Empty;
    });
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();
