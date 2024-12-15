using DAL.Interfaces;
using DAL.Models;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DAL.Repositories.MongoRep
{
    public class MongoPaymentTypeRepository : IPaymentTypeRepository
    {
        private readonly IMongoCollection<PaymentType> _paymentTypes;

        public MongoPaymentTypeRepository(string connectionString)
        {
            var client = new MongoClient(connectionString);
            var database = client.GetDatabase("test");
            _paymentTypes = database.GetCollection<PaymentType>("PaymentTypes");
        }

        // Add a new PaymentType to MongoDB
        public void Add(PaymentType paymentType)
        {
            try
            {
                _paymentTypes.InsertOne(paymentType);
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred while adding the payment type: " + ex.Message);
            }
        }

        // Delete a PaymentType by its ID from MongoDB
        public void Delete(int id)
        {
            try
            {
                var filter = Builders<PaymentType>.Filter.Eq(pt => pt.Id, id);
                _paymentTypes.DeleteOne(filter);
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred while deleting the payment type: " + ex.Message);
            }
        }

        // Retrieve all PaymentTypes from MongoDB
        public IEnumerable<PaymentType> GetAll()
        {
            try
            {
                return _paymentTypes.Find(_ => true).ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred while fetching the payment types: " + ex.Message);
                return new List<PaymentType>();
            }
        }

        // Retrieve a PaymentType by its ID from MongoDB
        public PaymentType GetById(int id)
        {
            try
            {
                var filter = Builders<PaymentType>.Filter.Eq(pt => pt.Id, id);
                return _paymentTypes.Find(filter).FirstOrDefault();
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred while fetching the payment type: " + ex.Message);
                return null;
            }
        }

        // Update an existing PaymentType in MongoDB
        public void Update(PaymentType paymentType)
        {
            try
            {
                var filter = Builders<PaymentType>.Filter.Eq(pt => pt.Id, paymentType.Id);
                var update = Builders<PaymentType>.Update
                    .Set(pt => pt.Name, paymentType.Name)
                    .Set(pt => pt.Description, paymentType.Description);

                _paymentTypes.UpdateOne(filter, update);
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred while updating the payment type: " + ex.Message);
            }
        }
    }
}
