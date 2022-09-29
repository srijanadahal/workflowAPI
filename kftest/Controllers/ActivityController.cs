using Dapper;
using kftest.Models;
using kftest.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Data;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;
using static Org.BouncyCastle.Math.EC.ECCurve;

namespace kftest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ActivityController : Controller
    {
        private readonly IDapper _dapper;
        private readonly IConfiguration _config;
        private string Connectionstring = "DefaultConnection";
        public ActivityController(IDapper dapper, IConfiguration config)
        {
            _dapper = dapper;
            _config = config;

        }

        [HttpPost]
        public string SaveActivities(ActivityViewModel ActivityList)
        {
            var returnMessage = "";
            using IDbConnection db = new SqlConnection(_config.GetConnectionString(Connectionstring));
            try
            {
                if (db.State == ConnectionState.Closed)
                    db.Open(); 
                
                using var tran = db.BeginTransaction();
                try
                {
                    var dbparams = new DynamicParameters();

                    dbparams.Add("@ActivityType", ActivityList.ActivityType, DbType.String);
                    dbparams.Add("@DisplayName", ActivityList.DisplayName, DbType.String);
                    dbparams.Add("@Description", ActivityList.Description, DbType.String);
                    dbparams.Add("@RuntimeDescription", ActivityList.RuntimeDescription, DbType.String);
                    dbparams.Add("@Category", ActivityList.Category, DbType.String);
                    dbparams.Add("@Icon", ActivityList.Icon, DbType.String);
                    //dbparams.Add("@Outcomes", null, DbType.String);
                    //dbparams.Add("@properties", ActivityList.Properties, DbType.String);

                    var activityId = _dapper.Execute("INSERT INTO Activity(ActivityType,DisplayName, Description,RuntimeDescription,Category,Icon) VALUES(@ActivityType,@DisplayName,@Description,@RuntimeDescription,@Category,@Icon)" +
                        "SELECT SCOPE_IDENTITY();"

                        , dbparams,
                        commandType: CommandType.Text);

                    if (ActivityList.properties != null)
                    {
                        foreach (var property in ActivityList.properties)
                        {

                            //var propertyId = _dapper.Get<int>("select id from Property where propertyName = '" + property + "'"
                            //, dbparams,
                            //commandType: CommandType.Text);

                            var dbparamsinsertrelation = new DynamicParameters();
                            dbparams.Add("@activityId", activityId, DbType.String);
                            dbparams.Add("@propertyId", property, DbType.String);
                            var insertrelation = _dapper.Execute("Insert into ActivityProperty(ActivityID,PropertyID) Values(@activityId,@propertyId)"
                                , dbparams,
                                commandType: CommandType.Text);
                        }
                    }


                    if (ActivityList.Outcomes != null)
                    {
                        foreach (var outcome in ActivityList.Outcomes)
                        {

                            var dboutcomeinsertrelation = new DynamicParameters();
                            dboutcomeinsertrelation.Add("@activityId", activityId, DbType.String);
                            dboutcomeinsertrelation.Add("@outcome", outcome, DbType.String);
                            var insertoutcomes = _dapper.Execute("Insert into ActivityOutcome(ActivityID,Outcome) Values(@activityId,@outcome)"
                                , dboutcomeinsertrelation,
                                commandType: CommandType.Text);
                        }
                    }
                    



                    var getAllActivity = _dapper.GetAll<ActivityStringViewModel>(@"Select ID as Id, 
                    ActivityType, DisplayName, Description, Category, Icon from Activity"
                        , null, commandType: CommandType.Text).AsList();

                    List<ActivityJsonViewModel> JsonList = new List<ActivityJsonViewModel>();
                    foreach (var activity in getAllActivity)
                    {
                        var getPropertiesList = _dapper.GetAll<PropertyJsonViewModel>(@"Select p.propertyName as name, p.propertyName as type, p.label, p.hint  from Property p
                        inner join ActivityProperty ap on p.ID = ap.PropertyID where activityid = '" + activity.Id + "'"
                            , null, commandType: CommandType.Text).AsList();

                        var getActivityOutcomes = _dapper.GetAll<string>(@"Select Outcome from ActivityOutcome 
                          where activityid = '" + activity.Id + "'", null, commandType: CommandType.Text).ToArray();

                        var JsonData = new ActivityJsonViewModel
                        {
                            type = activity.ActivityType,
                            displayName = activity.DisplayName,
                            description = activity.Description,
                            runtimeDescription = null,
                            category = activity.Category,
                            icon = activity.Icon,
                            outcomes = getActivityOutcomes,
                            properties = getPropertiesList
                        };
                        JsonList.Add(JsonData);
                    }


                    var data = "{activity:"+Newtonsoft.Json.JsonConvert.SerializeObject(JsonList)+"}";

                    //var dbparamsactivity = new DynamicParameters();
                    //dbparamsactivity.Add("@activities", data, DbType.String);
                    if(getAllActivity.Count > 1)
                    {
                        var InsertActivityJson = _dapper.Update<string>("update ActivityJson Set Activities = '" + data + "'"
                        , null,
                        commandType: CommandType.Text);
                    }
                    else
                    {
                        var dbparamactivities = new DynamicParameters();
                        dbparamactivities.Add("@activitities", data, DbType.String);

                        var InsertActivityJson = _dapper.Execute("Insert into ActivityJson (Activities) Values(@activitities)"
                            , dbparamactivities,
                            commandType: CommandType.Text);
                    }

                }
                catch (Exception ex)
                {
                    tran.Rollback();
                    throw ex;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (db.State == ConnectionState.Open)
                    db.Close();
            }

            returnMessage = "Successfully Added Activity";
            return returnMessage;

        }

        [HttpGet]
        public ActivityJsonModel getActivities()
        {
            var activities = _dapper.Get<string>(@" Select Activities from ActivityJson"
                , null, commandType: CommandType.Text).ToString();
            var jsonFormattedString = Newtonsoft.Json.JsonConvert.DeserializeObject<ActivityJsonModel>(activities);
            return jsonFormattedString;

        }
    }
}
