using Nancy.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestPassModule.Api.Data;
using TestPassModule.Api.Declare;

namespace TestPassModule.Api.Manager {
    public class RelationManager {
        public FileInfo FilePath { get; set; }

        public RelationManager(string FullPath) {

            var path = Path.Combine(FullPath, "uml-rel.json");
            this.FilePath = new FileInfo(path);

        }

        public void Load() {
            ActiveData.Relations = new List<Declare.RelationClass>();
            if (!System.IO.File.Exists(FilePath.FullName)) return;

            var content = File.ReadAllText(FilePath.FullName);

            var JSE = new JavaScriptSerializer();
            ActiveData.Relations = JSE.Deserialize<List<RelationClass>>(content);

            if (ActiveData.Relations[0].position == null) {
                ActiveData.Relations = new List<RelationClass>();
                var ctx = JSE.Deserialize<List<RelationClassOld>>(content);
                ctx.ForEach(old => {

                    var nr = new RelationClass();
                    nr.id = old.id;
                    nr.title = old.title;
                    nr.create = old.create;

                    nr.actionmode = old.actionmode;

                    nr.position = new PositionClass();
                    nr.position.parent = old.runtime.parent;
                    nr.position.isContinue = old.isContinue;
                    nr.position.index = old.index;
                    nr.position.from = old.from;
                    nr.position.to = old.to;


                    nr.runtime = new RuntimeClass();
                    nr.runtime.row = old.runtime.row;
                    nr.runtime.active = old.runtime.active;
                    nr.runtime.state = old.state;
                    nr.runtime.theme = old.theme;

                    nr.doc = new DocumentClass();
                    nr.doc.actions = new List<string>();
                    if (old.doc != null && old.doc.actions != null) nr.doc.actions.AddRange(old.doc.actions);


                    nr.group = new GroupClass();
                    nr.group.id = old.runtime.group;
                    nr.group.caption = "";


                    ActiveData.Relations.Add(nr);
                });

            }

            int ix = 0;
            ActiveData.Relations.OrderBy(k => k.position.index).ToList().ForEach(item => {

                item.position.index = item.position.isContinue ? ix : ++ix;
            });

            ActiveData.Relations.ForEach(item => {
                if (item.runtime == null) item.runtime = new RuntimeClass();

                item.runtime.active = false;
            });


            

        }
        public void Save() {

            //if (!FilePath.Directory.Exists) FilePath.Directory.Create();

            var JSE = new JavaScriptSerializer();
            File.WriteAllText(FilePath.FullName, JSE.Serialize(ActiveData.Relations));
        }

        internal void Add(RelationClass nr) {
            nr.id = 1 + (ActiveData.Relations.LastOrDefault()?.id ?? 0);
            nr.create = DateTime.Now;
            if (nr.runtime == null) nr.runtime = new RuntimeClass();
            ActiveData.Relations.Add(nr);
            this.Save();
        }
    }
}
