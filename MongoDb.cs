using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Threading.Tasks;
using AspNet.Identity.MongoDB;
using Microsoft.AspNet.Identity;
using MBTimeSheetWebApp.Models;
using System.Text;

namespace MBTimeSheetWebApp
{
    public class MongoDb
    {
        protected static IMongoClient _client = new MongoClient();
        protected static IMongoDatabase _database = _client.GetDatabase("TimeSheetsDb");
        //protected const string connectionString = "mongodb://iis-spare-hp:27017";
        protected const string connectionString = "mongodb://iis-sql02:27017";
        enum ProjType
        {
            Worksheet,
            CR,
            Checklist
        };

        public MongoDb()
        {
            _client = new MongoClient(connectionString);
            _database = _client.GetDatabase("TimeSheetsDb");
        }

        public IMongoDatabase getDb()
        {
            return _database;
        }

        public static List<object> GetAllDocs()
        {
            List<object> returnList = new List<object>();
            List<BsonDocument> docs = new List<BsonDocument>();
            List<ProjectInfo> projList = new List<ProjectInfo>();
            List<CRObj> crList = new List<CRObj>();
            List<TimeSheetObj> tsList = new List<TimeSheetObj>();

            _database = _client.GetDatabase("TimeSheetsDb");
            var collection = _database.GetCollection<BsonDocument>("projects");

            var test = new BsonDocument();

            List<BsonDocument> testdoc = collection.Find(test).ToList();

            docs = collection.Find(test).ToList();

            foreach (var doc in docs)
            {
                var proj = doc["ProjectInfo"];
                var timeSheet = doc["TimeSheet"];
                //var cr = doc["ChangeRequest"];

                projList.Add(new ProjectInfo
                {
                    CustCode = proj["CustCode"].ToString(),
                    CustName = proj["CustName"].ToString(),
                    ProjectCode = proj["ProjCode"].ToString(),
                    ProjectName = proj["ProjName"].ToString(),
                    PmName = proj["PmName"].ToString(),
                    PmEmail = proj["PmEmail"].ToString(),
                    Date = DateTime.Parse(proj["Date"].ToString()),
                    CustContact = proj["CustContact"].ToString(),
                    CustEmail = proj["CustEmail"].ToString(),
                    CustSignature = proj["Signature"].ToString() != null ? "Yes" : "No",
                    Version = proj["Version"].ToString()
                });
                tsList.Add(new TimeSheetObj
                {
                    TimeSpent = timeSheet["TimeSpend"].ToString(),
                    Agenda = ConvertBsonArrayToStringArray(timeSheet["Agenda"].AsBsonArray),
                    Deviation = timeSheet["Deviation"].ToString(),
                    CrRequired = timeSheet["CrRequired"].ToString(),
                    Outcome = ConvertBsonArrayToStringArray(timeSheet["Outcome"].AsBsonArray),


                });
                crList.Add(new CRObj
                    {
                        Estimates = timeSheet["Estimate"].ToString(),
                        DeptMb = timeSheet["DeptMb"].ToString(),
                        Time = timeSheet["Time"].ToString(),
                        Reasons = ConvertBsonArrayToStringArray(timeSheet["Reason"].AsBsonArray),
                        CustAuth = timeSheet["Authorisation"].ToString()
                    });
            }

            returnList.Add(projList);
            returnList.Add(crList);
            returnList.Add(tsList);

            return returnList;
        }

        public static List<ProjectInfo> GetProjectInfo()
        {
            List<ProjectInfo> projList = new List<ProjectInfo>();
            List<BsonDocument> docs = new List<BsonDocument>();
            _client = new MongoClient(connectionString);
            _database = _client.GetDatabase("TimeSheetsDb");

            if (HttpContext.Current.User.Identity.GetUserId() != null)
            {
                var collection = _database.GetCollection<BsonDocument>("projects");

                if (CheckUserPermissions() == "Admin")
                {
                    var filter = new BsonDocument();
                    docs = collection.Find(filter).ToList();
                }
                else
                {
                    var filter = Builders<BsonDocument>.Filter.Eq("UserInfo.UserName", HttpContext.Current.User.Identity.GetUserName());
                    docs = collection.Find(filter).ToList();
                }

                foreach (var doc in docs)
                {
                    var id = doc["_id"];
                    var proj = doc["ProjectInfo"];
                    var user = doc["UserInfo"];

                    projList.Add(new ProjectInfo
                    {
                        ID = id.AsObjectId,
                        UserName = user["UserName"].ToString(),
                        CustCode = proj["CustCode"].ToString(),
                        CustName = proj["CustName"].ToString(),
                        ProjectCode = proj["ProjCode"].ToString(),
                        ProjectName = proj["ProjName"].ToString(),
                        PmName = proj["PmName"].ToString(),
                        PmEmail = proj["PmEmail"].ToString(),
                        Date = DateTime.Parse(proj["Date"].ToString()),
                        CustContact = proj["CustContact"].ToString(),
                        CustEmail = proj["CustEmail"].ToString(),
                        CustSignature = proj["Signature"].ToString() != String.Empty ? "Yes" : "No",
                        Version = proj["Version"].ToString(),
                        //VersionId = proj["VersionID"].ToInt32(),
                        Type = proj["Type"].ToString(),

                    });

                }
            }

            return projList;

        }

