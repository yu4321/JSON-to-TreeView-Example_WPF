using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using Newtonsoft.Json.Linq;
using System.Collections.ObjectModel;
using System.Data;
using System.Windows;
using System.Windows.Input;
using JSONVisualizer.GlobalContainer;
using JSONVisualizer.Messages;
using JSONVisualizer.Model;

namespace JSONVisualizer.ViewModel
{
    public class ContentListViewModel : ViewModelBase
    {
        public ICommand AddCommand { get; set; }
        public ICommand DeleteCommand { get; set; }
        public ICommand ViewCommand { get; set; }

        public ICommand SelectCommand { get; set; }

        public ICommand FileOpenCommand { get; set; }

        public bool canwork { get; set; }

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
            if (GlobalJSONData.filepath.Length > 3) canwork = true;

            Contents = new ObservableCollection<ElementModel>();
            AddCommand = new RelayCommand(() => ExecuteAddCommand());
            SelectCommand = new RelayCommand<object>(ExecuteSelectCommand);
            DeleteCommand = new RelayCommand(() => ExecuteDeleteCommand());
            FileOpenCommand = new RelayCommand(() => ExecuteFileOpenCommand());
            ViewCommand = new RelayCommand(() => ExecuteViewCommand());
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
            /*foreach (JObject tmp in GlobalJSONData.contentJObject.Children<JObject>())
            {
                System.Console.WriteLine("dddd");
                System.Console.WriteLine(tmp.Count);
                foreach (var x in tmp)
                {
                    //string name = x.Key;
                }
            }*/

            //RecursiveTreeViewPrint(GlobalJSONData.contentJObject);

            //GlobalJSONData.contentJObject.
            /*foreach(var x in GlobalJSONData.contentJObject)
            {
                System.Console.WriteLine(i + "th item");
                System.Console.WriteLine("key: " + x.Key);
                System.Console.WriteLine("Value: " + x.Value);
                //if(x.Value!=null)
            }*/

            /*
            foreach(var tmp in GlobalJSONData.contentJObject.Children())
            {
                JObject child=JObject.Parse()
            }
            */
            //JArray arr = JArray.Parse(GlobalJSONData.contentJObject.Root.ToString());
            /*Dictionary<string, string> dictobj = GlobalJSONData.contentJObject.ToObject<Dictionary<string, string>>();
            foreach(var tmp in dictobj)
            {
                System.Console.WriteLine("Key: " + tmp.Key);
                System.Console.WriteLine("Value: " + tmp.Value);
            }
            */
            /*
            foreach (JObject tmp in arr)
            {
                //System.Console.WriteLine(tmp.ToString());
                //JArray inarr = JArray.Parse(tmp.ToString());
                RecursiveTreeViewPrint(tmp);
            }
            */
            //RecursiveTreeViewPrint(GlobalJSONData.contentJObject.Children());

            /* ObservableCollection<TreeData> sampleChild = new ObservableCollection<TreeData>();
             sampleChild.Add(new TreeData() { Name = "부모1", Value = "Value" });
             sampleChild.Add(new TreeData() { Name = "부모2", Value = "Value" });
             sampleChild.Add(new TreeData() { Name = "부모3", Value = "Value" });

             ObservableCollection<TreeData> sampleChild2 = new ObservableCollection<TreeData>();
             sampleChild2.Add(new TreeData() { Name = "자식1", Value = "Value" });
             sampleChild2.Add(new TreeData() { Name = "자식2", Value = "Value" });
             sampleChild2.Add(new TreeData() { Name = "자식3", Value = "Value" });

             sampleChild.Add(new TreeData() { Name = "부모4", Value = "Value", Children = sampleChild2 });

             TreeData treeData = new TreeData() { Name = "루트", Children = sampleChild, Value = "Value" };

             //binding할 collection에 추가하기

             */
            if (GlobalJSONData.Type == 0)
                TreeItems = MakeTreeDataChildren(GlobalJSONData.contentJObject).Children;
            else TreeItems = MakeTreeDataChildren(GlobalJSONData.contentJArray).Children;
        }

