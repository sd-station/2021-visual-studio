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
using TestPassModule.Ui.Colorize;

namespace TestPassModule.Modules.Display {
    /// <summary>
    /// Interaction logic for ModulePartView.xaml
    /// </summary>
    public partial class ModulePartView : UserControl {
        internal Action<string, int, Point> OnRequest { get; set; }

        public ModulePartView(Api.Declare.StructClass part) {
            this.item = part;
            InitializeComponent();


            TypesMenu.Items.Clear();

            new List<string> { "none", "property,method,event" }.ForEach(txt => {

                if (TypesMenu.Items.Count > 0) {
                    TypesMenu.Items.Add(new Separator());
                }

                txt.Split(",").ToList().ForEach(t => {
                    var mn = new MenuItem { Header = t, Uid = t };
                    mn.Click += (s, e) => {
                        item.mode = mn.Uid;
                        new StructLoader().Save();
                        SHow_Theme();
                    };
                    TypesMenu.Items.Add(mn);
                });
            });




            // old version
            if (string.IsNullOrEmpty(item.name) && !item.title.StartsWith("new item"))
                item.name = item.title
                    .ToLower()
                    .Replace(" ", "-")
                    .Replace("_", "-").Replace("--", "-");

            txtTitle.Visibility = Visibility.Collapsed;
            txtTitle.Text = this.item.title;



            this.BorderBrush = BrushGenerator.FromString(this.item.theme);

            Theme.Items.Clear();
            new Ui.Theme.ThemeBrush().GetAsMenuList().ToList().ForEach(MN => {
                Theme.Items.Add(MN);
                MN.Click += (s, e) => {
                    this.item.theme = MN.Uid;
                    this.BorderBrush = BrushGenerator.FromString(this.item.theme);
                    new StructLoader().Save();
                };
            });


            this.Loaded += ModulePartView_Loaded;
        }

        private void SHow_Theme() {
            IconShape.Data = new Ui.Icon.IconFinder().GetIcons(this.item.mode);
        }

        private void ModulePartView_Loaded(object sender, RoutedEventArgs e) {
            if (this.item.title.StartsWith("new item")) {
                txtTitle.Visibility = Visibility.Visible;
                txtTitle.Focus();
                txtTitle.SelectAll();
            }

            SHow_Theme();
        }

        public StructClass item { get; private set; }
        public Point GPoint { get; internal set; }



        private void btnAction_Click(object sender, RoutedEventArgs e) {
            OnRequest?.Invoke("info", this.item.id, new Point(GPoint.X + this.ActualWidth, GPoint.Y));
        }

        private void btnTitle_Click(object sender, RoutedEventArgs e) {
            OnRequest?.Invoke("nav", this.item.id, new Point(GPoint.X + DisplayText.ActualWidth + 20, GPoint.Y));
        }

        private void EditMenu_Click(object sender, RoutedEventArgs e) {
            txtTitle.Visibility = Visibility.Visible;
            txtTitle.Focus();
            txtTitle.SelectAll();
        }

        private void txtTitle_PreviewKeyDown(object sender, KeyEventArgs e) {
            if (e.Key == Key.Enter) {
                txtTitle.Visibility = Visibility.Collapsed;
                if (!this.item.title.Equals(txtTitle.Text)) this.item.title = txtTitle.Text;
                new StructLoader().Save();
            }
        }

        private void MakeSubMenu_click(object sender, RoutedEventArgs e) {
            OnRequest?.Invoke("move-to-sub", this.item.id, new Point(GPoint.X + DisplayText.ActualWidth + 10, GPoint.Y));
        }

        private void txtTitle_TextChanged(object sender, TextChangedEventArgs e) {
            if (!IsLoaded) return;
            if (!this.item.title.Equals(txtTitle.Text)) this.item.title = txtTitle.Text;
        }

        private void DeleteMenu_Click(object sender, RoutedEventArgs e) {
            OnRequest?.Invoke("del", this.item.id, new Point(GPoint.X + this.ActualWidth, GPoint.Y));
        }
    }
}
