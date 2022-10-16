namespace Tournament.Client.Models
{
    public abstract class BaseViewModel<T> where T : class
    {
        public T Data { get; set; }
    }
}
