using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestPassModule.Api.Declare {
    public class RelationClassOld {
        public int id { get; set; }
        public string title { get; set; }
        public DateTime create { get; set; }


        public string state { get; set; }
        public SqActionModes actionmode { get; set; }
         
        public RuntimeClassOld runtime { get; set; }
        public DocumentClassOld doc { get; set; }
        public string theme { get; set; }


        //>> Delete
        public bool isContinue { get; set; }
        public int from { get; set; }
        public int to { get; set; }

        public int index { get; set; }
    }

    public class DocumentClassOld {
        public List<string> actions { get; set; }
    }


    public class RuntimeClassOld {
        public int parent { get; set; }
        public int group { get; set; }
        public bool active { get; set; }
        public int row { get; set; }
    }
   
}
