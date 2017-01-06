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

            List<Project> projects = null;

            IRestResponse response = client.Execute(request);
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                JObject jsonResponse = (JObject)JsonConvert.DeserializeObject(response.Content);
                projects = JsonConvert.DeserializeObject<List<Project>>(jsonResponse["projects"].ToString());
            }
            else
                throw new Exception("Expected response status code OK, received response status code " + response.StatusCode);

            return projects;
        }

        public Project GetProject(string projectToken)
        {
            var client = new RestClient(ApiUrl);
            var request = new RestRequest("projects/{token}", Method.GET);
            request.AddParameter("api_key", ApiKey);
            request.AddUrlSegment("token", projectToken);

            Project project = null;

            IRestResponse response = client.Execute(request);
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                project = JsonConvert.DeserializeObject<Project>(response.Content);
            }
            else
                throw new Exception("Expected response status code OK, received response status code " + response.StatusCode);

            return project;
        }

        public Run RunProject(string projectToken, string startUrl = null, string startTemplate = null, string startValueOverride = null, bool sendEmail = false)
        {
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

            Run run = null;

            IRestResponse response = client.Execute(request);
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
                run = JsonConvert.DeserializeObject<Run>(response.Content);
            else
                throw new Exception("Expected response status code OK, received response status code " + response.StatusCode);

            return run;
        }

        public Run GetRun(string runToken)
        {
            var client = new RestClient(ApiUrl);

            var request = new RestRequest("runs/{token}", Method.GET);
            request.AddParameter("api_key", ApiKey);
            request.AddUrlSegment("token", runToken);

            Run run = null;

            IRestResponse response = client.Execute(request);
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                run = JsonConvert.DeserializeObject<Run>(response.Content);
                if (run != null && run.Status != null)
                {
                    if (run.Status.ToLower().Equals("complete"))
                        run.Data = GetRunData(runToken);
                }
                else
                    throw new Exception("Response was OK, but ParseHub Run or Run Status is null");
            }
            else
                throw new Exception("Expected response status code OK, received response status code " + response.StatusCode);

            return run;
        }

        private string GetRunData(string runToken)
        {
            var client = new RestClient(ApiUrl);

            var request = new RestRequest("runs/{token}/data", Method.GET);
            request.AddParameter("api_key", ApiKey);
            request.AddUrlSegment("token", runToken);
            string runData = String.Empty;

            IRestResponse response = client.Execute(request);
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
                runData = response.Content;
            else
                throw new Exception("Expected response status code OK, received response status code " + response.StatusCode);

            return runData;
        }
    }
}
