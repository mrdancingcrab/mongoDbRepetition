using MongoDB.Driver;
using MongoDB.Driver.Linq;
using mongoDbRepetition.Models;

namespace mongoDbRepetition.Data
{
    public class MongoCrud
    {
        private IMongoDatabase db;

        public MongoCrud(string database)
        {
            var client = new MongoClient();
            db = client.GetDatabase(database);
        }

        //Add player
        public async Task<List<Players>> AddPlayer(string table, Players player)
        {
            var collection = db.GetCollection<Players>(table);
            await collection.InsertOneAsync(player);
            return collection.AsQueryable().ToList();
        }


        //Get all players
        public async Task<List<Players>> GetAllPlayers(string table)
        {
            var collection = db.GetCollection<Players>(table);
            var players = await collection.AsQueryable().ToListAsync();
            return players;
                
        }


        //Get player by id
        public async Task<Players> GetPlayerById(string table, string id)
        {
            var collection = db.GetCollection<Players>(table);
            var player = await collection.Find(x => x.Id == id).FirstOrDefaultAsync();
            return player;

        }


        //Update player by id
        public async Task<Players> UpdatePlayerById(string table, string id, Players updatedPlayer)
        {
            var collection = db.GetCollection<Players>(table);

            var existingPlayer = await collection.Find(x => x.Id == id).FirstOrDefaultAsync();
            if (existingPlayer == null)
            {
                return null;
            }
            else
            {
                var update = Builders<Players>.Update
                    .Set(x => x.FirstName, updatedPlayer.FirstName)
                    .Set(x => x.LastName, updatedPlayer.LastName)
                    .Set(x => x.Country, updatedPlayer.Country)
                    .Set(x => x.Position, updatedPlayer.Position)
                    .Set(x => x.Club, updatedPlayer.Club);

                await collection.UpdateOneAsync(x => x.Id == id, update);
                return await collection.Find(x => x.Id == id).FirstOrDefaultAsync();

            }
        }

        //Delete player
        public async Task<string> DeletePlayer(string table, string id)
        {
            var collection = db.GetCollection<Players>(table);
            var player = await collection.DeleteOneAsync(x => x.Id == id);
            return "Deleted Player";

        }
    }
}
