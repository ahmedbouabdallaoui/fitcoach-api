using FitCoach.Api.Infrastructure.MongoDB;
using FitCoach.Api.Security;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// --- Controllers & API Docs ---
builder.Services.AddControllers();
builder.Services.AddOpenApi();

// --- JWT Authentication ---
// TEMPORARY: JWT validation is handled here until the YARP API Gateway is implemented.
// Once the gateway is in place, remove this block — the gateway will validate tokens
// and forward trusted requests directly to this service.
builder.Services.AddAuthentication(defaultScheme: JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.Authority = builder.Configuration["Jwt:Issuer"];
        options.TokenValidationParameters = new()
        {
            ValidateIssuer = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidateAudience = false,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true
        };
    });

builder.Services.AddAuthorization();
builder.Services.AddSingleton<EncryptionService>();
//"It is recommended to store a MongoClient instance in a global place,
//either as a static variable or in an IoC container with a singleton lifetime."//

builder.Services.AddSingleton<MongoDbContext>();

// ---------------------------------------------------
var app = builder.Build();
// ---------------------------------------------------

// --- API Docs (Development only) ---
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference(options =>
    {
        options.Title = "FitCoach API";
        options.Theme = ScalarTheme.DeepSpace;
    });
}

// --- Middleware Pipeline (order matters!) ---
app.UseHttpsRedirection();
app.UseAuthentication(); // 1. Identify the user from JWT
app.UseAuthorization();  // 2. Check what the user is allowed to do
app.MapControllers();

app.Run();