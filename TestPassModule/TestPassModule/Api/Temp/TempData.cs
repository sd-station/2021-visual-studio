using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestPassModule.Api.Temp {
    class TempData {
         

        public static TempRow NR { get; set; }

        public static List<int> RelationshipCollection { get; internal set; }
    }

    public class TempRow {
        /// <summary>
        /// Begin Module ID
        /// </summary>
        public int Begin { get; set; } = 0;
        /// <summary>
        /// End Module ID
        /// </summary>
        public int End { get; set; } = 0;

        /// <summary>
        /// Index Request In Place
        /// </summary>
        public int IndexRequest { get; set; }

        public RelationTempState State { get; set; } = RelationTempState.None;
        public double StartPoint { get; set; }
        public int Parent { get; internal set; }
    }

    public enum RelationTempState {
        None = 0,
        Begin = 1,
        Endpoint = 2
    }
}
