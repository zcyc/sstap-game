using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;

namespace SSTap.Model
{
    internal class UserInfo
    {
        public string BaseUrl = Config.BaseUrl;
        public string Username;
        public string Name;
        public string Password;
        public int Class;
        public string Token;
        public int Id;
        public float Money;
        public string NodePassword;
        public int Port;
        public List<SsManagerNode> Nodes;
        public RestClient client;
        public Notice notice;
        public long flow;
        public long transfer_enable;
        public long used;
        public long Expires;
        public string SubLink;
        public string msg;

        public UserInfo()
        {
            this.Username = "";
            this.Name = "";
            this.Id = 0;
            this.NodePassword = "";
            this.Port = 0;
            this.flow = 0L;
            this.used = 0L;
            this.SubLink = "";
            this.msg = "";
            this.Nodes = new List<SsManagerNode>();
            this.client = new RestClient();
            this.client.BaseUrl = new Uri(this.BaseUrl);
            this.client.CookieContainer = new CookieContainer();
            this.notice = new Notice();
        }

        public bool Login(string username, string password)
        {
            string text = "/auth/logins";
            RestRequest restRequest = new RestRequest(text, (Method)1);
            restRequest.AddParameter("email", username);
            restRequest.AddParameter("passwd", password);
            this.client.Execute(restRequest);
            text = "/api/token";
            RestRequest restRequest2 = new RestRequest(text, (Method)1);
            restRequest2.AddParameter("email", username);
            restRequest2.AddParameter("passwd", password);
            IRestResponse restResponse = this.client.Execute(restRequest2);
            bool flag = restResponse.StatusCode != HttpStatusCode.OK;
            bool result;
            if (flag)
            {
                result = false;
            }
            else
            {
                JsonTextReader jsonTextReader = new JsonTextReader(new StringReader(restResponse.Content));
                while (jsonTextReader.Read())
                {
                    bool flag2 = jsonTextReader.TokenType == (JsonToken)1;
                    if (flag2)
                    {
                        JObject jobject = JObject.Load(jsonTextReader);
                        int num = (int)jobject["ret"];
                        this.msg = (string)jobject["msg"];
                        bool flag3 = num != 1;
                        if (flag3)
                        {
                            return false;
                        }
                        JToken jtoken = jobject["data"];
                        this.Id = (int)jtoken["user_id"];
                        this.Token = (string)jtoken["token"];
                        break;
                    }
                }
                bool account = this.GetAccount();
                if (account)
                {
                    this.Password = password;
                    this.Username = username;
                    result = true;
                }
                else
                {
                    result = false;
                }
            }
            return result;
        }

        public bool GetAccount()
        {
            string text = "/api/user/" + this.Id;
            RestRequest restRequest = new RestRequest(text, (Method)0);
            restRequest.AddParameter("access_token", this.Token);
            IRestResponse restResponse = this.client.Execute(restRequest);
            bool flag = restResponse.StatusCode != HttpStatusCode.OK;
            bool result;
            if (flag)
            {
                result = false;
            }
            else
            {
                JsonTextReader jsonTextReader = new JsonTextReader(new StringReader(restResponse.Content));
                while (jsonTextReader.Read())
                {
                    bool flag2 = jsonTextReader.TokenType == (JsonToken)1;
                    if (flag2)
                    {
                        JObject jobject = JObject.Load(jsonTextReader);
                        int num = (int)jobject["ret"];
                        bool flag3 = num != 1;
                        if (flag3)
                        {
                            return false;
                        }
                        JToken jtoken = jobject["data"];
                        this.Port = (int)jtoken["port"];
                        this.NodePassword = (string)jtoken["passwd"];
                        this.Expires = UserInfo.GetTimeStamp((string)jtoken["class_expire"]);
                        this.Name = (string)jtoken["user_name"];
                        this.Money = (float)jtoken["money"];
                        this.Class = (int)jtoken["class"];
                        this.transfer_enable = (long)jtoken["transfer_enable"];
                        this.used = (long)jtoken["u"] + (long)jtoken["d"];
                        return true;
                    }
                }
                result = true;
            }
            return result;
        }

