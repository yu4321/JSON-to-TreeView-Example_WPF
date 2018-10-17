using GalaSoft.MvvmLight;
using System.Collections.ObjectModel;

namespace JSONVisualizer.Model
{
    public class TreeData : ObservableObject

    {
        private string _Name;

        public string Name

        {
            get { return _Name; }

            set { _Name = value; RaisePropertyChanged("ObservableObject"); }
        }

        private string _Value;

        public string Value

        {
            get { return _Value; }

            set { _Value = value; RaisePropertyChanged("ObservableObject"); }
        }

        private ObservableCollection<TreeData> _Children = new ObservableCollection<TreeData>();

        public ObservableCollection<TreeData> Children

        {
            get { return _Children; }

            set { _Children = value; RaisePropertyChanged("Children"); }
        }

        public TreeData()
        {
            Name = "(ArrayMember)";
        }
    }
}