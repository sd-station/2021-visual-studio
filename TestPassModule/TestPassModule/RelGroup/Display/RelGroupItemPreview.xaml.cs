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
using TestPassModule.Api.Data;
using TestPassModule.Api.Declare;
using TestPassModule.Ui.Events;

namespace TestPassModule.RelGroup.Display {
    /// <summary>
    /// Interaction logic for RelGroupItemPreview.xaml
    /// </summary>
    public partial class RelGroupItemPreview : UserControl {
        public RelGroupClass item { get; set; }

        public Action<Events.ItemRequest> OnRequest;
        public RelGroupItemPreview(RelGroupClass rg) {
            this.item = rg;
            InitializeComponent();

            Display_Data();

        }

        private void Display_Data() {
            txtID.Text = item.id.ToString();
            txtTitle.Text = item.title ?? "";
            txtMemberInStage.Text = item.rels().Count.ToString();

            var MemberItems = item.AllRelations().OrderBy(k => k.position.index).ToList();
            txtMembers.Text = MemberItems.Count.ToString();

            MemberContainer.Children.Clear();
            MemberItems.ForEach(p => {
                var x = new Button() { Content = $"{p.id}-{p.title}" };
                MemberContainer.Children.Add(x);
            });

            IconDrawable.Visibility = MemberItems.Any() ? Visibility.Visible : Visibility.Collapsed;
            IconEmpty.Visibility = MemberItems.Any() ? Visibility.Collapsed : Visibility.Visible;
        }

        private void txtTitle_LostFocus(object sender, RoutedEventArgs e) {
            item.title = txtTitle.Text;
        }

        private void DeleteMenu_Click(object sender, RoutedEventArgs e) {


            ActiveData.Relations.ForEach(k => {
                if (k.group == null) return;

                if (k.group.id == item.id) {
                    k.group.id = 0;
                    k.group.caption = "";
                }
            });

            ActiveData.RelGroup.Remove(item);

            OnRequest.Invoke(Events.ItemRequest.Reload);
            DataEvents.OnDataChange(DataChangeOn.RelGroup);
        }

        private void RemoveAllMenu_Click(object sender, RoutedEventArgs e) {

            ActiveData.Relations.ForEach(k => {
                if (k.group == null) return;

                if (k.group.id == item.id) {
                    k.group.id = 0;
                    k.group.caption = "";
                }
            });
            Display_Data();
        }

        private void DeleteWithKeepMenu_Click(object sender, RoutedEventArgs e) {

            ActiveData.RelGroup.Remove(item);
            OnRequest.Invoke(Events.ItemRequest.Reload);
            DataEvents.OnDataChange(DataChangeOn.RelGroup);


        }
    }
}
