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

namespace TestPassModule.Api.Manager
{
    public class StructLoader
    {
        public FileInfo FilePath { get; set; }

        public StructLoader()
        {

            this.FilePath = new FileInfo(Path.Combine(AppData.RootDirectory, "struct.json"));


        }

        public void Load()
        {
            ActiveData.General.Structs = new List<Declare.StructClass>();
            if (!System.IO.File.Exists(FilePath.FullName)) return;

            var content = File.ReadAllText(FilePath.FullName);

            var JSE = new JavaScriptSerializer();
            ActiveData.General.Structs = JSE.Deserialize<List<StructClass>>(content);

            ActiveData.General.Structs.ForEach(j =>
            {
                if (j.position == null) j.position = new StructPosition() { parent = j.parent };
            });

        }
        public void Save()
        {

            if (!FilePath.Directory.Exists) FilePath.Directory.Create();

            var JSE = new JavaScriptSerializer();
            File.WriteAllText(FilePath.FullName, JSE.Serialize(ActiveData.General.Structs));
        }

       
    }
}
