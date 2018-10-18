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
        public ICommand AddCommand { get; set; }
        public ICommand DeleteCommand { get; set; }
        public ICommand SourceViewCommand { get; set; }

        public ICommand SelectCommand { get; set; }

        public ICommand FileOpenCommand { get; set; }

        public ICommand URLGetCommand { get; set; }

        public bool canwork { get; set; }

        //public bool canuseornot { get; set; }

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

        //public bool urlmodeornot { get; set; }

        public static ObservableCollection<ElementModel> Contents { get; set; }

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

        public int nowindex { get; set; }

        private ObservableCollection<TreeData> _TreeItems = new ObservableCollection<TreeData>();

        public ObservableCollection<TreeData> TreeItems

        {
            get { return _TreeItems; }

            set { _TreeItems = value; RaisePropertyChanged("TreeItems"); }
        }

        public ContentListViewModel()
        {
            //MessageBox.Show("listview start.");
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

            Contents = new ObservableCollection<ElementModel>();
            AddCommand = new RelayCommand(() => ExecuteAddCommand());
            SelectCommand = new RelayCommand<object>(ExecuteSelectCommand);
            DeleteCommand = new RelayCommand(() => ExecuteDeleteCommand());
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
            //MessageBox.Show("listview load complete");

            //sample data 생성

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
            //System.Console.WriteLine("invoked with " + td.GetType());
            TreeData result = new TreeData();
            if (td is JProperty)
            {
                //System.Console.WriteLine("invoked with JProperty...");
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

        private void RecursiveTreeViewPrint(JObject td)
        {
            int i = 0;
            //System.Console.WriteLine("AllPrint: "+td.ToString()+"Allprint>");
            //JArray inarr = JArray.Parse(td.ToString());
            //System.Console.WriteLine(inarr.Count);
            System.Console.WriteLine("RPrint: ");
            foreach (var z in td) System.Console.WriteLine(z.Value.ToString() + " Now Type: " + z.Value.GetType());
            System.Console.WriteLine("RPrint>");
            //System.Console.WriteLine(td.ToString());
            foreach (var x in td)
            {
                //System.Console.WriteLine(i + "th item");
                //System.Console.WriteLine("key: " + x.Key);
                //System.Console.WriteLine("Value: " + x.Value);
                if (x.Value != null)
                {
                    try
                    {
                        //System.Console.WriteLine(x.Value.ToString());
                        foreach (JObject y in x.Value.Children())
                        {
                            //System.Console.WriteLine(y.Count);
                            //System.Console.WriteLine(y.ToString());
                            RecursiveTreeViewPrint(y);
                            //System.Console.WriteLine(y.GetType());
                        }
                    }
                    catch
                    {
                        //System.Console.WriteLine("Cannot JObject Cast - "+x.Value);
                        System.Console.WriteLine("PPrint: ");
                        foreach (JProperty y in x.Value.Children())
                        {
                            System.Console.WriteLine(y.ToString() + " Now Type: " + y.Value.GetType());
                            //System.Console.WriteLine(y.GetType());
                        }
                        System.Console.WriteLine("PPrint>");
                        return;
                    }
                }
                i++;
            }
        }

        private void ExecuteSourceViewCommand()
        {
            System.Console.WriteLine("executed");
            //Messenger.Default.Send(new NotificationMessage("OpenSourceView"));
            var sourceView = new SourceView();
            sourceView.DataContext = new SourceViewViewModel();
            Messenger.Default.Send<NewWindowMessage>(new NewWindowMessage(Type.SourceView));
            sourceView.Show();
        }

        private async void ExecuteURLGetCommand()
        {
            GlobalJSONData.prevURL = URL;
            canuseornot = false;
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

        private void ExecuteAddCommand()
        {
            Messenger.Default.Send<PageChangeMessage>(new PageChangeMessage(PageName.Add, Contents));
        }

        private void ExecuteSelectCommand()
        {
            MessageBoxResult result = MessageBox.Show("ddd", "111", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                Application.Current.Shutdown();
            }
        }

        private void ExecuteSelectCommand(object param)
        {
            try
            {
                DataRowView drv = param as DataRowView;
                GlobalJSONData.nextindex = (int)drv[0];
                Messenger.Default.Send<PageChangeMessage>(new PageChangeMessage(PageName.View));
            }
            catch
            {
            }
        }

        private void ExecuteDeleteCommand()
        {
            try
            {
                GlobalJSONData.DeleteRow(nowindex);
            }
            catch
            {
            }
        }

        private void ExecuteFileOpenCommand()
        {
            Messenger.Default.Send<PageChangeMessage>(new PageChangeMessage(PageName.OpenFile));
        }
    }
}