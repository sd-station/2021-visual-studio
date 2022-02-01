using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace TestPassModule.Ui.Icon;


public class Icons {

    public static Geometry Up { get => new IconFinder().GetIcons("up"); }
    public static Geometry Down { get => new IconFinder().GetIcons("down"); }
    public static Geometry Split { get => new IconFinder().GetIcons("split"); }
    public static Geometry Reply { get => new IconFinder().GetIcons("reply"); }
    public static Geometry DashArrow { get => new IconFinder().GetIcons("dash-arrow"); }
    public static Geometry DownRight { get => new IconFinder().GetIcons("down-right"); }
    public static Geometry NextLocation { get => new IconFinder().GetIcons("next-location"); }
    public static Geometry Direction { get => new IconFinder().GetIcons("direction"); }
    public static Geometry Return { get => new IconFinder().GetIcons("return"); }
    public static Geometry OutPut { get => new IconFinder().GetIcons("output"); }
    public static Geometry Forward { get => new IconFinder().GetIcons("forward"); }
 
}

internal class IconFinder {

    public static List<IconClass> Icons { get; set; }

    public Geometry GetIcons(string name) {
        if (Icons == null) Icons =
              File.ReadAllLines(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Ui\\Icon\\package.txt"))
              .Where(j => !string.IsNullOrWhiteSpace(j))
              .Select(t => new IconClass(t)).ToList();

        return Geometry.Parse(Icons.FirstOrDefault(k => k.keys.Any(h => h == name))?.value ?? "M 20,20 180,20 180,160 20,160 Z");
    }
}

public class IconClass {

    public IconClass(string str) {
        this.keys = str.Substring(0, str.IndexOf(" ")).Split(',').ToList();
        this.value = str.Substring(str.IndexOf(" "));
    }

    public List<string> keys { get; private set; }
    public string value { get; private set; }


}

