using CommonServiceLocator;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;
using JSONVisualizer.GlobalContainer;
using JSONVisualizer.Messages;
using JSONVisualizer.Model;
using Microsoft.Win32;
using Newtonsoft.Json.Linq;
using System.Collections.ObjectModel;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Input;

namespace JSONVisualizer.ViewModel
{
    /// <summary>
    /// This class contains properties that the main View can data bind to.
    /// <para>
    /// See http://www.mvvmlight.net
    /// </para>
    /// </summary>
    public class MainViewModel : ViewModelBase
    {
        //private readonly IDataService _dataService;

        /// <summary>

        /// </summary>
        //public const string WelcomeTitlePropertyName = "WelcomeTitle";
        private string _welcomeTitle = string.Empty;

        public ICommand AddCommand { get; set; }
        public ICommand DeleteCommand { get; set; }
        public ICommand ViewCommand { get; set; }
        private OpenFileDialog dlg;

        public ObservableCollection<ElementModel> Contents { get; set; }

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

        //public readonly static ContentListViewModel _contentListViewModel = new ContentListViewModel();

        /// <summary>
        /// Gets the WelcomeTitle property.
        /// Changes to that property's value raise the PropertyChanged event.
        /// </summary>

        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>

        public MainViewModel()
        {
            System.Console.WriteLine("MainWindow Start");
            GlobalJSONData.filepath = "";
            dlg = new OpenFileDialog();
            Contents = new ObservableCollection<ElementModel>();
            Messenger.Default.Register<PageChangeMessage>(this, (action) => ReceiveMessage(action));
            CurrentViewModel = ServiceLocator.Current.GetInstance<ContentListViewModel>();
        }

        private void ParseJSON(string importedstring)
        {
            try
            {
                GlobalJSONData.Type = 0;
                JObject obj = JObject.Parse(importedstring);
                GlobalJSONData.contentJObject = obj;
            }
            catch
            {
                try
                {
                    GlobalJSONData.Type = 1;
                    System.Console.WriteLine("Try parse as array");
                    JArray obj = JArray.Parse(importedstring);
                    GlobalJSONData.contentJArray = obj;
                }
                catch
                {
                    try
                    {
                        GlobalJSONData.Type = 0;
                        System.Console.WriteLine("Try parse as object after trim");
                        string jsonResult = importedstring;
                        jsonResult = jsonResult.TrimStart(new char[] { '[' }).TrimEnd(new char[] { ']' });
                        //JArray obj = JArray.Parse(sr.ReadToEnd());
                        JObject obj = JObject.Parse(jsonResult);
                        GlobalJSONData.contentJObject = obj;
                    }
                    catch
                    {
                        try
                        {
                            GlobalJSONData.Type = 1;
                            System.Console.WriteLine("Try parse as array after trim");
                            string jsonResult = importedstring;
                            jsonResult = jsonResult.TrimStart(new char[] { '[' }).TrimEnd(new char[] { ']' });
                            //JArray obj = JArray.Parse(sr.ReadToEnd());
                            JArray obj = JArray.Parse(jsonResult);
                            GlobalJSONData.contentJArray = obj;
                        }
                        catch
                        {
                            MessageBox.Show("Please use valid json file");
                        }
                    }
                }
            }
        }

        private void InitializeJSONbyFile()
        {
            string importedfilestring = "";
            dlg.Reset();
            dlg.DefaultExt = ".JSON";
            dlg.Filter = "JSON files (*.JSON)|*.JSON";
            System.Console.WriteLine("start");
            Contents.Clear();
            dlg.ShowDialog();
            if (dlg.FileName.Length > 3)
            {
                try
                {
                    GlobalJSONData.filepath = dlg.FileName;
                    System.Console.WriteLine("File Name: " + GlobalJSONData.filepath);
                }
                catch
                {
                    MessageBox.Show("Please select JSON file before start.");
                    return;
                }
                finally
                {
                    dlg.Reset();
                }
            }
            else
            {
                MessageBox.Show("Please select file before start.");
                return;
            }

            using (FileStream fs = new FileStream(GlobalJSONData.filepath, FileMode.Open))
            {
                StreamReader sr = new StreamReader(fs, Encoding.UTF8);
                importedfilestring = sr.ReadToEnd();
            }
            ParseJSON(importedfilestring);
        }

