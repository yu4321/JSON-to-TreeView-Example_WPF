using System.Windows;
using System.Windows.Controls;

namespace JSONVisualizer.Views
{
    /// <summary>
    /// Description for ContentList.
    /// </summary>
    public partial class ContentList : UserControl
    {
        /// <summary>
        /// Initializes a new instance of the ContentList class.
        /// </summary>
        public ContentList()
        {
            InitializeComponent();
        }

        private void Button_01_Click(object sender, RoutedEventArgs e)
        {
        }

        private void Button_02_Click(object sender, RoutedEventArgs e)
        {
        }

        private void ContentListView_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
        }

        private void DataGrid_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            //DataGridRow s = sender as DataGridRow;
            string alerter = sender.GetType().ToString();
            string alerter2 = sender.ToString();
            //string alerter = "index: " + s.GetIndex();
            MessageBoxResult result = MessageBox.Show(alerter, alerter2, MessageBoxButton.OK, MessageBoxImage.Question);
        }
    }
}