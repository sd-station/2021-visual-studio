using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestPassModule.Api.Data;
using TestPassModule.Api.Declare;

namespace TestPassModule.Relationships.Api.ext;
public static class relationship_ext {
    public static double Left(this RelationClass item) {

        int StartIndex = 0, EndIndex = 0;

        if (item.position.from > 0)
            StartIndex = ActiveData.Stage.SenarioData.Modules.First(n => n.id == item.position.from).index;

        if (item.position.to > 0)
            EndIndex = ActiveData.Stage.SenarioData.Modules.First(n => n.id == item.position.to).index;

        return StartIndex > EndIndex ? EndIndex : StartIndex;
    }

   public static double Top(this RelationClass item) {
        return item.runtime.row;
    }
}
