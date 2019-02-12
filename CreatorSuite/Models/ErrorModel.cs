
namespace CreatorSuite
{
    public class ErrorModel
    {
        public string RequestIdentifier { get; }
        public bool ShowRequestIdentifier => !string.IsNullOrEmpty(RequestIdentifier);

        public ErrorModel(string identifier)
        {
            RequestIdentifier = identifier;
        }
    }
}