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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using TestPassModule.Api.Active;
using TestPassModule.Api.Declare;
using TestPassModule.Api.Data;
using TestPassModule.Api.Temp;
using TestPassModule.Stage.Relations.Editor;
using TestPassModule.Ui.Access;

namespace TestPassModule.Stage.Items {
    /// <summary>
    /// Interaction logic for TempMessage.xaml
    /// </summary>
    public partial class RelationOnScreen : UserControl {
        public RelationEditor ItemModifier { get; internal set; }

        public RelationOnScreen() {
            InitializeComponent();
            this.Loaded += TempMessage_Loaded;
            this.Height = SConfig.Grid_Height;
        }

        private void TempMessage_Loaded(object sender, RoutedEventArgs e) {

            Display_Data();


        }

        private void Display_Data() {
            //>> Default
            ArrowBox.Visibility = Visibility.Collapsed;
            ThemeColor.Visibility = Visibility.Collapsed;
            if (TempData.NR.State == RelationTempState.Begin) {
                TargetMarker.Visibility = Visibility.Collapsed;
  

                var BeginMargin = new Thickness(0, TempData.NR.StartPoint - SConfig.Grid_Height / 2, 0, 0);

                var NextMargin = new Thickness(0, TempData.NR.IndexRequest * SConfig.Grid_Height, 0, 0);
                // Create a duration of 2 seconds.
                Duration duration = new Duration(TimeSpan.FromSeconds(1));

                ThicknessAnimation TA = new ThicknessAnimation(BeginMargin, NextMargin, duration);
                TA.Duration = duration;


                var sb = new Storyboard();
                sb.Duration = duration;
                sb.Children.Add(TA);

                Storyboard.SetTarget(TA, this);
                Storyboard.SetTargetProperty(TA, new PropertyPath("Margin"));

                // Make the Storyboard a resource.
                this.Resources.Add("unique_id", sb);

                // Begin the animation.
                sb.Begin();

                var Rq = ActiveData.Stage.SenarioData.Modules.First(h => h.id == TempData.NR.Begin).index;
                BeginMarker.Margin = new Thickness((Rq - 1) * SConfig.Grid_Width + SConfig.Grid_Width_Half - 10, 0, 0, 0);


            }

            if (TempData.NR.State == RelationTempState.Endpoint) {

                var NextMargin = new Thickness(0, TempData.NR.IndexRequest * SConfig.Grid_Height, 0, 0);
                this.Margin = NextMargin;


                var Rfrom = ActiveData.Stage.SenarioData.Modules.First(h => h.id == TempData.NR.Begin).index;
                BeginMarker.Margin = new Thickness((Rfrom - 1) * SConfig.Grid_Width + SConfig.Grid_Width_Half - 10, 0, 0, 0);



                var Rto = ActiveData.Stage.SenarioData.Modules.First(h => h.id == TempData.NR.End).index;
                TargetMarker.Margin = new Thickness((Rto - 1) * SConfig.Grid_Width + SConfig.Grid_Width_Half - 10, 0, 0, 0);

                if (Rto != Rfrom) {
                    AngleArrow.Visibility = ArrowBox.Visibility = Visibility.Visible;
                    ThemeColor.Visibility = Visibility.Visible;
            
                    var minx = Rfrom > Rto ? Rto : Rfrom;
                    ArrowBox.Margin = new Thickness((minx - 1) * SConfig.Grid_Width + SConfig.Grid_Width_Half  , 0, 0, 0);
                    ArrowBox.Width = SConfig.Grid_Width * Math.Abs(Rfrom - Rto);
                    AngleArrow.Margin = new Thickness((Rto - 1) * SConfig.Grid_Width + SConfig.Grid_Width_Half - AngleArrow.ActualWidth , 0, 0, 0);

                    if (Rto < Rfrom) {
                        AngleArrow.RenderTransformOrigin = new Point(1 , 0.5);
                        RotateTransform rotateTransform1 = new RotateTransform(180);
                        AngleArrow.RenderTransform = rotateTransform1;
                    }



                    ColorBox.Children.Clear();
                    ThemeColor.Width = SConfig.Grid_Width - 20;
                    var x = new Ui.Theme.ThemeBrush();
                    x.States.Keys.ToList().ForEach(vl => {
                        var btn1 = new Button() { Width = 24, Height = 24 , Background = x.GetBrush(vl) };
                        btn1.Click += (o, e) => {
                            ArrowBox.Fill =  btn1.Background;
                            ItemModifier.item.runtime.theme = x.States[vl];
                        };
                        ColorBox.Children.Add(btn1);
                    });
                }
            }
        }

        void CancelRelationShip() {
            TempData.NR.State = RelationTempState.None;

        }

        private void RemoveButton_Click(object sender, RoutedEventArgs e) {
            TempData.NR.State = RelationTempState.None;
            this.Visibility = Visibility.Collapsed;
        }
        private void TargetRemoveButton_Click(object sender, RoutedEventArgs e) {
            TempData.NR.State = RelationTempState.Begin;
            TargetMarker.Visibility = Visibility.Collapsed;
            AppHome.Side.Close();
   
        }

        private void SwapButton_Click(object sender, RoutedEventArgs e) {
            var k = TempData.NR.Begin;
            TempData.NR.Begin = TempData.NR.End;
            TempData.NR.End = k;
            DataEvents.OnDataChange(DataChangeOn.RelTemp);
        }
    }
}
