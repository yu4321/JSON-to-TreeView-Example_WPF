using JSONVisualizer.ViewModel;
using System.Windows.Controls;

namespace JSONVisualizer.Views
{
    /// <summary>
    /// Description for ContentView.
    /// </summary>
    public partial class ContentView : UserControl
    {
        /// <summary>
        /// Initializes a new instance of the ContentView class.
        /// </summary>
        public ContentView()
        {
            InitializeComponent();
            ContextMenuClosing += (s, e) => ViewModelLocator.Cleanup();
        }
    }
}