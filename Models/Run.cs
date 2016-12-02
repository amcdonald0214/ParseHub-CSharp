using Newtonsoft.Json;
using System;

namespace ParseHub.Client.Models
{
    public class Run
    {
        [JsonProperty(PropertyName = "project_token")]
        public string ProjectToken { get; set; }

        [JsonProperty(PropertyName = "run_token")]
        public string RunToken { get; set; }

        [JsonProperty(PropertyName = "status")]
        public string Status { get; set; }

        [JsonProperty(PropertyName = "data_ready")]
        public bool DataReady { get; set; }

        [JsonProperty(PropertyName = "start_time")]
        public DateTime StartTime { get; set; }

        [JsonProperty(PropertyName = "end_time")]
        public DateTime? EndTime { get; set; }

        [JsonProperty(PropertyName = "pages")]
        public int? Pages { get; set; }

        [JsonProperty(PropertyName = "md5sum")]
        public string MD5Sum { get; set; }

        [JsonProperty(PropertyName = "start_url")]
        public string StartUrl { get; set; }

        [JsonProperty(PropertyName = "start_template")]
        public string StartTemplate { get; set; }

        [JsonProperty(PropertyName = "start_value")]
        public string StartValue { get; set; }

        [JsonProperty(PropertyName = "data")]
        public string Data { get; set; }
    }
}
