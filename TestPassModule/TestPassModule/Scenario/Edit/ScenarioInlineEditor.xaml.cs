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
using TestPassModule.Ui.Events;
using TestPassSenario.Api.Manager;

namespace TestPassModule.Senario.Edit {
    /// <summary>
    /// Interaction logic for SenarioInlineEditor.xaml
    /// </summary>
    public partial class SenarioInlineEditor : UserControl {
        internal Action<Events.EditorRequests> OnRequest;
        public SenarioClass item { get; private set; }
        public SenarioInlineEditor(SenarioClass itm) {
            this.item = itm;
            InitializeComponent();
            txtTitle.Text = item.Name;
            this.Loaded += SenarioInlineEditor_Loaded;
        }

        private void SenarioInlineEditor_Loaded(object sender, RoutedEventArgs e) {
            if (string.IsNullOrWhiteSpace(txtTitle.Text) || txtTitle.Text.StartsWith("New ")) {
                txtTitle.Focus();
                txtTitle.SelectAll();
            }
        }

        private void btnSave_Click(object sender, RoutedEventArgs e) {
            SaveCommand();
        }

        private void SaveCommand() {
            if (string.IsNullOrWhiteSpace(txtTitle.Text)) {
                txtTitle.Focus(); return;
            }
            item.Name = txtTitle.Text;
            var handler = new SenarioManager();
            if (string.IsNullOrEmpty(item.id)) handler.Add(item);
            handler.Save();
            OnRequest?.Invoke(Events.EditorRequests.Save);
        }

        private void btnClose_Click(object sender, RoutedEventArgs e) {
            OnRequest?.Invoke(Events.EditorRequests.Close);
        }

        private void txtTitle_PreviewKeyDown(object sender, KeyEventArgs e) {
            if(e.Key == Key.Enter) {
                e.Handled = true;
                SaveCommand();
            }
        }
    }
  
}
