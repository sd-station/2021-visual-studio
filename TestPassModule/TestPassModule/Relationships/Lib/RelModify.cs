using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestPassModule.Api.Active;
using TestPassModule.Api.Data;
using TestPassModule.Api.Declare;
using TestPassModule.Api.Manager;
using TestPassModule.Api.Temp;

namespace TestPassModule.Relationships.Lib {
    public class RelModify {
        public RelModify(RelationClass item) {
            this.item = item;
        }

        public RelationClass item { get; }

        public void MoveUp() {
            if (item.position.index <= 1) return;
            ActiveData.Relations.OrderBy(j => j.position.index).Last(p => p.position.index < item.position.index).position.index += 1;
            item.position.index -= 1;
            DataEvents.OnDataChange?.Invoke(DataChangeOn.Relationship);
        }

        public void MoveDown() {
            if (item.position.index >= ActiveData.Relations.Count) return;
            ActiveData.Relations.OrderBy(j => j.position.index).First(p => p.position.index > item.position.index).position.index -= 1;
            item.position.index += 1;
            DataEvents.OnDataChange?.Invoke(DataChangeOn.Relationship);
        }


        public void MoveDownAll() {
            ActiveData.Relations.ForEach(p => {
                if (p.position.index > item.position.index) p.position.index++;
            });
            item.position.index += 1;
            DataEvents.OnDataChange?.Invoke(DataChangeOn.Relationship);
        }

        internal void AddReply() {
            ActiveData.Relations.ForEach(p => { if (p.position.index > item.position.index) p.position.index += 1; });

            var nr = new RelationClass() {

                actionmode = SqActionModes.Reply
            };
            nr.position = new PositionClass();
            nr.position.index = item.position.index + 1;
            nr.position.from = item.position.to;
            nr.position.to = item.position.from;
            nr.position.parent = item.id;

            nr.runtime = new RuntimeClass() { };

            new RelationManager(AppData.current).Add(nr);

            DataEvents.OnDataChange?.Invoke(DataChangeOn.Relationship);
        }

        internal void AddDuplicate() {
            var nr = new RelationClass() {
                actionmode = item.actionmode,
                title = item.title,
            };
            nr.position = new PositionClass();
            nr.position.index = ActiveData.Relations.Max(h => h.position.index) + 1;
            nr.position.from = item.position.from;
            nr.position.to = item.position.to;
            nr.position.parent = item.position.parent;

            nr.runtime = new RuntimeClass() { };

            new RelationManager(AppData.current).Add(nr);

            DataEvents.OnDataChange?.Invoke(DataChangeOn.Relationship);
        }

        internal void AddSubLine() {
            TempData.NR.Begin = item.position.to;
            TempData.NR.IndexRequest = ActiveData.Stage.Relations.Max(h => h.runtime.row) + 1;
            TempData.NR.StartPoint = this.item.runtime.row * SConfig.Grid_Height + SConfig.Grid_Height / 2;
            TempData.NR.State = RelationTempState.Begin;
            TempData.NR.Parent = item.id;
            DataEvents.OnDataChange?.Invoke(DataChangeOn.RelTemp);
        }
    }
}
