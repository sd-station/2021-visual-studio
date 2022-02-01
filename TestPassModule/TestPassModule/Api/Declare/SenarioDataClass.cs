using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestPassModule.Api.Declare {
    public class SenarioDataClass {
        public List<SenarioModuleClass> Modules { get; set; }
    }

    /// <summary>
    /// Internal Module Can Be temp
    /// </summary>
    public class SenarioModuleClass : IThemeColor {
        public int id { get; set; }
        public int index { get; set; }
        public int module { get; set; }
        public string theme { get; set; }
        public string title { get; set; }
        public DateTime update { get; set; }
    }
}
