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

namespace TestPassModule.Stage.Relations.Editor {
    /// <summary>
    /// Interaction logic for RelationEditor.xaml
    /// </summary>
    public partial class RelationEditor : Page {
        public Action<string> OnRequest;

        public RelationClass item { get; private set; }
        private List<SqActionModes> Modes { get; set; }
        private List<PropertyClass> OtherParents { get; set; }
        private List<PropertyClass> Groups { get; set; }

        public RelationEditor(Api.Declare.RelationClass nr) {
            this.item = nr;
            InitializeComponent();

            this.Modes = Enum.GetValues(typeof(SqActionModes)).Cast<SqActionModes>().ToList();
            cmbRelMode.ItemsSource = this.Modes;

            this.Loaded += RelationEditor_Loaded;



            this.OtherParents = new List<PropertyClass>();
            this.OtherParents.Add(new PropertyClass { index = 0, id = 0, title = "No Parent" });

            var index = 1;
            this.OtherParents.AddRange(ActiveData.Stage.Relations
                .Where(l => l.position.to == this.item.position.from && l.position.index < this.item.position.index)
                .OrderBy(l => l.runtime.row)
                .Select(k => new PropertyClass { index = index++, id = k.id, title = "--> " + k.title }));

            txtParent.ItemsSource = OtherParents;
            txtParent.DisplayMemberPath = "title";


            this.Groups = new List<PropertyClass>();
            this.Groups.Add(new PropertyClass { index = 0, id = 0, title = "No Group" });

            index = 1;
            this.Groups.AddRange(ActiveData.RelGroup 
                .Select(k => new PropertyClass { index = index++, id = k.id, title = k.title }));

            txtGroup.ItemsSource = Groups;
            txtGroup.DisplayMemberPath = "title";

        }

        private void RelationEditor_Loaded(object sender, RoutedEventArgs e) {

            Display_Data();


            if (string.IsNullOrEmpty(txtTitle.Text)) txtTitle.Focus();


        }

        private void Display_Data() {


            if (this.item.position.from == this.item.position.to) cmbRelMode.SelectedItem = SqActionModes.Self;

            //>> Properties
            txtID.Text = item.id.ToString();
            txtLine.Text = item.position.index.ToString();
            txtTitle.Text = this.item.title ?? "";
            txtCreate.Text = this.item.create.ToString();
            cmbRelMode.SelectedItem = this.item.actionmode;

            //>> POSITION 
            if (item.position == null) item.position = new PositionClass();

       
                txtParent.SelectedIndex = this.OtherParents.FirstOrDefault(k => k.id == item.position.parent)?.index ?? 0;

            chkContinue.IsChecked = item.position.isContinue;
            txtLine.Text = item.position.index.ToString();
            txtDrawFrom.Text = item.position.from.ToString();
            txtDrawTo.Text = item.position.to.ToString();

            //>> Runtime 
            if (item.runtime == null) item.runtime = new RuntimeClass();
            if (string.IsNullOrEmpty(item.runtime.state)) item.runtime.state = "pending";
            txtState.Text = item.runtime.state;
            if (string.IsNullOrEmpty(item.runtime.theme)) item.runtime.theme = "#707070";
            txtTheme.Text = item.runtime.theme;
            txtIndex.Text = item.runtime.row.ToString();

            //>> Actions
            if (item.doc == null) item.doc = new DocumentClass();
            if (item.doc.actions == null) item.doc.actions = new List<string>();
            txtActions.Text = string.Join("\n", item.doc.actions);


            if (item.group == null) item.group = new GroupClass();
       
                txtGroup.SelectedIndex = this.Groups.FirstOrDefault(k => k.id == item.group.id)?.index ?? 0;
            txtGroupCaption.Text = item.group.caption;



        }

        private void BtnSave_Click(object sender, RoutedEventArgs e) {
            SaveRequest();
        }

        private void SaveRequest() {


            item.title = txtTitle.Text;
            item.actionmode = Modes[cmbRelMode.SelectedIndex];

            if (item.position == null) item.position = new PositionClass();
            item.position.parent =
              this.OtherParents.FirstOrDefault(k => k.index == txtParent.SelectedIndex)?.id ?? 0;

            item.position.isContinue = (bool)chkContinue.IsChecked;
            item.position.index = GetNumberFromText(txtLine.Text);
            item.position.from = GetNumberFromText(txtDrawFrom.Text);
            item.position.to = GetNumberFromText(txtDrawTo.Text);


            if (item.runtime == null) item.runtime = new RuntimeClass();

            item.runtime.state = txtState.Text;
            item.runtime.theme = txtTheme.Text;

            if (item.doc == null) item.doc = new DocumentClass();
            item.doc.actions = txtActions.Text.Replace("\r\n", "\n").Split('\n').ToList();


            if (item.group == null) item.group = new GroupClass();
            item.group.id =  this.Groups.FirstOrDefault(k => k.index == txtGroup.SelectedIndex)?.id ?? 0;

            item.group.caption = txtGroupCaption.Text;

            ActiveData.Relations.ForEach(j => { if (j.position.index >= this.item.position.index) j.position.index++; });


            var RM = new RelationManager(AppData.current);
            if (item.id < 0) RM.Add(this.item);
            RM.Save();

            OnRequest?.Invoke("save");
        }

        private int GetNumberFromText(string text) {
            int np = 0;
            if (string.IsNullOrWhiteSpace(text)) return np;
            if (int.TryParse(text, out np)) return np;
            return 0;
        }



        private void txtTitle_PreviewKeyDown(object sender, KeyEventArgs e) {
            if (e.Key == Key.Enter) {
                e.Handled = true;
                SaveRequest();
                return;
            }
        }
    }


}
