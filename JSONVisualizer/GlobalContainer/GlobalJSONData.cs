using JSONVisualizer.Model;
using Newtonsoft.Json.Linq;
using System.Collections.ObjectModel;
using System.Data;
using System.IO;
using System.Text;
using System.Windows;

namespace JSONVisualizer.GlobalContainer
{
    public static class GlobalJSONData
    {
        public static DataTable data;
        public static int nextindex;
        public static string filepath;
        public static string prevURL = "";

        public static void FileSave()
        {
            using (FileStream fs = new FileStream(GlobalJSONData.filepath, FileMode.Truncate, FileAccess.ReadWrite))
            {
                StreamWriter sw = new StreamWriter(fs, Encoding.UTF8);
                foreach (DataRow nowrow in data.Rows)
                {
                    string temp = "";
                    for (int i = 0; i < data.Columns.Count; i++)
                    {
                        temp += nowrow[i] + ",";
                    }
                    sw.WriteLine(temp);
                }
                sw.Flush();
            }
            MessageBox.Show("File had been saved.");
        }

        public static void DeleteRow(int index)
        {
            GlobalJSONData.data.Rows[index].Delete();
            MessageBox.Show("Index " + index + " is deleted.");
            FileSave();
        }

        public static void WriteNewRow()
        {
            FileSave();
        }

        public static void UpdateRow(int index)
        {
            FileSave();
        }

        public static void AddNewColumn(string columnname)
        {
            FileSave();
        }

        public static void AddNewColumn()
        {
            FileSave();
        }

        /*
        ObservableCollection<TreeData> _GlobalTreeItems = new ObservableCollection<TreeData>();

        public ObservableCollection<TreeData> GlobalTreeItems

        {
            get { return _GlobalTreeItems; }

            set { _GlobalTreeItems = value; RaisePropertyChanged("GlobalTreeItems"); }
        }
        */
        public static ObservableCollection<TreeData> GlobalTreeItems;

        public static JObject contentJObject;

        public static JArray contentJArray;

        public static int Type = 0;
    }
}