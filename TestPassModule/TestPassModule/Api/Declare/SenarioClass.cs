using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestPassModule.Api.Declare {
    public class SenarioClass {
        public string id { get; set; }
        public string Name { get; set; }

        public SenarioMode mode { get; set; }
        public DateTime create { get; set; }
        public DateTime update { get; set; }
        public int index { get;   set; }
        public DateTime visit { get;   set; }
    }


    public enum SenarioMode {
        None,Diagram 
    }
}
