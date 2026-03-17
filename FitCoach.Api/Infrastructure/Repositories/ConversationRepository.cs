using FitCoach.Api.Domain.Entities;
using FitCoach.Api.Infrastructure.MongoDB;
using FitCoach.Api.Infrastructure.Repositories.Interfaces;
using MongoDB.Driver;

namespace FitCoach.Api.Infrastructure.Repositories;

// Handles all MongoDB operations for conversations.
// Implements IConversationRepository — services never touch this class directly.
public class ConversationRepository : IConversationRepository
{
    private readonly IMongoCollection<Conversation> _collection;

    // MongoDbContext is injected — we never create it with 'new' (Dependency Injection)
    public ConversationRepository(MongoDbContext context)
    {
        // Get the "conversations" collection from MongoDB
        // If it doesn't exist MongoDB creates it automatically on first insert
        _collection = context.GetCollection<Conversation>("conversations");
    }
    public async Task DeleteAsync(string id)
    {
        await _collection.DeleteOneAsync(c => c.Id == id);
    }

    // Find one conversation by its MongoDB ObjectId
    // Returns null if not found — caller decides what to do with null
    public async Task<Conversation?> GetByIdAsync(string id)
        => await _collection.Find(x => x.Id == id).FirstOrDefaultAsync();

    // Find all conversations belonging to a user
    // Returns empty list if none found — never returns null
    public async Task<List<Conversation>> GetByUserIdAsync(string userId)
        => await _collection.Find(x => x.UserId == userId).ToListAsync();

    // Insert a new conversation document into MongoDB
    // Returns the same conversation — now has a confirmed Id
    public async Task<Conversation> CreateAsync(Conversation conversation)
    {
        await _collection.InsertOneAsync(conversation);
        return conversation;
    }

    // Replace the entire conversation document in MongoDB
    // Used when messages are added — replaces old document with updated one
    public async Task UpdateAsync(Conversation conversation)
        => await _collection.ReplaceOneAsync(x => x.Id == conversation.Id, conversation);
}
