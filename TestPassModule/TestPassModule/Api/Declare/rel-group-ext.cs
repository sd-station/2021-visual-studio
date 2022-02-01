using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestPassModule.Api.Data;

namespace TestPassModule.Api.Declare;


public static class rel_group_ext {
    public static List<RelationClass> rels(this RelGroupClass item) {
        return ActiveData.Stage.Relations.Where(h => h.group.id == item.id).ToList();
    }

    public static List<RelationClass> AllRelations(this RelGroupClass item) {
        return ActiveData.Relations.Where(h => h.group.id == item.id).ToList();
    }
}


