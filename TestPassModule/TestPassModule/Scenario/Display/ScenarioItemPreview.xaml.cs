using System;
using System.Collections.Generic;
using System.IO;
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
using TestPassModule.Api.Active;
using TestPassModule.Api.Data;
using TestPassModule.Api.Declare;
using TestPassModule.Api.Manager;
using TestPassModule.Ui.Events;
using TestPassModule.Ui.Windows;
using TestPassSenario.Api.Manager;

namespace TestPassModule.Senario.Display {
    /// <summary>
    /// Interaction logic for SenarioItemPreview.xaml
    /// </summary>
    public partial class SenarioItemPreview : UserControl {
        public Action<Events.ItemRequest> OnUpdate;
        public SenarioItemPreview(Api.Declare.SenarioClass sn) {
            this.item = sn;
            InitializeComponent();




            if (AppData.current != null && AppData.current.IndexOf(sn.id) > 0)
                txtTitle.Foreground = Brushes.DeepSkyBlue;

            this.Loaded += SenarioItemPreview_Loaded;
        }

        private void SenarioItemPreview_Loaded(object sender, RoutedEventArgs e) {
            Display_Data();
        }

        private void Display_Data() {
            txtTitle.Text = item.Name;
        }



        public SenarioClass item { get; private set; }

        private void BtnActive_Click(object sender, RoutedEventArgs e) {
            SelectItem();
        }

        private void btnMove_Click(object sender, RoutedEventArgs e) {
            var txt = (sender as MenuItem).Uid;
            if (txt == "up") {
                if (item.index <= 1) return;
                ActiveData.General.Senarios.OrderBy(j => j.index).Last(p => p.index < item.index).index += 1;
                item.index -= 1;
            }
            if (txt == "down") {
                if (item.index >= ActiveData.General.Senarios.Count) return;
                ActiveData.General.Senarios.OrderBy(j => j.index).First(p => p.index > item.index).index -= 1;
                item.index += 1;
            }
            new SenarioManager().Save();
            OnUpdate?.Invoke(Events.ItemRequest.Position);
        }

        private void DupMenu_Click(object sender, RoutedEventArgs e) {
            var handler = new SenarioManager();
            handler.DuplicateAndSave(item);

        }

        internal void SelectItem() {
            AppData.current = System.IO.Path.Combine(AppData.RootDirectory, this.item.id);
            this.item.visit = DateTime.Now;
            new SenarioManager().Save();
            var k = new Frame();
            k.Content = new Stage.StagePage();
            Ui.Access.AppHome.ContentBorder.Child = k;

            new SenarioDataManager(AppData.current).Load();
            DataEvents.OnDataChange(DataChangeOn.Source);
            OnUpdate?.Invoke(Events.ItemRequest.Selection);
        }

        private void RenameMenu_Click(object sender, RoutedEventArgs e) {
            var t = new Edit.SenarioInlineEditor(this.item);
            var A = new PopupWindow() { Title = "Edit Senario" };
            A.ContentFrame.Content = t;
            A.Show();
            t.OnRequest += txt => {
                switch (txt) {
                    case Events.EditorRequests.Save:
                        Display_Data();
                        A.Close();
                        return;
                    case Events.EditorRequests.Close:
                        A.Close();
                        return;
                }

            };
        }

        private void DeleteMenu_Click(object sender, RoutedEventArgs e) {

            if (AppData.current == System.IO.Path.Combine(AppData.RootDirectory, this.item.id)) {
                MessageBox.Show("Can Not Delete Active Item");
                return;
            }

            var handler = new SenarioManager();
            handler.DeleteAndUpdate(item);
          

            OnUpdate?.Invoke(Events.ItemRequest.Delete);
        }
    }
}
