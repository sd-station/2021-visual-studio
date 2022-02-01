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
using TestPassModule.Api.Manager;
using TestPassModule.Api.Temp;
using TestPassModule.Relationships.Lib;
using TestPassModule.Ui.Colorize;
using TestPassModule.Ui.Theme;

namespace TestPassModule.Stage.Items {
    /// <summary>
    /// Interaction logic for DrawLine.xaml
    /// </summary>
    public partial class RelationArrow : UserControl {
        public RelationArrow(Api.Declare.RelationClass unit, bool ltr) {
            this.item = unit;
            InitializeComponent();

            if (ltr) {
                ProgressTheme.HorizontalAlignment = HorizontalAlignment.Right;
                Rhead.Visibility = Visibility.Collapsed;
                Lhead.Visibility = Visibility.Visible;
            } else {
                Rhead.Visibility = Visibility.Visible;
                Lhead.Visibility = Visibility.Collapsed;
            }





            this.Height = SConfig.Grid_Height - 1;
            CoverBorder.Visibility = Visibility.Collapsed;
            this.Loaded += DrawLine_Loaded;
        }

        private void DrawLine_Loaded(object sender, RoutedEventArgs e) {
            Display_Data();
            Showtheme();
        }



        private void Display_Data() {



            txtTitle.Text = item.title ?? "";




            switch (item.actionmode) {
                case SqActionModes.Solid:
                    BaseLine.StrokeDashArray = new DoubleCollection { };
                    BaseLine.Visibility = Visibility.Visible;
                    SelfLine.Visibility = Visibility.Collapsed;
                    break;
                case SqActionModes.Reply:
                    BaseLine.StrokeDashArray = new DoubleCollection { 5, 5 };
                    BaseLine.Visibility = Visibility.Visible;
                    SelfLine.Visibility = Visibility.Collapsed;
                    break;
                case SqActionModes.Self:
                    Rhead.Visibility = Visibility.Collapsed;
                    Lhead.Visibility = Visibility.Collapsed;
                    BaseLine.Visibility = Visibility.Collapsed;
                    SelfLine.Visibility = Visibility.Visible;
                    break;
                default:
                    break;
            }


        }



        public RelationClass item { get; private set; }



        private void MenuState_Click(object sender, RoutedEventArgs e) {
            item.runtime.state = (sender as MenuItem).Uid;
            Showtheme();
        }

        private void Showtheme() {


            if (item.position.to < 0) {
                CoverBorder.Visibility = Visibility.Visible;
                CoverBorder.Stroke = Brushes.Red;
            }

            if (string.IsNullOrWhiteSpace(item.runtime.theme) || !item.runtime.theme.StartsWith("#")) item.runtime.theme = "#707070";
            var color = (Color)ColorConverter.ConvertFromString(item.runtime.theme);
            this.BorderBrush = new SolidColorBrush(ColorLighter.ChangeColorBrightness(color, (float)-0.3)); ;


            if (item.runtime.state == null) item.runtime.state = "pending";


            IconContainer.Children.OfType<Path>().ToList().ForEach(n => {
                n.Visibility = n.Uid.Contains(item.runtime.state) ? Visibility.Visible : Visibility.Collapsed;
            });

            if (ActiveData.DependencyMode) return;
            this.Foreground = ProgressTheme.BorderBrush = new StateBrush().GetColor(item.runtime.state);
            // new StateBrush().GetColor(item.runtime.state);



            this.ToolTip = item.runtime.state;
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e) {
            ActiveData.Relations.Remove(item);
            DataEvents.OnDataChange?.Invoke(DataChangeOn.Relationship);
        }

        private void Button_ContextMenuOpening(object sender, ContextMenuEventArgs e) {
            MenuStates.Items.Clear();
            var SB = new StateBrush();
            SB.States.Keys.ToList().ForEach(state => {

                MenuItem MN = new MenuItem() { Header = state.Replace("-", " "), Uid = state };
                MN.Foreground = SB.GetBrush(state);
                MN.Click += MenuState_Click;
                MenuStates.Items.Add(MN);
            });

            MenuTheme.Items.Clear();
            var TB = new ThemeBrush();


            TB.States.Keys.ToList().ForEach(state => {

                MenuItem MN = new MenuItem() { Header = state.Replace("-", " "), Uid = state };
                MN.Foreground = TB.GetBrush(state);
                MN.Click += (s, e) => {
                    item.runtime.theme = TB.States[state];
                    Showtheme();
                };
                MenuTheme.Items.Add(MN);
            });

            MenuTypes.Items.Clear();
            var values = Enum.GetValues(typeof(SqActionModes)).Cast<SqActionModes>().ToList();
            values.ForEach(Eitem => {
                var p = new MenuItem() { Header = Eitem.ToString(), Tag = (int)Eitem };
                p.Click += (x1, x2) => {

                    item.actionmode = Eitem;
                    Display_Data();
                };
                MenuTypes.Items.Add(p);
            });


            MenuRelationship.Items.Clear();

            if (TempData.RelationshipCollection != null && TempData.RelationshipCollection.Any()) {
                var mnuAddR = new MenuItem() { Header = "Add To Current Group", Foreground = Brushes.Green };
                mnuAddR.Click += (qq, qqq) => { RunCommand("rel-group/add"); };
                MenuRelationship.Items.Add(mnuAddR);
            }
            var mnuCreateR = new MenuItem() { Header = "Create New Group" };
            mnuCreateR.Click += (qq, qqq) => { RunCommand("rel-group/create"); };
            MenuRelationship.Items.Add(mnuCreateR);
        }



