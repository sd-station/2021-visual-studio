using Nancy.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using TestPassModule.Api.Active;
using TestPassModule.Api.Data;
using TestPassModule.Api.Declare;

namespace TestPassModule.Api.Manager {
    public class ProjectInfoManager {
        public FileInfo FilePath { get; set; }


        public ProjectInfoManager(string FileName) {
            this.FilePath = new FileInfo(FileName);
        }

        public void Load() {
            ActiveData.ProjectInfo = new ProjectInfoClass();
            if (!File.Exists(FilePath.FullName)) return;
            var content = File.ReadAllText(FilePath.FullName);
            var JSE = new JavaScriptSerializer();
            ActiveData.ProjectInfo = JSE.Deserialize<ProjectInfoClass>(content);

        }
        public void Save() {
            if (FilePath.Directory == null) return;
            if (!FilePath.Directory.Exists) FilePath.Directory.Create();

            var JSE = new JavaScriptSerializer();
            File.WriteAllText(FilePath.FullName, JSE.Serialize(ActiveData.ProjectInfo));
        }

        public void Create() {
            ActiveData.ProjectInfo = new ProjectInfoClass();
            ActiveData.ProjectInfo.CreateTime = DateTime.Now;
            Save();
        }
    }
}
