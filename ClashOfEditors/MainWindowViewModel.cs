using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClashOfEditors
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        private string _template;
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        public string Template
        {
            get { return _template; }
            set
            {
                _template = value;
                OnPropertyChanged("Template");
            }
        }
    }
}
