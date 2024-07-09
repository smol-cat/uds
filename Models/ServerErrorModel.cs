public class ServerErrorModel
{
    public string Error { get; set; }

    public string StackTrace { get; set; }

    public ServerErrorModel(string errorMessage, string message = null, string stackTrace = null)
    {
        Error = errorMessage;
        StackTrace = stackTrace;
    }
}

