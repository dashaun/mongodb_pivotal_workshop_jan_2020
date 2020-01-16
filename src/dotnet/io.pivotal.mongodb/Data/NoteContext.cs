using Microsoft.Extensions.Options;
using MongoDB.Driver;
using io.pivotal.mongodb.Models;

namespace io.pivotal.mongodb.Data {
    public class NoteContext
    {
        private readonly IMongoDatabase _database = null;

        public NoteContext(IMongoClient mongoClient)
        {
            var client = mongoClient;
            if (client != null)
                _database = client.GetDatabase("NotesDb");
        }

        public IMongoCollection<Note> Notes
        {
            get
            {
                return _database.GetCollection<Note>("Note");
            }
        }
    }
}