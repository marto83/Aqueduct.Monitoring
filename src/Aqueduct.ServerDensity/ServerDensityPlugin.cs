using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace Aqueduct.ServerDensity
{
    public class ServerDensityPlugin : Dictionary<string, dynamic>
    {
        [JsonIgnore]
        public string Name { get; set; }

        public ServerDensityPlugin(string name)
            : base()
        {
            Name = name;
        }
    }
}
