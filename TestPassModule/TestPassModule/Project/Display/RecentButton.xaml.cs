using Data.Recent;
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
using TestPassModule.Api.Manager;
using TestPassModule.Project.Lib;

namespace TestPassModule.Home.Display {
    /// <summary>
    /// Interaction logic for RecentButton.xaml
    /// </summary>
    public partial class RecentButton : UserControl {
       
        internal Action<string> OnStart;

        public StoreItem item { get; set; }
        public string fileName { get;   set; }

        public RecentButton(Data.Recent.StoreItem txt, string v) {
            this.item = txt;
            this.fileName = v;
            InitializeComponent();
            txtTitle.Text = System.IO.Path.GetFileNameWithoutExtension(v);
        }

        private void btnTitle_Click(object sender, RoutedEventArgs e) {
            StartProject();
        }

        internal void StartProject() {
            new ProjectCommand(fileName).Start();
            OnStart?.Invoke(item.path);
        }

        private void btnOpenDir_Click(object sender, RoutedEventArgs e) {
            if(System.IO.Directory.Exists(item.path))
                System.Diagnostics.Process.Start("explorer.exe", "/select," + item.path);
        }
    }
}
