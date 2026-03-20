using FitCoach.Api.Infrastructure.HttpClients;
using FitCoach.Api.Infrastructure.HttpClients.Interfaces;
using FitCoach.Api.Infrastructure.Messaging;
using FitCoach.Api.Infrastructure.MongoDB;
using FitCoach.Api.Infrastructure.Repositories;
using FitCoach.Api.Infrastructure.Repositories.Interfaces;
using FitCoach.Api.Security;
using FitCoach.Api.Services;
using FitCoach.Api.Services.Interfaces;
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
builder.Services.AddHttpClient<IMLServiceClient, MLServiceClient>(client =>
{
    client.BaseAddress = new Uri(builder.Configuration["MLService:BaseUrl"]
                                 ?? "http://localhost:8087");
    client.Timeout = TimeSpan.FromSeconds(30);
    client.DefaultRequestHeaders.Add("X-Service-Token",
        builder.Configuration["MLService:ServiceToken"]);
});


builder.Services.AddAuthorization();
//"It is recommended to store a MongoClient instance in a global place,
//either as a static variable or in an IoC container with a singleton lifetime."//
builder.Services.AddSingleton<MongoDbContext>();
builder.Services.AddSingleton<IInjuryAlertPublisher, InjuryAlertPublisher>();
builder.Services.AddSingleton<IProfileCompletenessChecker, ProfileCompletenessChecker>();
builder.Services.AddSingleton<EncryptionService>();

builder.Services.AddScoped<IConversationRepository, ConversationRepository>();
builder.Services.AddScoped<ITrainingPlanRepository, TrainingPlanRepository>();
builder.Services.AddScoped<INutritionAdviceRepository, NutritionAdviceRepository>();
builder.Services.AddScoped<IInjuryPredictionRepository, InjuryPredictionRepository>();
builder.Services.AddScoped<IEquipmentRecommendationRepository, EquipmentRecommendationRepository>();
builder.Services.AddScoped<IUserProfileRepository, UserProfileRepository>();
builder.Services.AddScoped<IGroqService, GroqService>();
builder.Services.AddScoped<IProfileService, ProfileService>();
builder.Services.AddScoped<ITrainingPlanService, TrainingPlanService>();
builder.Services.AddScoped<INutritionService, NutritionService>();
builder.Services.AddScoped<IInjuryPredictionService, InjuryPredictionService>();
builder.Services.AddScoped<IRAGService, RAGService>();

var app = builder.Build();

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

// --- Middleware Pipeline ---
app.UseHttpsRedirection();
app.UseAuthentication(); // 1. Identify the user from JWT
app.UseAuthorization();  // 2. Check what the user is allowed to do
app.MapControllers();

app.Run();