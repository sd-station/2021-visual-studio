using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestPassModule.Api.Data;
using TestPassModule.Api.Declare;

namespace TestPassModule.RelGroup.Lib {
    public class RelGroupBoundGenerator {

        public BoundClass Bound { get; private set; }

        public RelGroupBoundGenerator() {
            this.Bound = new BoundClass() {
                Left = 0,
                Right = 0,
                Top = 0,
                Bottom = 0,
            };
        }

        internal RelGroupBoundGenerator  Review(RelGroupClass item) {

         


            item.rels().ForEach(f => {

                var rel = ActiveData.Stage.Relations.FirstOrDefault(k => k.id == f.id);
                if (rel == null) return;
                if (Bound.Top == 0 || Bound.Top > rel.runtime.row) Bound.Top = rel.runtime.row;
                if (Bound.Bottom < rel.runtime.row) Bound.Bottom = rel.runtime.row;

                var to = ActiveData.Stage.SenarioData.Modules.FirstOrDefault(k => k.id == rel.position.to) ?? ActiveData.Stage.SenarioData.Modules.Last();
                if (Bound.Left == 0 || Bound.Left > to.index) Bound.Left = to.index;
                if (Bound.Right < to.index) Bound.Right = to.index;

                var from = ActiveData.Stage.SenarioData.Modules.FirstOrDefault(k => k.id == rel.position.from) ?? ActiveData.Stage.SenarioData.Modules.First();
                if (Bound.Left == 0 || Bound.Left > from.index) Bound.Left = from.index;
                if (Bound.Right < from.index) Bound.Right = from.index;

            });

            return this;
        }
    }
}
