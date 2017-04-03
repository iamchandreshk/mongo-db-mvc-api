using MongoDB.Models;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using System.Collections.Generic;
using System.Web.Http;
using System.Linq;
using System;

namespace MongoDB.Controllers
{
    /// <summary>
    /// Sample CRUD operation with MongoDB.
    /// </summary>
    public class MongoController : ApiController
    {
        private MongoServer objServer;
        private MongoDatabase objDatabse;
        private MongoCollection<BsonDocument> UserDetails;

        public MongoController()
        {
            objServer = MongoServer.Create("Server=localhost:27017");
            objDatabse = objServer.GetDatabase("UniqueTestDatabase");
            UserDetails = objDatabse.GetCollection<BsonDocument>("Users");
            UserDetails.EnsureIndex(new IndexKeysBuilder().Ascending("Name"), IndexOptions.SetUnique(true));
        }

        /// <summary>
        /// Return all the collection stored in Database.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("ReadDocument")]
        public IHttpActionResult ReadDocument()
        {
            List<UserModels> users = objDatabse.GetCollection<UserModels>("Users").FindAll().ToList();
            return Ok(users);
        }

        /// <summary>
        /// InsertDocument
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("InsertDocument")]
        public IHttpActionResult InsertDocument(string name)
        {
            UserModels um = new UserModels
            {
                Created = DateTime.UtcNow,
                Id = Guid.NewGuid(),
                Name = name
            };

            var result = UserDetails.Insert<UserModels>(um);
            return Ok(um);
        }

        /// <summary>
        /// RetreiveSelected
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("RetreiveSelected")]
        public IHttpActionResult RetreiveSelected(string name)
        {
            IMongoQuery query = Query.EQ("Name", name);
            List<UserModels> users = objDatabse.GetCollection<UserModels>("Users").Find(query).ToList();
            return Ok(users);
        }

        /// <summary>
        /// DeleteDocument
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("DeleteDocument")]
        public IHttpActionResult DeleteDocument(string name)
        {
            IMongoQuery query = Query.EQ("Name", name);
            objDatabse.GetCollection<UserModels>("Users").Remove(query);
            return Ok();
        }

        /// <summary>
        /// EditDocument
        /// </summary>
        /// <param name="OldName"></param>
        /// <param name="NewName"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("EditDocument")]
        public IHttpActionResult EditDocument(string OldName, string NewName)
        {
            IMongoQuery query = Query.EQ("Name", OldName);
            IMongoUpdate updateQuery = Update.Set("Name", NewName);

            UserModels user = objDatabse.GetCollection<UserModels>("Users").Find(query).SingleOrDefault();
            objDatabse.GetCollection("Users").Update(query, updateQuery);

            return Ok();
        }
    }
}
