namespace JSONVisualizer.Messages
{
    public enum PageName { Main, Add, View, Modify, OpenFile }

    public class PageChangeMessage
    {
        public PageName PageName { get; set; }
        public object Param { get; set; }

        public PageChangeMessage(PageName pageName)
        {
            PageName = pageName;
        }

        public PageChangeMessage(PageName pageName, object param) : this(pageName)
        {
            Param = param;
        }
    }
}