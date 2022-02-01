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
using TestPassModule.ModuleLine.Lib;

namespace TestPassModule.ModuleLine.Display {
    /// <summary>
    /// Interaction logic for ModuleButton.xaml
    /// </summary>
    public partial class ModuleButton : UserControl {
        internal Action<string> OnRequest;

        public ModuleButton(Api.Declare.ModuleClass tb, bool Allowtheme = true) {
            this.item = tb;
            
            InitializeComponent();
            this.txtTitle.Text = tb.title;
           if(Allowtheme) ShowTheme();
        }

        public ModuleClass item { get; private set; }

        private void BtnAction_Click(object sender, RoutedEventArgs e) {
            OnRequest?.Invoke("accept");
        }

        private void ShowTheme() {           
      
            this.BorderBrush = item.BorderBrush();
            this.Foreground = item.Foreground();
        }
    }
}
