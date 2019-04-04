using System;
using System.Security;

namespace ServiceFabric.ConfigManager
{
    public class Program
    {
        public Program()
        {
            var x = Config.Get<INodeManagerConfig>().NodeDownGraceIntervalSeconds;
        }
    }

    public static class Config
    {
        public static T Get<T>(string instanceName = null)
        {
            return default(T);
        }
    }


    [ComponentConfig(SectionName = "NodeManager")]
    public interface INodeManagerConfig
    {
        [ConfigProperty(60, UpdatePolicy = ConfigPropertyUpdatePolicy.Dynamic)]
        int NodeDownGraceIntervalSeconds { get; set; }
    }

    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class ConfigPropertyAttribute : Attribute
    {
        public ConfigPropertyAttribute()
           : this(null)
        {
        }

        public ConfigPropertyAttribute(object defaultValue)
        {
            this.UpdatePolicy = ConfigPropertyUpdatePolicy.Dynamic;
            this.DefaultValue = defaultValue;
        }

        public string Name { get; set; }

        public string Description { get; set; }

        public object DefaultValue { get; set; }

        public ConfigPropertyUpdatePolicy UpdatePolicy { get; set; }
    }

    public enum ConfigPropertyUpdatePolicy
    {
        Dynamic = 0,
        Static = 1
    }

    [AttributeUsage(AttributeTargets.Interface, AllowMultiple = false, Inherited = true)]
    public class ComponentConfigAttribute : Attribute
    {
        public ComponentConfigAttribute()
        {
        }

        public string PackageName { get; set; }

        public string SectionName { get; set; }
    }

    public class ConfigurationStore
    {

    }

    public interface IConfigurationStore
    {
        event EventHandler<ConfigurationPropertyEventArgs> ConfigurationPropertyUpdated;

        string GetProperty(
            string sectionName,
            string propertyName,
            string defaultValue,
            ConfigPropertyUpdatePolicy updatePolicy);

        SecureString GetSecureProperty(
            string sectionName,
            string propertyName,
            string defaultValue,
            ConfigPropertyUpdatePolicy updatePolicy);
    }

    public class ConfigurationPropertyEventArgs : EventArgs
    {
        public string SectionName { get; set; }

        public string PropertyName { get; set; }
    }
}