        void RunCommand(string cmd) {
            if (cmd == "rel-group/create") {
                TempData.RelationshipCollection = new List<int>() { item.id };
            }
            if (cmd == "rel-group/add") {
                TempData.RelationshipCollection.Add(item.id);
            }
            DataEvents.OnDataChange?.Invoke(DataChangeOn.Relationship);
        }

        private void mnuMove_Click(object sender, RoutedEventArgs e) {
            var uid = (sender as UIElement).Uid;

            var changer = new RelModify(item);
            if (uid == "up") changer.MoveUp();
            if (uid == "down") changer.MoveDown();
            if (uid == "all/down") changer.MoveDownAll();

        }

        private void MenuReply_Click(object sender, RoutedEventArgs e) {

            new RelModify(item).AddReply();
          

        }

        private void MenuDuplicate_Click(object sender, RoutedEventArgs e) {
            new RelModify(item).AddDuplicate();
           
        }

        private void SubLine_Click(object sender, RoutedEventArgs e) {
            new RelModify(item).AddSubLine();

         
        }

        private void btnLine_Click(object sender, RoutedEventArgs e) {

            if (ActiveData.RelGroup.Any(h => h.rels().Any(rel => rel.id == item.id))) {

                if (item.runtime.active) {
                    item.runtime.active = false;
                    DataEvents.OnDataChange(DataChangeOn.Relationship);
                    return;
                }

                var rg = ActiveData.RelGroup.First(h => h.rels().Any(rel => rel.id == item.id));
                rg.rels().ForEach(k => {
                    ActiveData.Relations.ForEach(rel => { if (rel.id == k.id) rel.runtime.active = false; });
                });
                item.runtime.active = true;
                DataEvents.OnDataChange(DataChangeOn.Relationship);
                return;
            }

            GotoEdit();
        }

        private void GotoEdit() {
            var ed = new Relations.Editor.RelationEditor(item);

            Ui.Access.AppHome.Side.Navigate(ed);

            ed.OnRequest += cmd => {

                if (cmd == "save") {
                    Display_Data();
                    Showtheme();
                    Ui.Access.AppHome.Side.Close();
                }

                if (cmd == "close") {
                    Ui.Access.AppHome.Side.Close();
                }
            };
        }


        private void EditMenu_Click(object sender, RoutedEventArgs e) {
            GotoEdit();
        }

   

        private void EndPoint_Click(object sender, RoutedEventArgs e) {
            var nr = new RelationClass() {

                title = "End point",
                actionmode = SqActionModes.EndPoint
            };
            nr.position = new PositionClass {
                index = ActiveData.Relations.Max(h => h.position.index) + 1,
                from = item.position.to,
                to = -1,
                parent = item.id
            };

            nr.runtime = new RuntimeClass() { };

            new RelationManager(AppData.current).Add(nr);

            DataEvents.OnDataChange?.Invoke(DataChangeOn.Relationship);
        }

        private void ApplyThemeToAllChildMenu_Click(object sender, RoutedEventArgs e) {
            var x = this.item.runtime.theme;
            ApplyTheme(x, this.item.id, true);
            DataEvents.OnDataChange?.Invoke(DataChangeOn.Relationship);
        }
        private void ApplyThemeToChildMenu_Click(object sender, RoutedEventArgs e) {
            var x = this.item.runtime.theme;
            ApplyTheme(x, this.item.id, false);
            DataEvents.OnDataChange?.Invoke(DataChangeOn.Relationship);
        }

        private void ApplyTheme(string x, int id, bool call) {
            ActiveData.Relations.ForEach(h => {
                if (h.position.parent == id) {
                    h.runtime.theme = x;
                    if (call) ApplyTheme(x, h.id, call);
                }
            });

        }

        private void btnDeleteAfter_Click(object sender, RoutedEventArgs e) {
            ActiveData.Stage.Relations.Where(x => x.position.index > item.runtime.row).ToList()
                .ForEach(h => {
                    ActiveData.Relations.ForEach(k => {
                        if (k.position.parent == h.id) k.position.parent = this.item.id;
                    });
                    ActiveData.Relations.RemoveAll(X => X.id == h.id);
                });
            DataEvents.OnDataChange?.Invoke(DataChangeOn.Relationship);
        }
    }
}
