using RestSharp;
using System;

namespace SSTap.Util
{
    internal class Report
    {
        private static string base_url = "https://client.moess.moe";

        public static void ReportClientUpdate()
        {
            string text = "/api/log/update_log";
            RestClient restClient = new RestClient();
            restClient.BaseUrl = new Uri(Report.base_url);
            RestRequest restRequest = new RestRequest(text, (Method)0);
            restRequest.AddParameter("type", 0);
            restRequest.AddParameter("new_version", Program.latestVersion);
            restRequest.AddParameter("client_hash", Config.siteHash);
            IRestResponse restResponse = restClient.Execute(restRequest);
        }
    }
}