        private void InitializeJSONTreeVIew()
        {
            dlg.Reset();
            dlg.DefaultExt = ".JSON";
            dlg.Filter = "JSON files (*.JSON)|*.JSON";
            System.Console.WriteLine("start");
            Contents.Clear();
            dlg.ShowDialog();
            if (dlg.FileName.Length > 3)
            {
                try
                {
                    GlobalJSONData.filepath = dlg.FileName;
                    System.Console.WriteLine("File Name: " + GlobalJSONData.filepath);
                }
                catch
                {
                    MessageBox.Show("Please select JSON file before start.");
                    return;
                }
                finally
                {
                    dlg.Reset();
                }
            }
            else
            {
                MessageBox.Show("Please select file before start.");
                return;
            }

            using (FileStream fs = new FileStream(GlobalJSONData.filepath, FileMode.OpenOrCreate))
            {
                StreamReader sr = new StreamReader(fs, Encoding.UTF8);
                string importedfilestring = sr.ReadToEnd();

                try
                {
                    GlobalJSONData.Type = 0;
                    JObject obj = JObject.Parse(importedfilestring);
                    GlobalJSONData.contentJObject = obj;
                }
                catch
                {
                    try
                    {
                        GlobalJSONData.Type = 1;
                        System.Console.WriteLine("Try parse as array");
                        JArray obj = JArray.Parse(importedfilestring);
                        GlobalJSONData.contentJArray = obj;
                    }
                    catch
                    {
                        try
                        {
                            GlobalJSONData.Type = 0;
                            System.Console.WriteLine("Try parse as object after trim");
                            string jsonResult = importedfilestring;
                            jsonResult = jsonResult.TrimStart(new char[] { '[' }).TrimEnd(new char[] { ']' });
                            //JArray obj = JArray.Parse(sr.ReadToEnd());
                            JObject obj = JObject.Parse(jsonResult);
                            GlobalJSONData.contentJObject = obj;
                        }
                        catch
                        {
                            try
                            {
                                GlobalJSONData.Type = 1;
                                System.Console.WriteLine("Try parse as array after trim");
                                string jsonResult = importedfilestring;
                                jsonResult = jsonResult.TrimStart(new char[] { '[' }).TrimEnd(new char[] { ']' });
                                //JArray obj = JArray.Parse(sr.ReadToEnd());
                                JArray obj = JArray.Parse(jsonResult);
                                GlobalJSONData.contentJArray = obj;
                            }
                            catch
                            {
                                MessageBox.Show("Please use valid json file");
                            }
                        }
                    }
                }
            }
        }

        public object ReceiveMessage(PageChangeMessage action)
        {
            ViewModelLocator.Cleanup();
            switch (action.PageName)
            {
                case PageName.Add:
                    CurrentViewModel = ServiceLocator.Current.GetInstance<ContentAddViewModel>();
                    break;

                case PageName.View:
                    CurrentViewModel = CommonServiceLocator.ServiceLocator.Current.GetInstance<ContentViewViewModel>();
                    break;

                case PageName.Main:
                    CurrentViewModel = ServiceLocator.Current.GetInstance<ContentListViewModel>();
                    break;

                case PageName.Modify:
                    CurrentViewModel = ServiceLocator.Current.GetInstance<ContentModifyViewModel>();
                    break;

                case PageName.OpenFile:
                    InitializeJSONbyFile();
                    Messenger.Default.Send<PageChangeMessage>(new PageChangeMessage(PageName.Main));
                    break;

                case PageName.OpenURL:
                    ParseJSON((string)action.Param);
                    Messenger.Default.Send<PageChangeMessage>(new PageChangeMessage(PageName.Main));
                    break;
            }
            return null;
        }

        private void ExecuteAddCommand()
        {
            Messenger.Default.Send<PageChangeMessage>(new PageChangeMessage(PageName.Add));
        }
    }
}