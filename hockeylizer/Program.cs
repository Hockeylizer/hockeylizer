using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;

namespace hockeylizer
{
    public class Program
    {
        public static void Main(string[] args)
        {
            //var shots = Services.AnalysisBridge.DecodeFrames("C:\\Users\\holger\\mvk-sudo\\CppConversion\\IMG_8068.MOV", Services.BlobCredentials.AccountName, Services.BlobCredentials.Key, "holgers-test", new Models.DecodeInterval[1] { new Models.DecodeInterval(0, 6000) });
            var host = new WebHostBuilder()
                .UseKestrel()
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseIISIntegration()
                .UseStartup<Startup>()
                .Build();

            host.Run();
        }
    }
}
