using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;
using JSONVisualizer.GlobalContainer;
using JSONVisualizer.Messages;

namespace JSONVisualizer.ViewModel
{
    /// <summary>
    /// This class contains properties that a View can data bind to.
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class SourceViewViewModel : ViewModelBase
    {
        /// <summary>
        /// Initializes a new instance of the SourceViewViewModel class.
        /// </summary>
        ///
        public string Content { get; set; }

        public SourceViewViewModel()
        {
            Messenger.Default.Register<NewWindowMessage>(this, (action) => ReceiveMessage(action));
        }

        public object ReceiveMessage(NewWindowMessage action)
        {
            Content = (GlobalJSONData.Type == 0) ? GlobalJSONData.contentJObject.ToString() : GlobalJSONData.contentJArray.ToString();
            return null;
        }
    }
}