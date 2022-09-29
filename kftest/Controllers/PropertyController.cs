using Dapper;
using kftest.Models;
using kftest.Service;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace kftest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PropertyController : Controller
    {
        private readonly IDapper _dapper;
        public PropertyController(IDapper dapper)
        {
            _dapper = dapper;
        }
        [HttpPost]
        public string SaveProperty(PropertyViewModel Property)
        {
            var returnMessage = "";
            var dbparams = new DynamicParameters();

            dbparams.Add("@propertyName", Property.propertyName, DbType.String);
            dbparams.Add("@propertyType", Property.propertyType, DbType.String);
            dbparams.Add("@label", Property.label, DbType.String);
            dbparams.Add("@hint", Property.hint, DbType.String);
            var propertyId = _dapper.Execute("INSERT INTO Property(propertyName,propertyType,label,hint) " +
                "VALUES(@propertyName,@propertyType,@label,@hint) SELECT SCOPE_IDENTITY();"
                , dbparams,
                commandType: CommandType.Text);

            returnMessage = "Successfully Added Activity";
            return returnMessage;

        }

        [HttpGet]
        public List<PropertyViewModel> getProperty()
        {
            var PropertyList = new List<PropertyViewModel>();
            var dbparams = new DynamicParameters();

            var result = _dapper.GetAll<PropertyViewModel>("SELECT * FROM Property", dbparams,
                commandType: CommandType.Text);
            PropertyList = result.ToList();
            return PropertyList;

        }

    }
}
