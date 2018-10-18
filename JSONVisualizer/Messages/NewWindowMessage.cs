namespace JSONVisualizer.Messages
{
    public enum Type { GetURL, SourceView }

    public class NewWindowMessage
    {
        public Type Type { get; set; }

        public NewWindowMessage(Type type)
        {
            Type = type;
        }
    }
}