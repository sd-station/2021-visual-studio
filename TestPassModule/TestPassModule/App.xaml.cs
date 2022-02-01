using Data.Recent;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using TestPassModule.Api.Active;
using TestPassModule.Api.Data;
using TestPassModule.Api.Declare;
using TestPassModule.Api.Temp;

namespace TestPassModule {
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application {
        public App() {
            VariableInitialize();
            this.Startup += App_Startup;

        }

        private void VariableInitialize() {
            ActiveData.Stage = new StageActiveData();
            ActiveData.Relations = new List<RelationClass>();

            ActiveData.General = new GeneralActiveData();
            ActiveData.General.Modules = new List<ModuleClass>();
            ActiveData.General.Structs = new List<StructClass>();
            TempData.NR = new TempRow();
        }

        private void App_Startup(object sender, StartupEventArgs e) {         
            new MainWindow().Show();
        }
    }
}
