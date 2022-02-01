using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestPassModule.Api.Data {
    public class DataEvents {
        public static Action<DataChangeOn> OnDataChange {  get; set;}
    }

    public enum DataChangeOn {
        Module,
        Relationship,
        RelTemp,
        Source,
        RelGroup,
        Stage_RelationShips
    }
}
