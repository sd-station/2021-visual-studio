using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Lib {
    internal class IpFinder {
        public string Responsetext { get; set; }

        public async Task<string> GetIPAdressAsync() {

            var runned = await Method1Async("https://ipv4.icanhazip.com/");
            if(runned) return Responsetext;

            runned = await Method1Async("http://api.ipify.org");
            if (runned) return Responsetext;


            return "";
        }

        private async Task<bool> Method1Async(string link) {
            HttpClient client = new HttpClient();
            try {
                this.Responsetext = await client.GetStringAsync(link);
            } catch (Exception ex) {
               
                return false;
            }

            if(this.Responsetext.Split(".").Length == 4)  return true;

            return false;
        }
    }
}
