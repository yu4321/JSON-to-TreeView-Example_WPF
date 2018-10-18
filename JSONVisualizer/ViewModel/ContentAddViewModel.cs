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
    ///
    public class ContentAddViewModel : ViewModelBase
    {
        public ICommand BackCommand { get; set; }
        public ICommand ConfirmCommand { get; set; }

        public string Main { get; set; }
        public string Sub { get; set; }
        public string Comment { get; set; }
        public ElementModel model;
        /// <summary>
        /// Initializes a new instance of the ContentAddViewModel class.
        /// </summary>

        public ContentAddViewModel()
        {
            BackCommand = new RelayCommand(() => ExecuteBackCommand());
            ConfirmCommand = new RelayCommand(() => ExecuteConfirmCommand());
            model = new ElementModel();
        }

        private void ExecuteBackCommand()
        {
            Messenger.Default.Send<PageChangeMessage>(new PageChangeMessage(PageName.Main));
        }

        private void ExecuteConfirmCommand()
        {
            model.main = Main;
            model.sub = Sub;
            model.comment = Comment;
            DataRow newrow = GlobalJSONData.data.NewRow();
            newrow[0] = GlobalJSONData.data.Rows.Count;
            newrow[1] = Main;
            newrow[2] = Sub;
            newrow[3] = Comment;
            GlobalJSONData.data.Rows.Add(newrow);
            GlobalJSONData.FileSave();
            Messenger.Default.Send<PageChangeMessage>(new PageChangeMessage(PageName.Main));
        }
    }
}