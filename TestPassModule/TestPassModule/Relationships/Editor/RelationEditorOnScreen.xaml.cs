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
using TestPassModule.Api.Declare;
using TestPassModule.Relationships.Lib;
using TestPassModule.Ui.Colorize;

namespace TestPassModule.Relationships.Editor {
    /// <summary>
    /// Interaction logic for RelationEditorOnScreen.xaml
    /// </summary>
    public partial class RelationEditorOnScreen : UserControl {
        public Action<string> OnChange;

        public RelationEditorOnScreen(RelationClass item) {
            InitializeComponent();
            Item = item;

            setup_theme();
            setup_state();
        }

        private void setup_state() {

            txtState.Text = Item.runtime.state;

            StateContainer.Children.Clear();
            var states = new Ui.Theme.StateBrush();
            var icons = new Ui.Icon.IconFinder();
            states.States.Keys.ToList().ForEach(state => {

                var path = new Path() { Data = icons.GetIcons(state) };
                path.Fill = states.GetBrush(state);

                var n = new Button() { Uid = state, ToolTip = state ,Opacity= 0.5, Width= 32, Height = 32 ,Padding = new Thickness(4)  };
                if (Item.runtime.state == state) {
                    n.Opacity = 1;
                    n.Background = BrushGenerator.FromString("#161616");
                }

                n.Content = path;
                n.Click += (o, e) => {
                    Item.runtime.state = state;
                    OnChange?.Invoke("theme");
                    setup_state();
                };
                StateContainer.Children.Add(n);

            });

        }

        private void setup_theme() {
            var x = new Ui.Theme.ThemeBrush();
            x.States.Keys.ToList().ForEach(vl => {
                var btn1 = new Button() { Width = 24, Height = 24, Background = x.GetBrush(vl) };
                btn1.Click += (o, e) => {
                    Item.runtime.theme = x.States[vl];
                    OnChange?.Invoke("theme");
                };
                ColorBox.Children.Add(btn1);
            });
        }

        public RelationClass Item { get; }





        private void BtnAddReply_Click(object sender, RoutedEventArgs e) {
            new RelModify(Item).AddReply();
         

        }

        private void BtnAddSubLine_Click(object sender, RoutedEventArgs e) {
          //  new RelModify(item).AddDuplicate();
            new RelModify(Item).AddSubLine();
        }

        private void BtnMove_Click(object sender, RoutedEventArgs e) {
            var uid = (sender as UIElement)!.Uid;
            var changer = new RelModify(Item);
            if (uid == "up") changer.MoveUp();
            if (uid == "down") changer.MoveDown();
            if (uid == "all/down") changer.MoveDownAll();
        }
    }
}
