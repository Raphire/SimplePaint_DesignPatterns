using System.Collections.Generic;
using System.Linq;

namespace Paint.Commands
{
    /// <summary>
    /// Base class of Canvas, implementation of ICommandQueue is found here
    /// </summary>
    public abstract class CommandQueue<T> : ICommandQueue<T>
    {
        protected List<ICommand<T>> _commandsUndo = new List<ICommand<T>>();
        protected List<ICommand<T>> _commandsRedo = new List<ICommand<T>>();

        public abstract void UndoLast();

        public void UndoLast(T target)
        {
            if (_commandsUndo.Any())
            {
                var last = _commandsUndo.Last();
                _commandsUndo.Remove(last);

                last.Undo(target);
                _commandsRedo.Add(last);
            }
        }

        public abstract void RedoLast();

        public void RedoLast(T target)
        {
            if (_commandsRedo.Any())
            {
                var last = _commandsRedo.Last();
                _commandsRedo.Remove(last);

                last.Execute(target);
                _commandsUndo.Add(last);
            }
        }

        public abstract void ExecuteCommand(ICommand<T> cmd);

        public void ExecuteCommand(ICommand<T> cmd, T target)
        {
            _commandsUndo.Add(cmd);
            _commandsUndo.Last().Execute(target);
            _commandsRedo.Clear();
        }

        public void ClearUndo()
        {
            _commandsUndo.Clear();
        }

        public void ClearRedo()
        {
            _commandsRedo.Clear();
        }
    }
}
