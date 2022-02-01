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
using TestPassModule.Api.Manager;
using TestPassModule.ModuleLine.Display.Theme;
using TestPassModule.Ui.Theme;
using TestPassModule.Ui.Colorize;
using TestPassModule.ModuleLine.Lib;
using TestPassModule.Ui.Windows;

namespace TestPassModule.Stage.Items {
    /// <summary>
    /// Interaction logic for ModuleItemPreview.xaml
    /// </summary>
    public partial class ModuleItemPreview : UserControl {


        public SenarioModuleClass Department { get; private set; }
        public ModuleItemPreview(SenarioModuleClass nr) {
            this.Department = nr;

            InitializeComponent();



            txtTitle.TextChanged += TxtTitle_TextChanged;
            this.Loaded += ModuleItem_Loaded;



            // this.ToolTip = item.mode.ToString();


            MenuTheme.Items.Clear();
            var TB = new ThemeBrush();


            TB.States.Keys.ToList().ForEach(state => {

                MenuItem MN = new MenuItem() { Header = state.Replace("-", " "), Uid = state };
                MN.Foreground = TB.GetBrush(state);
                MN.Click += (s, e) => {
                    Department.theme = TB.States[state];
                    ShowTheme();
                };
                MenuTheme.Items.Add(MN);
            });
        }

        private void TxtTitle_TextChanged(object sender, TextChangedEventArgs e) {
            Department.title = txtTitle.Text;
            Department.update = DateTime.Now;
        }

        private void ModuleItem_Loaded(object sender, RoutedEventArgs e) {


            Display_Data();
            ShowTheme();
        }

        private void ShowTheme() {
            this.Margin = new Thickness((Department.index - 1) * SConfig.Grid_Width, 0, 0, 0);
            this.BorderBrush = Department.BorderBrush();
            this.Foreground = Department.Foreground();



        }



        private void btnMove_Click(object sender, RoutedEventArgs e) {
            var id = (sender as MenuItem).Uid;
            if (id == "1") {
                if (Department.index >= ActiveData.Stage.SenarioData.Modules.Count) return;
                ActiveData.Stage.SenarioData.Modules.OrderBy(j => j.index).First(p => p.index > Department.index).index -= 1;
                Department.index += 1;
            }
            if (id == "-1") {
                if (Department.index <= 1) return;

                ActiveData.Stage.SenarioData.Modules.OrderBy(j => j.index).Last(p => p.index < Department.index).index += 1;
                Department.index -= 1;
            }

            DataEvents.OnDataChange?.Invoke(DataChangeOn.Module);
        }

        public bool Expand { set => ContentBorder.Visibility = value ? Visibility.Visible : Visibility.Collapsed; }

        private void btnAddModule_Click(object sender, RoutedEventArgs e) {
            var s = sender as MenuItem;


            var nr = new ModuleClass();
            nr.theme = "#707070";
            new ModuleManager().Add(nr);
            nr.title = "Module " + nr.id;
            new ModuleManager().Save();


            var nco = new SenarioModuleClass();
            nco.id = 1;
            if (ActiveData.Stage.SenarioData.Modules.Any()) nco.id += ActiveData.Stage.SenarioData.Modules.Last().id;
            nco.module = nr.id;
            ActiveData.Stage.SenarioData.Modules.ForEach(t => { if (t.index > Department.index) t.index++; });
            nco.index = this.Department.index;

            if (s.Uid == "before") {
                Department.index += 1;
            }
            if (s.Uid == "after") {
                nco.index += 1;
            }


            ActiveData.Stage.SenarioData.Modules.Add(nco);
            new SenarioDataManager(AppData.current).Save();
            DataEvents.OnDataChange?.Invoke(DataChangeOn.Module);
        }



        private void BtnIcon_Click(object sender, RoutedEventArgs e) {
            if (Department.module > 0) {
                var or = ActiveData.General.Modules.First(r => r.id == Department.module);
                var s = new Modules.Windows.ModelWindow(or);
                s.Show();
            }
        }

        private void DeleteMenu_Click(object sender, RoutedEventArgs e) {

            ActiveData.Relations.ForEach(rel => {
                if (rel.position.to == this.Department.id) rel.position.to = -1;
                if (rel.position.from == this.Department.id) rel.position.from = -1;
            });

            ActiveData.Stage.SenarioData.Modules.Remove(this.Department);

            var i = 1;
            ActiveData.Stage.SenarioData.Modules.OrderBy(k => k.index).ToList().ForEach(j => j.index = i++);
            new ModuleManager().Save();
            DataEvents.OnDataChange?.Invoke(DataChangeOn.Module);
        }

        private void btnModify_Click(object sender, RoutedEventArgs e) {
            var OS = new Departments.Edit.DepartmentEditorOnScreen(Department);
            var g = new SideWindow(OS);
            g.Show();

            OS.OnChange += txt => {
                if (txt == "theme") ShowTheme();
                if (txt == "save") {
                    g.Close();
                    Display_Data();
                }
            };
        }

        private void Display_Data() {


            if (Department.module > 0) {
                var or = ActiveData.General.Modules.First(r => r.id == Department.module);
                if (string.IsNullOrWhiteSpace(Department.theme)) Department.theme = or.theme;
                if (string.IsNullOrWhiteSpace(Department.title)) Department.title = or.title;

                var ModuleIcon = new ModuleTypeIcon();
                ContentBorder.Child = ModuleIcon;
                ModuleIcon.Display(or.mode.ToString().ToLower().Replace("_", "-"));
            }
            txtTitle.Text = this.Department.title;
        }
    }
}