        public bool GetSubLink()
        {
            string text = "/api/sublink";
            RestRequest restRequest = new RestRequest(text, (Method)0);
            restRequest.AddParameter("access_token", this.Token);
            IRestResponse restResponse = this.client.Execute(restRequest);
            bool flag = restResponse.StatusCode != HttpStatusCode.OK;
            bool result;
            if (flag)
            {
                result = false;
            }
            else
            {
                JsonTextReader jsonTextReader = new JsonTextReader(new StringReader(restResponse.Content));
                while (jsonTextReader.Read())
                {
                    bool flag2 = jsonTextReader.TokenType == (JsonToken)1;
                    if (flag2)
                    {
                        JObject jobject = JObject.Load(jsonTextReader);
                        int num = (int)jobject["ret"];
                        bool flag3 = num != 1;
                        if (flag3)
                        {
                            return false;
                        }
                        this.SubLink = (string)jobject["data"];
                        return true;
                    }
                }
                result = true;
            }
            return result;
        }

        public bool GetNodes()
        {
            string text = "/api/node";
            RestRequest restRequest = new RestRequest(text, (Method)0);
            restRequest.AddParameter("access_token", this.Token);
            IRestResponse restResponse = this.client.Execute(restRequest);
            bool flag = restResponse.StatusCode != HttpStatusCode.OK;
            bool result;
            if (flag)
            {
                result = false;
            }
            else
            {
                JsonTextReader jsonTextReader = new JsonTextReader(new StringReader(restResponse.Content));
                this.Nodes.Clear();
                while (jsonTextReader.Read())
                {
                    bool flag2 = jsonTextReader.TokenType == (JsonToken)1;
                    if (flag2)
                    {
                        JObject jobject = JObject.Load(jsonTextReader);
                        int num = (int)jobject["ret"];
                        bool flag3 = num != 1;
                        if (flag3)
                        {
                            return false;
                        }
                        JArray jarray = (JArray)jobject["data"];
                        foreach (JToken jtoken in jarray)
                        {
                            JObject jobject2 = (JObject)jtoken;
                            SsManagerNode ssManagerNode = new SsManagerNode();
                            ssManagerNode.method = (string)jobject2["method"];
                            ssManagerNode.name = (string)jobject2["remarks"];
                            ssManagerNode.group = (string)jobject2["group"];
                            ssManagerNode.obfs = (string)jobject2["obfs"];
                            ssManagerNode.obfsparam = (string)jobject2["obfsparam"];
                            ssManagerNode.remarks_base64 = (string)jobject2["remarks_base64"];
                            ssManagerNode.udp_over_tcp = (bool)jobject2["udp_over_tcp"];
                            ssManagerNode.protocol = (string)jobject2["protocol"];
                            ssManagerNode.enable = (bool)jobject2["enable"];
                            ssManagerNode.host = (string)jobject2["server"];
                            bool flag4 = ssManagerNode.host.Contains(":");
                            if (flag4)
                            {
                                ssManagerNode.host = ssManagerNode.host.Substring(ssManagerNode.host.IndexOf(':') + 1);
                            }
                            this.Nodes.Add(ssManagerNode);
                        }
                    }
                }
                result = true;
            }
            return result;
        }

