﻿using Inventory.API.Entities.Abstraction;
using Inventory.API.Extensions;
using MongoDB.Driver;
using System.Linq.Expressions;

namespace Inventory.API.Repositories.Abstraction
{
    public class MongoDbRepository<T> : IMongoDbRepositoryBase<T> where T : MongoEntity
    {
        private IMongoDatabase Database { get; }

        public MongoDbRepository(IMongoClient mongoClient, DatabaseSettings settings)
        {
            Database = mongoClient.GetDatabase(settings.DatabaseName);
        }

        public Task CreateAsync(T entity) => Collection.InsertOneAsync(entity);

        public Task DeleteAsync(string id) => Collection.DeleteOneAsync(x => x.Id.Equals(id));

        public IMongoCollection<T> FindAll(ReadPreference? readPreference = null)
            => Database.WithReadPreference(readPreference ?? ReadPreference.Primary)
            .GetCollection<T>(GetCollectionName());

        public Task UpdateAsync(T entity)
        {
            Expression<Func<T, string>> func = f => f.Id;
            var value = (string)entity.GetType()
                .GetProperty(func.Body.ToString().Split(".")[1])?.GetValue(entity, null);
            var filter = Builders<T>.Filter.Eq(func, value);

            return Collection.ReplaceOneAsync(filter, entity);
        }

        protected virtual IMongoCollection<T> Collection =>
            Database.GetCollection<T>(GetCollectionName());

        private static string GetCollectionName()
        {
            return (typeof(T).GetCustomAttributes(typeof(BsonCollectionAttribute), true)
                .FirstOrDefault() as BsonCollectionAttribute)?.CollectionName;
        }
    }
}
