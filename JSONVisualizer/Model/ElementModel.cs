using GalaSoft.MvvmLight;

namespace JSONVisualizer.Model
{
    public class ElementModel : ViewModelBase
    {
        public int Index { get; set; }
        public string main { get; set; }
        public string sub { get; set; }
        public string comment { get; set; }
    }
}