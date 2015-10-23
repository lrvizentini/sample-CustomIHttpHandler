using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomHttpHandler.Configuration
{
    public class ImageConverterSection : ConfigurationSection
    {
        [ConfigurationProperty("useWatermark", DefaultValue = "true")]
        public bool UseWatermark
        {
            get
            {
                return (bool)this["useWatermark"];
            }
            set
            {
                this["useWatermark"] = value;
            }
        }

        [ConfigurationProperty("watermarkText")]
        public string WatermarkText
        {
            get
            {
                return (string)this["watermarkText"];
            }
            set
            {
                this["watermarkText"] = value;
            }
        }
    }
}
