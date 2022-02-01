using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using TestPassModule.Api.Data;
using TestPassModule.Api.Declare;
using TestPassModule.Api.Manager;
using TestPassModule.Structs.Display;
using TestPassModule.Structs.Lib;

namespace TestPassModule.Modules.Pages;
/// <summary>
/// Interaction logic for ModulePartPage.xaml
/// </summary>
public partial class ModulePartPage : Page {
    public Action<StructureRequest> OnRequest;

    public string scope { get; set; }
    public int scopeid { get; private set; }
    public int level { get; set; }

    public ModulePartPage() {

        InitializeComponent();

    }


    internal void Display(int scopeid, int level, int id, Point p) {


        OnPackageRequest("refresh", level);

        var hbo = new StructurePackage();
        hbo.scope = "module:" + scopeid;
        hbo.scopeid = scopeid;
        hbo.ParentID = id;
        hbo.level = level +1;

        hbo.OnItemRequest = this.OnRequest;
        hbo.OnPackageRequest = this.OnPackageRequest;
        hbo.BasePoint = p;

        hbo.Margin = new Thickness(p.X, p.Y, 0, 0);

        hbo.HorizontalAlignment = HorizontalAlignment.Left;
        hbo.VerticalAlignment = VerticalAlignment.Top;

        ContentContainer.Children.Add(hbo);


    }

    private void OnPackageRequest(string cmd, int level) {
        if (cmd == "refresh") {
            ContentContainer.Children.OfType<StructurePackage>().Where(u => u.level > level).ToList()
         .ForEach(k => ContentContainer.Children.Remove(k));
        }
    }
}


