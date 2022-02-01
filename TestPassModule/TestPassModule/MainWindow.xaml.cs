using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
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
using TestPassModule.RelGroup.Pages;
using TestPassModule.Stage;
using TestPassModule.Ui.Access;
using TestPassSenario.Api.Manager;

namespace TestPassModule;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{



    public MainWindow()
    {
        InitializeComponent();




#if !DEBUG
   var extime = new DateTime(2022, 1, 1);
 if (DateTime.Now > extime) {
            MessageBox.Show("This version is Expired");
            Application.Current.Shutdown();
        }
    string version = Assembly.GetExecutingAssembly().GetName().Version.ToString();
        txtVersion.Text = version;
          ExpireTime.Text = Math.Floor((extime - DateTime.Now).TotalDays).ToString();
#endif




        this.Loaded += MainWindow_Loaded;

        AppHome.HomeWindow = this;
        AppHome.ContentBorder = ContentBorder;
        AppHome.Side = new Home.Lib.SideHandler()
        {
            El = SideFrame,
            Cover = SideBar,
            Closer = btnCloseSide
        };
    }

    private void MainWindow_Loaded(object sender, RoutedEventArgs e)
    {

        if (string.IsNullOrEmpty(AppData.RootDirectory))
        {
            Goto_ProjectIndex();
            return;
        }

        ShowSideSenario();
    }



    private void ShowSideSenario()
    {
        AppHome.Side.Navigate(new Senario.SenarioIndexPage(), "#330033");


    }



    private void BtnHome_Click(object sender, RoutedEventArgs e)
    {
        Goto_ProjectIndex(false);
    }

    private void Goto_ProjectIndex(bool AutoNav = true)
    {
        var hsc = new Home.ProjectIndex();
        hsc.AutoAccept = AutoNav;

        AppHome.Side.Navigate(hsc, "#2f4f4f");
        hsc.OnAccpet += ShowSideSenario;


        //var x1 = new WebBrowser();
        // AppHome.ContentBorder.Child = x1;
        //x1.Navigate(System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "help/index-fa.html"));
    }

    private void BtnDiagram_Click(object sender, RoutedEventArgs e)
    {
        if (string.IsNullOrEmpty(AppData.RootDirectory)) return;
        ShowSideSenario();
    }

    private void BtnClose_Click(object sender, RoutedEventArgs e)
    {
        AppHome.Side.Close();
    }

    private void BtnRelGroup_Click(object sender, RoutedEventArgs e)
    {
        if (string.IsNullOrEmpty(AppData.current)) return;
        AppHome.Side.Navigate(new RelGroupIndex(), "#003333");
    }

    private void BtnModelize_Click(object sender, RoutedEventArgs e)
    {
        if (string.IsNullOrEmpty(AppData.RootDirectory) || !AppData.current.StartsWith(AppData.RootDirectory)) return;

        var k = new Frame();
        k.Content = new Structures.Pages.StructListPage();
        Ui.Access.AppHome.ContentBorder.Child = k;
     
       //AppHome.Side.Navigate(, "#330033");
    }
}



