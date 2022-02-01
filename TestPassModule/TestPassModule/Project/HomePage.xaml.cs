using Data.Recent;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
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
using TestPassModule.Api.Manager;
using TestPassModule.Home.Display;
using TestPassModule.Project.Lib;

namespace TestPassModule.Home {
    /// <summary>
    /// Interaction logic for HomePage.xaml
    /// </summary>
    public partial class ProjectIndex : Page {

        public bool AutoAccept { get; set; }
        public ProjectIndex() {
            InitializeComponent();
            history = new RecentDir();
            this.Loaded += HomePage_Loaded;
        }

        public Action OnAccpet;

        internal RecentDir history { get; private set; }

        private void HomePage_Loaded(object sender, RoutedEventArgs e) {

            Display_Data();


            if (AutoAccept) Container.Children.OfType<RecentButton>().ToList().FirstOrDefault(y => y.item.path == history.Last)?.StartProject();

        }

        private void Display_Data() {
            Container.Children.Clear();
            history.Store.Items.OrderByDescending(h => h.run).ToList()
                .ForEach(txt => {
                    if (!Directory.Exists(txt.path)) return;

                    var fl = Directory.GetFiles(txt.path, "*.tspm").ToList();
                    if (fl.Any()) {
                        var b = new RecentButton(txt, fl.First());
                        Container.Children.Add(b);

                        b.OnStart = pth => {
                            history.Last = pth;
                            this.OnAccpet();
                        } ;
                    }

                })
                ;

        }



        private void btnNewProject_Click(object sender, RoutedEventArgs e) {
            var nf = new SaveFileDialog();
            nf.Filter = "Test Module Files|*.tspm";
            nf.FileName = "Untitled.tspm";
            if (nf.ShowDialog() == true) {
                var fn = nf.FileName;
                if (!fn.EndsWith(".tspm")) fn += ".tspm";
                new ProjectCommand(fn).Start();
                var fl = new FileInfo(fn);
                history.Last = fl.Directory.FullName;
                OnAccpet?.Invoke();



            }
        }

        private void btnBrowseProject_Click(object sender, RoutedEventArgs e) {
            var nf = new OpenFileDialog();
            nf.Filter = "Test Module Files|*.tspm";
            if (nf.ShowDialog() == true) {
                var fl = new FileInfo(nf.FileName);
                if (fl.Directory.Exists) {
                    AppData.RootDirectory = fl.Directory.FullName;
                    OnAccpet?.Invoke();
                    history.Last = fl.Directory.FullName;
                }
            }
        }
    }
}
