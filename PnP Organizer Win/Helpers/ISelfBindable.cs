namespace PnPOrganizer.Core
{
    public interface ISelfBindable<T> where T : class
    {
        public T BindableInstance { get; }
    }
}
