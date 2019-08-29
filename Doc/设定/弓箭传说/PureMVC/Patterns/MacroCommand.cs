namespace PureMVC.Patterns
{
    using PureMVC.Interfaces;
    using System;
    using System.Collections.Generic;

    public class MacroCommand : Notifier, ICommand, INotifier
    {
        private readonly IList<object> m_subCommands;

        public MacroCommand()
        {
            this.m_subCommands = new List<object>();
            this.InitializeMacroCommand();
        }

        public MacroCommand(IEnumerable<ICommand> commands)
        {
            this.m_subCommands = new List<object>(commands);
            this.InitializeMacroCommand();
        }

        public MacroCommand(IEnumerable<object> commandCollection)
        {
            this.m_subCommands = new List<object>(commandCollection);
            this.InitializeMacroCommand();
        }

        public MacroCommand(IEnumerable<Type> types)
        {
            this.m_subCommands = new List<object>(types);
            this.InitializeMacroCommand();
        }

        protected void AddSubCommand(ICommand command)
        {
            this.m_subCommands.Add(command);
        }

        protected void AddSubCommand(Type commandType)
        {
            this.m_subCommands.Add(commandType);
        }

        public void Execute(INotification notification)
        {
            while (this.m_subCommands.Count > 0)
            {
                Type type = this.m_subCommands[0] as Type;
                if (type != null)
                {
                    object obj2 = Activator.CreateInstance(type);
                    if (obj2 is ICommand)
                    {
                        ICommand command = (ICommand) obj2;
                        command.InitializeNotifier(base.MultitonKey);
                        command.Execute(notification);
                    }
                }
                else
                {
                    ICommand command2 = this.m_subCommands[0] as ICommand;
                    if (command2 != null)
                    {
                        command2.InitializeNotifier(base.MultitonKey);
                        command2.Execute(notification);
                    }
                }
                this.m_subCommands.RemoveAt(0);
            }
        }

        protected virtual void InitializeMacroCommand()
        {
        }
    }
}

