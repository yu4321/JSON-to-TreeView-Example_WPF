using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using JSONVisualizer.GlobalContainer;
using JSONVisualizer.Messages;
using JSONVisualizer.Model;
using JSONVisualizer.Views;
using Newtonsoft.Json.Linq;
using System.Collections.ObjectModel;
using System.Data;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace JSONVisualizer.ViewModel
{
    public class ContentListViewModel : ViewModelBase
    {
        public ICommand SourceViewCommand { get; set; }

        public ICommand FileOpenCommand { get; set; }

        public ICommand URLGetCommand { get; set; }

        public bool canwork { get; set; }

        private bool _canuseornot;

        public bool canuseornot
        {
            get
            {
                return _canuseornot;
            }
            set
            {
                Set(nameof(canuseornot), ref _canuseornot, value);
            }
        }

        private bool _urlmodeornot;

        public bool urlmodeornot
        {
            get
            {
                return _urlmodeornot;
            }
            set
            {
                Set(nameof(urlmodeornot), ref _urlmodeornot, value);
            }
        }

        private ViewModelBase _currentViewModel;

        public static DataTable JSONData { get; set; }

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

        private string _url;

        public string URL
        {
            get
            {
                return _url;
            }
            set
            {
                Set(nameof(URL), ref _url, value);
            }
        }

        private string _loadingstring;

        public string loadingstring
        {
            get
            {
                return _loadingstring;
            }
            set
            {
                Set(nameof(loadingstring), ref _loadingstring, value);
            }
        }

        private ObservableCollection<TreeData> _TreeItems = new ObservableCollection<TreeData>();

        public ObservableCollection<TreeData> TreeItems

        {
            get { return _TreeItems; }

            set { _TreeItems = value; RaisePropertyChanged("TreeItems"); }
        }

        public ContentListViewModel()
        {
            JSONData = GlobalJSONData.data;
            canwork = false;
            try
            {
                if (GlobalJSONData.contentJArray.ToString().Length > 3) canwork = true;
            }
            catch
            {
                try
                {
                    if (GlobalJSONData.contentJObject.ToString().Length > 3) canwork = true;
                }
                catch
                {
                }
            }
            loadingstring = "Hidden";
            FileOpenCommand = new RelayCommand(() => ExecuteFileOpenCommand());
            SourceViewCommand = new RelayCommand(() => ExecuteSourceViewCommand());
            URLGetCommand = new RelayCommand(() => ExecuteURLGetCommand());
            urlmodeornot = true;
            canuseornot = true;
            if (GlobalJSONData.prevURL != null)
            {
                System.Console.WriteLine("prev: " + GlobalJSONData.prevURL);
                URL = GlobalJSONData.prevURL;
            }
            StartTreeView();
        }

        private void StartTreeView()
        {
            try
            {
                System.Console.WriteLine("JObject Count: " + GlobalJSONData.contentJObject.Count);
            }
            catch
            {
                try
                {
                    System.Console.WriteLine("JArray Count: " + GlobalJSONData.contentJArray.Count);
                }
                catch
                {
                    System.Console.WriteLine("파일없음");
                    return;
                }
            }

            if (GlobalJSONData.Type == 0)
                TreeItems = MakeTreeDataChildren(GlobalJSONData.contentJObject).Children;
            else TreeItems = MakeTreeDataChildren(GlobalJSONData.contentJArray).Children;
        }

        private TreeData MakeTreeDataChildren(object td, string keyname)
        {
            TreeData result = new TreeData();
            result.Name = keyname;
            if (td is JProperty)
            {
                JProperty jp = (JProperty)td;
                result.Name = jp.Name;
                result.Children = MakeTreeDataChildren(jp).Children;
                return result;
            }
            else if (td is JValue)
            {
                JValue jv = (JValue)td;

                if (jv.Value != null)
                    result.Value = jv.Value.ToString();
                else
                {
                    result.Value = "null";
                }

                return result;
            }
            else if (td is JObject)
            {
                JObject jo = (JObject)td;
                foreach (var x in jo)
                {
                    result.Children.Add(MakeTreeDataChildren(x.Value, x.Key));
                }
                return result;
            }
            else if (td is JArray)
            {
                JArray ja = (JArray)td;
                int i = 0;
                foreach (var x in ja)
                {
                    result.Children.Add(MakeTreeDataChildren(x, i));
                    i++;
                }
                return result;
            }
            else
            {
                System.Console.WriteLine("Error Type: " + td.GetType());
            }

            return null;
        }

        private TreeData MakeTreeDataChildren(object td, int index)
        {
            TreeData result = new TreeData();
            result.Name = "Element " + index;
            if (td is JProperty)
            {
                JProperty jp = (JProperty)td;
                result.Name = jp.Name;
                result.Children = MakeTreeDataChildren(jp).Children;
                return result;
            }
            else if (td is JValue)
            {
                JValue jv = (JValue)td;

                if (jv.Value != null)
                    result.Value = jv.Value.ToString();
                else
                {
                    result.Value = "null";
                }

                return result;
            }
            else if (td is JObject)
            {
                JObject jo = (JObject)td;
                foreach (var x in jo)
                {
                    result.Children.Add(MakeTreeDataChildren(x.Value, x.Key));
                }
                return result;
            }
            else if (td is JArray)
            {
                JArray ja = (JArray)td;
                int i = 0;
                foreach (var x in ja)
                {
                    result.Children.Add(MakeTreeDataChildren(x, i));
                    i++;
                }
                return result;
            }
            else
            {
                System.Console.WriteLine("Error Type: " + td.GetType());
            }

            return null;
        }

        private TreeData MakeTreeDataChildren(object td)
        {
            TreeData result = new TreeData();
            if (td is JProperty)
            {
                JProperty jp = (JProperty)td;
                result.Name = jp.Name;
                result.Children = MakeTreeDataChildren(jp).Children;
                return result;
            }
            else if (td is JValue)
            {
                JValue jv = (JValue)td;

                if (jv.Value != null)
                    result.Value = jv.Value.ToString();
                else
                {
                    result.Value = "null";
                }
                return result;
            }
            else if (td is JObject)
            {
                JObject jo = (JObject)td;
                foreach (var x in jo)
                {
                    result.Children.Add(MakeTreeDataChildren(x.Value, x.Key));
                }
                return result;
            }
            else if (td is JArray)
            {
                JArray ja = (JArray)td;
                int i = 0;
                foreach (var x in ja)
                {
                    result.Children.Add(MakeTreeDataChildren(x, i));
                    i++;
                }
                return result;
            }
            else
            {
                System.Console.WriteLine("Error Type: " + td.GetType());
            }
            return null;
        }

        private void ExecuteSourceViewCommand()
        {
            System.Console.WriteLine("executed");
            var sourceView = new SourceView();
            sourceView.DataContext = new SourceViewViewModel();
            Messenger.Default.Send<NewWindowMessage>(new NewWindowMessage(Type.SourceView));
            sourceView.Show();
        }

        private async void ExecuteURLGetCommand()
        {
            GlobalJSONData.prevURL = URL;
            canuseornot = false;
            loadingstring = "Visible";
            string result = await GetDocumentfromURL();
            Messenger.Default.Send<PageChangeMessage>(new PageChangeMessage(PageName.OpenURL, result));
            canuseornot = true;
        }

        private async Task<string> GetDocumentfromURL()
        {
            string result = "";
            using (var webClient = new WebClient())
            {
                try
                {
                    System.Console.WriteLine(URL);
                    webClient.Encoding = Encoding.UTF8;
                    var task = webClient.DownloadStringTaskAsync(URL);
                    result = await task;
                }
                catch
                {
                    MessageBox.Show("Invalid http Address!");
                }
            }
            return result;
        }

        private void ExecuteFileOpenCommand()
        {
            loadingstring = "Visible";
            Messenger.Default.Send<PageChangeMessage>(new PageChangeMessage(PageName.OpenFile));
        }
    }
}