        public static List<ProjectInfo> GetProjectInfo(List<string> userNames)
        {
            List<ProjectInfo> projList = new List<ProjectInfo>();
            List<BsonDocument> docs = new List<BsonDocument>();
            _client = new MongoClient(connectionString);
            _database = _client.GetDatabase("TimeSheetsDb");

            if (HttpContext.Current.User.Identity.GetUserId() != null)
            {
                var collection = _database.GetCollection<BsonDocument>("projects");
                if (userNames.Count != 0)
                {
                    var builder = Builders<BsonDocument>.Filter;
                    FilterDefinition<BsonDocument> filter = BuildFilter(userNames);
                    docs = collection.Find(filter).ToList();
                }

            }
            foreach (var doc in docs)
            {
                var proj = doc["ProjectInfo"];
                var user = doc["UserInfo"];
                var id = doc["_id"];
                projList.Add(new ProjectInfo
                {
                    ID = id.AsObjectId,
                    UserName = user["UserName"].ToString(),
                    CustCode = proj["CustCode"].ToString(),
                    CustName = proj["CustName"].ToString(),
                    ProjectCode = proj["ProjCode"].ToString(),
                    ProjectName = proj["ProjName"].ToString(),
                    PmName = proj["PmName"].ToString(),
                    PmEmail = proj["PmEmail"].ToString(),
                    Date = DateTime.Parse(proj["Date"].ToString()),
                    CustContact = proj["CustContact"].ToString(),
                    CustEmail = proj["CustEmail"].ToString(),
                    CustSignature = proj["Signature"].ToString() != String.Empty ? "Yes" : "No",
                    Version = proj["Version"].ToString(),
                    Type = proj["Type"].ToString()
                });

            }

            return projList;
        }

        public static List<CRObj> GetCRinfo()
        {
            List<CRObj> crList = new List<CRObj>();
            List<BsonDocument> docs = new List<BsonDocument>();
            _client = new MongoClient(connectionString);
            _database = _client.GetDatabase("TimeSheetsDb");

            if (HttpContext.Current.User.Identity.GetUserId() != null)
            {
                var collection = _database.GetCollection<BsonDocument>("projects");

                if (CheckUserPermissions() == "Admin")
                {
                    var filter = Builders<BsonDocument>.Filter.Eq("ProjectInfo.Type", "CR");
                    docs = collection.Find(filter).ToList();
                }
                else
                {
                    var builder = Builders<BsonDocument>.Filter;
                    var filter = builder.Eq("ProjectInfo.Type", "CR") & builder.Eq("UserInfo.UserName", HttpContext.Current.User.Identity.GetUserName());
                    //var filter = Builders<BsonDocument>.Filter.Eq("UserInfo.UserName", HttpContext.Current.User.Identity.GetUserName());
                    docs = collection.Find(filter).ToList();
                }

                foreach (var doc in docs)
                {
                    var proj = doc["ProjectInfo"];
                    var user = doc["UserInfo"];
                    var cr = doc["ChangeRequest"];
                    var id = doc["_id"];
                    crList.Add(new CRObj
                    {
                        ID = id.AsObjectId,
                        UserName = user["UserName"].ToString(),
                        CustCode = proj["CustCode"].ToString(),
                        CustName = proj["CustName"].ToString(),
                        ProjectCode = proj["ProjCode"].ToString(),
                        ProjectName = proj["ProjName"].ToString(),
                        PmName = proj["PmName"].ToString(),
                        PmEmail = proj["PmEmail"].ToString(),
                        Date = DateTime.Parse(proj["Date"].ToString()),
                        CustContact = proj["CustContact"].ToString(),
                        CustEmail = proj["CustEmail"].ToString(),
                        CustSignature = proj["Signature"].ToString() != String.Empty ? "Yes" : "No",
                        Version = proj["Version"].ToString(),
                        //VersionId = proj["VersionID"].ToInt32(),
                        Type = proj["Type"].ToString(),
                        Estimates = cr["Estimate"].ToString(),
                        DeptMb = cr["DeptMb"].ToString(),
                        Time = cr["Time"].ToString()

                    });

                }
            }

            return crList;
        }

