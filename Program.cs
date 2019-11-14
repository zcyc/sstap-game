using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using SSTap.Controller;
using SSTap.View;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace SSTap
{
    internal static class Program
    {
        public static List<string> downloadLink = new List<string>();
        public static string newPackageName = "";
        public static string latestVersion = Config.Version;
        public static LoginController loginController;
        public static FormMain formMain;

        [STAThread]
        private static void Main()
        {
            bool createdNew = false;
            Mutex mutex = new Mutex(true, "MoeSS", out createdNew);
            if (!createdNew)
            {
                int num = (int)MessageBox.Show("有另一个实例正在运行！");
                Environment.Exit(1);
            }
            Program.CheckUpdate();
            Program.FixLsp();
            AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(Program.CurrentDomain_UnhandledException);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Program.loginController = new LoginController();
            Program.loginController.ShowLogin();
            Program.formMain = new FormMain();
            Application.Run((Form)Program.formMain);
        }

        private static void CurrentDomain_UnhandledException(
          object sender,
          UnhandledExceptionEventArgs e)
        {
            int num = (int)MessageBox.Show(e.ExceptionObject.ToString());
            Application.Exit();
        }

        public static void FixLsp()
        {
            try
            {
                new Process()
                {
                    StartInfo = new ProcessStartInfo()
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
                Console.WriteLine(ex);
            }
        }

        public static void CheckUpdate()
        {
            Program.latestVersion = Program.getLatestVersion();
            if (!(Program.latestVersion != Config.Version))
                return;
            int num1 = (int)MessageBox.Show("发现新版本，即将自动更新！");
            try
            {
                int num2 = (int)new FormUpdate().ShowDialog();
            }
            catch (Win32Exception ex)
            {
                int num2 = (int)MessageBox.Show("更新出现问题，即将推出");
                Console.WriteLine(ex);
            }
            finally
            {
                Environment.Exit(0);
            }
        }

        private static string getLatestVersion()
        {
            string result;
            try
            {
                string text = "/api/check_update/" + Config.siteHash;
                RestClient restClient = new RestClient();
                restClient.BaseUrl = new Uri("https://client.moess.moe");
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
                Console.WriteLine(ex);
            }
            return result;
        }

        public static string GetMd5Hash(string input)
        {
            if (input == null)
                return (string)null;
            byte[] hash = MD5.Create().ComputeHash(Encoding.UTF8.GetBytes(input));
            StringBuilder stringBuilder = new StringBuilder();
            for (int index = 0; index < hash.Length; ++index)
                stringBuilder.Append(hash[index].ToString("x2"));
            return stringBuilder.ToString();
        }
    }
}
