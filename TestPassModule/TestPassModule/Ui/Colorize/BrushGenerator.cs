using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace TestPassModule.Ui.Colorize {
    public class BrushGenerator {
        internal static SolidColorBrush FromString(string color) => new SolidColorBrush((Color)ColorConverter.ConvertFromString(color));
    }
}
