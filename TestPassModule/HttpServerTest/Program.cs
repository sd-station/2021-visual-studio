using System;
using System;
using System.Net;
using System.IO;
using System.Text;
using System.Linq;
using System.Diagnostics;
using System.Collections.Generic;

namespace HttpServerTest {
    internal class Program {

        static string RootDir = @"C:\Program Files\SaveData\TestPass\Client";
        static string ApiDir = @"C:\Program Files\SaveData\TestPass\Data";
        static string Senario = "";
        static void Main(string[] args) {

            var dic = new Dictionary<string, string>();
            if (args.Any()) {
                args.ToList().ForEach(value => {
                    if (value.StartsWith("--")) dic.Add(value, "");
                    else dic[dic.Keys.Last()] = value;
                });              
            }
            if (dic.Keys.Contains("--dir") && !string.IsNullOrWhiteSpace(dic["--dir"])) {
                ApiDir = dic["--dir"];
                Console.WriteLine("Api Update {0}", ApiDir);
            }
            if (dic.Keys.Contains("--senario") && !string.IsNullOrWhiteSpace(dic["--senario"])) {
                Senario = dic["--senario"];
                Console.WriteLine("Senario Update {0}", Senario);
            }

            if (Directory.Exists(@"E:\VisualStudio\TestPassModule\root")) {
                RootDir = @"E:\VisualStudio\TestPassModule\root";
                Console.WriteLine("Development Mode");
            }

            var port = 1652;
            var domain = $"http://localhost:{port}/index.html";

            if (!string.IsNullOrWhiteSpace(Senario)) domain = $"{domain}?senario={Senario}";

            HttpListener server = new HttpListener();
            server.Prefixes.Add($"http://127.0.0.1:{port}/");
            server.Prefixes.Add($"http://localhost:{port}/");
            ProcessStartInfo psi = new ProcessStartInfo {
                FileName = "cmd.exe",
                Arguments = $"/C start " + domain,
                WindowStyle = ProcessWindowStyle.Hidden,
                CreateNoWindow = true
            };
            Process.Start(psi);
            server.Start();

            Console.WriteLine($"Listening... {port}" );

            while (true) {
                HttpListenerContext context = server.GetContext();
                HttpListenerResponse response = context.Response;

                string page = context.Request.Url.LocalPath.Replace("/", "\\");

                page = page.EndsWith(".json") ? ApiDir + page : RootDir + page;

                if (page == string.Empty)
                    page = "index.html";

                string msg = page + " File Not Found!";
                if (System.IO.File.Exists(page)) {
                    using (TextReader tr = new StreamReader(page)) {
                        msg = tr.ReadToEnd();
                    };

                } else context.Response.StatusCode = (int)HttpStatusCode.NotFound;


                byte[] buffer = Encoding.UTF8.GetBytes(msg);
                string mime;
                context.Response.ContentType = Lib.HttpServer._mimeTypeMappings.TryGetValue(Path.GetExtension(page), out mime) ? mime : "application/octet-stream";

                context.Response.AddHeader("Date", DateTime.Now.ToString("r"));

                response.ContentLength64 = buffer.Length;
                Stream st = response.OutputStream;
                st.Write(buffer, 0, buffer.Length);

                context.Response.Close();
            }

        }
    }
}
