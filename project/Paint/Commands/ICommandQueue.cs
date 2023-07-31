namespace Paint.Commands
{
    /// <summary>
    /// Interface declaring behavior used in CommandPattern
    /// </summary>
    /// <typeparam name="T">
    /// The class of the root object that this type of CommandQueue works with.
    /// this should be equivalent to the generic parameter T of the commands it will manage.
    /// </typeparam>
    public interface ICommandQueue<T>
    {
        // Undo last command
        void UndoLast(T target);

        // Redo last undone command
        void RedoLast(T target);

        // Execute a new command
        void ExecuteCommand(ICommand<T> cmd, T target);
    }
}
