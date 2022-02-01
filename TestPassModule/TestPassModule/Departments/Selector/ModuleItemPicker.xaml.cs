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
using TestPassModule.Api.Temp;
using TestPassModule.ModuleLine.Display;

namespace TestPassModule.ModuleLine.Selector {
    /// <summary>
    /// Interaction logic for ModuleItemPicker.xaml
    /// </summary>
    public partial class ModuleItemPicker : Page {
        public ModuleItemPicker() {
            InitializeComponent();
            this.Loaded += ModuleItemPicker_Loaded;
        }

        private void ModuleItemPicker_Loaded(object sender, RoutedEventArgs e) {

            var onboard = new List<int>();

            ActiveData.Stage.SenarioData.Modules.ForEach(A => {

                var tb = ActiveData.General.Modules.First(k => k.id == A.module);
                var k = new ModuleButton(tb);
               Container.Children.Add(k);

                k.OnRequest += txt => {

                    if (txt == "accept") {
                        TempData.NR.End = tb.id;
                        TempData.NR.State = RelationTempState.Endpoint;
                        Api.Data.DataEvents.OnDataChange(DataChangeOn.RelTemp);
                    }

                };

                onboard.Add(tb.id);
            });


            ActiveData.General.Modules.ForEach(tb => {

                if (onboard.Contains(tb.id)) return;

                var k = new ModuleButton(tb , false);

                ProjectContainer.Children.Add(k);

                k.OnRequest += txt => {

                    if (txt == "accept") {
                        TempData.NR.End = tb.id;
                        TempData.NR.State = RelationTempState.Endpoint;
                        Api.Data.DataEvents.OnDataChange(DataChangeOn.RelTemp);
                    }

                };

            });
        }
    }
}
