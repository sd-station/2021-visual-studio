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
using TestPassModule.Api.Data;
using TestPassModule.Api.Declare;
using TestPassModule.RelGroup.Lib;

namespace TestPassModule.Stage.Items {
    /// <summary>
    /// Interaction logic for RelGroupPreview.xaml
    /// </summary>
    public partial class RelGroupPreview : UserControl {
        public RelGroupClass item { get; private set; }

        public RelGroupPreview(Api.Declare.RelGroupClass RG) {
            this.item = RG;
            InitializeComponent();
            this.Visibility = Visibility.Hidden;

            this.Loaded += RelGroupPreview_Loaded;
        }

        private void RelGroupPreview_Loaded(object sender, RoutedEventArgs e) {

            var posiible = this.item.rels().Any();

            if (posiible) {
                Relocation_Item();
                Display_Content();
                this.Visibility = Visibility.Visible;
            }




        }

        private void Relocation_Item() {

            this.item.bound = new RelGroupBoundGenerator().Review(this.item).Bound;


        }

        private void Display_Content() {

            txtTitle.ToolTip = item.title;

            this.Margin = new Thickness(
           (this.item.bound.Left - 1) * SConfig.Grid_Width + SConfig.Grid_Width_Half - 40,
           this.item.bound.Top * SConfig.Grid_Height - 10,
           0, 0);

            this.Height = (this.item.bound.Bottom - this.item.bound.Top) * SConfig.Grid_Height + SConfig.Grid_Height + 20;
            this.Width = (this.item.bound.Right - this.item.bound.Left) * SConfig.Grid_Width + 80;

        }



        private void btnDelete_Click(object sender, RoutedEventArgs e) {
            this.item.rels().ForEach(rl => {
                var x = ActiveData.Stage.Relations.FirstOrDefault(p => p.id == rl.id);
                if (x != null) x.group.id = 0;
            });

            ActiveData.RelGroup.Remove(item);

            Api.Data.DataEvents.OnDataChange?.Invoke(DataChangeOn.RelGroup);

        }

        private void BtnAction_Click(object sender, RoutedEventArgs e) {

        }
    }
}
