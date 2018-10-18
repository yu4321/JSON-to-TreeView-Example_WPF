using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using JSONVisualizer.GlobalContainer;
using JSONVisualizer.Messages;
using JSONVisualizer.Model;
using JSONVisualizer.Views;
using Newtonsoft.Json.Linq;
using System.Collections.ObjectModel;
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
    
        public ICommand ChangeEncodingCommand { get; set; }


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
        private string _currentencoding;
        public string currentencoding
        {
            get
            {
                return _currentencoding;
            }
            set
            {
                Set(nameof(currentencoding), ref _currentencoding, value);
            }
        }

        private string _FilePath;
        public string FilePath
        {
            get
            {
                return _FilePath;
            }
            set
            {
                Set(nameof(FilePath), ref _FilePath, value);
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

        private ObservableCollection<TreeNode> _TreeItems;
        public ObservableCollection<TreeNode> TreeItems

        {
            get
            {
                return _TreeItems;
            }

            set
            {
                Set(nameof(TreeItems), ref _TreeItems, value);
            }
        }

        public ContentListViewModel()
        {
            FilePath = "";
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
            ChangeEncodingCommand = new RelayCommand(() => ExecuteChangeEncodingCommand());
            urlmodeornot = true;
            canuseornot = true;
            if (GlobalJSONData.prevURL != null)
            {
                if (GlobalJSONData.prevURL.Length > 5)
                {
                    URL = GlobalJSONData.prevURL;
                    System.Console.WriteLine("prevurl: " + URL);
                }
            }
            if (GlobalJSONData.filepath != null)
            {
                if (GlobalJSONData.filepath.Length > 3)
                {
                    FilePath = GlobalJSONData.filepath;
                    System.Console.WriteLine("prevfilepath: " + FilePath);
                }
            }
            if (GlobalJSONData.nowencoding == Encoding.UTF8) currentencoding = "UTF8->EUC-KR";
            else currentencoding = "EUC-KR->UTF-8";

            StartTreeView();
        }

        private void StartTreeView()
        {
            if (GlobalJSONData.contentJObject == null && GlobalJSONData.contentJArray == null) return;
            TreeNode root;
            if (GlobalJSONData.Type == 0)
            {
                root = MakeTreeDataChildren(GlobalJSONData.contentJObject);
                TreeItems = root.Children;
                SetCountforChildrens(root);
            }
            else
            {
                root = MakeTreeDataChildren(GlobalJSONData.contentJArray);
                TreeItems = root.Children;
                SetCountforChildrens(root);
            }
        }

        void SetCountforChildrens(TreeNode parent)
        {
            parent.setCount();
            foreach (TreeNode i in parent.Children) SetCountforChildrens(i);
        }
        private TreeNode MakeTreeDataChildrenWork(object node, TreeNode result)
        {
            TreeItems = new ObservableCollection<TreeNode>();
            if (node is JProperty)
            {
                JProperty jp = (JProperty)node;
                result.Key = jp.Name;
                result.Children = MakeTreeDataChildren(jp).Children;
                return result;
            }
            else if (node is JValue)
            {
                JValue jv = (JValue)node;

                if (jv.Value != null)
                    result.Value = jv.Value.ToString();
                else
                {
                    result.Value = "null";
                }

                return result;
            }
            else if (node is JObject)
            {
                JObject jo = (JObject)node;
                foreach (var x in jo)
                {
                    result.Children.Add(MakeTreeDataChildren(x.Value, x.Key));
                }
                return result;
            }
            else if (node is JArray)
            {
                JArray ja = (JArray)node;
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
                return null;
            }
        }

        private TreeNode MakeTreeDataChildren(object node, string keyname)
        {
            TreeNode result = new TreeNode();
            result.Key = keyname;
            return MakeTreeDataChildrenWork(node, result);
        }

        private TreeNode MakeTreeDataChildren(object node, int index)
        {
            TreeNode result = new TreeNode();
            result.Key = "Element " + index;
            return MakeTreeDataChildrenWork(node, result);
        }

        private TreeNode MakeTreeDataChildren(object node)
        {
            TreeNode result = new TreeNode();
            return MakeTreeDataChildrenWork(node, result);
        }

        private void ExecuteSourceViewCommand()
        {
            var sourceView = new SourceView();
            sourceView.DataContext = new SourceViewViewModel();
            Messenger.Default.Send<NewWindowMessage>(new NewWindowMessage());
            sourceView.Show();
        }

        private async void ExecuteURLGetCommand()
        {
            GlobalJSONData.prevURL = URL;
            GlobalJSONData.filepath = "";
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
                    webClient.Encoding = Encoding.UTF8;
                    var task = webClient.DownloadStringTaskAsync(URL);
                    result = await task;
                }
                catch
                {
                    MessageBox.Show("Invalid HTTP Address!");
                }
            }
            return result;
        }

        private void ExecuteFileOpenCommand()
        {
            loadingstring = "Visible";
            GlobalJSONData.prevURL = "";
            Messenger.Default.Send<PageChangeMessage>(new PageChangeMessage(PageName.OpenFile));
        }


        void ExecuteChangeEncodingCommand()
        {
            if (GlobalJSONData.nowencoding == Encoding.UTF8)
            {
                GlobalJSONData.nowencoding = Encoding.GetEncoding(51949);
            }
            else
            {
                GlobalJSONData.nowencoding = Encoding.UTF8;
            }
            TreeItems.Clear();
            try
            {
                GlobalJSONData.contentJArray.Clear();
            }
            catch
            {

            }
            try
            {
                GlobalJSONData.contentJObject.RemoveAll();
            }
            catch
            {

            }

            GlobalJSONData.prevURL = "";
            GlobalJSONData.filepath = "";
            Messenger.Default.Send<PageChangeMessage>(new PageChangeMessage(PageName.Main));
        }
    }
}