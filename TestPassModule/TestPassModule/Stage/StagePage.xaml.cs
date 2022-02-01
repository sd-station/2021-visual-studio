using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
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
using TestPassModule.Api.Temp;
using TestPassModule.Stage.Items;
using TestPassModule.Stage.TemplateItems;
using TestPassModule.Relationships.Display;
using TestPassModule.Relationships.Command;
using TestPassModule.Stage.Display;

namespace TestPassModule.Stage {
    /// <summary>
    /// Interaction logic for StagePage.xaml
    /// </summary>
    public partial class StagePage : Page {

        public object item { get; private set; }

        public StagePage() {
            InitializeComponent();
            MainGrid.Children.Insert(0, new Display.GridLine());
            this.Loaded += StagePage_Loaded;

            DataEvents.OnDataChange += OnDataRequest;
        }

        internal void ShowAll() {
            BehindCanvas.Children.Clear();
            ActiveData.Stage.Relations = ActiveData.Relations;

            if (!ActiveData.Stage.Relations.Any()) return;

            var inx = 1;
            ActiveData.Stage.Relations.OrderBy(k => k.position.index).ToList().ForEach(unit => {
                unit.runtime.row = inx++;
                var n = new RelationPreview(unit);
                BehindCanvas.Children.Add(n);


            });


        }

        private void OnDataRequest(DataChangeOn R) {



            switch (R) {
                case DataChangeOn.Module:
                    break;
                case DataChangeOn.Relationship:
                    break;
                case DataChangeOn.Stage_RelationShips:
                    Update_Data_Relation();
                    return;
                case DataChangeOn.RelTemp:
                    Display_Data_Relation_OnScreen();
                    return;
                case DataChangeOn.Source:
                    new ModuleManager().Load();
                    new StructLoader().Load();
                    new RelationManager(AppData.current).Load();
                    new RelGroupManager(AppData.current).Load();
                    Application.Current.MainWindow.Title = AppData.current;
                    break;
                case DataChangeOn.RelGroup:
                    break;
                default:
                    break;
            }



            Display_Data_Module();
            Display_Data_Relation();
            Display_Data_Relation_OnScreen();
            Display_Data_RelGroup();
            Display_Data_RelGroup_OnScreen();

        }



        private void Display_Data_RelGroup() {
            RelGroupCanvas.Children.Clear();

            if (!ActiveData.RelGroup.Any()) { RelGroupCanvas.Visibility = Visibility.Collapsed; return; }

            RelGroupCanvas.Visibility = Visibility.Visible;
            ActiveData.RelGroup.ForEach(rg => {
                var k = new RelGroupPreview(rg);
                RelGroupCanvas.Children.Add(k);
            });




        }

        private void Display_Data_RelGroup_OnScreen() {
            RelationshipCanvas.Children.Clear();

            if (TempData.RelationshipCollection != null && TempData.RelationshipCollection.Any()) {
                var k = new RelationTemplate();
                RelationshipCanvas.Children.Add(k);
            }



        }

        private void StagePage_Loaded(object sender, RoutedEventArgs e) {


            if (AppData.current != null) OnDataRequest(DataChangeOn.Source);


            //>> Setup Menu Container Of Other Project Modules
            GlobalModules.Items.Clear();
            ActiveData.General.Modules.Where(A => !ActiveData.Stage.SenarioData.Modules.Any(p => p.module == A.id)).ToList()
                .ForEach(g => {
                    var x = new MenuItem() { Header = g.title };
                    GlobalModules.Items.Add(x);
                    x.Click += (ww, www) => {
                        var nco = new SenarioModuleClass();
                        nco.module = g.id;
                        nco.id = 1;
                        if (ActiveData.Stage.SenarioData.Modules.Any()) nco.id += ActiveData.Stage.SenarioData.Modules.Last().id;
                        nco.index = 1;
                        if (ActiveData.Stage.SenarioData.Modules.Any())
                            nco.index += ActiveData.Stage.SenarioData.Modules.Max(h => h.index);


                        ActiveData.Stage.SenarioData.Modules.Add(nco);
                        new SenarioDataManager(AppData.current).Save();

                        DataEvents.OnDataChange?.Invoke(DataChangeOn.Module);
                    };
                });


            //>> Setup Menu Container Of New Modules
            NewModules.Items.Clear();
            var i = 0;
            var cp = NewModules;
            Enum.GetValues(typeof(ModuleModes)).Cast<ModuleModes>().ToList().ForEach(g => {

                if ((int)g - i > 20) {
                    var r = new MenuItem() { Header = g.GetCategory() };
                    NewModules.Items.Add(r);
                    cp = r;
                }
                i = (int)g;

                var x = new MenuItem() { Header = g.GetTitle() };
                cp.Items.Add(x);
                x.Click += (ww, www) => {
                    var nr = new ModuleClass();
                    nr.mode = g;
                    nr.theme = "#707070";

                    new ModuleManager().Add(nr);
                    nr.title += nr.mode.ToString().Replace("_", " ") + " " + nr.id;
                    new ModuleManager().Save();
                    if (AppData.current == null) return;

                    Display_Data_Module();

                    var nco = new SenarioModuleClass();
                    nco.id = nr.id;
                    nco.index = 1;
                    if (ActiveData.Stage.SenarioData.Modules.Any())
                        nco.index += ActiveData.Stage.SenarioData.Modules.Max(h => h.index);


                    ActiveData.Stage.SenarioData.Modules.Add(nco);
                    new SenarioDataManager(AppData.current).Save();

                    DataEvents.OnDataChange?.Invoke(DataChangeOn.Module);
                };
            });

        }

