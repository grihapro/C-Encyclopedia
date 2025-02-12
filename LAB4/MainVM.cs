using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Media3D;
using static System.Net.Mime.MediaTypeNames;

namespace LAB4
{
    internal class MainVM:ChangerBase
    {
        private readonly ObservableCollection<Group> _groups = new ObservableCollection<Group>();
        private object _selitem = null;
        public object SelectedItem { get { return _selitem; } set { _selitem = value; } }
        
        private object _tag = null;
        public object Tag { get { return _tag; } set { _tag = value; } }

        private RelayCommand addCommand;
        public RelayCommand AddCommand
        {
            get
            {
                return addCommand ?? (addCommand = new RelayCommand(addFun, checkAdd));
            }
        }

        private RelayCommand removeCommand;
        public RelayCommand RemoveCommand
        {
            get
            {
                return removeCommand ??(removeCommand = new RelayCommand(removeFun,checkItem));
            }
        }

        private RelayCommand chooseCommand;
        public RelayCommand ChooseCommand
        {
            get
            {
                return chooseCommand ?? (chooseCommand = new RelayCommand(chooseFun, checkItem));
            }
        }

        private RelayCommand editCommand;
        public RelayCommand EditCommand
        {
            get
            {
                return editCommand ?? (editCommand = new RelayCommand(editFun, checkEdit));
            }
        }

        public MainVM(){}

        public ObservableCollection<Group> Groups
        {
            get 
            {
                return _groups; 
            }
            set 
            {
                _groups.Clear();
                foreach (var item in value)
                    _groups.Add(item);
                OnPropertyChanged("Groups");
            }
        }

        public event RoutedEventHandler CloseAdd;
        private void addFun(object parameter)
        {
            var values = (object[])parameter;
            var g = (string)values[0];
            var f = (string)values[1];
            var ge = (string)values[2];
            var k = (string)values[3];
            var d = (string)values[4];
            var a = (string)values[5];
            var p = (string)values[6];
            Group thisG = null; Family thisF = null; Genus thisGe = null; Kind thisK = null;
            bool flag = false;
            foreach (Group group in _groups)
            {
                if (group.Name == g as string)
                {
                    thisG = group;
                    flag = true;
                }
            }
            if (!flag)
            {
                Groups.Add(new Group(g as string));
                thisG = Groups.Last();
                thisG.Families.Add(new Family(f as string, thisG));
                thisF = thisG.Families.Last();
                thisF.Genuses.Add(new Genus(ge as string, thisF));
                thisGe = thisF.Genuses.Last();
                thisGe.Kinds.Add(new Kind(k as string, thisGe, d as string, p as string, a as string));
                thisK = thisGe.Kinds.Last();
            }
            else
            {
                flag = false;
                foreach (Family family in thisG.Families)
                {
                    if (family.Name == f as string)
                    {
                        thisF = family;
                        flag = true;
                    }
                }
                if (!flag)
                {
                    thisG.Families.Add(new Family(f as string, thisG));
                    thisF = thisG.Families.Last();
                    thisF.Genuses.Add(new Genus(ge as string, thisF));
                    thisGe = thisF.Genuses.Last();
                    thisGe.Kinds.Add(new Kind(k as string, thisGe, d as string, p as string, a as string));
                    thisK = thisGe.Kinds.Last();
                }
                else
                {
                    flag = false;
                    foreach (Genus genus in thisF.Genuses)
                    {
                        if (genus.Name == ge as string)
                        {
                            thisGe = genus;
                            flag = true;
                        }
                    }
                    if (!flag)
                    {
                        thisF.Genuses.Add(new Genus(ge as string, thisF));
                        thisGe = thisF.Genuses.Last();
                        thisGe.Kinds.Add(new Kind(k as string, thisGe, d as string, p as string, a as string));
                        thisK = thisGe.Kinds.Last();
                    }
                    else
                    {
                        flag = false;
                        foreach (Kind kind in thisGe.Kinds)
                        {
                            if (kind.Name == k as string)
                            {
                                thisK = kind;
                                flag = true;
                            }
                        }
                        if (!flag)
                        {
                            thisGe.Kinds.Add(new Kind(k as string, thisGe, d as string, p as string, a as string));
                            thisK = thisGe.Kinds.Last();
                        }
                        else
                        {
                            thisK.Description = d as string;
                            thisK.Path = p as string;
                            thisK.Area = a as string;
                        }
                    }
                }
            }
            if (CloseAdd != null)
                CloseAdd(this, new RoutedEventArgs());
        }

        private void removeFun(object sender=null)
        {
                if (SelectedItem is Group)
                    Groups.Remove(SelectedItem as Group);
                if (SelectedItem is Family)
                {
                    var parent = (SelectedItem as Family).Parent;
                    (parent as IBIRDItem).RemoveItem(SelectedItem);
                }
                if (SelectedItem is Genus)
                {
                    var parent = (SelectedItem as Genus).Parent;
                    (parent as IBIRDItem).RemoveItem(SelectedItem);
                }
                if (SelectedItem is Kind)
                {
                    var parent = (SelectedItem as Kind).Parent;
                    (parent as IBIRDItem).RemoveItem(SelectedItem);
                }
                (Tag as TreeViewItem).IsSelected = false;
                SelectedItem = null;
        }
        public event RoutedEventHandler GroupChosen, FamilyChosen, GenusChosen, KindChosen;