        public long GetNodeFlow(int userid, int nodeid)
        {
            string text = string.Concat(new object[]
            {
                "/api/user/flow/",
                nodeid,
                "/",
                userid
            });
            RestRequest restRequest = new RestRequest(text, (Method)0);
            IRestResponse restResponse = this.client.Execute(restRequest);
            bool flag = restResponse.StatusCode != HttpStatusCode.OK;
            long result;
            if (flag)
            {
                result = -1L;
            }
            else
            {
                JsonTextReader jsonTextReader = new JsonTextReader(new StringReader(restResponse.Content));
                while (jsonTextReader.Read())
                {
                    bool flag2 = jsonTextReader.TokenType == (JsonToken)2;
                    if (flag2)
                    {
                        JArray jarray = JArray.Load(jsonTextReader);
                        return (long)jarray[0];
                    }
                }
                result = -1L;
            }
            return result;
        }

        public QrCodeOrder GetPayQrCode(string order_type)
        {
            string text = "/api/user/order/qrcode";
            RestRequest restRequest = new RestRequest(text, (Method)1);
            restRequest.AddParameter("accountId", this.Id);
            restRequest.AddParameter("orderType", order_type);
            IRestResponse restResponse = this.client.Execute(restRequest);
            bool flag = restResponse.StatusCode != HttpStatusCode.OK;
            QrCodeOrder result;
            if (flag)
            {
                result = null;
            }
            else
            {
                QrCodeOrder qrCodeOrder = new QrCodeOrder();
                JsonTextReader jsonTextReader = new JsonTextReader(new StringReader(restResponse.Content));
                while (jsonTextReader.Read())
                {
                    bool flag2 = jsonTextReader.TokenType == (JsonToken)1;
                    if (flag2)
                    {
                        JObject jobject = JObject.Load(jsonTextReader);
                        qrCodeOrder.OrderId = (string)jobject["orderId"];
                        qrCodeOrder.QrCode = (string)jobject["qrCode"];
                        return qrCodeOrder;
                    }
                }
                result = null;
            }
            return result;
        }

        public AlipayPrice GetAlipayPrice()
        {
            string text = "/api/user/order/price";
            RestRequest restRequest = new RestRequest(text, (Method)0);
            IRestResponse restResponse = this.client.Execute(restRequest);
            bool flag = restResponse.StatusCode != HttpStatusCode.OK;
            AlipayPrice result;
            if (flag)
            {
                result = null;
            }
            else
            {
                JsonTextReader jsonTextReader = new JsonTextReader(new StringReader(restResponse.Content));
                AlipayPrice alipayPrice = new AlipayPrice();
                while (jsonTextReader.Read())
                {
                    bool flag2 = jsonTextReader.TokenType == (JsonToken)1;
                    if (flag2)
                    {
                        JObject jobject = JObject.Load(jsonTextReader);
                        JToken jtoken = jobject["alipay"];
                        try
                        {
                            alipayPrice.Hour = (int)jtoken["hour"];
                        }
                        catch (ArgumentNullException)
                        {
                        }
                        try
                        {
                            alipayPrice.Day = (int)jtoken["day"];
                        }
                        catch (ArgumentNullException)
                        {
                        }
                        try
                        {
                            alipayPrice.Week = (int)jtoken["week"];
                        }
                        catch (ArgumentNullException)
                        {
                        }
                        try
                        {
                            alipayPrice.Month = (int)jtoken["month"];
                        }
                        catch (ArgumentNullException)
                        {
                        }
                        try
                        {
                            alipayPrice.Season = (int)jtoken["season"];
                        }
                        catch (ArgumentNullException)
                        {
                        }
                        try
                        {
                            alipayPrice.Year = (int)jtoken["year"];
                        }
                        catch (ArgumentNullException)
                        {
                        }
                        return alipayPrice;
                    }
                }
                result = null;
            }
            return result;
        }

        public string UpdateNodeFlow()
        {
            long used = this.used;
            long transferEnable = this.transfer_enable;
            float num = (float)((double)used / (double)transferEnable * 100.0);
            return this.ConvertBytes(used) + "/" + this.ConvertBytes(transferEnable) + "\n (" + string.Format("{0:F}", (object)num) + "%)";
        }

        private SsManagerNode GetNodeByNodeId(int node_id)
        {
            foreach (SsManagerNode node in this.Nodes)
                ;
            return (SsManagerNode)null;
        }

