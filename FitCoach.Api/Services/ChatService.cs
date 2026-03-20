using FitCoach.Api.Domain.Entities;
using FitCoach.Api.DTOs.Requests;
using FitCoach.Api.DTOs.Responses;
using FitCoach.Api.Infrastructure.Repositories.Interfaces;
using FitCoach.Api.Mappers;
using FitCoach.Api.Services.Interfaces;

namespace FitCoach.Api.Services;

public class ChatService : IChatService
{
    private readonly IConversationRepository _conversationRepository;
    private readonly IProfileService _profileService;
    private readonly IProfileCompletenessChecker _profileChecker;
    private readonly IGroqService _groqService;
    private readonly ITrainingPlanService _trainingPlanService;
    private readonly INutritionService _nutritionService;
    private readonly IInjuryPredictionService _injuryPredictionService;
    private readonly IRAGService _ragService;
    private readonly ILogger<ChatService> _logger;

    public ChatService(
        IConversationRepository conversationRepository,
        IProfileService profileService,
        IProfileCompletenessChecker profileChecker,
        IGroqService groqService,
        ITrainingPlanService trainingPlanService,
        INutritionService nutritionService,
        IInjuryPredictionService injuryPredictionService,
        IRAGService ragService,
        ILogger<ChatService> logger)
    {
        _conversationRepository = conversationRepository;
        _profileService = profileService;
        _profileChecker = profileChecker;
        _groqService = groqService;
        _trainingPlanService = trainingPlanService;
        _nutritionService = nutritionService;
        _injuryPredictionService = injuryPredictionService;
        _ragService = ragService;
        _logger = logger;
    }

    public async Task<ChatResponse> ProcessMessageAsync(
        ChatRequest request,
        string userId,
        string userName)
    {
        // --- Step 1: Get or create conversation ---
        var conversation = await GetOrCreateConversationAsync(request, userId);

        // --- Step 2: Save user message ---
        conversation.Messages.Add(new Message
        {
            Role = "user",
            Content = request.Message,
            Tag = request.Tag,
            Timestamp = DateTime.UtcNow
        });

        // --- Step 3: Load profile ---
        var profile = await _profileService.GetOrCreateAsync(userId);

        // --- Step 4: Check profile completeness ---
        var missingProfileFields = _profileChecker.GetMissingProfileFields(profile);

        if (missingProfileFields.Count > 0)
        {
            profile = await _profileService.UpdateFromMessageAsync(profile, request.Message);
            await _profileService.SaveAsync(profile);

            missingProfileFields = _profileChecker.GetMissingProfileFields(profile);

            if (missingProfileFields.Count > 0)
            {
                var question = await _groqService.GenerateProfileQuestionAsync(
                    userName, missingProfileFields
                );
                return await SaveAndReturnAsync(conversation, question, "question", false);
            }
        }

        // --- Step 5: No tag = RAG chat ---
        if (string.IsNullOrEmpty(request.Tag))
        {
            var response = await _ragService.GenerateAsync(conversation, profile, request.Message);
            return await SaveAndReturnAsync(conversation, response.Message, "chat", false);
        }

        // --- Step 6: Check conversation context ---
        var missingContextFields = _profileChecker.GetMissingContextFields(
            conversation.Context, request.Tag
        );

        if (missingContextFields.Count > 0)
        {
            var extracted = await _groqService.ExtractContextDataAsync(
                request.Message, missingContextFields, request.Tag
            );
            ConversationContextMapper.UpdateFromExtracted(conversation.Context, extracted);
            await _conversationRepository.UpdateAsync(conversation);

            missingContextFields = _profileChecker.GetMissingContextFields(
                conversation.Context, request.Tag
            );

            if (missingContextFields.Count > 0)
            {
                var question = await _groqService.GenerateContextQuestionAsync(
                    userName, missingContextFields, request.Tag
                );
                return await SaveAndReturnAsync(conversation, question, "question", false);
            }
        }

        // --- Step 7: Route to correct service ---
        var result = request.Tag.ToLower() switch
        {
            "training" => await _trainingPlanService.GenerateAsync(conversation, profile, userName),
            "nutrition" => await _nutritionService.GenerateAsync(conversation, profile, userName),
            "injury"   => await _injuryPredictionService.GenerateAsync(conversation, profile, userId, userName),
            _          => await _ragService.GenerateAsync(conversation, profile, request.Message)
        };

        return await SaveAndReturnAsync(conversation, result.Message, result.MessageType, result.IsGenerating);
    }

    private async Task<Conversation> GetOrCreateConversationAsync(ChatRequest request, string userId)
    {
        if (!string.IsNullOrEmpty(request.ConversationId))
        {
            var existing = await _conversationRepository.GetByIdAsync(request.ConversationId);
            if (existing != null && existing.UserId == userId)
                return existing;
        }

        var conversation = new Conversation
        {
            UserId = userId,
            Title = "New Conversation",
            CreatedAt = DateTime.UtcNow,
            Messages = new List<Message>(),
            Context = new ConversationContext()
        };

        await _conversationRepository.CreateAsync(conversation);
        return conversation;
    }

    private async Task<ChatResponse> SaveAndReturnAsync(
        Conversation conversation,
        string assistantMessage,
        string messageType,
        bool isGenerating)
    {
        conversation.Messages.Add(new Message
        {
            Role = "assistant",
            Content = assistantMessage,
            Timestamp = DateTime.UtcNow
        });

        conversation.UpdatedAt = DateTime.UtcNow;
        await _conversationRepository.UpdateAsync(conversation);

        return new ChatResponse
        {
            ConversationId = conversation.Id,
            Message = assistantMessage,
            MessageType = messageType,
            IsGenerating = isGenerating
        };
    }
}