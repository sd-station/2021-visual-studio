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
using TestPassModule.Api.Active;
using TestPassModule.Api.Declare;
using TestPassModule.Api.Data;
using TestPassModule.Api.Temp;
using TestPassModule.ModuleLine.Lib;

namespace TestPassModule.Stage.Items {
    /// <summary>
    /// Interaction logic for ModuleItem.xaml
    /// </summary>
    public partial class ModuleLineItem : UserControl {
        public SenarioModuleClass Connect { get; private set; }
 
        public ModuleLineItem(SenarioModuleClass nr) {
            this.Connect = nr;
          
            InitializeComponent();
            this.Loaded += ModuleItem_Loaded;

        }



        private void ModuleItem_Loaded(object sender, RoutedEventArgs e) {
            ShowTheme();
            ShowActionLine();
        }



        private void ShowTheme() {
            this.Margin = new Thickness((Connect.index - 1) * SConfig.Grid_Width, 0, 0, 0);
            this.BorderBrush = Connect.BorderBrush();
            this.Foreground = Connect.Foreground();
         }


        private void btnLine_Click(object sender, RoutedEventArgs e) {
            new Relationships.Command.RelGenerator().Modify(PointA, Connect.id);
        }



        Point PointA;
        private void Button_PreviewMouseUp(object sender, MouseButtonEventArgs e) {
            PointA = e.MouseDevice.GetPosition(sender as UIElement);
        }


        private void ShowActionLine() {
            if ((int)Connect.module <= 0) return;
            if (Connect.theme == null) Connect.theme = "#707070";

            var color = (Color)ColorConverter.ConvertFromString(Connect.theme);
            color.A = 200;
            var brush = new SolidColorBrush(color);
            var InComings = ActiveData.Relations.Where(k => k.position.to == Connect.id).OrderBy(p => p.position.index).ToList();

            var Boundaries = new Dictionary<int, int>();
            foreach (var rel in InComings) {
                if (!Boundaries.Keys.Any()) {
                    Boundaries.Add(rel.position.index, 1);
                    continue;
                }

                if (Boundaries.Keys.Last() == rel.position.index) continue;

                if (Boundaries[Boundaries.Keys.Last()] + Boundaries.Keys.Last() == rel.position.index) {
                    Boundaries[Boundaries.Keys.Last()]++;
                    continue;
                }

                Boundaries.Add(rel.position.index, 1);

            }
            Boundaries.Keys.ToList().ForEach(inx => {
                var p = new Rectangle() {
                    Width = 8,
                    Height = Boundaries[inx] * SConfig.Grid_Height + 20,
                    Fill = brush,
                    VerticalAlignment = VerticalAlignment.Top,
                    HorizontalAlignment = HorizontalAlignment.Center
                };

                p.Margin = new Thickness(0, inx * SConfig.Grid_Height + SConfig.Grid_Height / 2 - 10, 0, 0);


                ActionGrid.Children.Add(p);
            });

        }
    }
}
