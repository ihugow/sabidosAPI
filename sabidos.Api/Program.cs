using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using sabidos.Domain.Interfaces;
using sabidos.Infrastructure.Repositories;
using sabidos.Application.Services;
using Google.Cloud.Firestore;

var builder = WebApplication.CreateBuilder(args);

// Configurando a autentica��o JWT com o Firebase
var firebaseProjectId = builder.Configuration["Firebase:ProjectId"];

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
.AddJwtBearer(options =>
{
    options.Authority = $"https://securetoken.google.com/{firebaseProjectId}";
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidIssuer = $"https://securetoken.google.com/{firebaseProjectId}",
        ValidateAudience = true,
        ValidAudience = firebaseProjectId,
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

// Configurar credenciais automaticamente a partir do appsettings.json
var credentialsPath = builder.Configuration["Firebase:CredentialsPath"];
if (!string.IsNullOrEmpty(credentialsPath) && System.IO.File.Exists(credentialsPath))
{
    Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", credentialsPath);
}

// Registrar o FirestoreDb
// A biblioteca do Google puxar� automaticamente as credenciais se a vari�vel de ambiente GOOGLE_APPLICATION_CREDENTIALS estiver configurada.
builder.Services.AddSingleton(provider => FirestoreDb.Create(firebaseProjectId));

// 3. Registrar nossas camadas (Inje��o de Depend�ncia)
builder.Services.AddScoped<IFlashcardRepository, FirestoreFlashcardRepository>();
builder.Services.AddScoped<IFlashcardCollectionRepository, FirestoreFlashcardCollectionRepository>();
builder.Services.AddScoped<FlashcardService>();
builder.Services.AddScoped<FlashcardCollectionService>();
builder.Services.AddScoped<PointService>();
builder.Services.AddScoped<AchievementService>();
builder.Services.AddScoped<PointRepository>();
builder.Services.AddScoped<FirebaseService>();

builder.Services.AddControllers();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "sabidos.Api v1");
        c.RoutePrefix = string.Empty;
    });
}

// app.UseHttpsRedirection(); // Desativado para o Emulador Android (evitar Erro 307)
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();



