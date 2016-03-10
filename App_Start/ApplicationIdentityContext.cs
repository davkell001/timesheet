using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AspNet.Identity.MongoDB;
using MongoDB.Driver;
using MBTimeSheetWebApp.Models;

namespace MBTimeSheetWebApp.App_Start
{
    public class ApplicationIdentityContext : IDisposable
    {
        public static ApplicationIdentityContext Create()
        {
           
            // todo add settings where appropriate to switch server & database in your own application
            //var client = new MongoClient("mongodb://iis-spare-hp:27017");
            var client = new MongoClient("mongodb://iis-sql02:27017");
            var database = client.GetDatabase("TimeSheetsDb");
            var users = database.GetCollection<ApplicationUser>("users");
            var roles = database.GetCollection<IdentityRole>("roles");
            return new ApplicationIdentityContext(users, roles);
        }

        private ApplicationIdentityContext(IMongoCollection<ApplicationUser> users, IMongoCollection<IdentityRole> roles)
        {
            Users = users;
            Roles = roles;
        }

        public IMongoCollection<IdentityRole> Roles { get; set; }

        public IMongoCollection<ApplicationUser> Users { get; set; }

        public Task<List<IdentityRole>> AllRolesAsync()
        {
            return Roles.Find(r => true).ToListAsync();
        }

        public void Dispose()
        {
        }
    }
}