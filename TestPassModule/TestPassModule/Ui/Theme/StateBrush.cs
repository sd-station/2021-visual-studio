
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using TestPassModule.Ui.Colorize;

namespace TestPassModule.Ui.Theme {
    public class StateBrush {

        public new Dictionary<string, string> States {
            get => new Dictionary<string, string> {
                {"pending","#607d8b" },
                {"waiting","#2196f3"},
                {"in-progress","#4caf50"},
                {"harry-up","#ffeb3b"},
                {"out-of-time","#ff9800"},
                {"pause","#ff5722"},
                {"stop","#d32f2f"},
                {"cancel","#000000"},
                {"success","#009688"},
                {"failed","#9e9e9e"},

            };
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
