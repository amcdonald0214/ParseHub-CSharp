using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ParseHub.Client.Models;
using RestSharp;
using RestSharp.Extensions.MonoHttp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParseHub.Client
{
    public class ProjectRunner
    {
        private string ApiKey;
        private string ApiUrl = @"https://www.parsehub.com/api/v2";
        public ProjectRunner(string apiKey)
        {
            ApiKey = apiKey;
        }
        public ProjectRunner(string apiKey, string apiUrl)
        {
            ApiKey = apiKey;
            ApiUrl = apiUrl;
        }

        public List<Project> GetAllProjects()
        {
            var client = new RestClient(ApiUrl);

            var request = new RestRequest("projects", Method.GET);
            request.AddParameter("api_key", ApiKey);

            IRestResponse response = client.Execute(request);
            JObject jsonResponse = (JObject)JsonConvert.DeserializeObject(response.Content);
            List<Project> projects = JsonConvert.DeserializeObject<List<Project>>(jsonResponse["projects"].ToString());

            return projects;
        }

        public Project GetProject(string projectToken)
        {
            var client = new RestClient(ApiUrl);

            var request = new RestRequest("projects/{token}", Method.GET);
            request.AddParameter("api_key", ApiKey);
            request.AddUrlSegment("token", projectToken);

            IRestResponse response = client.Execute(request);
            Project project = JsonConvert.DeserializeObject<Project>(response.Content);

            return project;
        }

        public Run RunProject(string projectToken, string startUrl = null, string startTemplate = null, string startValueOverride = null, bool sendEmail = false)
        {
            Run run = null;
            var client = new RestClient(ApiUrl);

            var request = new RestRequest("projects/{token}/run", Method.POST);
            request.AddParameter("api_key", ApiKey);
            request.AddUrlSegment("token", projectToken);

            if (!string.IsNullOrEmpty(startUrl))
                request.AddParameter("start_url", HttpUtility.UrlEncode(startUrl));

            if (!string.IsNullOrEmpty(startTemplate))
                request.AddParameter("start_template", HttpUtility.UrlEncode(startTemplate));

            if (!string.IsNullOrEmpty(startValueOverride))
                request.AddParameter("start_value_override", startValueOverride);

            if (sendEmail)
                request.AddParameter("send_email", 1);

            IRestResponse response = client.Execute(request);
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
                run = JsonConvert.DeserializeObject<Run>(response.Content);

            return run;
        }

        public Run GetRun(string runToken)
        {
            var client = new RestClient(ApiUrl);

            var request = new RestRequest("runs/{token}", Method.GET);
            request.AddParameter("api_key", ApiKey);
            request.AddUrlSegment("token", runToken);

            IRestResponse response = client.Execute(request);
            Run run = JsonConvert.DeserializeObject<Run>(response.Content);
            if (run != null)
            {
                if (run.Status.ToLower().Equals("complete"))
                    run.Data = GetRunData(runToken);
            }

            return run;
        }

        private string GetRunData(string runToken)
        {
            var client = new RestClient(ApiUrl);

            var request = new RestRequest("runs/{token}/data", Method.GET);
            request.AddParameter("api_key", ApiKey);
            request.AddUrlSegment("token", runToken);

            IRestResponse response = client.Execute(request);
            return response.Content;
        }
    }
}
