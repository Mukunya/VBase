using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace MukuBase
{
    public class Config
    {
        public enum Scope { application, user }
        public Scope ConfigScope { get; set; } = Scope.application;
        public bool AutoSave { get; set; } = false;
        private Dictionary<string,string> config = new Dictionary<string,string>();
        private Dictionary<string,string> defaults = new Dictionary<string,string>();

        public Config() { }
        public string this[string key]
        {
            get 
            {
                if (config.TryGetValue(key,out string value))
                {
                    return value;
                }
                else
                {
                    throw new KeyNotFoundException(key);
                }
            } 
            set 
            {
                if (config.ContainsKey(key.ToLower().Trim().Replace(':',';')))
                {
                    config[key.ToLower().Trim().Replace(':', ';')] = value; 
                }
                else
                {
                    config.Add(key.ToLower().Trim().Replace(':', ';'), value);
                }
                if (AutoSave)
                {
                    Save();
                }
            }
        }

        public string[] Keys => config.Keys.ToArray();

        public void SetDefault(string key, string value)
        {
            defaults[key] = value;
        }

        public void SetIfEmpty(string key, string value)
        {
            config.TryAdd(key, value);
        }
        public void SetDefaults(params (string Key, string Value)[] values)
        {
            foreach (var pair in values)
            {
                SetDefault(pair.Key,pair.Value);
            }
        }
        private string getdefaultlocation()
        {
            switch (ConfigScope)
            {
                default:
                case Scope.application:
                    return Path.Join(AppDomain.CurrentDomain.BaseDirectory, "config.txt");
                case Scope.user:
                    return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),AppDomain.CurrentDomain.FriendlyName,"config.txt");
            }
        }
        /// <summary>
        /// Checks if the config is empty
        /// </summary>
        /// <returns>true if empty</returns>
        public bool IsEmpty()
        {
            return config.Count == 0;
        }
        /// <summary>
        /// Loads the config file from the default location
        /// </summary>
        public void Load()
        {
            Load(getdefaultlocation());
        }
        /// <summary>
        /// Loads the config file from the specified path
        /// </summary>
        /// <param name="configfilelocation"></param>
        public void Load(string configfilelocation)
        {
            config.Clear();
            foreach (var keyValuePair in defaults)
            {
                config.Add(keyValuePair.Key, keyValuePair.Value);
            }
            if (!File.Exists(configfilelocation))
                return;
            string[] configf = File.ReadAllLines(configfilelocation);
            foreach (string configfile in configf)
            {
                if (configfile == "" || !configfile.Contains(':'))
                {
                    continue;
                }
                config.Add(configfile.Trim().Split(':')[0], configfile.Trim().Split(':').Skip(1).Aggregate((string prev, string current) => prev+':'+current));
            }
        }
        /// <summary>
        /// Saves the config values into a file at the default location.
        /// </summary>
        public void Save()
        {
            Save(getdefaultlocation());
        }
        /// <summary>
        /// Saves the config values into a file at the specified locatiom
        /// </summary>
        /// <param name="configfilelocation"></param>
        public void Save(string configfilelocation)
        {
            if(File.Exists(configfilelocation))
                File.Delete(configfilelocation);
            StreamWriter s = File.CreateText(configfilelocation);
            foreach (var item in config.Keys)
            {
                s.Write(item);
                s.Write(":");
                s.Write(this[item]);
                s.WriteLine();
            }
            s.Close();
        }
    }
}
