using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace TestPassModule.Structs.Lib {
    public class StructureRequest {
        public string command { get; internal set; }
        public int scopeid { get; internal set; }
        public int id { get; internal set; }
 
        public int level { get; internal set; }
        public Point p { get; internal set; }
    }
}
