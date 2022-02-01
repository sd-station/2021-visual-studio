using System;
using System.Collections.Generic;
using System.IO;
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
using TestPassModule.Api.Active;
using TestPassModule.Api.Data;
using TestPassModule.Api.Declare;
using TestPassModule.Relationships.Command;

namespace TestPassModule.Relationships.Display {
    /// <summary>
    /// Interaction logic for RelationEnd.xaml
    /// </summary>
    public partial class RelationEnd : UserControl {


        public RelationEnd(RelationClass item) {
            Item = item;
            InitializeComponent();
        }

        public RelationClass Item { get; }


        private void BtnEndPoint_Click(object sender, RoutedEventArgs e) {
            if (Item.doc?.actions?.Any() ?? false) {
                Item.doc.actions.ForEach(n => {
                    if (string.IsNullOrWhiteSpace(n)) return;
                    if (n.StartsWith("--")) return;
                    if (n.StartsWith("//")) return;

                    if (n.StartsWith("open senario")) {
                        var after = n.Substring("open senario".Length).Trim();
                        if (string.IsNullOrWhiteSpace(after)) return;

                        var npath = System.IO.Path.Combine( AppData.RootDirectory, after);

                        if (!Directory.Exists(npath)) Directory.CreateDirectory(npath);

                         AppData.current = npath;

                        DataEvents.OnDataChange(DataChangeOn.Source);



                        return;
                    }
                });
            }
        }

        private void EditMenu_Click(object sender, RoutedEventArgs e) {
            new ModifyRelation(Item).GotoEdit();
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e) {
            new ModifyRelation(Item).Remove();
        }
    }
}
