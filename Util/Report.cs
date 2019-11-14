// Decompiled with JetBrains decompiler
// Type: SSTap.Util.Report
// Assembly: SS-TAP_对接91, Version=30.5.26.2, Culture=neutral, PublicKeyToken=null
// MVID: 3FC77BE2-506D-4E87-81A5-F87143593C29
// Assembly location: C:\Program Files (x86)\Kaguya\SS-TAP_对接91.exe

using RestSharp;
using System;

namespace SSTap.Util
{
  internal class Report
  {
    private static string base_url = "https://client.kaguya-hime.me";

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
