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
using System.Windows.Shapes;
using TestPassModule.Ui.Access;

namespace TestPassModule.Ui.Windows {
    /// <summary>
    /// Interaction logic for SideWindow.xaml
    /// </summary>
    public partial class SideWindow : Window {
        public SideWindow(UIElement OS) {
            InitializeComponent();

            this.WindowStartupLocation = WindowStartupLocation.Manual;
            this.Topmost = true;
            this.Width = 400;
            this.Height = AppHome.HomeWindow.Height - 50;
            this.Left = AppHome.HomeWindow.Left + AppHome.HomeWindow.Width - 420;
            this.Top = AppHome.HomeWindow.Top + 30;
            this.Content = OS;
        }
    }
}
