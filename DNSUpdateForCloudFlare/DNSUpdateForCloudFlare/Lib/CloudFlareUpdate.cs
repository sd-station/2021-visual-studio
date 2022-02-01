using Nancy.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lib;
internal class CloudFlareUpdate {
    // 


    internal async Task<string> UpdateZone(DomainDNS dns) {

        HttpClient client = new HttpClient();


        var R = new UpdateDNS(dns);
        var link = $"{"http"}://localhost:8101/cf/dns/update/{dns.zone_id}/{dns.id}";

        var JSE = new JavaScriptSerializer();
        var content = new StringContent(JSE.Serialize(R), Encoding.UTF8, "application/json");

        try {
            var response = await client.PostAsync(link, content);

            var responseString = await response.Content.ReadAsStringAsync();

            return responseString;

        } catch (Exception ex) {
            Console.WriteLine(ex.Message);
            return "error";
        }



        return "done";
    }

}


public class UpdateDNS {
    public UpdateDNS(DomainDNS dns) {
        type = "A";
        name = dns.name;
        content = dns.content;
        type = "A";
        ttl = 1;
        proxied = dns.proxied;
    }

    public string type { get; set; }
    public int ttl { get; set; }
    public bool proxied { get;   set; }
    public string name { get; set; }
    public string content { get; set; }
}