using Nancy.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Lib;


public class CloudFlareZone {
    internal async Task<List<ClodeFlareZone>> GetZoneAsync() {
        var responseString = "";
        HttpClient client = new HttpClient();

        try {
            responseString = await client.GetStringAsync("http://localhost:8101/cf/zones/index");

        } catch (Exception ex) {
            Console.WriteLine(ex.Message);
            return new List<ClodeFlareZone>();
        }

        var JSE = new JavaScriptSerializer();

        var k = JSE.Deserialize<List<ClodeFlareZone>>(responseString);

        return k;
    }
}




public class ClodeFlareZone {

    public string id { get; set; }
    public string name { get; set; }
    public string status { get; set; }
}