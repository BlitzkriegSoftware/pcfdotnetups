using System;
using System.Linq;
using System.IO;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace BlitzkriegSoftware.PCF.CFEnvParser
{
    /// <summary>
    /// Cloud Foundry Helper Library Parse User-Provided-Services (UPS) from VCAP_SERVICES
    /// </summary>
    public sealed class UpsEnvParser
    {

        #region "CTOR"

        private UpsEnvParser() { }

        /// <summary>
        /// CTOR
        /// </summary>
        /// <param name="localPath">Local path (folder) to UPS JSON files</param>
        public UpsEnvParser(string localPath) : this()
        {
            LocalPath = localPath;
        }

        #endregion

        #region "Properties"

        private bool _isLocal = true;

        /// <summary>
        /// Is configuration aquired locally (false means from UPS)
        /// </summary>
        public bool IsLocal
        {
            get
            {
                if (_upsvalues == null) LoadValues();
                return _isLocal;
            }

            private set { _isLocal = value; }
        }

        /// <summary>
        /// Local path (folder) to UPS JSON files
        /// </summary>
        public string LocalPath { get; set; }

        private Dictionary<string, Dictionary<string, string>> _upsvalues;

        private Dictionary<string, Dictionary<string, string>> UpsValues
        {
            get
            {
                if (_upsvalues == null)
                {
                    _upsvalues = new Dictionary<string, Dictionary<string, string>>();
                    LoadValues();
                }
                return _upsvalues;
            }
        }

        /// <summary>
        /// How many UPS got loaded?
        /// </summary>
        public int UpsCount
        {
            get
            {
                var upses = this.UpsValues;
                return upses.Keys.Count;
            }
        }

        #endregion

        #region "Loaders"

        /// <summary>
        /// Environment Variable: VCAP_SERVICES
        /// </summary>
        public const string ENV_VAR = "VCAP_SERVICES";

        private void LoadValues()
        {
            var vcap = Environment.GetEnvironmentVariable(ENV_VAR);

            if (string.IsNullOrWhiteSpace(vcap))
            {
                LoadValuesFromLocal();
            }
            else
            {
                LoadValuesFromEnv(vcap);
            }
        }

        private void LoadValuesFromEnv(string vcap)
        {
            this.IsLocal = false;

            JObject json = JObject.Parse(vcap);

            IEnumerable<JToken> services = json.SelectTokens("$..user-provided");

            if (services != null)
            {
                foreach (var s in services.Children())
                {
                    var creds = new Dictionary<string, string>();
                    var serviceName = s.SelectToken("name").ToString();
                    var credsTokens = s.SelectTokens("$..credentials");
                    var list = credsTokens.First().Children();
                    foreach (var item in list)
                    {
                        var key = ((Newtonsoft.Json.Linq.JProperty)item).Name;
                        var value = ((Newtonsoft.Json.Linq.JProperty)item).Value.ToString();
                        creds.Add(key, value);
                    }
                    if (this.UpsValues.ContainsKey(serviceName))
                    {
                        this.UpsValues.Remove(serviceName);
                    }
                    this.UpsValues.Add(serviceName, creds);
                }
            }
        }

        private void LoadValuesFromLocal()
        {
            this.IsLocal = true;

            var di = new DirectoryInfo(this.LocalPath);
            foreach (var fi in di.GetFiles("*.json"))
            {
                var serviceName = Path.GetFileNameWithoutExtension(fi.Name);
                var vals = new Dictionary<string, string>();
                var text = File.ReadAllText(fi.FullName);
                JObject jo = JObject.Parse(text);

                foreach (JProperty x in (JToken)jo)
                {
                    string name = x.Name;
                    JToken jt = x.Value;
                    string value = jt.ToString();

                    vals.Add(name, value);
                }

                if (this.UpsValues.ContainsKey(serviceName))
                {
                    this.UpsValues.Remove(serviceName);
                }
                this.UpsValues.Add(serviceName, vals);
            }
        }

        #endregion

        #region "Parser Methods"

        /// <summary>
        /// Return the list of UPS names
        /// </summary>
        public List<string> UpsNames
        {
            get
            {
                return new List<string>(this.UpsValues.Keys);
            }
        }

        /// <summary>
        /// Is this UPS in the collection?
        /// </summary>
        /// <param name="upsName">Name of UPS</param>
        /// <returns>True if so</returns>
        public bool UpsExists(string upsName)
        {
            return this.UpsValues.ContainsKey(upsName);
        }

        /// <summary>
        /// Is this UPS Key in the collection?
        /// </summary>
        /// <param name="upsName">Name of UPS</param>
        /// <param name="key">Key to UPS value</param>
        /// <returns>True if so</returns>
        public bool UpsKeyExists(string upsName, string key)
        {
            var ok = false;
            if (UpsExists(upsName))
            {
                var vals = this.UpsValues[upsName];
                if (vals.ContainsKey(key)) ok = true;
            }
            return ok;
        }

        /// <summary>
        /// Get the value for a UPS for a key (or provided default)
        /// </summary>
        /// <param name="upsName">Name of UPS</param>
        /// <param name="key">Key to UPS value</param>
        /// <param name="defaultValue"></param>
        /// <returns>Value or defaultValue (default: empty string)</returns>
        public string UpsKeyGetValue(string upsName, string key, string defaultValue = "")
        {
            if (UpsKeyExists(upsName, key))
            {
                var vals = this.UpsValues[upsName];
                return vals[key];
            }
            else
            {
                return defaultValue;
            }
        }

        /// <summary>
        /// Return all Keys for a UPS
        /// </summary>
        /// <param name="upsName">Name of UPS</param>
        /// <returns>List of keys for the UPS</returns>
        public IEnumerable<string> UpsKeys(string upsName)
        {
            var list = new List<string>();
            if (UpsExists(upsName))
            {
                var ups = this.UpsValues[upsName];
                list.AddRange(ups.Keys);
            }
            return list;
        }

        /// <summary>
        /// Return the dictionary of key, values for a UPS
        /// </summary>
        /// <param name="upsName">Name of UPS</param>
        /// <returns>Return name, value dictionary</returns>
        public IDictionary<string, string> UpsKeyValues(string upsName)
        {
            var d = new Dictionary<string, string>();
            if (UpsExists(upsName))
            {
                d = this.UpsValues[upsName];
            }
            return d;
        }
        #endregion
    }
}
