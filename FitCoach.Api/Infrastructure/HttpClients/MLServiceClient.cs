using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using FitCoach.Api.Domain.Entities;
using FitCoach.Api.DTOs.Requests;
using FitCoach.Api.Infrastructure.HttpClients.Interfaces;
using FitCoach.Api.Infrastructure.HttpClients.Responses;
using FitCoach.Api.Mappers;
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

    public async Task<GoalValidationMLResponse> ValidateGoalAsync(UserProfile profile, string goal)
    {
        return await PostAsync<GoalValidationMLResponse>(
            "/predict/validate-goal",
            MLPayloadMapper.ToGoalValidationPayload(profile, goal)
        );
    }

    public async Task<TrainingPlanMLResponse> PredictTrainingPlanAsync(UserProfile profile, ConversationContext context)
    {
        return await PostAsync<TrainingPlanMLResponse>(
            "/predict/training-plan",
            MLPayloadMapper.ToTrainingPlanPayload(profile, context)
        );
    }

    public async Task<InjuryPredictionMLResponse> PredictInjuryRiskAsync(UserProfile profile, ConversationContext context)
    {
        return await PostAsync<InjuryPredictionMLResponse>(
            "/predict/injury-risk",
            MLPayloadMapper.ToInjuryPredictionPayload(profile, context)
        );
    }

    public async Task<NutritionMLResponse> CalculateNutritionAsync(UserProfile profile, ConversationContext context)
    {
        return await PostAsync<NutritionMLResponse>(
            "/predict/nutrition",
            MLPayloadMapper.ToNutritionPayload(profile, context)
        );
    }
    private async Task<T> PostAsync<T>(string endpoint, object payload)
    {
        try
        {
            var json = JsonSerializer.Serialize(payload);
            var encrypted = _encryptionService.Encrypt(json);

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