using Dapper;
using Npgsql;
using OfficeOpenXml;
using SpireHL.Core.Models;
using SpireHL.Core.Utils;
using System.Configuration;
using System.Data;

/// <summary>
/// Base configs for querying database
/// </summary>
namespace SpireHL.Core.Repository
{
    public class BaseRepository
    {
        private readonly string _connectionString;
        
        public BaseRepository()
        {
            _connectionString = GetConnectionString("PostgreSqlConnectionString");

            SqlMapper.AddTypeHandler(typeof(UDF), new JsonObjectTypeHandler());
            SqlMapper.AddTypeHandler(typeof(SpireSaleAnalysisData), new JsonObjectTypeHandler());
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
        }

        private static string GetConnectionString(string name)
        {
            var HilineConnectionString = ConfigurationManager.ConnectionStrings[name].ConnectionString;
            if(HilineConnectionString == null)
            {
                throw new System.Exception($"Connection string {name} was not found");
            }
            return HilineConnectionString;
        }

        protected IDbConnection GetConnection() => new NpgsqlConnection(_connectionString);
    }
}
