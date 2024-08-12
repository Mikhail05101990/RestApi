namespace WebApi.Exceptions
{
    public class SecureException: Exception
    {
        public SecureException(string Message):base(Message) 
        {
        }
    }
}
