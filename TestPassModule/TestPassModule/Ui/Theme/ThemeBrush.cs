
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using TestPassModule.Ui.Colorize;

namespace TestPassModule.Ui.Theme {
    public class ThemeBrush {

        public new Dictionary<string, string> States {
            get => new Dictionary<string, string> {
                {"red","#d32f2f"},
                {"pink","#c2185b"},
                {"purple","#7b1fa2"},
                {"deep-purple","#512da8"},
                {"indigo","#303f9f"},
                {"blue","#1976d2"},
                {"light-blue","#0288d1"},
                {"cyan","#0097a7"},
                {"teal","#00796b"},
                {"green","#388e3c"},
                {"light-green","#689f38"},
                {"lime","#afb42b"},
                {"yellow","#fbc02d"},
                {"amber","#ffa000"},
                {"orange","#ffa000"},
                {"deep-orange","#e64a19"},
                {"gray","#707070" },
                {"blue-gray","#455a64" },

            };
        }

       
        internal IEnumerable<MenuItem> GetAsMenuList() {

            foreach (var state in States.Keys.ToList()) {
                MenuItem MN = new MenuItem() { Header = state.Replace("-", " "), Uid = States[state] };
                MN.Foreground = GetBrush(state);              
                yield return MN;
            }
        
        }

        public Brush GetBrush(string code) {

            if (string.IsNullOrEmpty(code)) code = "pending";

            if (!States.Keys.Contains(code)) code = "cancel";

            var color = (Color)ColorConverter.ConvertFromString(States[code]);

            return new SolidColorBrush(ColorLighter.ChangeColorBrightness(color, (float)-0.3));
        }
        public Brush GetColor(string code) {

            if (string.IsNullOrEmpty(code)) code = "pending";

            if (!States.Keys.Contains(code)) code = "cancel";

            var color = (Color)ColorConverter.ConvertFromString(States[code]);



            return new SolidColorBrush(ColorLighter.ChangeColorBrightness(color, (float)0.5));

        }





    }




}
