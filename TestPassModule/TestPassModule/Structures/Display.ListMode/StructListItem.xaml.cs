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
using TestPassModel.Structures.Lib;
using TestPassModel.Ui.enums;
using TestPassModule.Api.Data;
using TestPassModule.Api.Declare;
using TestPassModule.Ui.Colorize;

namespace TestPassModule.Structures.Display;



/// <summary>
/// Interaction logic for NameSpaceItem.xaml
/// </summary>
public partial class StructListItem : UserControl {
    public Action<string, int> OnRequest;

    public StructClass item { get; }

    public int MyProperty { get; set; }


    public ItemSelectionMode SelectOnLoad { get; set; }
    public bool FocusOnLoad {
        get {
            if (SelectOnLoad == ItemSelectionMode.Auto && item.title.StartsWith("new item")) return true;
            if (SelectOnLoad == ItemSelectionMode.Selected) return true;
            return false;
        }
    }

    public bool IsLocked { get; set; } = false;

    public StructListItem(StructClass ns) {
        item = ns;
        InitializeComponent();
        txtLog.Text = "";
        BgIcon.Fill = BrushGenerator.FromString(item.theme);

        this.Loaded += NameSpaceItem_Loaded;
    }

    private void UpdateLevel() {
        ContentGrid.Margin = new Thickness(item.position.level * 40, 0, 0, 0);
    }

    private void NameSpaceItem_Loaded(object sender, RoutedEventArgs e) {
        if (this.FocusOnLoad) {
            TitleTextBox.Focus();
            if (item.title.StartsWith("new item")) TitleTextBox.SelectAll();
        }

        UpdateInfo();
        UpdateLevel();
    }


    private void DeleteMenu_Click(object sender, RoutedEventArgs e) {
        OnRequest?.Invoke("delete", item.id);
    }




