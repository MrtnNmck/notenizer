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
        private static String _hostName = null;
        private static int _port;
        private static String _password;
        private static String _userName;

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
                {
                    if (_password.IsNullOrEmpty() || _userName.IsNullOrEmpty())
                        _client = new MongoClient(String.Format(@"mongodb://{0}:{1}/{2}", _hostName, _port, DatabaseName));
                    else
                        _client = new MongoClient(String.Format(@"mongodb://{0}:{1}@{2}:{3}/{4}", _userName, _password, _hostName, _port, DatabaseName));
                }

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
                        _database = Client.GetDatabase(DatabaseName);

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
            get { return _databaseName.IsNullOrEmpty() ? DBConstants.DatabaseName : _databaseName; }
            set { _databaseName = value; }
        }

        /// <summary>
        /// Host name.
        /// </summary>
        public static String HostName
        {
            get { return _hostName; }
            set { _hostName = value; }
        }

        /// <summary>
        /// Password of database user.
        /// </summary>
        public static String Password
        {
            get { return _password; }
            set { _password = value; }
        }

        /// <summary>
        /// Port of host.
        /// </summary>
        public static int Port
        {
            get { return _port; }
            set { _port = value; }
        }

        /// <summary>
        /// Name of database user.
        /// </summary>
        public static String UserName
        {
            get { return _userName; }
            set { _userName = value; }
        }

        #endregion Properties

        #region Methods

        #endregion Methods
    }
}
