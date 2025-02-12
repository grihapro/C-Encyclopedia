using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;
using Newtonsoft.Json;

namespace LAB4
{
    internal class Kind : ChangerBase
    {
        private Genus _parent;
        private string name = string.Empty, description = string.Empty, path = null, area = string.Empty;

        [JsonIgnore]
        public Genus Parent
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
                if (path is null || path == string.Empty)
                    return "./../../../images/empty_720x540_e0a.png";
                return path;
            }
            set
            {
                path = value; 
                OnPropertyChanged("Path");
            }
        }

        public string Area
        {
            get
            {
                return area;
            }
            set
            {
                area = value;
                OnPropertyChanged("Area");
            }
        }

        public Kind(string d, Genus p)
        {
            Name = d;
            _parent = p;
        }

        [JsonConstructor]
        public Kind(string d, Genus p, string text, string path, string area)
        {
            Name = d;
            Description = text;
            Path = path;
            Area = area;
            Parent = p;
        }
    }
}
