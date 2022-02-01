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
using TestPassModel.Structures.Lib;
using TestPassModule.Api.Data;
using TestPassModule.Api.Declare;
using TestPassModule.Api.Manager;
using TestPassModule.Modules.Display;
using TestPassModule.Structs.Lib;
using TestPassModule.Ui.Colorize;

namespace TestPassModule.Structs.Display;
/// <summary>
/// Interaction logic for StructurePackage.xaml
/// </summary>
public partial class StructurePackage : UserControl {
    public Action<StructureRequest> OnItemRequest;
    public Action<string, int> OnPackageRequest;

    public StructurePackage() {
        InitializeComponent();
        this.Loaded += StructurePackage_Loaded;
    }

    private void StructurePackage_Loaded(object sender, RoutedEventArgs e) {
        Display_Parts();
    }

    public string scope { get; internal set; }
    public int scopeid { get; internal set; }

    public int level { get; internal set; }
    public int ParentID { get; internal set; }
    public Point BasePoint { get; internal set; }


    private void Display_Parts() {


        //for (double tx = 0; tx < 360; tx++) {
        //    var y = new Line();
        //    y.Stroke = Brushes.Red;

        //    y.X1 = 40 * Math.Sin((tx).ToRadians());
        //    y.Y1 = 40 * Math.Cos((tx).ToRadians());

        //    y.X2 = 140 * Math.Sin((tx).ToRadians());
        //    y.Y2 = 140 * Math.Cos((tx).ToRadians());

        //    Lines.Children.Add(y);

        //    Canvas.SetTop(y, 200);
        //    Canvas.SetLeft(y, 200);

        //}




        LineContainer.Children.Clear();
        PartContainer.Children.Clear();

        var filtered = ActiveData.General.Structs.Where(h => h.scope == scope && h.position.parent == ParentID).ToList();


        double CurrentDeg = 0, StartDeg = 90;
        var incdeg = 20;
        var longtile = 140;
        if (filtered.Count > 5) {
            longtile += filtered.Count * 5;
            incdeg = 15;
        }

        if (filtered.Count > 1) StartDeg -= (filtered.Count - 1) * (incdeg / 2);

        var index = 0;

        for (int i = 0; i < filtered.Count; i++) {

            var part = filtered[i];


            CurrentDeg = StartDeg + i * incdeg;
            if (i > filtered.Count / 2) index--; else index++;

            var hbtn = new ModulePartView(part);
            Panel.SetZIndex(hbtn, index);

            hbtn.ToolTip = CurrentDeg + " - " + i;
            hbtn.Margin = new Thickness(-20, -20, 0, 0);
            hbtn.OnRequest += (cmd, id, p) => {

                if (cmd == "move-to-sub") {
                    var builder = new StructBuilder();
                    builder.Create(this.scope, part.position.parent);                    
                    builder.item.position.index = part.position.index;
                    part.position.parent = builder.item.id;
                    OnPackageRequest("refresh", level);
                    Display_Parts();
                    return;
                }

                if (cmd == "del") {
                    ActiveData.General.Structs.RemoveAll(h => h.id == id);
                    new StructLoader().Save();
                    OnPackageRequest("refresh", level);
                    Display_Parts();
                    return;
                }

                var hx = new StructureRequest();
                hx.level = level;
                hx.id = id;
                hx.p = new Point(BasePoint.X + p.X, BasePoint.Y + p.Y - 230);
                hx.scopeid = scopeid;
                hx.command = cmd;

                OnItemRequest?.Invoke(hx);
            };
            PartContainer.Children.Add(hbtn);

            var NY = new Line();
            NY.Stroke = BrushGenerator.FromString("#646464");

            NY.X1 = 10 * Math.Sin(CurrentDeg.ToRadians());
            NY.Y1 = -10 * Math.Cos(CurrentDeg.ToRadians());
            NY.X2 = longtile * Math.Sin(CurrentDeg.ToRadians());

            NY.Y2 = -longtile * Math.Cos(CurrentDeg.ToRadians());

            LineContainer.Children.Add(NY);

            Canvas.SetLeft(NY, 0);
            Canvas.SetTop(NY, 230);


            Canvas.SetLeft(hbtn, NY.X2 + 0);
            Canvas.SetTop(hbtn, NY.Y2 + 230);
            hbtn.GPoint = new Point(NY.X2 + 0, NY.Y2 + 230);




        }

        CurrentDeg = StartDeg + filtered.Count * incdeg;
        var NBtn = new NewStructButton();
        NBtn.Margin = new Thickness(-20, -20, 0, 0);
        PartContainer.Children.Add(NBtn);
        NBtn.OnRequest += NewItemAdd;
        var y = new Line();
        y.Stroke = BrushGenerator.FromString("#70464646");
        y.StrokeDashArray = new DoubleCollection() { 5, 5 };
        y.X1 = 10 * Math.Sin(CurrentDeg.ToRadians());
        y.Y1 = -10 * Math.Cos(CurrentDeg.ToRadians());
        y.X2 = longtile * Math.Sin(CurrentDeg.ToRadians());

        y.Y2 = -longtile * Math.Cos(CurrentDeg.ToRadians());

        LineContainer.Children.Add(y);

        Canvas.SetLeft(y, 0);
        Canvas.SetTop(y, 230);


        Canvas.SetLeft(NBtn, y.X2 + 0);
        Canvas.SetTop(NBtn, y.Y2 + 230);


    }


    private void NewItemAdd() {
        new StructBuilder().Create(this.scope, this.ParentID);
        OnPackageRequest("refresh", level);
        Display_Parts();
    }


}
public static class Foo {

    public static double ToRadians(this double angleIn10thofaDegree) {
        // Angle in 10th of a degree
        return (angleIn10thofaDegree * Math.PI) / 180.0;
    }

}
