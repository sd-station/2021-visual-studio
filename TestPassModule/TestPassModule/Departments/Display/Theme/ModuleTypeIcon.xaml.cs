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

namespace TestPassModule.ModuleLine.Display.Theme {
    /// <summary>
    /// Interaction logic for ModuleTypeIcon.xaml
    /// </summary>
    public partial class ModuleTypeIcon : UserControl {
        public ModuleTypeIcon() {
            InitializeComponent();
        }

        internal void Display(string v) {
            Shapes.Children.OfType<UIElement>().ToList().ForEach(h => {
                h.Visibility = h.Uid == v ? Visibility.Visible : Visibility.Collapsed;
            });
        }
    }
}
