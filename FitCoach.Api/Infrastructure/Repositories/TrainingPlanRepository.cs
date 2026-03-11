using FitCoach.Api.Domain.Entities;
using FitCoach.Api.Infrastructure.MongoDB;
using FitCoach.Api.Infrastructure.Repositories.Interfaces;
using MongoDB.Driver;

namespace FitCoach.Api.Infrastructure.Repositories;

// Handles all MongoDB operations for training plans.
// Implements ITrainingPlanRepository — services never touch this class directly.
public class TrainingPlanRepository : ITrainingPlanRepository
{
    private readonly IMongoCollection<TrainingPlan> _collection;

    // MongoDbContext injected — never instantiated with 'new'
    public TrainingPlanRepository(MongoDbContext context)
    {
        // MongoDB creates this collection automatically on first insert
        _collection = context.GetCollection<TrainingPlan>("training_plans");
    }

    // Find one training plan by its MongoDB ObjectId
    // Returns null if not found
    public async Task<TrainingPlan?> GetByIdAsync(string id)
        => await _collection.Find(x => x.Id == id).FirstOrDefaultAsync();

    // Find all training plans belonging to a user
    // Returns empty list if none found
    public async Task<List<TrainingPlan>> GetByUserIdAsync(string userId)
        => await _collection.Find(x => x.UserId == userId).ToListAsync();

    // Insert a new training plan document into MongoDB
    // Returns the same plan — now with a confirmed Id
    public async Task<TrainingPlan> CreateAsync(TrainingPlan plan)
    {
        await _collection.InsertOneAsync(plan);
        return plan;
    }
}