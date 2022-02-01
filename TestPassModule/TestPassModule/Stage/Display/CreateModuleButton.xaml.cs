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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using TestPassModule.Api.Active;
using TestPassModule.Api.Data;
using TestPassModule.Api.Declare;
using TestPassModule.Api.Manager;

namespace TestPassModule.Stage.Display {
    /// <summary>
    /// Interaction logic for CreateModuleButton.xaml
    /// </summary>
    public partial class CreateModuleButton : UserControl {
        public CreateModuleButton() {
            InitializeComponent();
            this.Loaded += CreateModuleButton_Loaded;
        }

        private void CreateModuleButton_Loaded(object sender, RoutedEventArgs e) {
            this.Margin = new Thickness((index -1) * SConfig.Grid_Width  , 0, 0, 0);

            if (Shining) ShiningMode();
        }

       

        public int index { get; internal set; }
        public bool Shining { get; internal set; }

        private void btnActive_Click(object sender, RoutedEventArgs e) {
            var nr = new ModuleClass();
            nr.mode =  ModuleModes.None;
            nr.theme = "#707070";
           
            new ModuleManager().Add(nr);
            nr.title = "New Item " + nr.id;
            new ModuleManager().Save();
            if (AppData.current == null) return;

            

            var nco = new SenarioModuleClass();
            nco.module = nr.id;
            nco.id = 1;
            if(ActiveData.Stage.SenarioData.Modules.Any()) nco.id += ActiveData.Stage.SenarioData.Modules.Last().id;
            nco.index = 1;
            if (ActiveData.Stage.SenarioData.Modules.Any())
                nco.index += ActiveData.Stage.SenarioData.Modules.Max(h => h.index);


            ActiveData.Stage.SenarioData.Modules.Add(nco);
            new SenarioDataManager(AppData.current).Save();

            DataEvents.OnDataChange?.Invoke(DataChangeOn.Module);
        }


        private void ShiningMode() {
            this.Foreground = Brushes.White;
            this.BorderBrush = Brushes.Yellow;
        }
       
    }
}
