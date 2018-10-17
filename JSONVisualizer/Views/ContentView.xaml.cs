using System.Windows;
using System.Windows.Controls;
using JSONVisualizer.ViewModel;

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

        private void Back_Click(object sender, RoutedEventArgs e)
        {
        }

        private void Modify_Click(object sender, RoutedEventArgs e)
        {
        }
    }
}