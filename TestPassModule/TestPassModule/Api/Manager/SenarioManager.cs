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

namespace TestPassSenario.Api.Manager {
    public class SenarioManager {
        public FileInfo FilePath { get; set; }


        public SenarioManager() {
            this.FilePath = new FileInfo(Path.Combine(AppData.RootDirectory, "senario.json"));
        }

        public void Load() {
            ActiveData.General.Senarios = new List<SenarioClass>();
            if (!File.Exists(FilePath.FullName)) return;
            var content = File.ReadAllText(FilePath.FullName);
            var JSE = new JavaScriptSerializer();
            ActiveData.General.Senarios = JSE.Deserialize<List<SenarioClass>>(content);

            var ix = 1;
            ActiveData.General.Senarios.OrderBy(k => k.index).ToList().ForEach(sn => {
                sn.mode = SenarioMode.Diagram;
                sn.index = ix++;
            });


        }
        public void Save() {

            if (!FilePath.Directory.Exists) FilePath.Directory.Create();

            var JSE = new JavaScriptSerializer();
            File.WriteAllText(FilePath.FullName, JSE.Serialize(ActiveData.General.Senarios));
        }

        internal void Add(SenarioClass nr) {
            nr.id = Guid.NewGuid().ToString();
            nr.create = DateTime.Now;
            nr.update = DateTime.Now;
            ActiveData.General.Senarios.Add(nr);

            try {
                Directory.CreateDirectory(Path.Combine(AppData.RootDirectory, nr.id));
            } catch (Exception) {

            }

        }

        internal void DeleteAndUpdate(SenarioClass item) {
            var id = item.id;
            try {
                Directory.Delete(Path.Combine(AppData.RootDirectory, id), true);
                ActiveData.General.Senarios.Remove(item);
            } catch (Exception ex) {
                MessageBox.Show(ex.Message);               
            }

            Save();
        }

        internal void DuplicateAndSave(SenarioClass item) {
            var nr = new SenarioClass() {
                mode = item.mode,
                index = ActiveData.General.Senarios.Count + 1
            };
            nr.Name = item.Name + " Copy";
            Add(nr);
            Save();

            CopyDir(System.IO.Path.Combine(AppData.RootDirectory, item.id), System.IO.Path.Combine(AppData.RootDirectory, nr.id));

        }


        private static void CopyDir(string sourcePath, string targetPath) {
            //Now Create all of the directories
            foreach (string dirPath in Directory.GetDirectories(sourcePath, "*", SearchOption.AllDirectories)) {
                Directory.CreateDirectory(dirPath.Replace(sourcePath, targetPath));
            }

            //Copy all the files & Replaces any files with the same name
            foreach (string newPath in Directory.GetFiles(sourcePath, "*.*", SearchOption.AllDirectories)) {
                File.Copy(newPath, newPath.Replace(sourcePath, targetPath), true);
            }
        }

    }
}
