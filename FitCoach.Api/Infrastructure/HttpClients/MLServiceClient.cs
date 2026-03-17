using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using FitCoach.Api.DTOs.Requests;
using FitCoach.Api.Infrastructure.HttpClients.Interfaces;
using FitCoach.Api.Infrastructure.HttpClients.Responses;
using FitCoach.Api.Security;

namespace FitCoach.Api.Infrastructure.HttpClients;

public class MLServiceClient : IMLServiceClient
{
    private readonly HttpClient _httpClient;
    private readonly EncryptionService _encryptionService;
    private readonly ILogger<MLServiceClient> _logger;

    // JSON options — match Python snake_case responses
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    public MLServiceClient(
        HttpClient httpClient,
        EncryptionService encryptionService,
        ILogger<MLServiceClient> logger)
    {
        _httpClient = httpClient;
        _encryptionService = encryptionService;
        _logger = logger;
    }

    public async Task<GoalValidationMLResponse> ValidateGoalAsync(
        GenerateTrainingPlanRequest request, string userId)
    {
        var payload = new
        {
            user_id = userId,
            age = request.Age,
            weight_kg = request.WeightKg,
            height_cm = request.HeightCm,
            gender = request.Gender,
            fitness_level = request.FitnessLevel,
            goal = request.Goal
        };

        return await PostAsync<GoalValidationMLResponse>("/predict/validate-goal", payload);
    }

    public async Task<TrainingPlanMLResponse> PredictTrainingPlanAsync(
        GenerateTrainingPlanRequest request, string userId)
    {
        var payload = new
        {
            user_id = userId,
            age = request.Age,
            weight_kg = request.WeightKg,
            height_cm = request.HeightCm,
            gender = request.Gender,
            fitness_level = request.FitnessLevel,
            goal = request.Goal,
            days_per_week = request.DaysPerWeek,
            duration_weeks = request.DurationWeeks
        };

        return await PostAsync<TrainingPlanMLResponse>("/predict/training-plan", payload);
    }

    public async Task<InjuryPredictionMLResponse> PredictInjuryRiskAsync(
        InjuryPredictionRequest request, string userId)
    {
        var payload = new
        {
            user_id = userId,
            age = request.Age,
            weight_kg = request.WeightKg,
            height_cm = request.HeightCm,
            weekly_training_hours = request.WeeklyTrainingHours,
            fitness_level = request.FitnessLevel
        };

        return await PostAsync<InjuryPredictionMLResponse>("/predict/injury-risk", payload);
    }

    public async Task<NutritionMLResponse> CalculateNutritionAsync(
        GenerateNutritionAdviceRequest request, string userId)
    {
        var payload = new
        {
            user_id = userId,
            age = request.Age,
            weight_kg = request.WeightKg,
            height_cm = request.HeightCm,
            gender = request.Gender,
            activity_level = request.ActivityLevel,
            goal = request.Goal
        };

        return await PostAsync<NutritionMLResponse>("/predict/nutrition", payload);
    }

    // --- Generic POST method — encrypt, send, deserialize ---
    private async Task<T> PostAsync<T>(string endpoint, object payload)
    {
        try
        {
            // Serialize payload to JSON
            var json = JsonSerializer.Serialize(payload);

            // Encrypt payload before sending to Python
            var encrypted = _encryptionService.Encrypt(json);

            // Build request body
            var content = new StringContent(
                JsonSerializer.Serialize(new { data = encrypted }),
                Encoding.UTF8,
                "application/json"
            );

            _logger.LogInformation("Calling ML service: {Endpoint}", endpoint);

            var response = await _httpClient.PostAsync(endpoint, content);

            response.EnsureSuccessStatusCode();

            var responseJson = await response.Content.ReadAsStringAsync();

            var result = JsonSerializer.Deserialize<T>(responseJson, JsonOptions);

            if (result == null)
                throw new Exception($"ML service returned null response for {endpoint}");

            return result;
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "ML service unreachable: {Endpoint}", endpoint);
            throw new Exception($"ML service unavailable. Please try again later.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "ML service error: {Endpoint}", endpoint);
            throw;
        }
    }
}