        private TreeData MakeTreeDataChildren(object td, string keyname)
        {
            //System.Console.WriteLine("invoked with " + td.GetType());
            TreeData result = new TreeData();
            result.Name = keyname;
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
                //System.Console.WriteLine("JValue Type: "+jv.Value.GetType());
                //System.Console.WriteLine("JValue Value: " + jv.Value);
                /*System.Console.WriteLine("invoked with JValue...");
                System.Console.WriteLine("to - "+jv.ToString()+" vato"+ jv.Value.ToString());
                System.Console.WriteLine("type" + jv.Type.ToString());
                System.Console.WriteLine("parent type " + jv.Parent);*/
                //result.Name = jv.ToString();

                if (jv.Value != null)
                    result.Value = jv.Value.ToString();
                else
                {
                    result.Value = "null";
                    //result.Value = null;
                }
                
                return result;

            }
            else if (td is JObject)
            {
                JObject jo = (JObject)td;
                foreach (var x in jo)
                {
                    //System.Console.WriteLine("invoked with JObject....");
                    result.Children.Add(MakeTreeDataChildren(x.Value, x.Key));
                    //result.Name = x.Key;
                }
                return result;
            }
            else if (td is JArray)
            {
                JArray ja = (JArray)td;
                foreach (var x in ja)
                {
                    //System.Console.WriteLine("invoked with JArray.....");
                    //result.Name=x.
                    result.Children.Add(MakeTreeDataChildren(x));
                }
                return result;
            }
            else
            {
                System.Console.WriteLine("Error Type: " + td.GetType());
            }
            //ObservableCollection<TreeData> sampleChild = new ObservableCollection<TreeData>();
            /*foreach (var x in td)
            {
                if (x.Value != null)
                {
                    try
                    {
                        foreach (JObject y in x.Value.Children())
                        {
                            if(y.Count>1)
                            MakeTreeDataChildren(y);
                            else
                            {
                            }
                        }
                    }
                    catch
                    {
                        foreach (JProperty y in x.Value.Children())
                        {
                            System.Console.WriteLine("PPRINT: " + y.ToString());
                        }
                    }
                }
            }*/
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
                //System.Console.WriteLine("JValue Type: "+jv.Value.GetType());
                //System.Console.WriteLine("JValue Value: " + jv.Value);
                /* System.Console.WriteLine("invoked with JValue...");
                 System.Console.WriteLine("to - " + jv.ToString() + " vato" + jv.Value.ToString());
                 System.Console.WriteLine("type" + jv.Type.ToString());
                 System.Console.WriteLine("parent type " + jv.Parent);*/
                //result.Name = jv.ToString();
                result.Value = jv.Value.ToString();
                return result;
            }
            else if (td is JObject)
            {
                JObject jo = (JObject)td;
                foreach (var x in jo)
                {
                    // System.Console.WriteLine("invoked with JObject....");
                    result.Children.Add(MakeTreeDataChildren(x.Value, x.Key));
                    //result.Name = x.Key;
                }
                return result;
            }
            else if (td is JArray)
            {
                JArray ja = (JArray)td;
                foreach (var x in ja)
                {
                    //System.Console.WriteLine("invoked with JArray.....");
                    //result.Name=x.
                    result.Children.Add(MakeTreeDataChildren(x));
                }
                return result;
            }
            else
            {
                System.Console.WriteLine("Error Type: " + td.GetType());
            }
            //ObservableCollection<TreeData> sampleChild = new ObservableCollection<TreeData>();
            /*foreach (var x in td)
            {
                if (x.Value != null)
                {
                    try
                    {
                        foreach (JObject y in x.Value.Children())
                        {
                            if(y.Count>1)
                            MakeTreeDataChildren(y);
                            else
                            {
                            }
                        }
                    }
                    catch
                    {
                        foreach (JProperty y in x.Value.Children())
                        {
                            System.Console.WriteLine("PPRINT: " + y.ToString());
                        }
                    }
                }
            }*/
            return null;
        }

        private void RecursiveTreeViewAdd(JObject td)
        {
            int i = 0;
            System.Console.WriteLine("RPrint: ");
            foreach (var z in td) System.Console.WriteLine(z.Value.ToString() + " Now Count: ");
            foreach (var x in td)
            {
                if (x.Value != null)
                {
                    try
                    {
                        foreach (JObject y in x.Value.Children())
                        {
                            RecursiveTreeViewPrint(y);
                        }
                    }
                    catch
                    {
                        int i2 = -1;
                        foreach (JProperty y in x.Value.Children())
                        {
                            i2++;
                            System.Console.WriteLine("PPRINT: " + y.ToString());
                        }
                        return;
                    }
                }
                i++;
            }
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

                    /*foreach(var y in new JObject(JObject.Parse(x.Value.ToString())))
                    {
                    }*/
                }
                i++;
            }
        }

        private void RecursiveTreeViewPrint2(object td)
        {
            //System.Console.WriteLine("AllPrint: " + td.ToString() + "Allprint>");
            //System.Console.WriteLine("RPrint: ");
            TreeData result = new TreeData();
            if (td is JProperty)
            {
                //result.Children.Add(new Tr)
            }
            else if (td is JValue)
            {
            }
            else if (td is JObject)
            {
            }
        }

        private void ExecuteViewCommand()
        {
        }

        /*public void intializeContents()
        {
            System.Console.WriteLine("start");
            Contents.Clear();
            using (FileStream fs = new FileStream("element.JSON", FileMode.OpenOrCreate))
            {
                StreamReader sr = new StreamReader(fs, Encoding.UTF8);
                int index = -1;
                while (!sr.EndOfStream)
                {
                    index++;
                    string s = sr.ReadLine();
                    string[] temp = s.Split(',');
                    try
                    {
                        ElementModel tmp = new ElementModel();
                        tmp.comment = temp[2];
                        tmp.sub = temp[1];
                        tmp.main = temp[0];
                        Contents.Add(tmp);
                        System.Console.WriteLine("작성 " + Contents[index].main);
                    }
                    catch
                    {
                        continue;
                    }
                }
            }
        }
        private object ReceiveMessage(PageChangeMessage action)
        {
            switch (action.PageName)
            {
                case PageName.Add:
                    CurrentViewModel = ServiceLocator.Current.GetInstance<ContentAddViewModel>();
                    break;

                case PageName.View:
                    CurrentViewModel = ServiceLocator.Current.GetInstance<ContentViewViewModel>();
                    break;
            }
            return null;
        }
        */

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