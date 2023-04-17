namespace PnPOrganizer.Helpers
{
    public interface ISelfBindable<T> where T : class
    {
        public T BindableInstance { get; }
    }
}
