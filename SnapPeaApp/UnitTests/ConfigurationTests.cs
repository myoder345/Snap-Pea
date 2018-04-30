using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SnapPeaApp.Config;
using System.Text;

namespace UnitTests
{
    [TestClass]
    public class ConfigurationTests
    {
        private const string alphanum = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";

        /*
         * This test ensures that Configuration correctly
         * sets and gets settings, as well as correctly
         * saves and loads config values.
         */
        [TestMethod]
        public void ConfigurationSetGetSaveLoad()
        {
            Random rand = new Random();

            int val = rand.Next();
            int notval = val - 1; //no matter val, this will always not be val

            int strlen = rand.Next(10);

            //generates a random string
            StringBuilder strb = new StringBuilder();

            for(int i = 0; i < strlen; ++i)
            {
                strb.Append(alphanum[rand.Next(alphanum.Length)]);
            }

            string rstr = strb.ToString();

            //set an int in the settings file with the specified key
            Configuration.SetIntSetting(rstr, val);

            //ensure that the config change took effect
            Assert.AreEqual(Configuration.GetIntSetting(rstr), val);

            //save the configuration pre-save
            Configuration.SaveConfig();

            //set the setting to not val
            Configuration.SetIntSetting(rstr, notval);

            //the setting should no longer be val
            Assert.AreNotEqual(Configuration.GetIntSetting(rstr), val);

            //ensure that the config change took effect
            Assert.AreEqual(Configuration.GetIntSetting(rstr), notval);

            //re-load the config
            Configuration.LoadConfig();

            //we saved before we changed the setting to notval,
            //so the setting should be val again
            Assert.AreEqual(Configuration.GetIntSetting(rstr), val);
        }
    }
}
