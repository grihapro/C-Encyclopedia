using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace LAB4
{
    internal class Genus : ChangerBase, IBIRDItem
    {
        private ObservableCollection<Kind> _kinds;
        private Family _parent;
        private string name = string.Empty, description = string.Empty, path = null;
        [JsonIgnore]
        public Family Parent
        {
            get
            {
                return _parent;
            }
            set
            {
                _parent = value;
            }
        }
        public ObservableCollection<Kind> Kinds
        {
            get
            {
                return _kinds;
            }
            set
            {
                _kinds = value;
                OnPropertyChanged("Kinds");
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
        public Genus(string n, Family p)
        {
            Name = n;
            _kinds = new ObservableCollection<Kind>();
            Parent = p;
        }
        public void RemoveItem(object o)
        {
            Kinds.Remove(o as Kind);
        }
    }
}
