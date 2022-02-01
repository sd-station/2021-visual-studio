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
using System.Windows.Shapes;
using TestPassModule.Api.Declare;
using TestPassModule.Modules.Pages;
using TestPassModule.Structs.Editor;
using TestPassModule.Structs.Lib;

namespace TestPassModule.Modules.Windows {
    /// <summary>
    /// Interaction logic for ModuleWindow.xaml
    /// </summary>
    public partial class ModelWindow : Window {
        public ModuleClass item { get; private set; }
        public ModulePartPage Moduler { get; }

        public ModelWindow(Api.Declare.ModuleClass item) {
            this.item = item;
            InitializeComponent();
            this.Title = item.title;
            this.Loaded += ModuleWindow_Loaded;
            Moduler = new ModulePartPage();
            Moduler.OnRequest += ModulerRequestHandler;
            StructFrame.Navigate(Moduler);
        }

        private void ModulerRequestHandler(StructureRequest opt) {
            if (opt.command == "nav") Moduler.Display(opt.scopeid, opt.level , opt.id, opt.p);
            if (opt.command == "info") SideFrame.Navigate(new StructEditor(opt.id));
           
        }



        private void ModuleWindow_Loaded(object sender, RoutedEventArgs e) {
            Moduler.Display(this.item.id, 0, 0, new Point(0, 300));

        }




    }
}
