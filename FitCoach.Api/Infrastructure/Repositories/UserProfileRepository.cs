using FitCoach.Api.Domain.Entities;
using FitCoach.Api.Infrastructure.MongoDB;
using FitCoach.Api.Infrastructure.Repositories.Interfaces;
using MongoDB.Driver;

namespace FitCoach.Api.Infrastructure.Repositories;

public class UserProfileRepository : IUserProfileRepository
{
    private readonly IMongoCollection<UserProfile> _collection;

    public UserProfileRepository(MongoDbContext context)
    {
        _collection = context.GetCollection<UserProfile>("user_profiles");
    }

    public async Task<UserProfile?> GetByUserIdAsync(string userId)
    {
        return await _collection
            .Find(p => p.UserId == userId)
            .FirstOrDefaultAsync();
    }

    public async Task CreateAsync(UserProfile profile)
    {
        await _collection.InsertOneAsync(profile);
    }

    public async Task UpdateAsync(UserProfile profile)
    {
        await _collection.ReplaceOneAsync(p => p.Id == profile.Id, profile);
    }
}