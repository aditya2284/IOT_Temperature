using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using IOT_Temperature.Interface;
using Temperatures;

namespace IOT_Temperature.Services
{
    public class TemperatureDapperRepository : ITemperatureRepository
    {
        private IDbConnection dbConnection;

        public TemperatureDapperRepository(string connstring)
        {
            dbConnection = new SqlConnection(connstring);
        }

        public void AddData(TemperatureData tempdata)
        {
            var date_time = tempdata.DateTime.ToDateTime();
            var sql=@"INSERT INTO [Temperature]
					([deviceID]
                    ,[date_time]
                    ,[temperature_value]
                    ,[unit])
                    VALUES(@deviceID,@date_time,@value,@unit)";
            dbConnection.Execute(sql,
                new { deviceID = tempdata.DeviceID, date_time = date_time, value = tempdata.Temperature, unit = tempdata.Unit });
        }
    }
}
