using System;
using System.Collections.Generic;
using System.Linq;

using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using TestPassModule.Api.Declare;

namespace TestPassModule.Ui.Edit {
    /// <summary>
    /// Interaction logic for PropertyEditor.xaml
    /// </summary>
    public partial class PropertyEditor : UserControl {
        public PropertyEditor(string v) {
            InitializeComponent();
            txtTitle.Content = v;
        }

        internal void AddEditor(int value) {
            AddEditor(nameof(value), value.ToString());
        }
        internal void AddEditor(string value) {
            AddEditor(nameof(value), value);
        }
        internal void AddEditor(string label, int value) {
            AddEditor(label, value.ToString());
        }
        internal void AddEditor(string label, string value) {
            ContentGrid.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });
            var chi = new Label() { Content = label, Style = (Style)TryFindResource("input-label") };
            Grid.SetColumn(chi, 0);
            Grid.SetRow(chi, ContentGrid.RowDefinitions.Count - 1);
            ContentGrid.Children.Add(chi);

            var txt = new TextBox();
            txt.Uid = label;
            txt.Text = value;
            Grid.SetColumn(txt, 1);
            Grid.SetRow(txt, ContentGrid.RowDefinitions.Count - 1);
            ContentGrid.Children.Add(txt);

        }

        internal void Show(string title, string value) {
            ContentGrid.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });
            var chi = new Label() { Content = title, Style = (Style)TryFindResource("input-label") };
            Grid.SetColumn(chi, 0);
            Grid.SetRow(chi, ContentGrid.RowDefinitions.Count - 1);
            ContentGrid.Children.Add(chi);

            var txt = new TextBlock();
            txt.Text = value;
            Grid.SetColumn(txt, 1);
            Grid.SetRow(txt, ContentGrid.RowDefinitions.Count - 1);
            ContentGrid.Children.Add(txt);
        }

        public void showname(dynamic obj) {
            Type myType = obj.GetType();
            var propertyInfo2 = myType.GetProperties();
            propertyInfo2.ToList().ForEach(state => {
                var kkkk = state.PropertyType.Name.ToLower();
                if (kkkk  == "string")
                    AddEditor(state.Name, $"{state.GetValue(obj, null).ToString()}");
                else
                    Show(state.Name, $"{state.GetValue(obj, null).ToString()}");
            });
        }

        public void update(dynamic obj) {
            Type myType = obj.GetType();
            var propertyInfo2 = myType.GetProperties();

            ContentGrid.Children.OfType<TextBox>().ToList().ForEach(txt => {
                propertyInfo2.ToList().ForEach(state => {
                    var name = state.Name;
                    if (name == txt.Uid) state.SetValue(obj, txt.Text);
                });
            });

         
        }
    }
}
