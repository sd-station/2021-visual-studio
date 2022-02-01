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
    public class SenarioDataManager {
        public FileInfo FilePath { get; set; }

        public SenarioDataManager(string FullPath) {
            var path = Path.Combine(FullPath, "module.info.json");
            this.FilePath = new FileInfo(path);


        }

        public void Load() {
            ActiveData.Stage.SenarioData = new SenarioDataClass();
            ActiveData.Stage.SenarioData.Modules = new List<SenarioModuleClass>();
            if (!System.IO.File.Exists(FilePath.FullName)) return;

            var content = File.ReadAllText(FilePath.FullName);

            var JSE = new JavaScriptSerializer();
            ActiveData.Stage.SenarioData = JSE.Deserialize<SenarioDataClass>(content);

            if(ActiveData.Stage.SenarioData.Modules == null) ActiveData.Stage.SenarioData.Modules = new List<SenarioModuleClass>();

            //>> Change Version
            ActiveData.Stage.SenarioData.Modules.ForEach(itm => {
                if (itm.id <= 0) itm.id = itm.module;
            });


        }
        public void Save() {

            if (!FilePath.Directory.Exists) FilePath.Directory.Create();
            var JSE = new JavaScriptSerializer();
            File.WriteAllText(FilePath.FullName, JSE.Serialize(ActiveData.Stage.SenarioData));
        }


    }
}

