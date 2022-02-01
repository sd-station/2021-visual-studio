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

namespace TestPassModel.Ui.Elements {
    /// <summary>
    /// Interaction logic for PageHeader.xaml
    /// </summary>
    public partial class PageHeader : UserControl {
        public PageHeader() {
            InitializeComponent();
        }

        public string Title { get=> txtTitle.Text; set => txtTitle.Text = value; }

        public Action<string> OnRequest { get; set; }
        private void BtnSave_Click(object sender, RoutedEventArgs e) {
            OnRequest?.Invoke("save");
        }
    }
}
