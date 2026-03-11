using FitCoach.Api.Domain.Entities;
using FitCoach.Api.Infrastructure.MongoDB;
using FitCoach.Api.Infrastructure.Repositories.Interfaces;
using MongoDB.Driver;

namespace FitCoach.Api.Infrastructure.Repositories;

// Handles all MongoDB operations for equipment recommendations.
// Implements IEquipmentRecommendationRepository — services never touch this class directly.
public class EquipmentRecommendationRepository : IEquipmentRecommendationRepository
{
    private readonly IMongoCollection<EquipmentRecommendation> _collection;

    // MongoDbContext injected — never instantiated with 'new'
    public EquipmentRecommendationRepository(MongoDbContext context)
    {
        // MongoDB creates this collection automatically on first insert
        _collection = context.GetCollection<EquipmentRecommendation>("equipment_recommendations");
    }

    // Find all equipment recommendations belonging to a user
    // Returns empty list if none found
    public async Task<List<EquipmentRecommendation>> GetByUserIdAsync(string userId)
        => await _collection.Find(x => x.UserId == userId).ToListAsync();

    // Insert a new equipment recommendation document into MongoDB
    // Returns the same recommendation — now with a confirmed Id
    public async Task<EquipmentRecommendation> CreateAsync(EquipmentRecommendation recommendation)
    {
        await _collection.InsertOneAsync(recommendation);
        return recommendation;
    }
}