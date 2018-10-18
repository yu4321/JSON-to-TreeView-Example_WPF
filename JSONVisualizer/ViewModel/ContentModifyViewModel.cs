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
    public class ContentModifyViewModel : ViewModelBase
    {
        /// <summary>
        /// Initializes a new instance of the ContentModifyViewModel class.
        /// </summary>
        public ICommand BackCommand { get; set; }

        public ICommand ConfirmCommand { get; set; }
        public ElementModel Data { get; set; }

        private DataRow dr;

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

        public ContentModifyViewModel()
        {
            Data = new ElementModel();
            dr = GlobalJSONData.data.Rows[GlobalJSONData.nextindex];
            Data.Index = (int)dr[0];
            Data.main = (string)dr[1];
            Data.sub = (string)dr[2];
            Data.comment = (string)dr[3];
            System.Console.WriteLine(Data.comment);
            BackCommand = new RelayCommand(() => ExecuteBackCommand());
            ConfirmCommand = new RelayCommand(() => ExecuteConfirmCommand());
        }

        private void ExecuteBackCommand()
        {
            Messenger.Default.Send<PageChangeMessage>(new PageChangeMessage(PageName.View));
            System.Console.Write("clicked");
        }

        private void ExecuteConfirmCommand()
        {
            DataRow newrow = dr;
            newrow[1] = Data.main;
            newrow[2] = Data.sub;
            newrow[3] = Data.comment;
            GlobalJSONData.FileSave();
            Messenger.Default.Send<PageChangeMessage>(new PageChangeMessage(PageName.Main));
        }
    }
}