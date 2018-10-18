using JSONVisualizer.Model;
using Newtonsoft.Json.Linq;
using System.Collections.ObjectModel;
using System.Text;

namespace JSONVisualizer.GlobalContainer
{
    public static class GlobalJSONData
    {
        public static string filepath;
        public static string prevURL = "";

        public static ObservableCollection<TreeNode> GlobalTreeItems;

        public static JObject contentJObject;

        public static JArray contentJArray;

        public static int Type = 0;

        public static Encoding nowencoding = Encoding.UTF8;
    }
}