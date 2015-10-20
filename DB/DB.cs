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
            var collection = ConnectionManager.Database.GetCollection<BsonDocument>(collectionName);

            var cursor = await collection.FindAsync(filter);

            // iterate over cursor, to get objects http://docs.mongodb.org/getting-started/csharp/query/

            return "OK.";
        }
    }
}
