namespace FfgTestTask.Exceptions
{
    /// <summary>
    /// Общий объект исключений для приложения FfgTestTask
    /// </summary>
    public class FfgTestTaskException : Exception
    {
        public FfgTestTaskException(string? message) : base(message)
        {
        }
    }
}
