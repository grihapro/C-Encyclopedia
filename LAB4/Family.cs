using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace LAB4
{
    internal class Family : ChangerBase, IBIRDItem
    {
        private ObservableCollection<Genus> _genuses;
        private Group _parent;
        private string name = string.Empty, description = string.Empty, path = null;
        [JsonIgnore]
        public Group Parent 
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
        public ObservableCollection<Genus> Genuses
        {
            get
            {
                return _genuses;
            }
            set
            {
                _genuses = value;
                OnPropertyChanged("Genuses");
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
        public Family(string n, Group p)
        {
            Name = n;
            _genuses = new ObservableCollection<Genus>();
            Parent = p;
        }
        public void RemoveItem(object o)
        {
            Genuses.Remove(o as Genus);
        }
    }
}
