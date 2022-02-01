
void log(string text) {
    Console.WriteLine("    " + text);
}

void tag(string tag, string text) {
    Console.WriteLine("{0} {1}", $"[{tag.ToUpper()}]", text);
}
var newIp = "";

/// <summary>
/// History Checking
/// </summary>
tag("Start Task", "Load History");
var oldip = "";
var fname = Path.Combine(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), "last-ip.txt");
log("DIR:" + fname);
if (File.Exists(fname)) {
    oldip = File.ReadAllText(fname);
    log("data found: " + oldip);
} else log("No History");

//newIp = oldip;
//HttpClient client = new HttpClient();

//var watch = new System.Diagnostics.Stopwatch();
//string externalip = "";

//watch.Start();
//externalip = await client.GetStringAsync("https://ipv4.icanhazip.com/");  
//tag("OP", externalip);
//watch.Stop();
//Console.WriteLine($"Execution Time: {watch.ElapsedMilliseconds} ms");

//watch.Reset();
//watch.Start();
//externalip = await client.GetStringAsync("http://checkip.dyndns.org/");
//tag("OP", externalip);
//watch.Stop();
//Console.WriteLine($"Execution Time: {watch.ElapsedMilliseconds} ms");


//watch.Reset();
//watch.Start();
//externalip = await client.GetStringAsync("http://api.ipify.org");
//tag("OP", externalip);
//watch.Stop();
//Console.WriteLine($"Execution Time: {watch.ElapsedMilliseconds} ms");


/// <summary>
/// New IP Address
/// </summary>
tag("Start Task", "Capturing Ip");
var ark = new Lib.IpFinder();
var responseString =await ark.GetIPAdressAsync();

tag("Start Task", "Check Ip Is Valied");
if (responseString.Split(".").Length == 4) {
    newIp = responseString.Trim();
    log("valid: " + newIp);

} else {
    log("Ip Is Invalid");
    return;
}

tag("Start Task", "Check It IS New IP");
if (oldip.Equals(newIp)) {
    log("same: " + oldip);
    return;
} else log($"IS NOT SAME: [{oldip}] = [{newIp}]"  );


tag("Start Task", "Get Donmain From Cloud Flare");
var data = await new Lib.CloudFlareZone().GetZoneAsync();
tag("Zone Found", "Number:" + data.Count);



foreach (var zone in data) {



    if (zone.status != "active") return;
    var datasub = await new Lib.CloudFlareDNS().GetZoneAsync(zone.id);

    tag("DNS", "Number:" + datasub.Count);


    foreach (var dns in datasub) {

        if (string.IsNullOrWhiteSpace(dns.content)) {
            log("invalid.\t" + dns.name);
            continue;
        }

        if (dns.content.Split(".").Length != 4) {
            log("Not IP. \t" + dns.name);
            continue;
        }

        var shouldUpdate = dns.type == "A" && dns.content != newIp;
        Console.WriteLine("    {0}\t{1}\t{2}\t{3}", shouldUpdate ? "UPDATE" : "FINE", dns.type, dns.content, dns.name);

        if (shouldUpdate) {
            tag("DOMAIN UPDATE", dns.name);
            dns.content = newIp;
            var res =  await new Lib.CloudFlareUpdate().UpdateZone(dns);
            log(res); 
        }

    }
}

tag("Start Task", "Save Current Ip");
File.WriteAllText(fname, newIp);

