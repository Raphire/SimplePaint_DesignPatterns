namespace Paint.Model
{
    public interface IDecorator<T>
    {
        /// <summary>
        /// The object that this decorator decorates
        /// </summary>
        T Target { get; } 

        T EndPoint { get; }
    }
}
