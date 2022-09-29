using Dapper;
using kftest.Models;
using kftest.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Net.Http;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace kftest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private readonly IDapper _dapper;
        public ValuesController(IDapper dapper)
        {
            _dapper = dapper;
        }
 
        [HttpPost]
        public string SaveActivities(ActivityViewModel ActivityList)
        {
            var dbparams = new DynamicParameters();
            dbparams.Add("@ActivityType", ActivityList.ActivityType, DbType.String);
            dbparams.Add("@DisplayName", ActivityList.DisplayName, DbType.String);
            dbparams.Add("@Description", ActivityList.Description, DbType.String);
            dbparams.Add("@RuntimeDescription", ActivityList.RuntimeDescription, DbType.String);
            dbparams.Add("@Category", ActivityList.Category, DbType.String);
            dbparams.Add("@Icon", ActivityList.Icon, DbType.String);
            //dbparams.Add("@Outcomes", ActivityList.Outcomes, DbType.String);
            var result =  Task.FromResult(_dapper.Execute("INSERT INTO Activity(ActivityType,DisplayName, Description,RuntimeDescription,Category,Icon,Outcomes) VALUES(@ActivityType,@DisplayName,@Description,@RuntimeDescription,@Category,@Icon,@Outcomes)"
                , dbparams,
                commandType: CommandType.Text));
            return "";

        }
    }
}
