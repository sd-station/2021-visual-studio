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
using TestPassModule.RelGroup.Display;
using TestPassModule.Ui.Events;

namespace TestPassModule.RelGroup.Pages;
/// <summary>
/// Interaction logic for RelGroupIndex.xaml
/// </summary>
public partial class RelGroupIndex : Page {
    public RelGroupIndex() {
        InitializeComponent();
        this.Loaded += RelGroupIndex_Loaded;
    }

    private void RelGroupIndex_Loaded(object sender, RoutedEventArgs e) {
        Display_Data();

    }

    private void Display_Data() {
        Container.Children.Clear();

        ActiveData.RelGroup.ForEach(rg => {
            var k = new RelGroupItemPreview(rg);
            k.OnRequest += cc => {
                if (cc == Events.ItemRequest.Reload) Display_Data();
            };
            Container.Children.Add(k);
        });

        var ids = ActiveData.Relations.Select(r => r.group.id).Distinct().Where(k => k > 0 && !ActiveData.RelGroup.Any(p => p.id == k)).ToList();

        if (ids.Any()) {
            ids.ForEach(r => {
                var k = new Button() { Content = "Register New Group " + r, Foreground = Brushes.LightGreen };
                Container.Children.Add(k);

                k.Click += (s, e) => {
                    var t = new RelGroupClass();
                    t.id = r;
                    t.create = DateTime.Now;
                    t.update = DateTime.Now;
                    t.bound = new BoundClass() { Bottom = 0, Left = 0, Right = 0, Top = 0, };
                    ActiveData.RelGroup.Add(t);
                    Display_Data();
                };

            });
        }

    }
}