        private void Display_Data_Relation() {
            ActiveData.Stage.Relations = new List<RelationClass>();
            AddRelChilds(0);
            // SideScroll.Maximum = x1.ScrollableHeight;
            var inx = 1;
            ActiveData.Stage.Relations.OrderBy(k => k.position.index).ToList().ForEach(unit => {
                unit.runtime.row = inx++;
            });
            Update_Data_Relation();
        }

        private void Update_Data_Relation() {
            BehindCanvas.Children.Clear();
            var inx = 1;
            ActiveData.Stage.Relations.OrderBy(k => k.position.index).ToList().ForEach(unit => {
                var n = new RelationPreview(unit);
                BehindCanvas.Children.Add(n);
            });
        }

        private void AddRelChilds(int parentid) {

            ActiveData.Relations.Where(n22 => n22.position.parent == parentid).ToList().ForEach(rel => {
                ActiveData.Stage.Relations.Add(rel);

                if (rel.group == null) rel.group = new GroupClass();
                if (rel.group.id <= 0 || (rel.group.id > 0 && rel.runtime.active)) {
                    AddRelChilds(rel.id);

                    if (rel.doc?.actions?.Any() ?? false) {
                        rel.doc.actions.ForEach(c => {
                            if (c.StartsWith("next ") || c.StartsWith("continue ")) {
                                var k = c.Substring(c.IndexOf(" ")).Trim();
                                if (k.StartsWith("to")) k = k.Substring(2).Trim();
                                int u = -1;
                                int.TryParse(k, out u);
                                if (u > 0) {
                                    var sp = ActiveData.Relations.FirstOrDefault(n22 => n22.id == u);
                                    if (sp != null) {

                                        ActiveData.Stage.Relations.Add(sp);
                                        AddRelChilds(sp.id);
                                    }
                                }
                            }
                        });
                    }
                }
            });

        }

        private void Display_Data_Module() {
            GridCanvas.Children.Clear();
            ModulePreview.Children.Clear();

            var mx = 1;
            if (ActiveData.Stage.SenarioData.Modules.Any()) {
                mx += ActiveData.Stage.SenarioData.Modules.Max(k => k.index);

                ActiveData.Stage.SenarioData.Modules.ForEach(unit => {
                    var n = new ModuleLineItem(unit);
                    GridCanvas.Children.Add(n);

                    var summary = new ModuleItemPreview(unit);
                    ModulePreview.Children.Add(summary);
                });
            }



            var cmb = new CreateModuleButton() { index = mx };
            cmb.Shining = ModulePreview.Children.Count == 0;

            ModulePreview.Children.Add(cmb);


        }



        private void mnuSave_Click(object sender, RoutedEventArgs e) {
            new ModuleManager().Save();
            new RelationManager(AppData.current).Save();
            new RelGroupManager(AppData.current).Save();
            new SenarioDataManager(AppData.current).Save();
        }

        private void mnuReload_Click(object sender, RoutedEventArgs e) {
            OnDataRequest(DataChangeOn.Source);
        }

        private void Display_Data_Relation_OnScreen() {
            TempCanvas.Children.Clear();

            if (TempData.NR.State == RelationTempState.None) return;

            var disp = new RelationOnScreen();
            TempCanvas.Children.Add(disp);



            if (TempData.NR.State == RelationTempState.Begin) {
                new RelGenerator().EndPointHandler();
                return;
            }

            if (TempData.NR.State == RelationTempState.Endpoint) {
                var k = new RelGenerator();
                k.Finilize();
                disp.ItemModifier = k.Editor;
                return;
            }



        }


        bool isExpand = true;

        private void SideScroll_Scroll(object sender, ScrollEventArgs e) {
            switch ((sender as ScrollBar).Orientation) {
                case Orientation.Horizontal:
                    x1.ScrollToHorizontalOffset(e.NewValue);
                    break;
                case Orientation.Vertical:
                    x1.ScrollToVerticalOffset(e.NewValue);

                    var ex = e.NewValue > 100 ? false : true;
                    if (isExpand == ex) return;
                    isExpand = ex;
                    ModulePreview.Children.OfType<ModuleItemPreview>().ToList().ForEach(el => {
                        el.Expand = isExpand;
                    });

                    break;
                default:
                    break;
            }
        }

        private void mnuOpenDir_Click(object sender, RoutedEventArgs e) {
            System.Diagnostics.Process.Start("explorer.exe", "/select," + AppData.RootDirectory);

        }

        private void ShowAllMenu_Click(object sender, RoutedEventArgs e) {
            this.ShowAll();
        }

        private void ExportAllMenu_Click(object sender, RoutedEventArgs e) {
            Process.Start(@"C:\Program Files\SaveData\TestPass\Server\HttpServerTest.exe", "--dir " + AppData.RootDirectory);
        }

        private void Rel_Refresh_Menu_Click(object sender, RoutedEventArgs e) {
            Display_Data_Relation();
        }

        private void ExportSenario_Click(object sender, RoutedEventArgs e) {
            Process.Start(@"C:\Program Files\SaveData\TestPass\Server\HttpServerTest.exe", "--senario " + AppData.current.Substring(AppData.RootDirectory.Length + 1)  + " --dir " + AppData.RootDirectory);
        }

        private void SenarioDirMenu_Click(object sender, RoutedEventArgs e) {
            System.Diagnostics.Process.Start("explorer.exe", "/select," + AppData.current);
        }
    }
}
