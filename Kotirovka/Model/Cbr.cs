using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Kotirovka.Model
{
    public class Cbr
    {
        [JsonProperty("Date")]
        public DateTimeOffset Date { get; set; }

        [JsonProperty("PreviousDate")]
        public DateTimeOffset PreviousDate { get; set; }

        [JsonProperty("PreviousURL")]
        public string PreviousUrl { get; set; }

        [JsonProperty("Timestamp")]
        public DateTimeOffset Timestamp { get; set; }

        [JsonProperty("Valute")]
        public Dictionary<string, Valute> Valute { get; set; }
    }
}
