using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.IO;

namespace SnapPeaApp.Config
{
    /*
     * The Configuration class contains some static method for retrieving and modifying
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
        /*
         * The path to the program data folder. This is always given by %userprofile%\SnapPea
         */
        public static readonly string programFolder = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + @"\SnapPea";

        /*
         * The path to the configuration XML file. This is always given by %userprofile%\SnapPea\config.xml
         */
        public static readonly string configFileName = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + @"\SnapPea\config.xml";

        /*
         * This is the string setting entries dict. You can modify default configuration values here,
         * if no configuration file exists (first-time run) then these values will remain unchanged.
         */
        private static Dictionary<string, string> stringEntries = new Dictionary<string, string>()
        {
            //This is the default path for the layouts folder
            { ConfigKeys.LayoutsPath, Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + @"\SnapPea\layouts" },

            //The default layout to be loaded on startup. this is blank by default
            { ConfigKeys.DefaultLayout, String.Empty }
        };

        /*
         * This is the int setting entries dict. You can modify default configuration values here,
         * if no configuration file exists (first-time run) then these values will remain unchanged.
         */
        private static Dictionary<string, int> intEntries = new Dictionary<string, int>();

        /*
         * This is the bool setting entries dict. You can modify default configuration values here,
         * if no configuration file exists (first-time run) then these values will remain unchanged.
         */
        private static Dictionary<string, bool> boolEntries = new Dictionary<string, bool>()
        {
            //whether the layout specified by default_layout will be loaded on startup
            { ConfigKeys.LoadLayoutOnStart, false }
        };

        /*
         * This function returns the setting value identified by key
         */
        public static string getStringSetting(string key)
        {
            return stringEntries[key];
        }

        /*
         * This function returns the setting value identified by key
         */
        public static int getIntSetting(string key)
        {
            return intEntries[key];
        }

        /*
         * This function returns the setting value identified by key
         */
        public static bool getBoolSetting(string key)
        {
            return boolEntries[key];
        }

        /*
         * This function sets the setting value identified by key
         */
        public static void setStringSetting(string key, string value)
        {
            stringEntries[key] = value;
        }

        /*
         * This function sets the setting value identified by key
         */
        public static void setIntSetting(string key, int value)
        {
            intEntries[key] = value;
        }

        /*
         * This function sets the setting value identified by key
         */
        public static void setBoolSetting(string key, bool value)
        {
            boolEntries[key] = value;
        }


        /*
         * This will save the current configuration state to the settings file
         */
        public static void saveToFile()
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
                //xmlWriter.WriteStartElement("entry");
                //xmlWriter.WriteAttributeString("name", p.Key);
                //xmlWriter.WriteAttributeString("type", "string");
                //xmlWriter.WriteAttributeString("data", p.Value);
                //xmlWriter.WriteEndElement();
            }

            //Write int entries
            foreach (KeyValuePair<string, int> p in intEntries)
            {
                WriteNode(xmlWriter,p.Key,p.Value.ToString(),"int","\t");
                //xmlWriter.WriteStartElement("entry");
                //xmlWriter.WriteAttributeString("name", p.Key);
                //xmlWriter.WriteAttributeString("type", "int");
                //xmlWriter.WriteAttributeString("data", p.Value.ToString());
                //xmlWriter.WriteEndElement();
            }

            //Write bool entries
            foreach (KeyValuePair<string, bool> p in boolEntries)
            {
                WriteNode(xmlWriter,p.Key,p.Value.ToString(),"bool","\t");
                //xmlWriter.WriteStartElement("entry");
                //xmlWriter.WriteAttributeString("name", p.Key);
                //xmlWriter.WriteAttributeString("type", "bool");
                //xmlWriter.WriteAttributeString("data", p.Value.ToString().ToLower());
                //xmlWriter.WriteEndElement();
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

        /*
         * Loads configuration values from the settings file.
         */
        public static void loadFromFile()
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
            catch (Exception exc)
            {
                //this is a non-fatal error; default values will be generated
            }
        }
    }
}