    private void TitleTextBox_PreviewKeyDown(object sender, KeyEventArgs e) {

        if (IsLocked || !IsLoaded) {
            e.Handled = true;
            return;
        }

        if (e.Key == Key.Enter) {
            //>> Parent And Target Index
            var parent = (Parent as StackPanel)!;
            var this_Index = parent.Children.IndexOf(this);
            var tatget_Index = this_Index + 1;

            //! Next Is Empty ?
            if (Keyboard.Modifiers == 0) {
                if (tatget_Index < parent.Children.Count) {
                    var pv = parent.Children[tatget_Index] as StructListItem;
                    if (pv.TitleTextBox.Text.Length == 0 || pv.TitleTextBox.Text.StartsWith("new item")) {
                        pv.TitleTextBox.Focus();
                        if (pv.TitleTextBox.Text.StartsWith("new item")) pv.TitleTextBox.SelectAll();
                        e.Handled = true;
                        return;
                    }
                }
            }

            //>> Find Parameters
            //>> Create New Item
            var Structer = new StructBuilder();

            Structer.scope = item.scope;
            Structer.parent = item.position.parent;
            Structer.index = item.position.index + 1;

            if ((Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control) {
                Structer.parent = item.id;
                Structer.index = -1;

            } else if ((Keyboard.Modifiers & ModifierKeys.Shift) == ModifierKeys.Shift) {
                tatget_Index = this_Index;
                Structer.index = item.position.index;
            }

            Structer.Finilize();

            var r = new StructListItem(Structer.item);
            parent.Children.Insert(tatget_Index, r);
            e.Handled = true;
        }

        /// Move to Left ← 
        if (((Keyboard.Modifiers & ModifierKeys.Shift) > 0 && e.Key == Key.Tab) || ((Keyboard.Modifiers & ModifierKeys.Alt) > 0 && e.SystemKey == Key.Left)) {
            e.Handled = true;
            txtLog.Text = "move left";
            var ix = this.item.position.index;

            var ParentStr = ActiveData.General.Structs.FirstOrDefault(k => k.id == this.item.position.parent);
            if (ParentStr == null) return;

            this.item.position.parent = ParentStr.position.parent;
            ActiveData.General.Structs
             .ForEach(k => {
                 if (k.position.parent == this.item.position.parent && k.position.index > ParentStr.position.index) {
                     k.position.index++;
                 }
             });

            item.position.index = ParentStr.position.index + 1;
            item.position.level -= 1;
            OnRequest?.Invoke("refresh", item.id);
            return;
        }

        /// Move to Right → 
        if (e.Key == Key.Tab || ((Keyboard.Modifiers & ModifierKeys.Alt) > 0 && e.SystemKey == Key.Right)) {
            e.Handled = true;
            txtLog.Text = "move right";
            var ix = this.item.position.index;
            if (ix == 1) return;

            //>> Find Parent 
            var parent = (Parent as StackPanel)!;
            var this_Index = parent.Children.IndexOf(this);

            if (this_Index == 0) return;
            var prev = (parent.Children[this_Index - 1] as StructListItem)!;

            //>> SAME COLLECTION
            if (prev.item.position.level == this.item.position.level) {
                item.position.index = 1;
                item.position.level += 1;
                item.position.parent = prev.item.id;
                OnRequest?.Invoke("refresh", item.id);
                return;
            }

            //>> OTHER COLLECTION
            if (prev.item.position.level > this.item.position.level) {
                item.position.index = prev.item.position.index + 1;
                item.position.level = prev.item.position.level;
                item.position.parent = prev.item.position.parent;
                OnRequest?.Invoke("refresh", item.id);
                return;
            }


            return;

        }

       


    }

    private void IncreaseIndex(int parent , int ix) {
        ActiveData.General.Structs
        .ForEach(k => {
            if (k.position.parent == parent && k.position.index > ix) {
                k.position.index++;
            }
        });
    }

    private void UpdateInfo() {
        txtInfo.Text = $"{item.position.index}   # {item.id} → {item.position.level} ♠ {item.position.parent} ";
        txtIndex.Text = item.position.index.ToString();
        txtLog.Text = "";
    }

    private void TitleTextBox_PreviewKeyUp(object sender, KeyEventArgs e) {

        if ((Keyboard.Modifiers & ModifierKeys.Alt) > 0 && e.SystemKey == Key.Up) {
            e.Handled = true;
            txtLog.Text = "move  up";
            //>> Find Parent 
            var parent = (Parent as StackPanel)!;
            var this_Index = parent.Children.IndexOf(this);
            if (this_Index <= 0) return;

            //>> Fill As Down
            int NewPlace = this_Index - 1;

            StructListItem prev = (parent.Children[this_Index - 1] as StructListItem)!;
            StructListItem MoveItem = this;


            if (prev.item.position.parent == item.position.parent) {
                var ix = prev.item.position.index;
                prev.item.position.index = item.position.index;
                item.position.index = ix;

                IsLocked = true;
                OnRequest?.Invoke("refresh", item.id);
                return;
            }

            //>> OTHER COLLECTION
            var trg = prev.item.position.index + 1;
            if (item.position.parent == prev.item.id) {
                trg = prev.item.position.index;
                prev.item.position.index++;
            }

            item.position.level = prev.item.position.level;
            item.position.parent = prev.item.position.parent;

            IncreaseIndex(item.position.parent, prev.item.position.index);

            item.position.index = trg;

            IsLocked = true;
            OnRequest?.Invoke("refresh", item.id);

            return;
        }

        if ((Keyboard.Modifiers & ModifierKeys.Alt) > 0 && e.SystemKey == Key.Down) {



            txtLog.Text = "move down";

            e.Handled = true;
            //>> Find Parent 
            var parent = (Parent as StackPanel)!;
            var this_Index = parent.Children.IndexOf(this);
            //>> Fill As Down
            int NewPlace = this_Index;
            if (this_Index >= parent.Children.Count - 1) return;

            StructListItem next = (parent.Children[this_Index + 1] as StructListItem)!; ;
            StructListItem MoveItem = next;


            if (next.item.position.level < item.position.level) {
                item.position.level = next.item.position.level;
                item.position.parent = next.item.position.parent;
                item.position.index = next.item.position.index;
                IncreaseIndex(item.position.parent, next.item.position.index);
                next.item.position.index += 1;


                OnRequest?.Invoke("refresh", item.id);
                return;

            }


            if (next.item.position.parent != item.position.parent) return;


            var ix = next.item.position.index;
            next.item.position.index = item.position.index;
            item.position.index = ix;

            OnRequest?.Invoke("refresh", item.id);
            return;

        }

    }
}

