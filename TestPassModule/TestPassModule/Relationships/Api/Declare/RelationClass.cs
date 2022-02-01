using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestPassModule.Api.Declare {
    public class RelationClass {
        public int id { get; set; }
        public string title { get; set; }
        public DateTime create { get; set; }

        public SqActionModes actionmode { get; set; }
        public PositionClass position { get; set; }
        public RuntimeClass runtime { get; set; }
        public DocumentClass doc { get; set; }
        public GroupClass group { get; set; }


    }


    public class PositionClass {
        public int parent { get; set; }
        /// <summary>
        /// Is Continue At End Of Parent Or Start Of previous When in Same Module?
        /// </summary>
        public bool isContinue { get; set; }
        public int index { get; set; }
        public int from { get; set; }
        public int to { get; set; }

    }


    public class GroupClass {
        public int id { get; set; }
        public string caption { get; set; }


    }

    public class DocumentClass {
        public List<string> actions { get; set; }
    }
    public class RuntimeClass {
        public int row { get; set; }
        public bool active { get; set; }
        public string state { get; set; }
        public string theme { get; set; }
    }

    public enum SqActionModes {
        Solid, Reply, Self,
        EndPoint,
        StartPoint
    }
}
