using MongoDB.Driver;
using nsConstants;

namespace nsDB
{
    public static class ConnectionManager
    {
        private static IMongoClient _client;
        private static IMongoDatabase _database;

        public static IMongoClient Client
        {
            get
            {
                if (_client == null)
                    _client = new MongoClient();

                return _client;
            }
        }

        public static IMongoDatabase Database
        {
            get
            {
                if (_database == null)
                    _database = Client.GetDatabase(DBConstants.DatabaseName);

                return _database;
            }
        }
    }
}
