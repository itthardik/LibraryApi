namespace LMS2.Utility
{
    /// <summary>
    /// Custom Exception class
    /// </summary>
    /// <remarks>
    /// constructor for custom exception
    /// </remarks>
    /// <param name="message"></param>
    public class CustomException(string message) : Exception(message)
    {

    }
}
