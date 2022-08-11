using depBridger.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading.Tasks;

namespace depBridger.Service
{
    public class ConfigurationService : IConfigurationService
    {
        public bool Save(Configuration configuration)
        {
            bool success;
            try
            {
                using (Stream stream = File.Open("Config.bin", FileMode.Create))
                {
                    var binaryFormatter = new BinaryFormatter();
                    binaryFormatter.Serialize(stream, configuration);
                    success = true;
                }
            }catch(Exception ex){
                success = false;
            }

            return success;
        }

        public  Configuration ReadConfiguration()
        {
            try
            {
                using (Stream stream = File.Open("Config.bin", FileMode.Open))
                {
                    var binaryFormatter = new BinaryFormatter();

                    return (Configuration)binaryFormatter.Deserialize(stream);
                }
            }catch(Exception ex) { }

            return new Configuration();
        }
    }
}
