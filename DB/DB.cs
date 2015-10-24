using MongoDB.Bson;
using MongoDB.Driver;
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
        public static async Task<String> GetAll(String collectionName)
        {
            BsonDocument filter = new BsonDocument();

           return await GetAll(collectionName, filter);
        }

        public static async Task<String> GetAll(String collectionName, BsonDocument filter)
        {
            IMongoCollection<BsonDocument> collection = GetCollection(collectionName);
            
            IAsyncCursor<BsonDocument> cursor = await collection.FindAsync(filter);

            // iterate over cursor, to get objects http://docs.mongodb.org/getting-started/csharp/query/

            return "OK.";
        }

        public static async Task<String> GetAll(String collectionName, FilterDefinition<BsonDocument> filter)
        {
            IMongoCollection<BsonDocument> collection = GetCollection(collectionName);

            IAsyncCursor<BsonDocument> cursor = await collection.FindAsync(filter);

            // iterate over cursor, to get objects http://docs.mongodb.org/getting-started/csharp/query/

            return "OK.";
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
    }
}
