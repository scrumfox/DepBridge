using depBridger.Models;

namespace depBridger.Service
{
    public interface IConfigurationService
    {
        public bool Save(Configuration configuration);
        public Configuration ReadConfiguration();
    }
}
