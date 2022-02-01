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

namespace TestPassModule.Structs.Display {
    /// <summary>
    /// Interaction logic for NewStructButton.xaml
    /// </summary>
    public partial class NewStructButton : UserControl {
        internal Action OnRequest;

        public NewStructButton() {
            InitializeComponent();
        }

        private void btnAction_Click(object sender, RoutedEventArgs e) {
            OnRequest?.Invoke();
        }
    }
}
