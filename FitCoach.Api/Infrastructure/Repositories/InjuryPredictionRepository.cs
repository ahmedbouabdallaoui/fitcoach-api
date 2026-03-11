using FitCoach.Api.Domain.Entities;
using FitCoach.Api.Infrastructure.MongoDB;
using FitCoach.Api.Infrastructure.Repositories.Interfaces;
using MongoDB.Driver;

namespace FitCoach.Api.Infrastructure.Repositories;

// Handles all MongoDB operations for injury predictions.
// Implements IInjuryPredictionRepository — services never touch this class directly.
public class InjuryPredictionRepository : IInjuryPredictionRepository
{
    private readonly IMongoCollection<InjuryPrediction> _collection;

    // MongoDbContext injected — never instantiated with 'new'
    public InjuryPredictionRepository(MongoDbContext context)
    {
        // MongoDB creates this collection automatically on first insert
        _collection = context.GetCollection<InjuryPrediction>("injury_predictions");
    }

    // Find one injury prediction by its MongoDB ObjectId
    // Returns null if not found
    public async Task<InjuryPrediction?> GetByIdAsync(string id)
        => await _collection.Find(x => x.Id == id).FirstOrDefaultAsync();

    // Find all injury predictions belonging to a user
    // Returns empty list if none found
    public async Task<List<InjuryPrediction>> GetByUserIdAsync(string userId)
        => await _collection.Find(x => x.UserId == userId).ToListAsync();

    // Insert a new injury prediction document into MongoDB
    // Returns the same prediction — now with a confirmed Id
    public async Task<InjuryPrediction> CreateAsync(InjuryPrediction prediction)
    {
        await _collection.InsertOneAsync(prediction);
        return prediction;
    }
}