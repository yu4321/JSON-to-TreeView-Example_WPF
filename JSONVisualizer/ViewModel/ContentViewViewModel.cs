using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using JSONVisualizer.GlobalContainer;
using JSONVisualizer.Messages;
using JSONVisualizer.Model;
using System.Data;
using System.Windows.Input;

namespace JSONVisualizer.ViewModel
{
    /// <summary>
    /// This class contains properties that a View can data bind to.
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class ContentViewViewModel : ViewModelBase
    {
        /// <summary>
        /// Initializes a new instance of the ContentViewViewModel class.
        /// </summary>
        public ICommand BackCommand { get; set; }

        public ICommand ModifyCommand { get; set; }
        public ElementModel Data { get; set; }

        private ViewModelBase _currentViewModel;

        public ViewModelBase CurrentViewModel
        {
            get
            {
                return _currentViewModel;
            }
            set
            {
                Set(nameof(CurrentViewModel), ref _currentViewModel, value);
            }
        }

        public ContentViewViewModel()
        {
            Data = new ElementModel();
            DataRow dr = GlobalJSONData.data.Rows[GlobalJSONData.nextindex];
            Data.Index = (int)dr[0];
            Data.main = (string)dr[1];
            Data.sub = (string)dr[2];
            Data.comment = (string)dr[3];
            System.Console.WriteLine(Data.comment);
            System.Console.WriteLine("Current Index: " + GlobalJSONData.nextindex);

            BackCommand = new RelayCommand(() => ExecuteBackCommand());
            ModifyCommand = new RelayCommand(() => ExecuteModifyCommand());
        }

        private void ExecuteBackCommand()
        {
            Messenger.Default.Send<PageChangeMessage>(new PageChangeMessage(PageName.Main));
            System.Console.Write("clicked");
        }

        private void ExecuteModifyCommand()
        {
            Messenger.Default.Send<PageChangeMessage>(new PageChangeMessage(PageName.Modify));
            System.Console.Write("modify clicked");
        }
    }
}