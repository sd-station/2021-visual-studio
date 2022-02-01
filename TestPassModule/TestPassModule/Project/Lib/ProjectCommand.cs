using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestPassModule.Api.Active;
using TestPassModule.Api.Manager;

namespace TestPassModule.Project.Lib {
    internal class ProjectCommand {
        private string fileName;

        public ProjectCommand(string fileName) {
            this.fileName = fileName;
        }

        internal void Start() {
            var file = new FileInfo(fileName);
            if (file.Directory == null) return;
            if (!file.Directory.Exists) file.Directory.Create();

            var MGR = new ProjectInfoManager(file.FullName);
            if (!file.Exists) MGR.Create(); else MGR.Load();

            AppData.RootDirectory = file.Directory.FullName;
          
        }
    }
}
