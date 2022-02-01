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

namespace TestPassModule.Stage.Display {
    /// <summary>
    /// Interaction logic for GridLine.xaml
    /// </summary>
    public partial class GridLine : UserControl {
        public GridLine() {
            InitializeComponent();
            this.Loaded += GridLine_Loaded;
      
        }

        

        private void GridLine_Loaded(object sender, RoutedEventArgs e) {

            MainGrid.Children.Clear();
            var h = (int)SConfig.Grid_Height / 2;

           

            while (h < this.ActualHeight) {

                var VL = new Line() { X2 = this.ActualWidth };
                VL.Margin = new Thickness(0, h, 0, 0);
                MainGrid.Children.Add(VL);
                h += SConfig.Grid_Height;

                if (h > 2000) break;
            }

        }
    }
}
