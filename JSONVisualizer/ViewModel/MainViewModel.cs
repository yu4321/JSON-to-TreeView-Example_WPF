using CommonServiceLocator;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;
using JSONVisualizer.GlobalContainer;
using JSONVisualizer.Messages;
using Microsoft.Win32;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
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
            GlobalJSONData.filepath = "";
            dlg = new OpenFileDialog();
            Messenger.Default.Register<PageChangeMessage>(this, (action) => ReceiveMessage(action));
            CurrentViewModel = ServiceLocator.Current.GetInstance<ContentListViewModel>();
        }

        void FixJSONString()
        {
            
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
                    JArray obj = JArray.Parse(importedstring);
                    GlobalJSONData.contentJArray = obj;
                }
                catch
                {
                    try
                    {
                        GlobalJSONData.Type = 0;
                        importedstring = importedstring.Replace("\":\"\",\"", "\":\" \",\"");
                        importedstring = importedstring.Replace("\":\"\"", "\":\"");
                        importedstring = importedstring.Replace("\"\",\"", "\",\"");
                        importedstring = importedstring.Replace("\\", "\\\\");
                        importedstring = importedstring.Replace("'", "\\'");
                        string jsonResult = importedstring;
                        JObject obj = JObject.Parse(jsonResult);
                        GlobalJSONData.contentJObject = obj;
                    }
                    catch
                    {
                        try
                        {
                            GlobalJSONData.Type = 1;
                            importedstring = importedstring.Replace("\":\"\",\"", "\":\" \",\"");
                            importedstring = importedstring.Replace("\":\"\"", "\":\"");
                            importedstring = importedstring.Replace("\"\",\"", "\",\"");
                            importedstring = importedstring.Replace("\\", "\\\\");
                            importedstring = importedstring.Replace("'", "\\'");
                            string jsonResult = importedstring;
                            JArray obj = JArray.Parse(jsonResult);
                            GlobalJSONData.contentJArray = obj;
                        }
                        catch
                        {
                            MessageBox.Show("Please use valid JSON file");
                            GlobalJSONData.filepath = "";
                            GlobalJSONData.prevURL = "";
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
            dlg.ShowDialog();
            if (dlg.FileName.Length > 3)
            {
                try
                {
                    GlobalJSONData.filepath = dlg.FileName;
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
                StreamReader sr = new StreamReader(fs, GlobalJSONData.nowencoding);
                importedfilestring = sr.ReadToEnd();
            }
            ParseJSON(importedfilestring);
        }

        public object ReceiveMessage(PageChangeMessage action)
        {
            ViewModelLocator.Cleanup();
            switch (action.PageName)
            {
                case PageName.Main:
                    CurrentViewModel = ServiceLocator.Current.GetInstance<ContentListViewModel>();
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
    }
}