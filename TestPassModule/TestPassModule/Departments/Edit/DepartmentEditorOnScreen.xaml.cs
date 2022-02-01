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
using TestPassModule.Api.Declare;
using TestPassModule.Api.Manager;
using TestPassModule.Ui.Edit;

namespace TestPassModule.Departments.Edit;
/// <summary>
/// Interaction logic for DepartmentEditor.xaml
/// </summary>
public partial class DepartmentEditorOnScreen : UserControl {
    internal Action<string> OnChange;

    public DepartmentEditorOnScreen(SenarioModuleClass department) {
        InitializeComponent();
        Item = department;
        InfoEditor = new Ui.Edit.PropertyEditor("Info");
        EditContainer.Children.Add(InfoEditor);

        if (Item.update.Year < 2020) Item.update = DateTime.Now;
        //k.Show("update", Item.update.ToString());

        //k.AddEditor("title", Item.title);
        //k.AddEditor("module", Item.module);
        //k.AddEditor(Item.index);
        //k.AddEditor(Item.theme);


        InfoEditor.showname(Item);

        ModuleBox.Items.Clear();

        this.Modules = new List<PropertyClass>();
        this.Modules.Add(new PropertyClass { index = 0, id = 0, title = "No Module" });
        var index = 1;
        this.Modules.AddRange(ActiveData.General.Modules
            .Select(k => new PropertyClass { index = index++, id = k.id, title = k.title }));
        ModuleBox.ItemsSource = Modules;
        ModuleBox.DisplayMemberPath = "title";


        this.Loaded += DepartmentEditorOnScreen_Loaded;

    }

    private void DepartmentEditorOnScreen_Loaded(object sender, RoutedEventArgs e) {

        ModuleBox.SelectedIndex = Modules.FirstOrDefault(k => k.id == Item.module)?.index ?? 0;
    }

    public SenarioModuleClass Item { get; }
    public PropertyEditor InfoEditor { get; private set; }
    public List<PropertyClass> Modules { get; private set; }

    private void BtnSave_Click(object sender, RoutedEventArgs e) {
        InfoEditor.update(Item);
        var newModule = Modules.FirstOrDefault(k => k.index == ModuleBox.SelectedIndex)?.id ?? 0;
        if (newModule != Item.module) {

          

            Item.module = newModule;
            new ModuleManager().Save();
            new RelationManager(AppData.current).Save();
            new RelGroupManager(AppData.current).Save();
            new SenarioDataManager(AppData.current).Save();
            DataEvents.OnDataChange?.Invoke(DataChangeOn.Source);
         
        }

        OnChange?.Invoke("save");
    }
}

