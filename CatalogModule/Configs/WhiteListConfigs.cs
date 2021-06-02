using IkanManageCore;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;

namespace WhitelistModule.Configs
{
    public class WhiteListConfigs : IModuleConfigs
    {
        public Dictionary<string, string> GetModuleConfigs()

        {           
            //var directory = System.AppDomain.CurrentDomain.BaseDirectory;
            //var configFiles = Directory.GetFiles(directory, "*.config");

            //var path = @"I:\Global Affinity\GlobalApps\trunk\IkanManage\IkanManage\bin\Debug\WhitelistModule.dll.config";
            //ExeConfigurationFileMap configMap = new ExeConfigurationFileMap();
            //configMap.ExeConfigFilename = path;
            //Configuration test = ConfigurationManager.OpenMappedExeConfiguration(configMap, ConfigurationUserLevel.None);           

            var whiteListDict = (ConfigurationManager.GetSection("WhiteListSettings") as System.Collections.Hashtable)
                 .Cast<System.Collections.DictionaryEntry>()
                 .ToDictionary(n => n.Key.ToString(), n => n.Value.ToString());

            return whiteListDict;
        }
    }
}
