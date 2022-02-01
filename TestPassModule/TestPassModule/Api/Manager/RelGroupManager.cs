using Nancy.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestPassModule.Api.Active;
using TestPassModule.Api.Data;
using TestPassModule.Api.Declare;

namespace TestPassModule.Api.Manager {
    public class RelGroupManager {
        public FileInfo FilePath { get; set; }

        public RelGroupManager(string FullPath) {
            var path = Path.Combine(FullPath, "uml-rel-group.json");
            this.FilePath = new FileInfo(path);


        }

        public void Load() {
            ActiveData.RelGroup = new List<Declare.RelGroupClass>();
            if (!System.IO.File.Exists(FilePath.FullName)) return;

            var content = File.ReadAllText(FilePath.FullName);

            var JSE = new JavaScriptSerializer();
            ActiveData.RelGroup = JSE.Deserialize<List<RelGroupClass>>(content);


            ActiveData.RelGroup.ForEach(u => {
                if (u.create.Year < DateTime.Now.Year) u.create = DateTime.Now;
                if (u.update.Year < DateTime.Now.Year) u.update = DateTime.Now;
            });


        }
        public void Save() {

            if (!FilePath.Directory.Exists) FilePath.Directory.Create();

            var JSE = new JavaScriptSerializer();
            File.WriteAllText(FilePath.FullName, JSE.Serialize(ActiveData.RelGroup));
        }

        internal RelGroupClass Add(RelGroupClass nr) {
            nr.id = 1;
            if (ActiveData.RelGroup.Any()) nr.id += ActiveData.RelGroup.Max(t => t.id);
            nr.create = DateTime.Now;
            nr.update = DateTime.Now;
            ActiveData.RelGroup.Add(nr);
            this.Save();
            return nr;
        }
    }
}
