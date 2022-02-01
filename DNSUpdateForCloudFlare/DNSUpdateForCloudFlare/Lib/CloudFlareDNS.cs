using Nancy.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lib;
internal class CloudFlareDNS {

    internal async Task<List<DomainDNS>> GetZoneAsync(string id) {
        var responseString = "";
        HttpClient client = new HttpClient();

        try {
            responseString = await client.GetStringAsync("http://localhost:8101/cf/dns/index/" + id);

        } catch (Exception ex) {
            Console.WriteLine(ex.Message);
            return new List<DomainDNS>();
        }

        var JSE = new JavaScriptSerializer();

        var k = JSE.Deserialize<List<DomainDNS>>(responseString);

        return k;
    }
}
 


public class Meta {
    public bool auto_added { get; set; }
    public bool managed_by_apps { get; set; }
    public bool managed_by_argo_tunnel { get; set; }
    public string source { get; set; }
}

public class DomainDNS {
    public string id { get; set; }
    public string zone_id { get; set; }
    public string zone_name { get; set; }
    public string name { get; set; }
    public string type { get; set; }
    public string content { get; set; }
    public bool proxiable { get; set; }
    public bool proxied { get; set; }
    public int ttl { get; set; }
    public bool locked { get; set; }
    public Meta meta { get; set; }
    public DateTime created_on { get; set; }
    public DateTime modified_on { get; set; }
}

 
