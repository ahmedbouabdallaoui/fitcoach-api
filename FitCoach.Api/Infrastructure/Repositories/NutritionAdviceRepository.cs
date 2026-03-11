using FitCoach.Api.Domain.Entities;
using FitCoach.Api.Infrastructure.MongoDB;
using FitCoach.Api.Infrastructure.Repositories.Interfaces;
using MongoDB.Driver;

namespace FitCoach.Api.Infrastructure.Repositories;

// Handles all MongoDB operations for nutrition advice.
// Implements INutritionAdviceRepository — services never touch this class directly.
public class NutritionAdviceRepository : INutritionAdviceRepository
{
    private readonly IMongoCollection<NutritionAdvice> _collection;

    // MongoDbContext injected — never instantiated with 'new'
    public NutritionAdviceRepository(MongoDbContext context)
    {
        // MongoDB creates this collection automatically on first insert
        _collection = context.GetCollection<NutritionAdvice>("nutrition_advice");
    }

    // Find one nutrition advice by its MongoDB ObjectId
    // Returns null if not found
    public async Task<NutritionAdvice?> GetByIdAsync(string id)
        => await _collection.Find(x => x.Id == id).FirstOrDefaultAsync();

    // Find all nutrition advice belonging to a user
    // Returns empty list if none found
    public async Task<List<NutritionAdvice>> GetByUserIdAsync(string userId)
        => await _collection.Find(x => x.UserId == userId).ToListAsync();

    // Insert a new nutrition advice document into MongoDB
    // Returns the same advice — now with a confirmed Id
    public async Task<NutritionAdvice> CreateAsync(NutritionAdvice advice)
    {
        await _collection.InsertOneAsync(advice);
        return advice;
    }
}