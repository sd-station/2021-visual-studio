using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using TestPassModule.Senario;

namespace TestPassModule.Home.Lib {
    public class SideHandler {
        public Frame El { get; init; }
        public Border Cover { get; init; }
        public Button Closer { get; internal set; }

        internal void Close() {
            El.Content = null;
            while (El.CanGoBack) El.RemoveBackEntry();
            Closer.Visibility = Cover.Visibility = System.Windows.Visibility.Collapsed;

        }

        internal void Navigate(Page senarioIndexPage ,string theme = "#707070") {
            El.Navigate(senarioIndexPage);
            Closer.Visibility = Cover.Visibility = System.Windows.Visibility.Visible;
            Closer.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString(theme) );
        }
    }
}
