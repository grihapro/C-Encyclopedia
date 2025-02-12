using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LAB4
{
    internal class Group : ChangerBase, IBIRDItem
    {
        private ObservableCollection<Family> _families;
        private string name = string.Empty, description = string.Empty, path = null;

        public ObservableCollection<Family> Families
        {
            get
            {
                return _families;
            }
            set
            {
                _families = value;
                OnPropertyChanged("Families");
            }
        }
        public string Description
        {
            get
            {
                if (description == string.Empty)
                    return "Нет описания";
                return description;
            }
            set
            {
                description = value;
                OnPropertyChanged("Description");
            }
        }

        public string Path
        {
            get
            {
                if (path is null)
                    return "./../../../images/empty_720x540_e0a.png";
                return path;
            }
            set
            {
                path = value;
                OnPropertyChanged("Path");
            }
        }
        public string Name
        {
            get
            {
                return name;
            }
            set
            {
                name = value;
                OnPropertyChanged("Name");
            }
        }
        public Group(string n)
        {
            Name = n;
            _families = new ObservableCollection<Family>();
        }
        public void RemoveItem(object o)
        {
            Families.Remove(o as Family);
        }
    }
}