        public bool GetLatestNotice()
        {
            Notice notice = new Notice();
            string text = "/api/announcement";
            RestRequest restRequest = new RestRequest(text, (Method)0);
            restRequest.AddParameter("access_token", this.Token);
            IRestResponse restResponse = this.client.Execute(restRequest);
            bool flag = restResponse.StatusCode != HttpStatusCode.OK;
            bool result;
            if (flag)
            {
                result = false;
            }
            else
            {
                JsonTextReader jsonTextReader = new JsonTextReader(new StringReader(restResponse.Content));
                while (jsonTextReader.Read())
                {
                    bool flag2 = jsonTextReader.TokenType == (JsonToken)1;
                    if (flag2)
                    {
                        JObject jobject = JObject.Load(jsonTextReader);
                        int num = (int)jobject["ret"];
                        bool flag3 = num != 1;
                        if (flag3)
                        {
                            return false;
                        }
                        JToken jtoken = jobject["data"];
                        notice.time = UserInfo.GetTimeStamp((string)jtoken["date"]);
                        notice.title = "最新公告";
                        notice.content = (string)jtoken["content"];
                        this.notice = notice;
                        return true;
                    }
                }
                result = false;
            }
            return result;
        }

        public string ConvertBytes(long len)
        {
            string[] strArray = new string[5]
            {
        "Bytes",
        "KB",
        "MB",
        "GB",
        "TB"
            };
            int index;
            for (index = 0; len >= 1024L && index + 1 < strArray.Length; len /= 1024L)
                ++index;
            return string.Format("{0:0.##} {1}", (object)len, (object)strArray[index]);
        }

        public bool ChangePassword(string currentpass, string newpass)
        {
            string text = "/user/password";
            RestRequest restRequest = new RestRequest(text, (Method)1);
            restRequest.AddParameter("pwd", newpass);
            restRequest.AddParameter("repwd", newpass);
            restRequest.AddParameter("oldpwd", currentpass);
            IRestResponse restResponse = this.client.Execute(restRequest);
            bool flag = restResponse.StatusCode != HttpStatusCode.OK;
            bool result;
            if (flag)
            {
                result = false;
            }
            else
            {
                JsonTextReader jsonTextReader = new JsonTextReader(new StringReader(restResponse.Content));
                while (jsonTextReader.Read())
                {
                    bool flag2 = jsonTextReader.TokenType == (JsonToken)1;
                    if (flag2)
                    {
                        JObject jobject = JObject.Load(jsonTextReader);
                        int num = (int)jobject["ret"];
                        return num == 1;
                    }
                }
                result = false;
            }
            return result;
        }

        public bool CheckStatus(string orderId)
        {
            string text = "/api/user/order/status";
            RestRequest restRequest = new RestRequest(text, (Method)1);
            restRequest.AddParameter("orderId", orderId);
            IRestResponse restResponse = this.client.Execute(restRequest);
            bool flag = restResponse.StatusCode != HttpStatusCode.OK;
            bool result;
            if (flag)
            {
                result = false;
            }
            else
            {
                JsonTextReader jsonTextReader = new JsonTextReader(new StringReader(restResponse.Content));
                while (jsonTextReader.Read())
                {
                    bool flag2 = jsonTextReader.TokenType == (JsonToken)1;
                    if (flag2)
                    {
                        JObject jobject = JObject.Load(jsonTextReader);
                        string a = (string)jobject["status"];
                        return a == "TRADE_SUCCESS";
                    }
                }
                result = false;
            }
            return result;
        }

        public static long GetTimeStamp(string DateStr)
        {
            DateTime localTime = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1, 0, 0, 0, 0));
            return (long)Math.Round((Convert.ToDateTime(DateStr) - localTime).TotalMilliseconds, MidpointRounding.AwayFromZero);
        }
    }
}