        public static List<TimeSheetObj> GetWorksheetinfo()
        {
            List<TimeSheetObj> tsList = new List<TimeSheetObj>();
            List<BsonDocument> docs = new List<BsonDocument>();
            _client = new MongoClient(connectionString);
            _database = _client.GetDatabase("TimeSheetsDb");

            if (HttpContext.Current.User.Identity.GetUserId() != null)
            {
                var collection = _database.GetCollection<BsonDocument>("projects");

                if (CheckUserPermissions() == "Admin")
                {
                    var filter = Builders<BsonDocument>.Filter.Eq("ProjectInfo.Type", "Worksheet");
                    docs = collection.Find(filter).ToList();
                }
                else
                {
                    var builder = Builders<BsonDocument>.Filter;
                    var filter = builder.Eq("ProjectInfo.Type", "Worksheet") & builder.Eq("UserInfo.UserName", HttpContext.Current.User.Identity.GetUserName());
                    //var filter = Builders<BsonDocument>.Filter.Eq("UserInfo.UserName", HttpContext.Current.User.Identity.GetUserName());
                    docs = collection.Find(filter).ToList();
                }

                foreach (var doc in docs)
                {
                    var proj = doc["ProjectInfo"];
                    var user = doc["UserInfo"];
                    var ts = doc["TimeSheet"];
                    var id = doc["_id"];
                    tsList.Add(new TimeSheetObj
                    {
                        ID = id.AsObjectId,
                        UserName = user["UserName"].ToString(),
                        CustCode = proj["CustCode"].ToString(),
                        CustName = proj["CustName"].ToString(),
                        ProjectCode = proj["ProjCode"].ToString(),
                        ProjectName = proj["ProjName"].ToString(),
                        PmName = proj["PmName"].ToString(),
                        PmEmail = proj["PmEmail"].ToString(),
                        Date = DateTime.Parse(proj["Date"].ToString()),
                        CustContact = proj["CustContact"].ToString(),
                        CustEmail = proj["CustEmail"].ToString(),
                        CustSignature = proj["Signature"].ToString() != String.Empty ? "Yes" : "No",
                        Version = proj["Version"].ToString(),
                        //VersionId = proj["VersionID"].ToInt32(),
                        Type = proj["Type"].ToString(),
                        Deviation = ts["Deviation"].ToString(),
                        CrRequired = ts["CrRequired"].ToString(),
                        TimeSpent = ts["TimeSpend"].ToString()

                    });

                }
            }

            return tsList;
        }

        public static FilterDefinition<BsonDocument> BuildFilter(List<string> userNames)
        {
            string filterFormat = "builder.Eq('UserName', ";
            var builder = Builders<BsonDocument>.Filter;
            int count = 0;

            var filter = builder.Eq("", "");

            foreach (string name in userNames)
            {

                if (count != (userNames.Count - 1))
                {
                    filter = builder.Eq("UserInfo.UserName", name) + " & ";
                }
                else
                {
                    filter = builder.Eq("UserInfo.UserName", name);
                }
                count++;

            }
            return filter;
        }

        public static string CheckUserPermissions()
        {
            string permission = String.Empty;
            List<BsonDocument> docs = new List<BsonDocument>();
            _client = new MongoClient(connectionString);
            _database = _client.GetDatabase("TimeSheetsDb");
            var users = _database.GetCollection<BsonDocument>("users");
            var userFilter = Builders<BsonDocument>.Filter.Eq("UserName", HttpContext.Current.User.Identity.Name);
            docs = users.Find(userFilter).ToList();

            foreach (var doc in docs)
            {
                var role = doc["Roles"];
                if (role[0] == "PM" || role[0] == "Admin")
                {
                    permission = "Admin";
                }
                else
                {
                    permission = String.Empty;
                }
            }

            return permission;

        }

