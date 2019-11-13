using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;

namespace SSTap.Model
{
    // Token: 0x02000023 RID: 35
    internal class UserInfo
    {
        // Token: 0x060000B2 RID: 178 RVA: 0x00008AD8 File Offset: 0x00006CD8
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

        // Token: 0x060000B3 RID: 179 RVA: 0x00008B98 File Offset: 0x00006D98
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

        // Token: 0x060000B4 RID: 180 RVA: 0x00008D1C File Offset: 0x00006F1C
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

        // Token: 0x060000B5 RID: 181 RVA: 0x00008EE0 File Offset: 0x000070E0
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

        // Token: 0x060000B6 RID: 182 RVA: 0x00008FC0 File Offset: 0x000071C0
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

        // Token: 0x060000B7 RID: 183 RVA: 0x00009248 File Offset: 0x00007448
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

        // Token: 0x060000B8 RID: 184 RVA: 0x00009308 File Offset: 0x00007508
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

        // Token: 0x060000B9 RID: 185 RVA: 0x000093F0 File Offset: 0x000075F0
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

        // Token: 0x060000BA RID: 186 RVA: 0x000095B4 File Offset: 0x000077B4
        public string UpdateNodeFlow()
        {
            long num = this.used;
            long num2 = this.transfer_enable;
            float num3 = (float)num / (float)num2 * 100f;
            return string.Concat(new string[]
            {
                this.ConvertBytes(num),
                "/",
                this.ConvertBytes(num2),
                "\n (",
                string.Format("{0:F}", num3),
                "%)"
            });
        }

        // Token: 0x060000BB RID: 187 RVA: 0x00009634 File Offset: 0x00007834
        private SsManagerNode GetNodeByNodeId(int node_id)
        {
            foreach (SsManagerNode ssManagerNode in this.Nodes)
            {
            }
            return null;
        }

        // Token: 0x060000BC RID: 188 RVA: 0x0000968C File Offset: 0x0000788C
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

        // Token: 0x060000BD RID: 189 RVA: 0x000097BC File Offset: 0x000079BC
        public string ConvertBytes(long len)
        {
            string[] array = new string[]
            {
                "Bytes",
                "KB",
                "MB",
                "GB",
                "TB"
            };
            int num = 0;
            while (len >= 1024L && num + 1 < array.Length)
            {
                num++;
                len /= 1024L;
            }
            return string.Format("{0:0.##} {1}", len, array[num]);
        }

        // Token: 0x060000BE RID: 190 RVA: 0x0000983C File Offset: 0x00007A3C
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

        // Token: 0x060000BF RID: 191 RVA: 0x00009918 File Offset: 0x00007B18
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

        // Token: 0x060000C0 RID: 192 RVA: 0x000099DC File Offset: 0x00007BDC
        public static long GetTimeStamp(string DateStr)
        {
            DateTime d = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1, 0, 0, 0, 0));
            DateTime d2 = Convert.ToDateTime(DateStr);
            return (long)Math.Round((d2 - d).TotalMilliseconds, MidpointRounding.AwayFromZero);
        }

        // Token: 0x040000AF RID: 175
        public string BaseUrl = Config.BaseUrl;

        // Token: 0x040000B0 RID: 176
        public string Username;

        // Token: 0x040000B1 RID: 177
        public string Name;

        // Token: 0x040000B2 RID: 178
        public string Password;

        // Token: 0x040000B3 RID: 179
        public int Class;

        // Token: 0x040000B4 RID: 180
        public string Token;

        // Token: 0x040000B5 RID: 181
        public int Id;

        // Token: 0x040000B6 RID: 182
        public float Money;

        // Token: 0x040000B7 RID: 183
        public string NodePassword;

        // Token: 0x040000B8 RID: 184
        public int Port;

        // Token: 0x040000B9 RID: 185
        public List<SsManagerNode> Nodes;

        // Token: 0x040000BA RID: 186
        public RestClient client;

        // Token: 0x040000BB RID: 187
        public Notice notice;

        // Token: 0x040000BC RID: 188
        public long flow;

        // Token: 0x040000BD RID: 189
        public long transfer_enable;

        // Token: 0x040000BE RID: 190
        public long used;

        // Token: 0x040000BF RID: 191
        public long Expires;

        // Token: 0x040000C0 RID: 192
        public string SubLink;

        // Token: 0x040000C1 RID: 193
        public string msg;
    }
}
