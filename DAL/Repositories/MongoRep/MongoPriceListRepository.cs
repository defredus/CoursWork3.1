using DAL.Interfaces;
using DAL.Models;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DAL.Repositories.MongoRep
{
    public class MongoPriceListRepository : IPriceListRepository
    {
        private readonly IMongoCollection<PriceList> _priceListRepository;

        public MongoPriceListRepository(string connectionString)
        {
            var client = new MongoClient(connectionString);
            var database = client.GetDatabase("test");
            _priceListRepository = database.GetCollection<PriceList>("PriceLists");
        }

        // Add a new PriceList document to MongoDB
        public void Add(PriceList priceList)
        {
            try
            {
                _priceListRepository.InsertOne(priceList);
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred while adding the price list: " + ex.Message);
            }
        }

        // Delete a PriceList document by its ID from MongoDB
        public void Delete(int id)
        {
            try
            {
                var filter = Builders<PriceList>.Filter.Eq(pl => pl.Id, id);
                _priceListRepository.DeleteOne(filter);
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred while deleting the price list: " + ex.Message);
            }
        }

        // Retrieve all PriceList documents from MongoDB
        public IEnumerable<PriceList> GetAll()
        {
            try
            {
                return _priceListRepository.Find(Builders<PriceList>.Filter.Empty).ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred while fetching the price lists: " + ex.Message);
                return new List<PriceList>();
            }
        }

        // Retrieve a PriceList document by its ID from MongoDB
        public PriceList GetById(int id)
        {
            try
            {
                var filter = Builders<PriceList>.Filter.Eq(pl => pl.Id, id);
                return _priceListRepository.Find(filter).FirstOrDefault();
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred while fetching the price list: " + ex.Message);
                return null;
            }
        }

        // Update an existing PriceList document in MongoDB
        public void Update(PriceList priceList)
        {
            try
            {
                var filter = Builders<PriceList>.Filter.Eq(pl => pl.Id, priceList.Id);
                _priceListRepository.ReplaceOne(filter, priceList);
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred while updating the price list: " + ex.Message);
            }
        }
    }
}