        public static List<ApplicationUser> GetAllUsers()
        {
            List<ApplicationUser> userList = new List<ApplicationUser>();
            List<BsonDocument> docs = new List<BsonDocument>();
            _client = new MongoClient(connectionString);
            _database = _client.GetDatabase("TimeSheetsDb");

            var collection = _database.GetCollection<BsonDocument>("users");
            var filter = new BsonDocument();
            docs = collection.Find(filter).ToList();

            foreach (var doc in docs)
            {
                userList.Add(new ApplicationUser
                {
                    UserName = doc["UserName"].ToString(),
                    Roles = new List<string>() { doc["Roles"].ToString() }
                });
            }

            return userList;
        }


        public static async Task<List<BsonDocument>> GetDoc(ObjectId id)
        {
            List<BsonDocument> doc = new List<BsonDocument>();
            _client = new MongoClient(connectionString);
            _database = _client.GetDatabase("TimeSheetsDb");
            var collection = _database.GetCollection<BsonDocument>("projects");
            //var filter = Builders<BsonDocument>.Filter.Eq("ProjectInfo.ProjectCode", projCode);
            var builder = Builders<BsonDocument>.Filter;
            var filter = builder.Eq("_id", id);

            doc = collection.Find(filter).ToList();

            return doc;
        }

        public static async Task<List<BsonDocument>> GetUserDocs(string userName)
        {
            List<BsonDocument> docs = new List<BsonDocument>();
            _client = new MongoClient(connectionString);
            _database = _client.GetDatabase("TimeSheetsDb");
            var collection = _database.GetCollection<BsonDocument>("projects");
            //var filter = Builders<BsonDocument>.Filter.Eq("ProjectInfo.ProjectCode", projCode);
            var builder = Builders<BsonDocument>.Filter;
            var filter = builder.Eq("UserInfo.UserName", userName); 

            docs = collection.Find(filter).ToList();

            return docs;
        }


        private static BsonDocument AddTimeSheet(TimeSheetObj tm, BsonArray agenda, BsonArray outcome)
        {
            var document = new BsonDocument 
                    {  
                        {
                            "TimeSheet", new BsonDocument
                            {
                                {"TimeSpend", tm.TimeSpent },
                                {"Agenda",agenda},
                                {"Deviation", tm.Deviation},
                                {"CrRequired", tm.CrRequired},
                                {"Outcome", outcome},
                                {"IsSigned", tm.IsSignedOff}
                                
                            }
                        }
                    };

            return document;

        }

        private static BsonDocument AddCRDoc(CRObj cr, BsonArray reason)
        {
            var document = new BsonDocument
            {
                 {
                     "ChangeRequest", new BsonDocument
                     {
                       {"Estimate", cr.Estimates},
                       {"DeptMb", cr.DeptMb},
                       {"Time", cr.Time},
                       {"Reason", reason},
                       {"Authorisation", cr.CustAuth},
                       {"IsSigned", cr.IsSignedOff}         
                     }         
                 }
            };
            return document;
        }

        private static BsonDocument AddChecklistDoc(int versionId, BsonArray vals)
        {
            var document = new BsonDocument
            {
                {
                    "Checklist", new BsonDocument
                     {
                        {"Values", vals},
                        {"VersionID", versionId}
                     }
               }
            };
            return document;

        }

        private static BsonDocument AddProjectInfo(ProjectInfo proj, int versionId, string type, string version)
        {
          
            var document = new BsonDocument 
                    {
                        {
                            "UserInfo",new BsonDocument
                            {
                                {"UserID", HttpContext.Current.User.Identity.GetUserId()},
                                {"UserName", HttpContext.Current.User.Identity.GetUserName()},
                            }
                        },
                        {
                            "ProjectInfo", new BsonDocument
                            {
                                {"ProjName",proj.ProjectName},
                                {"ProjCode", proj.ProjectCode},
                                {"PmName", proj.PmName},
                                {"PmEmail", proj.PmEmail},
                                {"CustCode", proj.CustCode},
                                {"CustName", proj.CustName},
                                {"Date", proj.Date},
                                {"CustContact", proj.CustContact},
                                {"CustEmail", proj.CustEmail},
                                {"Signature", proj.CustSignature},
                                {"Version", version},
                                {"VersionID", versionId},
                                {"Type", type}
                            }
                        },
                    };

            return document;
        }

