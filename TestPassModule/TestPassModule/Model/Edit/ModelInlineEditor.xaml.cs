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
using TestPassModule.Modules.Windows;
using TestPassModule.Ui.Colorize;

namespace TestPassModel.Model.Edit {
    /// <summary>
    /// Interaction logic for ModelInlineEditor.xaml
    /// </summary>
    public partial class ModelInlineEditor : UserControl {

        public ModelInlineEditor(ModuleClass ns) {
            item = ns;
            InitializeComponent();
            txtTitle.Text = ns.title;
            txtTitle.ToolTip = ns.title;
            IconShape.Stroke = BrushGenerator.FromString(ns.theme);
        }

        public ModuleClass item { get; }
        public Action<string> OnAction;

        private void btnAction_Click(object sender, RoutedEventArgs e) {
            OnAction?.Invoke("plus-button");
        }

        private void DisplayActionButton_Click(object sender, RoutedEventArgs e) {
            var s = new ModelWindow(item);
            s.Show();
        }
    }
}
