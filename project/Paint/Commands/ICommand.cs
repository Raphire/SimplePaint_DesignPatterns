namespace Paint.Commands
{
    /// <summary>
    /// Interface declaring Undo/Redo functionality as seen in the Command pattern
    /// </summary>
    /// <typeparam name="T">
    /// The class of the root object that this type of command works with.
    /// this should be equivalent to the generic parameter T of the 
    /// CommandQueue that manages commands of this type.
    /// </typeparam>
    public interface ICommand<T>
    {
        void Execute(T target);
        void Undo(T target);
    }
}
