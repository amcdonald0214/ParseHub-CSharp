using Newtonsoft.Json;

namespace ParseHub.Client.Models
{
    public class Project
    {
        [JsonProperty(PropertyName = "token")]
        public string Token { get; set; }

        [JsonProperty(PropertyName = "title")]
        public string Title { get; set; }

        [JsonProperty(PropertyName = "templates_json")]
        public string TemplatesJson { get; set; }

        [JsonProperty(PropertyName = "main_template")]
        public string MainTemplate { get; set; }

        [JsonProperty(PropertyName = "main_site")]
        public string MainSite { get; set; }

        [JsonProperty(PropertyName = "options_json")]
        public string OptionsJson { get; set; }
        [JsonProperty(PropertyName = "last_run")]
        public Run LastRun { get; set; }
        [JsonProperty(PropertyName = "last_ready_run")]
        public Run LastReadyRun { get; set; }

    }
}
