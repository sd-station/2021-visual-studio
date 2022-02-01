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
    public class ModuleManager {
        public FileInfo FilePath { get; set; }

        public ModuleManager() {

            this.FilePath = new FileInfo(Path.Combine(AppData.RootDirectory, "uml.json"));

          
        }

        public void Load() {
            ActiveData.General.Modules = new List<Declare.ModuleClass>();
            if (!System.IO.File.Exists(FilePath.FullName)) return;

            var content = File.ReadAllText(FilePath.FullName);

            var JSE = new JavaScriptSerializer();
            ActiveData.General.Modules = JSE.Deserialize<List<ModuleClass>>(content);


            ActiveData.General.Modules.ForEach(u => {

                if (u.create.Year < DateTime.Now.Year) u.create = DateTime.Now;
                if (u.update.Year < DateTime.Now.Year) u.update = DateTime.Now;

                
            });
 

        }
        public void Save() {

            if (!FilePath.Directory.Exists) FilePath.Directory.Create();

            var JSE = new JavaScriptSerializer();
            File.WriteAllText(FilePath.FullName, JSE.Serialize(ActiveData.General.Modules));
        }

        internal void Add(ModuleClass nr) {
            nr.id = 1 + (ActiveData.General.Modules.LastOrDefault()?.id ?? 0);
            nr.create = DateTime.Now;
            nr.update = DateTime.Now;
            ActiveData.General.Modules.Add(nr);

        }
    }
}
