using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using TestPassModule.Api.Declare;
using TestPassModule.Ui.Colorize;

namespace TestPassModule.ModuleLine.Lib {
    internal static class module_line_ext {

        public static SolidColorBrush Foreground(this  IThemeColor item) {
            if ( string.IsNullOrEmpty(item.theme)) item.theme = "#707070";

            var color = (Color)ColorConverter.ConvertFromString(item.theme);

            return  ColorLighter.AsBrush(color, 0.5);
        }
        public static SolidColorBrush BorderBrush(this IThemeColor item) {
            if ( string.IsNullOrEmpty(item.theme)) item.theme = "#707070";

            var color = (Color)ColorConverter.ConvertFromString(item.theme);

            return new SolidColorBrush(color);

        }
    }
}
