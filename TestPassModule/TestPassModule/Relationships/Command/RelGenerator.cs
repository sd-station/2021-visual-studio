using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using TestPassModule.Api.Active;
using TestPassModule.Api.Declare;
using TestPassModule.Api.Data;
using TestPassModule.Api.Temp;
using TestPassModule.Ui.Access;
using TestPassModule.Stage.Relations.Editor;

namespace TestPassModule.Relationships.Command {
    public class RelGenerator {
        public RelationEditor Editor { get; set; }

        internal void Finilize() {
            var nr = new RelationClass() {
                id = -1,
            };

            nr.position = new PositionClass();
            nr.position.index = TempData.NR.IndexRequest;
            nr.position.from = TempData.NR.Begin;
            nr.position.to = TempData.NR.End;

            nr.runtime = new RuntimeClass();
            if (TempData.NR.Parent > 0) nr.position.parent = TempData.NR.Parent;

            Editor = new Stage.Relations.Editor.RelationEditor(nr);
            AppHome.Side.Navigate(Editor);

            Editor.OnRequest += (cms) => {

                if (cms == "save") {
                    TempData.NR = new TempRow();
                    AppHome.Side.Close();
                    DataEvents.OnDataChange(DataChangeOn.Relationship);
                }

                if (cms == "close") {
                    TempData.NR = new TempRow();
                    AppHome.Side.Close();
                    DataEvents.OnDataChange(DataChangeOn.Relationship);

                }
            };

        }

        internal void Modify(Point PointA, int id) {

            switch (TempData.NR.State) {
                case RelationTempState.None:
                    TempData.NR.Begin = id;
                    var dec = Math.Round(PointA.Y - SConfig.Grid_Height / 2);

                    var indexDevide = Math.Ceiling(dec / SConfig.Grid_Height);
                    var index2 = (int)indexDevide;

                    TempData.NR.IndexRequest = index2;
                    TempData.NR.StartPoint = PointA.Y;
                    TempData.NR.State = RelationTempState.Begin;
                    if (ActiveData.Stage.Relations.Any(k => k.runtime.row == TempData.NR.IndexRequest)) {
                        ActiveData.Stage.Relations.ForEach(k => { if (k.runtime.row >= TempData.NR.IndexRequest) k.runtime.row++; });
                        DataEvents.OnDataChange?.Invoke(DataChangeOn.Stage_RelationShips);
                    }
                    DataEvents.OnDataChange?.Invoke(DataChangeOn.RelTemp);

                    return;
                case RelationTempState.Begin:
                    TempData.NR.End = id;
                    TempData.NR.State = RelationTempState.Endpoint;
                    DataEvents.OnDataChange?.Invoke(DataChangeOn.RelTemp);
                    return;
                case RelationTempState.Endpoint:
                    TempData.NR.State = RelationTempState.None;
                    Modify(PointA, id);

                    break;
                default:
                    break;
            }
        }

        internal void EndPointHandler() {
            var fr = new ModuleLine.Selector.ModuleItemPicker();
            AppHome.Side.Navigate(fr);
        }
    }
}
