using System;
using System.Collections.Generic;
using System.Text;

namespace CommandPattern.Business.Commands
{
    public class CommandManager
    {
        private Stack<ICommand> commands = new Stack<ICommand>();   // a LIFO data structure that allows us to undo

        public void Invoke(ICommand command)
        {
            if (command.CanExecute())
            {
                commands.Push(command);
                command.Execute();
            }
        }

        public void Undo()
        {
            while (commands.Count > 0)
            {
                var command = commands.Pop();
                command.Undo();
            }
        }
    }
}
