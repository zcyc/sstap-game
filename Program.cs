using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using SSTap.Controller;
using SSTap.View;

namespace SSTap
{
    // Token: 0x02000003 RID: 3
    internal static class Program
    {
        // Token: 0x06000004 RID: 4 RVA: 0x00002094 File Offset: 0x00000294
        [STAThread]
        private static void Main()
        {
            bool flag = false;
            Mutex mutex = new Mutex(true, "kaguya", out flag);
            bool flag2 = flag;
            if (!flag2)
            {
                MessageBox.Show("有另一个实例正在运行！");
                Environment.Exit(1);
            }
            Program.CheckUpdate();
            Program.FixLsp();
            AppDomain.CurrentDomain.UnhandledException += Program.CurrentDomain_UnhandledException;
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Program.loginController = new LoginController();
            Program.loginController.ShowLogin();
            Program.formMain = new FormMain();
            Application.Run(Program.formMain);
        }

        // Token: 0x06000005 RID: 5 RVA: 0x00002129 File Offset: 0x00000329
        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            MessageBox.Show(e.ExceptionObject.ToString());
            Application.Exit();
        }

        // Token: 0x06000006 RID: 6 RVA: 0x00002144 File Offset: 0x00000344
        public static void FixLsp()
        {
            try
            {
                new Process
                {
                    StartInfo = new ProcessStartInfo
                    {
                        FileName = "cmd.exe",
                        Arguments = "/C netsh winsock reset",
                        UseShellExecute = false,
                        RedirectStandardInput = false,
                        RedirectStandardOutput = true,
                        CreateNoWindow = true
                    }
                }.Start();
            }
            catch (Exception ex)
            {
            }
        }

        // Token: 0x06000007 RID: 7 RVA: 0x000021C0 File Offset: 0x000003C0
        public static void CheckUpdate()
        {
            Program.latestVersion = Program.getLatestVersion();
            bool flag = Program.latestVersion != Config.Version;
            if (flag)
            {
                MessageBox.Show("发现新版本，即将自动更新！");
                try
                {
                    FormUpdate formUpdate = new FormUpdate();
                    formUpdate.ShowDialog();
                }
                catch (Win32Exception ex)
                {
                    MessageBox.Show("更新出现问题，即将推出");
                }
                finally
                {
                    Environment.Exit(0);
                }
            }
        }

        // Token: 0x06000008 RID: 8 RVA: 0x00002244 File Offset: 0x00000444
        private static string getLatestVersion()
        {
            string result;
            try
            {
                string text = "/api/check_update/" + Config.siteHash;
                RestClient restClient = new RestClient();
                restClient.BaseUrl = new Uri("https://client.kaguya-hime.me");
                RestRequest restRequest = new RestRequest(text, (Method)0);
                restRequest.AddParameter("current_version", Config.Version);
                IRestResponse restResponse = restClient.Execute(restRequest);
                bool flag = restResponse.StatusCode != HttpStatusCode.OK;
                if (flag)
                {
                    result = Config.Version;
                }
                else
                {
                    string text2 = "";
                    JsonTextReader jsonTextReader = new JsonTextReader(new StringReader(restResponse.Content));
                    while (jsonTextReader.Read())
                    {
                        bool flag2 = jsonTextReader.TokenType == (JsonToken)1;
                        if (flag2)
                        {
                            JObject jobject = JObject.Load(jsonTextReader);
                            text2 = (string)jobject["version"];
                            JArray jarray = (JArray)jobject["download_link"];
                            foreach (JToken jtoken in jarray)
                            {
                                JValue jvalue = (JValue)jtoken;
                                Program.downloadLink.Add((string)jvalue);
                            }
                            Program.newPackageName = (string)jobject["package_name"];
                        }
                    }
                    string[] array = Config.Version.Split(new char[]
                    {
                        '.'
                    });
                    string[] array2 = text2.Split(new char[]
                    {
                        '.'
                    });
                    bool flag3 = int.Parse(array2[0]) < int.Parse(array[0]);
                    if (flag3)
                    {
                        result = Config.Version;
                    }
                    else
                    {
                        bool flag4 = int.Parse(array2[0]) > int.Parse(array[0]);
                        if (flag4)
                        {
                            result = text2;
                        }
                        else
                        {
                            bool flag5 = int.Parse(array2[1]) < int.Parse(array[1]);
                            if (flag5)
                            {
                                result = Config.Version;
                            }
                            else
                            {
                                bool flag6 = int.Parse(array2[1]) > int.Parse(array[1]);
                                if (flag6)
                                {
                                    result = text2;
                                }
                                else
                                {
                                    bool flag7 = int.Parse(array2[2]) < int.Parse(array[2]);
                                    if (flag7)
                                    {
                                        result = Config.Version;
                                    }
                                    else
                                    {
                                        bool flag8 = int.Parse(array2[2]) > int.Parse(array[2]);
                                        if (flag8)
                                        {
                                            result = text2;
                                        }
                                        else
                                        {
                                            bool flag9 = int.Parse(array2[3]) < int.Parse(array[3]);
                                            if (flag9)
                                            {
                                                result = Config.Version;
                                            }
                                            else
                                            {
                                                bool flag10 = int.Parse(array2[3]) > int.Parse(array[3]);
                                                if (flag10)
                                                {
                                                    result = text2;
                                                }
                                                else
                                                {
                                                    result = Config.Version;
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                result = Config.Version;
            }
            return result;
        }

        // Token: 0x06000009 RID: 9 RVA: 0x00002524 File Offset: 0x00000724
        public static string GetMd5Hash(string input)
        {
            bool flag = input == null;
            string result;
            if (flag)
            {
                result = null;
            }
            else
            {
                MD5 md = MD5.Create();
                byte[] array = md.ComputeHash(Encoding.UTF8.GetBytes(input));
                StringBuilder stringBuilder = new StringBuilder();
                for (int i = 0; i < array.Length; i++)
                {
                    stringBuilder.Append(array[i].ToString("x2"));
                }
                result = stringBuilder.ToString();
            }
            return result;
        }

        // Token: 0x04000005 RID: 5
        public static LoginController loginController;

        // Token: 0x04000006 RID: 6
        public static FormMain formMain;

        // Token: 0x04000007 RID: 7
        public static List<string> downloadLink = new List<string>();

        // Token: 0x04000008 RID: 8
        public static string newPackageName = "";

        // Token: 0x04000009 RID: 9
        public static string latestVersion = Config.Version;
    }
}
