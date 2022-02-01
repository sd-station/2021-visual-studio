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
using TestPassModule.Api.Active;
using TestPassModule.Api.Data;
using TestPassModule.Api.Manager;
using TestPassModule.Senario.Display;
using TestPassModule.Senario.Edit;
using TestPassModule.Ui.Events;
using TestPassSenario.Api.Manager;

namespace TestPassModule.Senario {
    /// <summary>
    /// Interaction logic for SenarioIndexPage.xaml
    /// </summary>
    public partial class SenarioIndexPage : Page {
        public SenarioIndexPage() {
            InitializeComponent();
            this.Loaded += SenarioIndexPage_Loaded;
        }

        private void SenarioIndexPage_Loaded(object sender, RoutedEventArgs e) {
            new SenarioManager().Load();
            UpdateSenario();

            if (Senarios.Children.Count == 0) {
                if (ActiveData.ProjectInfo.CreateTime == null) ActiveData.ProjectInfo.CreateTime = DateTime.Now;

                if ((DateTime.Now - ActiveData.ProjectInfo.CreateTime).TotalDays < 3) {
                    NewDiagram("New Diagram 1");
                }

            } else {
                if (string.IsNullOrWhiteSpace(AppData.current)) {
                    Senarios.Children.OfType<SenarioItemPreview>().OrderBy(h => h.item.visit).Last().SelectItem();
                    Ui.Access.AppHome.Side.Close();
                }
                    
            }

        }

        private void UpdateSenario() {
            Senarios.Children.Clear();

            ActiveData.General.Senarios.OrderBy(k => k.index).ToList().ForEach(sn => {


                var n = new Display.SenarioItemPreview(sn);
                Senarios.Children.Add(n);

                n.OnUpdate += txt => {

                    switch (txt) {
                        case Events.ItemRequest.Selection:
                            UpdateSenario();
                            break;
                        case Events.ItemRequest.Position:
                            UpdateSenario();
                            break;
                        case Events.ItemRequest.Delete:
                            UpdateSenario();
                            break;
                        default:
                            break;
                    }

                };

            });
        }

        private void btnNewSenario_Click(object sender, RoutedEventArgs e) {
            NewDiagram();
        }

        private void NewDiagram(string v = "") {
            var nr = new Api.Declare.SenarioClass() {
                mode = Api.Declare.SenarioMode.Diagram,
                index = ActiveData.General.Senarios.Count + 1
            };
            if (!string.IsNullOrWhiteSpace(v)) nr.Name = v;
            var P = new SenarioInlineEditor(nr);


            Senarios.Children.Add(P);

            P.OnRequest += txt => {
                UpdateSenario();

                Senarios.Children.OfType<SenarioItemPreview>().Last().SelectItem();

            };
        }



        private void btnHelp_Click(object sender, RoutedEventArgs e) {
            new Lib.Lancher.OpenInBrowser().OpenLink(System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "help/scenario-fa.html"));

        }
    }
}
