using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;

namespace BlitzkriegSoftware.PCF.CFEnvParser.Web.Controllers
{
    /// <summary>
    /// PcfEnv Demo Controller
    /// </summary>
    [Route("api/[controller]")]
    public class PcfEnvController : Controller
    {
        private PCF.CFEnvParser.UpsEnvParser _parser = null;

        private IHostingEnvironment _env;

        /// <summary>
        /// CTOR
        /// </summary>
        /// <param name="env">Hosting Environment</param>
        public PcfEnvController(IHostingEnvironment env)
        {
            _env = env;
            var path = System.IO.Path.Combine(env.WebRootPath, "UPS");
            _parser = new UpsEnvParser(path);
        }

        /// <summary>
        /// List UPS loaded
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("upses")]
        public IEnumerable<string> Upses()
        {
            return _parser.UpsNames;
        }

        /// <summary>
        /// Get a value for a UPS and Key
        /// </summary>
        /// <param name="upsName">UPS Name</param>
        /// <param name="key">Key</param>
        /// <returns>Value or default</returns>
        [HttpGet]
        [Route("Value")]
        public string Value(string upsName, string key)
        {
            return _parser.UpsKeyGetValue(upsName, key);
        }

        /// <summary>
        /// Does the UPS have a named key?
        /// </summary>
        /// <param name="upsName">UPS Name</param>
        /// <param name="key">Key</param>
        /// <returns>True if so</returns>
        [HttpGet]
        [Route("Exists")]
        public bool Exists(string upsName, string key)
        {
            return _parser.UpsKeyExists(upsName, key);
        }

        /// <summary>
        /// Is the parser using local files and not the CF UPS
        /// </summary>
        /// <returns>True if using files</returns>
        [HttpGet]
        [Route("IsLocal")]
        public bool IsLocal()
        {
            return _parser.IsLocal;
        }

    }
}
