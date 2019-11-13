using System;
using RestSharp;

namespace SSTap.Util
{
    // Token: 0x0200001C RID: 28
    internal class Report
    {
        // Token: 0x060000A5 RID: 165 RVA: 0x000089B0 File Offset: 0x00006BB0
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

        // Token: 0x04000090 RID: 144
        private static string base_url = "https://client.kaguya-hime.me";
    }
}
