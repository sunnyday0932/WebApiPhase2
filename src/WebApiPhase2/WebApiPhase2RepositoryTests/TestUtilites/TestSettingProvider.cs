using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WebApiPhase2RepositoryTests.TestUtilites
{
    public class TestSettingProvider
    {
        private static TestSettings Settings { get; set; }

        /// <summary>
        /// 取得測試設定內容
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        internal static TestSettings GetSettings(string fileName = "TestSettings.json")
        {
            if (Settings != null)
            {
                return Settings;
            }

            var testSettings = new TestSettings();
            var configuration = new ConfigurationBuilder().AddJsonFile(fileName).Build();
            configuration.Bind(testSettings);

            Settings = testSettings;
            return Settings;
        }

        /// <summary>
        /// 取得測試資料庫的類別
        /// </summary>
        /// <returns></returns>
        internal static string GetTestDatabaseType()
        {
            var testSetting = GetSettings();
            var types = new[] {"localdb","docker" };

            var databaseType = string.IsNullOrWhiteSpace(testSetting.DataBaseType) 
                ? "localdb" 
                :types.Contains(testSetting.DataBaseType.ToLower()).Equals(false) 
                    ?"localdb" 
                    : testSetting.DataBaseType.ToLower();

            return databaseType;
        }
    }

    internal class TestSettings
    { 
        /// <summary>
        /// 測試DB類型
        /// </summary>
        public string DataBaseType { get; set; }

        /// <summary>
        /// Docker 類型:Linux or Windows
        /// </summary>
        public string DockerType { get; set; }

        /// <summary>
        /// CotainerLabel
        /// </summary>
        public string CotainerLabel { get; set; }

        public Linuxdatabase Linuxdatabase { get; set; }
        
        public Windowsdatabase Windowsdatabase { get; set; }

    }

    internal class ContainerSetting
    { 
        public string DatabaseImage { get; set; }

        public string ContainerReadMessage { get; set; }
    }

    internal class Linuxdatabase : ContainerSetting
    { 
    
    }

    internal class Windowsdatabase : ContainerSetting
    { 
    
    }
}
