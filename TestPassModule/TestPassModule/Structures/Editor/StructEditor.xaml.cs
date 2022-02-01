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
using TestPassModule.Api.Data;
using TestPassModule.Api.Declare;
using TestPassModule.Api.Manager;

namespace TestPassModule.Structs.Editor {
    /// <summary>
    /// Interaction logic for StructEditor.xaml
    /// </summary>
    public partial class StructEditor : Page {
        public StructEditor(int id) {
            InitializeComponent();
            this.item = ActiveData.General.Structs.First(g => g.id == id);
            this.Loaded += StructEditor_Loaded;
        }

        private void StructEditor_Loaded(object sender, RoutedEventArgs e) {

         


            txtID.Text = item.id.ToString();

            txtIndex.Text = item.position.index.ToString();
            txtParent.Text = item.position.parent.ToString();
            txtPath.Text = item.position.path;

            txtCreate.Text = item.update.ToString();
            cmbRelMode.Text = item.mode.ToString();
            txtScope.Text = item.scope.ToString();

            txtTitle.Text = item.title;
            txtName.Text = item.name;
        }

        public StructClass item { get; private set; }

        private void txtTitle_PreviewKeyDown(object sender, KeyEventArgs e) {
            if (e.Key == Key.Enter) {
                item.title = txtTitle.Text;
            }
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e) {
            item.title = txtTitle.Text;
            item.mode = cmbRelMode.Text;
            item.scope = txtScope.Text;
            item.position.path = txtPath.Text;
            item.name = txtName.Text;

            new StructLoader().Save();
        }
    }
}
