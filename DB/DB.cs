using MongoDB.Bson;
using MongoDB.Driver;
using nsConstants;
using nsNotenizerObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nsDB
{
    public static class DB
    {
        #region Variables

        #endregion Variables

        #region Properties

        #endregion Properties

        #region Methods

        public static async Task<List<BsonDocument>> GetAll(String collectionName)
        {
            FilterDefinition<BsonDocument> filter = Builders<BsonDocument>.Filter.Empty;

            return await GetAll(collectionName, filter);
        }

        public static async Task<List<BsonDocument>> GetAll(String collectionName, FilterDefinition<BsonDocument> filter)
        {
            IMongoCollection<BsonDocument> collection = GetCollection(collectionName);

            IAsyncCursor<BsonDocument> cursor = await collection.FindAsync(filter);

            List<BsonDocument> results = cursor.ToListAsync().Result;

            return results;
        }

        public static async Task<BsonDocument> GetFirst(String collectionName, FilterDefinition<BsonDocument> filter)
        {
            List<BsonDocument> all = await GetAll(collectionName, filter);
            return all.FirstOrDefault();
        }

        /// <summary>
        /// Inserts one document into collection.
        /// </summary>
        /// <param name="collectionName">Name of the collection to insert into</param>
        /// <param name="document">Document to be inserted</param>
        /// <returns>ID (_id) of newly inserted document</returns>
        public static async Task<String> InsertToCollection(String collectionName, BsonDocument document)
        {
            IMongoCollection<BsonDocument> collection = GetCollection(collectionName);

            await collection.InsertOneAsync(document);

            return document["_id"].ToString();
        }

        private static IMongoCollection<BsonDocument> GetCollection(String collectionName)
        {
            return ConnectionManager.Database.GetCollection<BsonDocument>(collectionName);
        }

        public static async Task<String> ReplaceInCollection(String collectionName, String id, BsonDocument document)
        {
            IMongoCollection<BsonDocument> collection = GetCollection(collectionName);

            FilterDefinition<BsonDocument> filter = Builders<BsonDocument>.Filter.Eq(DBConstants.IdFieldName, ObjectId.Parse(id));

            await collection.ReplaceOneAsync(filter, document);

            return id.ToString();
        }

        public static async Task<String> Update(String collectionName, String id, String fieldName, BsonValue fieldValue)
        {
            IMongoCollection<BsonDocument> collection = GetCollection(collectionName);
            FilterDefinition<BsonDocument> filter = Builders<BsonDocument>.Filter.Eq(DBConstants.IdFieldName, ObjectId.Parse(id));
            UpdateDefinition<BsonDocument> update = Builders<BsonDocument>.Update
                .Set(fieldName, fieldValue)
                .Set(DBConstants.UpdatedAtFieldName, new BsonDateTime(DateTime.Now));

            UpdateResult result = await collection.UpdateOneAsync(filter, update);

            return result.UpsertedId.ToString();
        }

        #endregion Methods
    }
}
