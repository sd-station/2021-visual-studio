using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestPassModule.Api.Declare;

namespace TestPassModule.Api.Data {
    public class StageActiveData {
        public SenarioDataClass SenarioData { get; set; }
        public List<RelationClass> Relations { get; set; }
    }

    public class GeneralActiveData {
        public List<SenarioClass> Senarios { get; set; }
        public List<ModuleClass> Modules { get; set; }

        public   List<StructClass> Structs { get;   set; }
    }

    public class ActiveData {


        public static List<RelationClass> Relations { get; set; }
        public static bool DependencyMode { get;   set; }

        public static StageActiveData Stage { get; set; }
        public static GeneralActiveData General { get; set; }
        public static ProjectInfoClass ProjectInfo { get;   set; }
        internal static List<RelGroupClass> RelGroup { get; set; }
   
    }
}
