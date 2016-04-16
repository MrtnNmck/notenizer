using MongoDB.Driver;
using nsConstants;
using System;
using nsExtensions;

namespace nsDB
{
    public static class ConnectionManager
    {
        private static IMongoClient _client;
        private static IMongoDatabase _database;
        private static String _databaseName = null;

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
                    _database = Client.GetDatabase(DatabaseName.IsNullOrEmpty() ? DBConstants.DatabaseName : DatabaseName);

                return _database;
            }
        }

        public static String DatabaseName
        {
            get { return _databaseName; }
            set { _databaseName = value; }
        }
    }
}
