using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Paint
{
    /// <summary>
    /// Interface declaring behavior used in CommandPattern
    /// </summary>
    /// 
    /// <typeparam name="T">Baseclass of the commands the CommandQueue supports</typeparam>
    public interface ICommandQueue<T> where T : ICommand
    {
        // Undo last command
        void UndoLast();

        // Redo last undone command
        void RedoLast();

        // Execute a new command
        void ExecuteCommand(T cmd);
    }
}
