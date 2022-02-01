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
using TestPassModule.Relationships.Editor;
using TestPassModule.Stage.Items;
using TestPassModule.Ui.Access;
using TestPassModule.Ui.Windows;

namespace TestPassModule.Relationships.Display {
    /// <summary>
    /// Interaction logic for RelationPreview.xaml
    /// </summary>
    public partial class RelationPreview : UserControl {
        public RelationPreview(RelationClass unit) {
            this.item = unit;
            InitializeComponent();
  
            this.Loaded += RelationPreview_Loaded;
        }

        public RelationClass item { get; }
        public int StartIndex { get; private set; }
        public int EndIndex { get; private set; }

        private void RelationPreview_Loaded(object sender, RoutedEventArgs e) {

            Display_Content();

        }

        private void Setup_Location() {



            var leftMargin = 0;
            if (this.item.position.to > 0) {




                if (this.StartIndex > this.EndIndex) {
                    (this.StartIndex, this.EndIndex) = (this.EndIndex, this.StartIndex);

                    //IconBox.HorizontalAlignment = HorizontalAlignment.Right;
                    //Rhead.Visibility = Visibility.Collapsed;
                    //Lhead.Visibility = Visibility.Visible;
                } else {
                    //Rhead.Visibility = Visibility.Visible;
                    //Lhead.Visibility = Visibility.Collapsed;
                }
                this.Width = (this.EndIndex - this.StartIndex) * 250;


                leftMargin = (this.StartIndex - 1) * SConfig.Grid_Width + SConfig.Grid_Width_Half;
            } else {
                //IconBox.Visibility = Visibility.Collapsed;

                leftMargin = (this.StartIndex - 1) * SConfig.Grid_Width;

            }


            this.Margin = new Thickness(leftMargin,
                item.runtime.row * SConfig.Grid_Height,
                0, 0);

        }

        private void Display_Content() {

            var B = new RelBound();
            var ltr = true;

            this.Opacity = 1;
            if (item.runtime != null) {
                if (!item.runtime.active && item.group.id > 0) this.Opacity = 0.5;
            }

            //>> Find Index
            this.StartIndex = ActiveData.Stage.SenarioData.Modules.FirstOrDefault(n => n.id == this.item.position.from)?.index ?? 1 ;
            if (this.item.position.to > 0)
                this.EndIndex = ActiveData.Stage.SenarioData.Modules.FirstOrDefault(n => n.id == this.item.position.to)?.index ?? 1;

            //>> Correction
            if (this.StartIndex == this.EndIndex) item.actionmode = SqActionModes.Self;


            //>> Setup Content
            switch (item.actionmode) {
                case SqActionModes.Solid:

                    if (this.item.position.to > 0) {
                        var min = this.StartIndex > this.EndIndex ? this.EndIndex : this.StartIndex;
                        B.left = (min - 1) * SConfig.Grid_Width + SConfig.Grid_Width_Half;
                        B.top = item.runtime.row * SConfig.Grid_Height;
                        B.width = Math.Abs(this.StartIndex - this.EndIndex) * SConfig.Grid_Width;
                        SetupBound(B.left, B.top, B.width, SConfig.Grid_Height);

                        ltr = this.StartIndex > this.EndIndex;
                    }

                    MainBorder.Child = new RelationArrow(item, ltr);
                    break;
                case SqActionModes.Reply:

                    if (this.item.position.to > 0) {
                        var min = this.StartIndex > this.EndIndex ? this.EndIndex : this.StartIndex;
                        B.left = (min - 1) * SConfig.Grid_Width + SConfig.Grid_Width_Half;
                        B.top = item.runtime.row * SConfig.Grid_Height;
                        B.width = Math.Abs(this.StartIndex - this.EndIndex) * SConfig.Grid_Width;
                        SetupBound(B.left, B.top, B.width, SConfig.Grid_Height);

                        ltr = this.StartIndex > this.EndIndex;
                    }

                    MainBorder.Child = new RelationArrow(item, ltr);
                    break;
                case SqActionModes.Self:

                    B.left = (this.StartIndex - 1) * SConfig.Grid_Width + SConfig.Grid_Width_Half;
                    B.top = item.runtime.row * SConfig.Grid_Height;
                    B.width = SConfig.Grid_Width  ;
                    SetupBound(B.left, B.top, B.width, SConfig.Grid_Height);

                    ltr = this.StartIndex > this.EndIndex;

                    MainBorder.Child = new RelationArrow(item, ltr);
                    break;

                case SqActionModes.EndPoint:
                    B.left = (this.StartIndex - 1) * SConfig.Grid_Width;
                    B.top = item.runtime.row * SConfig.Grid_Height;
                    SetupBound(B.left, B.top, 100, SConfig.Grid_Height);
                    var Endpoint = new RelationEnd(item);
                    Endpoint.FlowDirection = FlowDirection.RightToLeft;
                    MainBorder.Child = Endpoint;
                    break;

                case SqActionModes.StartPoint:
                    break;
                default:
                    break;
            }
        }

        private void SetupBound(int left, int top, int width, int height) {
            this.Margin = new Thickness(0, top, 0, 0);
            MainBorder.Margin = new Thickness(left, 0, 0, 0);
            MainBorder.Width = width;
            this.Height = height;
        }

        private void BtnVerticalMenu_Click(object sender, RoutedEventArgs e) {

            var OS = new RelationEditorOnScreen(item);
            OS.OnChange += txt => {
                if (txt == "theme") Display_Content();
            };
            var g = new SideWindow(OS);          
            g.Show();

        }
    }

    internal class RelBound {
        public int left { get; set; }
        public int top { get; set; }
        public int width { get; set; }
    }
}
