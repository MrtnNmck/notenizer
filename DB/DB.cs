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

        /// <summary>
        /// Gets all documents from collection.
        /// </summary>
        /// <param name="collectionName">Name of collection</param>
        /// <returns>List of all documents</returns>
        public static async Task<List<BsonDocument>> GetAll(String collectionName)
        {
            FilterDefinition<BsonDocument> filter = Builders<BsonDocument>.Filter.Empty;

            return await GetAll(collectionName, filter);
        }

        /// <summary>
        /// Gets all documents which fulfill a filter condition.
        /// </summary>
        /// <param name="collectionName">Name of collection</param>
        /// <param name="filter">Filter condition</param>
        /// <returns>List of all documents which fulfill filter's conditions</returns>
        public static async Task<List<BsonDocument>> GetAll(String collectionName, FilterDefinition<BsonDocument> filter)
        {
            List<BsonDocument> results;
            IAsyncCursor<BsonDocument> cursor;
            IMongoCollection<BsonDocument> collection;

            try
            {
                collection = GetCollection(collectionName);
                cursor = await collection.FindAsync(filter);
                results = cursor.ToListAsync().Result;

                return results;
            }
            catch (Exception ex)
            {
                throw new Exception("Error getting data from collection." + Environment.NewLine + Environment.NewLine + ex.Message);
            }
        }

        /// <summary>
        /// Gets number of documents in collection.
        /// </summary>
        /// <param name="collectionName">Collection name</param>
        /// <returns></returns>
        public static async Task<long> GetCount(String collectionName)
        {
            try
            {
                return await GetCollection(collectionName).Find(Builders<BsonDocument>.Filter.Empty).CountAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Error getting number of documents in collection " + collectionName + Environment.NewLine + Environment.NewLine + ex.Message);
            }
        }

        /// <summary>
        /// Gets first documents from collections which fulfill filter conditions
        /// </summary>
        /// <param name="collectionName">Name of collections</param>
        /// <param name="filter">Filter conditions</param>
        /// <returns>First document or null</returns>
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
        /// <returns>ID of newly inserted document</returns>
        public static async Task<String> InsertToCollection(String collectionName, BsonDocument document)
        {
            IMongoCollection<BsonDocument> collection = GetCollection(collectionName);

            await collection.InsertOneAsync(document);

            return document["_id"].ToString();
        }

        /// <summary>
        /// Gets collection from database.
        /// </summary>
        /// <param name="collectionName">Collection name</param>
        /// <returns>Whole collection</returns>
        private static IMongoCollection<BsonDocument> GetCollection(String collectionName)
        {
            return ConnectionManager.Database.GetCollection<BsonDocument>(collectionName);
        }

        /// <summary>
        /// Replaces whole document in collection.
        /// Does not change id of replaced document.
        /// </summary>
        /// <param name="collectionName">Collection name</param>
        /// <param name="id">id of document</param>
        /// <param name="document">New document to replace the old one</param>
        /// <returns>ID of replaced document</returns>
        public static async Task<String> ReplaceInCollection(String collectionName, String id, BsonDocument document)
        {
            FilterDefinition<BsonDocument> filter;
            IMongoCollection<BsonDocument> collection;

            try
            {
                collection = GetCollection(collectionName);
                filter = Builders<BsonDocument>.Filter.Eq(DBConstants.IdFieldName, ObjectId.Parse(id));
                await collection.ReplaceOneAsync(filter, document);

                return id.ToString();
            }
            catch (Exception ex)
            {
                throw new Exception("Error replacing document in collection." + Environment.NewLine + Environment.NewLine + ex.Message);
            }
        }

        /// <summary>
        /// Updaes field in collection
        /// </summary>
        /// <param name="collectionName">Name of collections</param>
        /// <param name="id">ID of document in collections</param>
        /// <param name="fieldName">Name of field to update</param>
        /// <param name="fieldValue">Value to update field to</param>
        /// <returns>ID of updated document</returns>
        public static async Task<String> Update(String collectionName, String id, String fieldName, BsonValue fieldValue)
        {
            UpdateResult result;
            FilterDefinition<BsonDocument> filter;
            UpdateDefinition<BsonDocument> update;
            IMongoCollection<BsonDocument> collection;

            try
            {
                collection = GetCollection(collectionName);
                filter = Builders<BsonDocument>.Filter.Eq(DBConstants.IdFieldName, ObjectId.Parse(id));
                update = Builders<BsonDocument>.Update
                    .Set(fieldName, fieldValue)
                    .Set(DBConstants.UpdatedAtFieldName, new BsonDateTime(DateTime.Now));
                result = await collection.UpdateOneAsync(filter, update);

                return result.UpsertedId.ToString();
            }
            catch (Exception ex)
            {
                throw new Exception("Error updating field in database." + Environment.NewLine + Environment.NewLine + ex.Message);
            }
        }

        #endregion Methods
    }
}
