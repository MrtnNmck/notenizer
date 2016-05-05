using MongoDB.Driver;
using nsConstants;
using System;
using nsExtensions;

namespace nsDB
{
    /// <summary>
    /// Manages connections to database.
    /// </summary>
    public static class ConnectionManager
    {
        #region Variables

        private static IMongoClient _client;
        private static IMongoDatabase _database;
        private static String _databaseName = null;

        #endregion Variables

        #region Properties

        /// <summary>
        /// Mongo client.
        /// </summary>
        public static IMongoClient Client
        {
            get
            {
                if (_client == null)
                    _client = new MongoClient();

                return _client;
            }
        }

        /// <summary>
        /// Mongo database which is the Notenizer connected to.
        /// </summary>
        public static IMongoDatabase Database
        {
            get
            {
                try
                {
                    if (_database == null)
                        _database = Client.GetDatabase(DatabaseName.IsNullOrEmpty() ? DBConstants.DatabaseName : DatabaseName);

                    return _database;
                }
                catch (Exception ex)
                {
                    throw new Exception("Failed getting database." + Environment.NewLine + Environment.NewLine + ex.Message);
                }
            }
        }

        /// <summary>
        /// Name of database to connect to.
        /// </summary>
        public static String DatabaseName
        {
            get { return _databaseName; }
            set { _databaseName = value; }
        }

        #endregion Properties

        #region Methods

        #endregion Methods
    }
}
