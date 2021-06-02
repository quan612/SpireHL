using Dapper;
using Npgsql;
using SpireHL.Core.Models;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;

namespace SpireHL.Core.Repository
{
    public static class ModuleConfigs
    {
        private static string ConnectionString = ConfigurationManager.ConnectionStrings["PostgreSqlConnectionString"].ConnectionString.ToString();

        public static void CheckAndCreateTable()
        {
            var checkTable = @" SELECT * FROM pg_tables
                                WHERE schemaname = 'public'
                                AND tablename = 'spirehlsettings'";

            var createTable = @"CREATE TABLE spireHLSettings
                              (
                                  id serial NOT NULL,
                                  Module character varying(200),
                                  Section character varying(200) ,
                                  ParameterName character varying(200),
                              	  ParameterValue character varying(200)  
                              );";

            using (var connection = new NpgsqlConnection(ConnectionString))
            {
                connection.Open();

                var isTableExist = connection.Query(checkTable).ToList();

                if (isTableExist.Count < 1)
                {
                    connection.Execute(createTable);
                }
            }
        }
        public static List<SpireConfigs> GetConfigs(string moduleName, string sectionName)
        {
            var sql = @"Select * from public.spireHLSettings where module=@moduleName and section=@sectionName";

            using (var connection = new NpgsqlConnection(ConnectionString))
            {
                connection.Open();
                var parameters = new DynamicParameters();
                parameters.Add("@moduleName", moduleName, DbType.String);
                parameters.Add("@sectionName", sectionName, DbType.String);

                return connection.Query<SpireConfigs>(sql, parameters).ToList();
            }
        }

        public static void UpdateConfig(string moduleName, string sectionName, string parameterName, string parameterValue)
        {
            var exist = @"
                SELECT 1 FROM public.spirehlsettings WHERE module=@moduleName and section=@sectionName and parameterName=@parameterName";

            var insert = @"
                INSERT INTO public.spirehlsettings (Module, Section, ParameterName, ParameterValue)
                    VALUES (@moduleName, @sectionName, @parameterName, @parameterValue);";

            var update = @"
                UPDATE public.spirehlsettings
                SET parameterValue=@parameterValue
                WHERE module=@moduleName and section=@sectionName and parameterName=@parameterName;";

            using (var connection = new NpgsqlConnection(ConnectionString))
            {
                connection.Open();

                var parameters = new DynamicParameters();
                parameters.Add("@moduleName", moduleName, DbType.String);
                parameters.Add("@sectionName", sectionName, DbType.String);
                parameters.Add("@parameterName", parameterName, DbType.String);
                parameters.Add("@parameterValue", parameterValue, DbType.String);

                var isExist = connection.Query(exist, parameters).ToList();
                if (isExist.Count > 0)
                {
                    connection.Execute(update, parameters);
                }
                else
                {
                    connection.Execute(insert, parameters);
                }
            }
        }
    }
}
