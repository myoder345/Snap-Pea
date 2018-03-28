using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.IO;
using System.Windows;

namespace SnapPeaApp.Config
{
    /*
     * The Configuration class contains some static methods for retrieving and modifying
     * setting values.
     * 
     * To use this class:
     *   - invoke loadFromFile() on program startup
     *   - invoke saveToFile() when the settings are modified ("save" button in the config editor dialog, for instance)
     *   - invoke the appropriate functions for retrieving config values, as needed
     *     - Example: Configuration.getBoolSetting("load_layout_on_start") will return either true or false depending on
     *       the load_layout_on_start value from the config file.
     *     - Similarly, Configuration.setBoolSetting("load_layout_on_start", true) will set this value to "true". Using a Dictionary,
     *       mapping config keys to control IDs, or some other similar scheme, one could automate saving to a high degree.
     *       Once modifications to settings have been done, invoke saveToFile() as described above.
     * 
     * The XML config file stores entries in the following format:
     * 
     *   <entry name="load_layout_on_start" type="bool" value="true" />
     * 
     */
    class Configuration
    {
        /// <summary>
        /// Path to program data folder. This is always given by %userprofile%\SnapPea
        /// </summary>
        public static readonly string programFolder = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + @"\SnapPea";

        /// <summary>
        /// Path to config XML file. This is always given by %userprofile%\SnapPea\config.xml
        /// </summary>
        public static readonly string configFileName = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + @"\SnapPea\config.xml";

        /// <summary>
        /// String setting entries dict. 
        /// </summary>
        /// <remarks> You can modify default configuration values here, if no configuration file exists(first-time run) then these values will remain unchanged. </remarks>
        private static Dictionary<string, string> stringEntries = new Dictionary<string, string>()
        {
            //This is the default path for the layouts folder
            { ConfigKeys.LayoutsPath, Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + @"\SnapPea\layouts" },

            //The default layout to be loaded on startup. this is blank by default
            { ConfigKeys.DefaultLayout, Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + @"\SnapPea\layouts\FourSquare.json" }
        };

        /// <summary>
        /// Int setting entries dict. 
        /// </summary>
        /// <remarks> You can modify default configuration values here, if no configuration file exists(first-time run) then these values will remain unchanged. </remarks>
        private static Dictionary<string, int> intEntries = new Dictionary<string, int>();

        /// <summary>
        /// Bool setting entries dict. 
        /// </summary>
        /// <remarks> You can modify default configuration values here, if no configuration file exists(first-time run) then these values will remain unchanged. </remarks>
        private static Dictionary<string, bool> boolEntries = new Dictionary<string, bool>()
        {
            //whether the layout specified by default_layout will be loaded on startup
            { ConfigKeys.LoadLayoutOnStart, false }
        };

        /// <summary>
        /// Gets the setting identified by key
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public static string getStringSetting(string key)
        {
            return stringEntries[key];
        }

        /// <summary>
        /// Gets the setting identified by key
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public static int getIntSetting(string key)
        {
            return intEntries[key];
        }

        /// <summary>
        /// Gets the setting identified by key
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public static bool getBoolSetting(string key)
        {
            return boolEntries[key];
        }

        /// <summary>
        /// Sets the settings value identified by key
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public static void setStringSetting(string key, string value)
        {
            stringEntries[key] = value;
        }

        /// <summary>
        /// Sets the settings value identified by key
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public static void setIntSetting(string key, int value)
        {
            intEntries[key] = value;
        }

         /// <summary>
         /// Sets the settings value identified by key
         /// </summary>
         /// <param name="key"></param>
         /// <param name="value"></param>
        public static void setBoolSetting(string key, bool value)
        {
            boolEntries[key] = value;
        }

         /// <summary>
         /// Saves the current configuration state to the settings file
         /// </summary>
        public static void SaveConfig()
        {
            if (!Directory.Exists(programFolder))
            {
                Directory.CreateDirectory(programFolder);
            }

            var xmlWriter = XmlWriter.Create(configFileName);

            xmlWriter.WriteStartDocument();

            xmlWriter.WriteStartElement("entries");

            //Write string entries
            foreach (KeyValuePair<string, string> p in stringEntries)
            {
                WriteNode(xmlWriter,p.Key,p.Value,"string","\t");
            }

            //Write int entries
            foreach (KeyValuePair<string, int> p in intEntries)
            {
                WriteNode(xmlWriter,p.Key,p.Value.ToString(),"int","\t");
            }

            //Write bool entries
            foreach (KeyValuePair<string, bool> p in boolEntries)
            {
                WriteNode(xmlWriter,p.Key,p.Value.ToString(),"bool","\t");
            }

            xmlWriter.WriteEndElement();
            xmlWriter.WriteEndDocument();

            xmlWriter.Close();
        }

        /// <summary>
        /// Helper function for writing config
        /// </summary>
        /// <param name="textWriter"></param>
        /// <param name="elementName"></param>
        /// <param name="elementValue"></param>
        /// <param name="indentString"></param>
        private static void WriteNode(XmlWriter textWriter, string elementName, string elementValue, string elementType, string indentString)
        {
            textWriter.WriteWhitespace(Environment.NewLine + indentString);
            textWriter.WriteStartElement("entry");
            textWriter.WriteAttributeString("name", elementName);
            textWriter.WriteAttributeString("type", elementValue);
            textWriter.WriteAttributeString("data", elementType);
            textWriter.WriteEndElement();
        }

        /// <summary>
        /// Loads configuration values from the settings file.
        /// </summary>
        public static void LoadConfig()
        {
            try
            {
                var xmlReader = XmlReader.Create(configFileName);

                while (xmlReader.Read())
                {
                    if (xmlReader.Name.Equals("entry") && (xmlReader.NodeType == XmlNodeType.Element))
                    {
                        string id = xmlReader.GetAttribute("name");
                        string type = xmlReader.GetAttribute("type");
                        string data = xmlReader.GetAttribute("data");

                        switch (type.ToLower())
                        {
                            //Load string entries
                            case "string":
                                stringEntries[id] = data;

                                break;

                            //Load int entries. Need to parse the int as a string first
                            case "int":
                                int asInt;

                                if (int.TryParse(data, out asInt))
                                {
                                    intEntries[id] = asInt;
                                }

                                break;

                            //Load bool entries
                            case "bool":
                                if (data.ToLower().Equals("true"))
                                {
                                    boolEntries[id] = true;
                                }
                                else
                                {
                                    boolEntries[id] = false;
                                }

                                break;
                        }

                    }
                }

                xmlReader.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading config: {ex.Message}","Error");
            }
        }
    }
}