        public static async Task InsertDoc(ProjectInfo proj, TimeSheetObj tm, CRObj cr, List<ChecklistObj> cl, bool isWorksheet, bool isCR, bool isChecklist, bool isNewProject)
        {
            bool isExisting = false;
            List<BsonDocument> list = new List<BsonDocument>();
            try
            {
                _client = new MongoClient(connectionString);
                _database = _client.GetDatabase("TimeSheetsDb");
                var projects = _database.GetCollection<BsonDocument>("projects");

                var builder = Builders<BsonDocument>.Filter;
                var filter = builder.Eq("_id", proj.ID);

                var result = await projects.Find(filter).ToListAsync();
                //check if project Id exists in the database
                isExisting = result.Count != 0 ? true : false;

                List<BsonArray> arrays = ConvertArrays(tm, cr, cl);

                if (isWorksheet)
                {
                    BsonDocument doc = AddProjectInfo(proj,0, ProjType.Worksheet.ToString(), tm.Version);
                    doc.AddRange(AddTimeSheet(tm, arrays[0], arrays[1]));
                    list.Add(doc);
                }

                if (isCR)
                {
                    BsonDocument doc1 = AddProjectInfo(proj, 0, ProjType.CR.ToString(), cr.Version);
                    doc1.AddRange(AddCRDoc(cr, arrays[2]));
                    
                    list.Add(doc1);
                }

                if (isChecklist)
                {
                    int versionId = proj.VersionId + 1;
                    BsonDocument doc2 = AddProjectInfo(proj, versionId, ProjType.Checklist.ToString(), proj.Version);
                    doc2.AddRange(AddChecklistDoc(versionId, arrays[3]));
                    list.Add(doc2);
                }

                if (isExisting && !isNewProject)
                {
                    //check to see what type the current project retrieved from the DB is.
                    var typ = result[0]["ProjectInfo"]["Type"];
                    //If that type matches the type of a document in the list then replace that document in the DB.
                    foreach (BsonDocument doc in list)
                    {
                        var projType = doc["ProjectInfo"]["Type"];

                        if (projType == typ)
                        {
                            list.Remove(doc);
                            await projects.ReplaceOneAsync(filter, doc);
                            break;
                        }
                    }

                    await projects.InsertManyAsync(list);
                }
                else
                {
                    await projects.InsertManyAsync(list);
                }
                

            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }

        }


        public static List<BsonArray> ConvertArrays(TimeSheetObj tab1, CRObj tab2, List<ChecklistObj> tab3)
        {
            List<BsonArray> list = new List<BsonArray>();

            BsonArray bAgenda = new BsonArray();
            BsonArray bOutcome = new BsonArray();
            BsonArray bReason = new BsonArray();
            BsonArray bChecklist = new BsonArray();

            if (tab1.Agenda != null)
            {
                foreach (string x in tab1.Agenda)
                {
                    if (x != null)
                    {
                        bAgenda.Add(x);
                    }

                }
            }
            if (tab1.Outcome != null)
            {
                foreach (string y in tab1.Outcome)
                {
                    if (y != null)
                    {
                        bOutcome.Add(y);
                    }

                }
            }
            if (tab2.Reasons != null)
            {
                foreach (string z in tab2.Reasons)
                {
                    if (z != null)
                    {
                        bReason.Add(z);
                    }
                }
            }
            

            foreach (var item in tab3)
            {
                if (item != null)
                {
                    bChecklist.Add(item.ToBsonDocument());
                }
            }

            list.Add(bAgenda);
            list.Add(bOutcome);
            list.Add(bReason);
            list.Add(bChecklist);

            return list;
        }


        public static async Task DeleteEntry(ObjectId id)
        {
            _client = new MongoClient(connectionString);
            _database = _client.GetDatabase("TimeSheetsDb");
            var collection = _database.GetCollection<BsonDocument>("projects");
            var builder = Builders<BsonDocument>.Filter;
            var filter = builder.Eq("_id", id);

            try
            {
                var result = await collection.DeleteOneAsync(filter);                
            }
            catch(MongoException ex)
            {
                Console.WriteLine(ex.Message);
            }
            
        }

        public static string[] ConvertBsonArrayToStringArray(BsonArray bsonArr)
        {
            string[] returnArr = new string[bsonArr.Count()];
            int count = 0;

            foreach (BsonValue val in bsonArr)
            {
                returnArr[count] = val.ToString();
                count++;
            }

            return returnArr;
        }

        private static BsonDocument ConfigureDoc(BsonDocument document, bool isNewDocId, string projType, string projVersion)
        {
            if (isNewDocId)
            {
                ObjectId id = ObjectId.GenerateNewId();
                document["_id"] = id;
            }

            document["ProjectInfo"]["Type"] = projType;
            document["ProjectInfo"]["Version"] = projVersion;

            return document;
        }
    }
}