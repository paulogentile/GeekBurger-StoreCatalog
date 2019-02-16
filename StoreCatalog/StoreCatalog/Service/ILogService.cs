namespace GeekBurger.StoreCatalog.Service
{
    public interface ILogService
    {
        void SendMessagesAsync(string message);
    }
}