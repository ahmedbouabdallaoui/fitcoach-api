using MongoDB.Driver;

namespace FitCoach.Api.Infrastructure.MongoDB;

// The single entry point for all MongoDB operations.
// Repositories use this to get their collections — nothing else connects to MongoDB directly.
public class MongoDbContext
{
    private readonly IMongoDatabase _database;

    public MongoDbContext(IConfiguration configuration)
    {
        var connectionString = configuration["MongoDB:ConnectionString"]
                               ?? throw new InvalidOperationException("MongoDB:ConnectionString is missing.");

        var databaseName = configuration["MongoDB:DatabaseName"]
                           ?? throw new InvalidOperationException("MongoDB:DatabaseName is missing.");

        var client = new MongoClient(connectionString);
        _database = client.GetDatabase(databaseName);
    }

    // Returns a typed collection — repositories call this to get their collection
    public IMongoCollection<T> GetCollection<T>(string collectionName)
        => _database.GetCollection<T>(collectionName);
}