        private void chooseFun(object sender)
        {
            if (SelectedItem is Group)
            {
                if (GroupChosen != null)
                    GroupChosen(this, new RoutedEventArgs());
            }
            else if (SelectedItem is Family)
            {
                if (FamilyChosen != null)
                    FamilyChosen(this, new RoutedEventArgs());
            }
            else if (SelectedItem is Genus)
            {
                if (GenusChosen != null)
                    GenusChosen(this, new RoutedEventArgs());
            }
            else if (SelectedItem is Kind)
            {
                if (KindChosen != null)
                    KindChosen(this, new RoutedEventArgs());
            }
        }

        private void ifGroupExists(Group group, object[] values, bool ed)
        {
            bool flag = true;
            foreach (Group item in Groups)
            {
                if (values[0] as string == item.Name && !(group == item))
                {
                    flag = false;
                    foreach (Family family in group.Families)
                    {
                        item.Families.Add(family);
                        family.Parent = item;
                    }
                    if (ed)
                    {
                        item.Path = values[6] as string;
                        item.Description = values[4] as string;
                    }
                    Groups.Remove(group);
                    break;
                }
            }
            if (flag)
            {
                group.Name = values[0] as string;
                if (ed)
                {
                    group.Path = values[6] as string;
                    group.Description = values[4] as string;
                }
            }
        }
        private void ifFamilyExists(Family family, object[] values, bool ed)
        {
            bool flag = true;
            foreach (Family item in family.Parent.Families)
            {
                if (values[1] as string == item.Name && !(family == item))
                {
                    flag = false;
                    foreach (Genus genus in family.Genuses)
                    {
                        item.Genuses.Add(genus);
                        genus.Parent = item;
                    }
                    if (ed)
                    {
                        item.Path = values[6] as string;
                        item.Description = values[4] as string;
                    }
                    family.Parent.Families.Remove(family);
                    break;
                }
            }
            if (flag)
            {
                family.Name = values[1] as string;
                if (ed)
                {
                    family.Path = values[6] as string;
                    family.Description = values[4] as string;
                }
            }
        }

        private void ifGenusExists(Genus genus, object[] values, bool ed)
        {
            bool flag = true;
            foreach (Genus item in genus.Parent.Genuses)
            {
                if (values[2] as string == item.Name && !(genus == item))
                {
                    flag = false;
                    foreach (Kind kind in genus.Kinds)
                    {
                        item.Kinds.Add(kind);
                        kind.Parent = item;
                    }
                    if (ed)
                    {
                        item.Path = values[6] as string;
                        item.Description = values[4] as string;
                    }
                    genus.Parent.Genuses.Remove(genus);
                    break;
                }
            }
            if (flag)
            {
                genus.Name = values[2] as string;
                if (ed)
                {
                    genus.Path = values[6] as string;
                    genus.Description = values[4] as string;
                }
            }
        }

        private void editFun(object sender)
        {
            var values = (object[])sender;
            if (SelectedItem is Group)
            {
                Group group = SelectedItem as Group;
                ifGroupExists(group, values, true);
            }
            if (SelectedItem is Family)
            {
                Family family = SelectedItem as Family;
                Group group = family.Parent;
                ifGroupExists(group, values, false);
                ifFamilyExists(family, values, true);
            }
            if (SelectedItem is Genus)
            {
                Genus genus = SelectedItem as Genus;
                Family family = genus.Parent;
                Group group = family.Parent;
                ifGroupExists(group, values, false);
                ifFamilyExists(family, values, false);
                ifGenusExists(genus, values, true);
            }
            if (SelectedItem is Kind)
            {
                Kind kind = SelectedItem as Kind;
                Genus genus = kind.Parent;
                Family family = genus.Parent;
                Group group = family.Parent;
                ifGroupExists(group, values, false);
                ifFamilyExists(family, values, false);
                ifGenusExists(genus, values, false);
                kind.Name = values[3] as string;
                kind.Path = values[6] as string;
                kind.Area = values[5] as string;
                kind.Description = values[4] as string;
            }
            if (CloseAdd != null)
                CloseAdd(this, new RoutedEventArgs());
        }

        private bool checkAdd(object arg)
        {
            var values = (object[])arg;
            for (int i = 0; i < 4; i++)
            {
                var v = values[i];
                if (v == string.Empty || v == null)
                    return false;
            }
            return true;
        }

        private bool checkItem(object arg)
        {
            if(SelectedItem != null)
                return true;
            return false;
        }

        private bool checkEdit(object arg)
        {
            var values = (object[])arg;
            if (SelectedItem is Group)
            {
                if (values[0] == string.Empty || values[0] == null)
                    return false;
            }
            if (SelectedItem is Family)
            {
                for (int i = 0; i < 2; i++)
                {
                    var v = values[i];
                    if (v == string.Empty || v == null)
                        return false;
                }
            }
            if (SelectedItem is Genus)
            {
                for (int i = 0; i < 3; i++)
                {
                    var v = values[i];
                    if (v == string.Empty || v == null)
                        return false;
                }
            }
            if (SelectedItem is Kind)
            {
                for (int i = 0; i < 4; i++)
                {
                    var v = values[i];
                    if (v == string.Empty || v == null)
                        return false;
                }
            }
            return true;
        }
        
    }
}
