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
using TestPassModel.Model.Edit;
using TestPassModule.Api.Data;
using TestPassModule.Api.Manager;
using TestPassModule.Ui.Colorize;

namespace TestPassModule.Structures.Pages
{
    /// <summary>
    /// Interaction logic for NameSpacePage.xaml
    /// </summary>
    public partial class StructListPage : Page
    {
        public StructListPage()
        {
            InitializeComponent();
            this.Loaded += StructListPage_Loaded;

           

        }

        private void StructListPage_Loaded(object sender, RoutedEventArgs e) {

            Display();
        }

        private void Display() {
            Container.Children.Clear();
            ActiveData.General.Modules.ForEach(ns => {
                var chr = new ModelInlineEditor(ns);
                Container.Children.Add(chr);

                var pack = new Structures.TableMode.StructListView(ns.id);
                Container.Children.Add(pack);

                chr.OnAction += cmd => {
                    if (cmd == "plus-button") pack.AddItem();
                };

            });
        }

        public void OnRequest_Click( string cmd) {
            new Api.Manager.ModuleManager().Save();
            new StructLoader().Save();
        }
    }
}
