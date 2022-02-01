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
using TestPassModel.Structures.Lib;
using TestPassModel.Ui.enums;
using TestPassModule.Api.Data;

namespace TestPassModule.Structures.TableMode {
    /// <summary>
    /// Interaction logic for StructTableView.xaml
    /// </summary>
    public partial class StructListView : UserControl {
        private int ns_id;

        public string scope { get => "module:" + ns_id; }
        public int SelectedItem { get; private set; }

        public StructListView(int id) {
            this.ns_id = id;
            InitializeComponent();


            Display_Data();
        }

        public void Display_Data() {
            Container.Children.Clear();
            FindItems(0, 0);
        }

        private void FindItems(int parent, int level) {
            var ix = 1;
            ActiveData.General.Structs
                .Where(k => k.scope == scope && k.position.parent == parent)
                .OrderBy(k => k.position.index).ToList().ForEach(ns => {
                    ns.position.index = ix++;
                    ns.position.level = level;
                    var x = new Display.StructListItem(ns);
                    x.SelectOnLoad = ItemSelectionMode.Auto;
                    if (this.SelectedItem > 0) x.SelectOnLoad = this.SelectedItem == ns.id ? ItemSelectionMode.Selected : ItemSelectionMode.None;


                    x.OnRequest += (cmd, i) => {
                        if (cmd == "delete") {
                            ActiveData.General.Modules.RemoveAll(g => g.id == i);
                            Display_Data();
                        }
                        if (cmd == "refresh") {
                            this.SelectedItem = i;
                            Display_Data();
                        }
                    };
                    Container.Children.Add(x);

                    FindItems(ns.id, level + 1);
                });
        }

        internal void AddItem() {
            new StructBuilder().Create(scope);
            Display_Data();
        }
    }
}
