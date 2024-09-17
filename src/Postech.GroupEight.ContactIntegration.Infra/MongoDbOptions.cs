namespace Postech.GroupEight.ContactIntegration.Infra
{
    /// <summary>
    /// Represents the options for connecting to MongoDB.
    /// </summary>
    public class MongoDbOptions
    {
        /// <summary>
        /// The connection string for MongoDB.
        /// </summary>
        public string ConnectionString { get; set; }

        /// <summary>
        /// The name of the database in MongoDB.
        /// </summary>
        public string Database { get; set; }
    }
}
