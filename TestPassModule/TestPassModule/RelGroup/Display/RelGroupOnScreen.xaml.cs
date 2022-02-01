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
using TestPassModule.Api.Temp;
using TestPassModule.Relationships.Api.ext;
using TestPassModule.RelGroup.Lib;

namespace TestPassModule.Stage.TemplateItems {
    /// <summary>
    /// Interaction logic for RelationTemplate.xaml
    /// </summary>
    public partial class RelationTemplate : UserControl {
        public RelGroupClass RG { get; private set; }

        public RelationTemplate() {
            InitializeComponent();
            this.Loaded += RelationTemplate_Loaded;

            RG = new RelGroupClass();

        }

        private void RelationTemplate_Loaded(object sender, RoutedEventArgs e) {





            RG.bound = new RelGroupBoundGenerator().Bound;

            TempData.RelationshipCollection.ForEach(f => {

                var rel = ActiveData.Relations.First(k => k.id == f);
                if (RG.bound.Top == 0 || RG.bound.Top > rel.position.index) RG.bound.Top = rel.position.index;
                if (RG.bound.Bottom < rel.position.index) RG.bound.Bottom = rel.position.index;

                var to = ActiveData.Stage.SenarioData.Modules.First(k => k.id == rel.position.to);
                if (RG.bound.Left == 0 || RG.bound.Left > to.index) RG.bound.Left = to.index;
                if (RG.bound.Right < to.index) RG.bound.Right = to.index;

                var from = ActiveData.Stage.SenarioData.Modules.First(k => k.id == rel.position.from);
                if (RG.bound.Left == 0 || RG.bound.Left > from.index) RG.bound.Left = from.index;
                if (RG.bound.Right < from.index) RG.bound.Right = from.index;


            });


            this.Margin = new Thickness(
                (RG.bound.Left - 1) * SConfig.Grid_Width + SConfig.Grid_Width_Half - 20,
                RG.bound.Top * SConfig.Grid_Height,
                0, 0);

            this.Height = (RG.bound.Bottom - RG.bound.Top) * SConfig.Grid_Height + SConfig.Grid_Height;
            this.Width = (RG.bound.Right - RG.bound.Left) * SConfig.Grid_Width + 40;

            var x = 1;
            TempData.RelationshipCollection.Select(R => ActiveData.Relations.First(k => k.id == R)).ToList().ForEach(s => {
                MainGrid.Children.Add(new TextBox {
                    Uid = s.id.ToString(),
                    HorizontalAlignment = HorizontalAlignment.Left,
                    VerticalAlignment = VerticalAlignment.Top,
                    Margin = new Thickness(
                       (s.Left() - RG.bound.Left) * SConfig.Grid_Width + 10,
                       (s.Top() - RG.bound.Top) * SConfig.Grid_Height + 10,
                        0, 0),
                    MinWidth = 70,
                    Background = Brushes.Transparent,
                    CaretBrush = Brushes.Orange,
                    Foreground = Brushes.DeepPink,
                    Text = "Untitled Section " + x++
                }); ;
            });





        }

        private void btnSave_Click(object sender, RoutedEventArgs e) {

            var G = new Api.Manager.RelGroupManager(AppData.current).Add(RG);


            var i = 0;
            TempData.RelationshipCollection.Select(R => ActiveData.Relations.First(k => k.id == R)).ToList()
                .ForEach(k => {
                    k.group.id = G.id;
                    k.group.caption = MainGrid.Children.OfType<TextBox>().FirstOrDefault(txt => txt.Uid == k.id.ToString())?.Text ?? "";
                    k.runtime.active = i++ == 0 ? true : false;
                });

            Api.Temp.TempData.RelationshipCollection.Clear();

            DataEvents.OnDataChange(DataChangeOn.RelGroup);
        }
    }
}
