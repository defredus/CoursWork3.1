using DAL.Interfaces;
using DAL.Models;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DAL.Repositories.MongoRep
{
    public class MongoSalesVolumeRepository : ISalesVolumeRepository
    {
        private readonly IMongoCollection<SalesVolume> _salesVolume;

        public MongoSalesVolumeRepository(string connectionString)
        {
            var client = new MongoClient(connectionString);
            var database = client.GetDatabase("test");
            _salesVolume = database.GetCollection<SalesVolume>("SalesVolumes");
        }

        // Add a new SalesVolume to MongoDB
        public void Add(SalesVolume salesVolume)
        {
            try
            {
                _salesVolume.InsertOne(salesVolume);
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred while adding the sales volume: " + ex.Message);
            }
        }

        // Delete a SalesVolume by its ID from MongoDB
        public void Delete(int id)
        {
            try
            {
                var filter = Builders<SalesVolume>.Filter.Eq(s => s.Id, id);
                _salesVolume.DeleteOne(filter);
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred while deleting the sales volume: " + ex.Message);
            }
        }

        // Retrieve all SalesVolumes from MongoDB
        public IEnumerable<SalesVolume> GetAll()
        {
            try
            {
                return _salesVolume.Find(_ => true).ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred while fetching the sales volumes: " + ex.Message);
                return new List<SalesVolume>();
            }
        }

        // Retrieve a SalesVolume by its ID from MongoDB
        public SalesVolume GetById(int id)
        {
            try
            {
                var filter = Builders<SalesVolume>.Filter.Eq(s => s.Id, id);
                return _salesVolume.Find(filter).FirstOrDefault();
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred while fetching the sales volume: " + ex.Message);
                return null;
            }
        }

        // Update an existing SalesVolume in MongoDB
        public void Update(SalesVolume salesVolume)
        {
            try
            {
                var filter = Builders<SalesVolume>.Filter.Eq(s => s.Id, salesVolume.Id);
                var update = Builders<SalesVolume>.Update
                    .Set(s => s.ServiceId, salesVolume.ServiceId)
                    .Set(s => s.QuantitySold, salesVolume.QuantitySold)
                    .Set(s => s.MonthYear, salesVolume.MonthYear);

                _salesVolume.UpdateOne(filter, update);
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred while updating the sales volume: " + ex.Message);
            }
        }
    }
}
