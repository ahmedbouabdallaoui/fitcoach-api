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
        var secretKey = builder.Configuration["Jwt:SecretKey"];
        options.TokenValidationParameters = new()
        {
            ValidateIssuer = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidateAudience = true,
            ValidAudience = builder.Configuration["Jwt:Audience"],
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = string.IsNullOrWhiteSpace(secretKey)
                ? null
                : new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(secretKey))
        };
    });
builder.Services.AddHttpClient<IMLServiceClient, MLServiceClient>(client =>
{
    client.BaseAddress = new Uri(builder.Configuration["MLService:BaseUrl"]
                                 ?? "http://localhost:8087");
    client.Timeout = TimeSpan.FromSeconds(30);
    client.DefaultRequestHeaders.Add("X-Service-Token",
        builder.Configuration["MLService:ServiceKey"]);
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
builder.Services.AddScoped<IConversationService, ConversationService>();
builder.Services.AddScoped<IInjuryPredictionService, InjuryPredictionService>();
builder.Services.AddScoped<IRAGService, RAGService>();
builder.Services.AddScoped<IChatService, ChatService>();

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
var isRunningInContainer = string.Equals(
    Environment.GetEnvironmentVariable("DOTNET_RUNNING_IN_CONTAINER"),
    "true",
    StringComparison.OrdinalIgnoreCase);
var hasHttpsPortConfigured = !string.IsNullOrWhiteSpace(builder.Configuration["ASPNETCORE_HTTPS_PORT"]);

if (!isRunningInContainer || hasHttpsPortConfigured)
{
    app.UseHttpsRedirection();
}
app.UseAuthentication(); // 1. Identify the user from JWT
app.UseAuthorization();  // 2. Check what the user is allowed to do
app.MapControllers();

app.Run();
