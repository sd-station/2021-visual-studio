using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestPassModule.Api.Data;
using TestPassModule.Api.Declare;
using TestPassModule.Api.Manager;

namespace TestPassModel.Structures.Lib {
    public class StructBuilder {

        public StructClass item { get; private set; }

        public StructBuilder() {
            item = new StructClass();
            item.position = new StructPosition();
        }

        public StructBuilder(StructClass nr) {
            this.item = nr;
        }

        public void Create(string scope, int parent = 0, int index = -1) {
            item.scope = scope;
            item.position.parent = parent;
            item.position.index = index;
            Finilize();
        }

        public void Create((string scope, int parent, int index) param) {
            item.scope = param.scope;
            item.position.parent = param.parent;
            item.position.index = param.index;

            Finilize();

        }

        public void Finilize() {



            item.id = 1;
            item.id += ActiveData.General.Structs.LastOrDefault()?.id ?? 0;

            item.name = "item-" + item.id;
            item.title = "new item " + item.id;

            item.theme = "#707070";
            item.mode = "none";
            item.type = "none";


            item.position.path = "";



            var Siblings = ActiveData.General.Structs.Where(j => j.scope == item.scope && j.position.parent == item.position.parent).ToList();

            if (item.position.index == -1) {
                item.position.index = 1;
                if (Siblings.Any()) item.position.index += Siblings.Count;
            } else {
                Siblings.ForEach(k => { if (k.position.index >= item.position.parent) k.position.index++; });
            }

            item.create = DateTime.Now;
            item.update = DateTime.Now;

            ActiveData.General.Structs.Add(item);

            new StructLoader().Save();

        
        }



        public string scope { get => item.scope; set => item.scope = value; }
        public int parent { get => item.position.parent; set => item.position.parent = value; }
        public int index { get => item.position.index; set => item.position.index = value; }


    }
}
