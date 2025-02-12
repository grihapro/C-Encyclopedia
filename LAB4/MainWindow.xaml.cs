using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
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
using Newtonsoft.Json;
using System.Runtime.CompilerServices;
using Microsoft.Win32;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json.Linq;

namespace LAB4
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public class MultiConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
                return values.Clone();
            //return null;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public partial class MainWindow : Window
    {
        string docPath = @".\..\..\..\data.json";
        List<Group> list = new List<Group>();
        internal MainVM MainVM { get; set; }
        public MainWindow()
        {
            MainVM = new();
            InitializeComponent();
            this.DataContext = MainVM;
            MainVM.CloseAdd += this.CloseAdd;
            MainVM.GroupChosen += this.EditGroup; MainVM.FamilyChosen += this.EditFamily; MainVM.GenusChosen += this.EditGenus; MainVM.KindChosen += this.EditKind;
            if (File.Exists(docPath) && new FileInfo(docPath).Length > 0 )
            {
                list = JsonConvert.DeserializeObject<List<Group>>(File.ReadAllText(docPath));
                foreach (Group item in list)
                {
                    MainVM.Groups.Add(item);
                    foreach (Family family in item.Families)
                    {
                        family.Parent = item;
                        foreach (Genus genus in family.Genuses)
                        {
                            genus.Parent = family;
                            foreach (Kind kind in genus.Kinds)
                            {
                                kind.Parent = genus;
                            }
                        }
                    }
                }
            }
        }

        protected override void OnClosed(EventArgs e)
        {
            list.Clear();
            foreach (Group group in MainTree.Items)
            {
                list.Add(group);
            }
            var settings = new JsonSerializerSettings
            {
                Formatting = Formatting.Indented,
            };
            string output = JsonConvert.SerializeObject(list, settings);
            File.WriteAllText(docPath, output);
            base.OnClosed(e);
        }

        private void OnItemSelected(object sender, RoutedEventArgs e)
        {
            MainTree.Tag = e.OriginalSource;
            MainVM.SelectedItem = MainTree.SelectedItem;
            MainVM.Tag = MainTree.Tag;
        }

        private void Clear(object sender, RoutedEventArgs e)
        {
            Группа.Text = string.Empty; Семейство.Text = string.Empty;
            Отряд.Text = string.Empty; Вид.Text = string.Empty;
            Описание.Text = string.Empty; Среда.Text = string.Empty;
            Upload.Tag = string.Empty;
        }

        private void InfoToAdd(object sender, RoutedEventArgs e)
        {
            Clear(sender, e);
            Grid1.Visibility = Visibility.Collapsed;
            Grid2.Visibility = Visibility.Visible;
            Add.Visibility = Visibility.Visible;
            Edit.Visibility = Visibility.Collapsed;
            Группа.IsEnabled = true; 
            Семейство.IsEnabled = true;
            Отряд.IsEnabled = true;
            Вид.IsEnabled = true;
            Описание.IsEnabled = true; 
            Upload.IsEnabled = true; 
            Среда.IsEnabled = true;
        }

        private void CloseAdd(object sender, RoutedEventArgs e) 
        {
            Grid2.Visibility = Visibility.Collapsed;
            Grid1.Visibility = Visibility.Visible;
            MainTree.IsEnabled = true;
        }

        private void UploadImage(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Image files|*.jpg;*.png;*.jpeg;";
            bool? response = ofd.ShowDialog();
            if (response == true)
            {
                string filepath = ofd.FileName;
                string filename = System.IO.Path.GetFileName(filepath);
                if (!File.Exists($"./../../../images/{filename}"))
                    File.Copy(filepath, $"./../../../images/{filename}");
                Upload.Tag = $"./../../../images/{filename}";
            }
        }

        private void InfoToEdit(object sender, RoutedEventArgs e)
        {
            Clear(sender, e);
            Grid1.Visibility = Visibility.Collapsed;
            Grid2.Visibility = Visibility.Visible;
            Add.Visibility = Visibility.Collapsed;
            Edit.Visibility = Visibility.Visible;
            var item = MainTree.SelectedItem;
        }

        private void EditGroup(object sender, RoutedEventArgs e)
        {
            InfoToEdit(sender, e);
            var i = MainTree.SelectedItem; Group item = i as Group;
            Группа.IsEnabled = true; Группа.Text = item.Name;
            Семейство.IsEnabled = false;
            Отряд.IsEnabled = false;
            Вид.IsEnabled = false;
            Описание.IsEnabled = true; Описание.Text = item.Description;
            Upload.IsEnabled = true; Upload.Tag = item.Path;
            Среда.IsEnabled = false;
            MainTree.IsEnabled = false;
        }

        private void EditFamily(object sender, RoutedEventArgs e)
        {
            InfoToEdit(sender, e);
            var i = MainTree.SelectedItem; Family item = i as Family;
            Группа.IsEnabled = true; Группа.Text = item.Parent.Name;
            Семейство.IsEnabled = true; Семейство.Text = item.Name;
            Отряд.IsEnabled = false;
            Вид.IsEnabled = false;
            Описание.IsEnabled = true; Описание.Text = item.Description;
            Upload.IsEnabled = true; Upload.Tag = item.Path;
            Среда.IsEnabled = false;
            MainTree.IsEnabled = false;
        }

        private void EditGenus(object sender, RoutedEventArgs e)
        {
            InfoToEdit(sender, e);
            var i = MainTree.SelectedItem; Genus item = i as Genus;
            Группа.IsEnabled = true; Группа.Text = item.Parent.Parent.Name;
            Семейство.IsEnabled = true; Семейство.Text = item.Parent.Name;
            Отряд.IsEnabled = true; Отряд.Text = item.Name;
            Вид.IsEnabled = false;
            Описание.IsEnabled = true; Описание.Text = item.Description;
            Upload.IsEnabled = true; Upload.Tag = item.Path;
            Среда.IsEnabled = false;
            MainTree.IsEnabled = false;
        }

        private void EditKind(object sender, RoutedEventArgs e)
        {
            InfoToEdit(sender, e);
            var i = MainTree.SelectedItem; Kind item = i as Kind;
            Группа.IsEnabled = true; Группа.Text = item.Parent.Parent.Parent.Name;
            Семейство.IsEnabled = true; Семейство.Text = item.Parent.Parent.Name;
            Отряд.IsEnabled = true; Отряд.Text = item.Parent.Name;
            Вид.IsEnabled = true; Вид.Text = item.Name;
            Описание.IsEnabled = true; Описание.Text = item.Description;
            Upload.IsEnabled = true; Upload.Tag = item.Path;
            Среда.IsEnabled = true; Среда.Text = item.Area;
            MainTree.IsEnabled = false;
        }

        public int RecursionSearch(ItemsControl tree, string name)
        {
            if (tree == null) return 0;
            if (tree.Items.Count == 0) return 0;
            TreeViewItem item; int count = 0, count2 = 0;
            for (int i = 0; i < tree.Items.Count; i++)
            {
                item = tree.ItemContainerGenerator.ContainerFromIndex(i) as TreeViewItem;
                if (item != null)
                    item.IsExpanded = true;
                item.UpdateLayout();
                var obj = tree.Items.GetItemAt(i);
                if ( obj is Kind)
                {
                    if (((Kind)obj).Area.Contains(name))
                        item.Visibility = Visibility.Visible;
                    else
                    {
                        item.Visibility = Visibility.Collapsed;
                        count++;
                    }
                }
                count += RecursionSearch(item, name);
            }
            if (count == tree.Items.Count)
            {
                tree.Visibility = Visibility.Collapsed;
                return 1;
            }
            else
            {
                tree.Visibility = Visibility.Visible;
                return 0;
            }
        }

        private void Hide(object sender, RoutedEventArgs e)
        {
            TreeViewItem item;
            RecursionSearch(MainTree, ((Button)sender).Tag as string);
        }
    }
}
