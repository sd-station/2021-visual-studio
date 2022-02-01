using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestPassModule.Api.Data;
using TestPassModule.Api.Declare;
using TestPassModule.Stage.Relations.Editor;

namespace TestPassModule.Relationships.Command {
    public class ModifyRelation {
        public ModifyRelation(RelationClass item) {
            Item = item;
        }

        public RelationClass Item { get; }

        public void GotoEdit() {
            var ed = new  RelationEditor(Item);

            Ui.Access.AppHome.Side.Navigate(ed);

            ed.OnRequest += cmd => {

                if (cmd == "save") {
                    
                    Ui.Access.AppHome.Side.Close();
                }

                if (cmd == "close") {
                    Ui.Access.AppHome.Side.Close();
                }
            };
        }

        internal void Remove() {
            ActiveData.Relations.Remove(Item);
            DataEvents.OnDataChange?.Invoke(DataChangeOn.Relationship);
        }
    }
}
