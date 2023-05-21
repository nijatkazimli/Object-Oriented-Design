using OOD_PROJECT;
using System.Collections;
using System.Security.AccessControl;
using System.Xml;
using System.Xml.Linq;

namespace OOD_UML_FINAL
{
    public interface ICommand
    {
        void ArgumentsSetter(string[] arguments);
        void Execute();
        ICommand Clone();
    }
    public class ListCommand : ICommand
    {
        private string[] arguments;
        private readonly Dictionary<string, ICollection> collections;

        public void ArgumentsSetter(string[] arguments) 
        {
            this.arguments = arguments;
        }

        public ListCommand(Dictionary<string, ICollection> collections) 
        {
            this.collections = collections;
        }

        public void Execute() 
        {
            if(arguments.Length != 1) 
            {
                ConsoleColor previousColor = Console.ForegroundColor;
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Usage: list <class_name>");
                Console.ForegroundColor = previousColor;
                return;
            }
            string className = arguments[0];
            if (collections.TryGetValue(className, out ICollection collection))
            {
                foreach(IDisplay obj in collection) 
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    obj.Display();
                }
                Console.ForegroundColor = ConsoleColor.White;
            }
            else 
            {
                ConsoleColor previousColor = Console.ForegroundColor;
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"No objects of type {className} found.");
                Console.ForegroundColor = previousColor;
            }
        }

        public override string ToString()
        {
            string output = "list";
            foreach(string argument in arguments)
                output += (" " + argument);
            return output;
        }

        public ICommand Clone()
        {
            return new ListCommand(collections);
        }
    }
    public class FindCommand : ICommand
    {
        private readonly Dictionary<string, IFilter> filters;
        private string[] arguments;

        public void ArgumentsSetter(string[] arguments)
        {
            this.arguments = arguments;
        }

        public FindCommand(Dictionary<string, IFilter> filters)
        {
            this.filters = filters;
        }

        public void Execute()
        {
            if(arguments.Length < 2) 
            {
                ConsoleColor previousColor = Console.ForegroundColor;
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Usage: find <name_of_the_class> [<requirement> …]");
                Console.ForegroundColor = previousColor;
                return;
            }
            if(filters.TryGetValue(arguments[0], out IFilter filter))
            {
                filter.Filter(arguments, true);
            }
            else 
            {
                ConsoleColor previousColor = Console.ForegroundColor;
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Class `{arguments[0]}` is not found");
                Console.ForegroundColor = previousColor;
            }
        }
        public override string ToString()
        {
            string output = "find";
            foreach (string argument in arguments)
                output += (" " + argument);
            return output;
        }

        public ICommand Clone()
        {
            return new FindCommand(filters);
        }
    }
    public class ExitCommand : ICommand
    {
        private string[] arguments;

        public void ArgumentsSetter(string[] arguments)
        {
            this.arguments = arguments;
        }
        public void Execute()
        {
            if (arguments.Length != 0)
            {
                ConsoleColor previousColor = Console.ForegroundColor;
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("No command found. Did you mean `exit`?");
                Console.ForegroundColor = previousColor;
                return;
            }
            Environment.Exit(0);
        }
        public override string ToString()
        {
            string output = "exit";
            foreach (string argument in arguments)
                output += (" " + argument);
            return output;
        }

       public ICommand Clone()
        {
            return new ExitCommand();
        }
    }
    
    public class ClearCommand : ICommand 
    {
        private string[] arguments;

        public void ArgumentsSetter(string[] arguments)
        {
            this.arguments = arguments;
        }
        public void Execute() 
        {
            if(arguments.Length != 0) 
            {
                ConsoleColor previousColor = Console.ForegroundColor;
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("No command found. Did you mean `clear`?");
                Console.ForegroundColor = previousColor;
                return;
            }
            Console.Clear();
        }
        public override string ToString()
        {
            string output = "clear";
            foreach (string argument in arguments)
                output += (" " + argument);
            return output;
        }
        public ICommand Clone()
        {
            return new ClearCommand();
        }
    }

    public class AddCommand : ICommand 
    {
        private Dictionary<string, IAdder> adders;

        private string[] arguments;

        public void ArgumentsSetter(string[] arguments)
        {
            this.arguments = arguments;
        }

        public AddCommand(Dictionary<string, IAdder> adders) 
        {
            this.adders = adders;
        }

        public void Execute() 
        {
            if(arguments.Length != 2) 
            {
                ConsoleColor previousColor = Console.ForegroundColor;
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Usage: add <class_name> <representation>");
                Console.ForegroundColor = previousColor;
                return;
            }
            if (arguments[1].CompareTo("base") != 0 && arguments[1].CompareTo("secondary") != 0)
            {
                ConsoleColor previousColor = Console.ForegroundColor;
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("representation should be either `base` or `secondary`");
                Console.ForegroundColor = previousColor;
                return;
            }
            if (adders.TryGetValue(arguments[0], out IAdder adder))
            {
                adder.Add(arguments);
            }
            else
            {
                ConsoleColor previousColor = Console.ForegroundColor;
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Class `{arguments[0]}` is not found");
                Console.ForegroundColor = previousColor;
            }
        }
        public override string ToString()
        {
            string output = "add";
            foreach (string argument in arguments)
                output += (" " + argument);
            return output;
        }
        public ICommand Clone() 
        {
            return new AddCommand(adders);
        }
    }
   

    public class EditCommand : ICommand 
    {
        private readonly Dictionary<string, IEditor> editors;
        private string[] arguments;

        public EditCommand(Dictionary<string, IEditor> editors)
        {
            this.editors = editors;
        }

        public void ArgumentsSetter(string[] arguments)
        {
            this.arguments = arguments;
        }

        public void Execute()
        {
            if (arguments.Length < 2)
            {
                ConsoleColor previousColor = Console.ForegroundColor;
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Usage: edit <name_of_the_class> [<requirement> …]");
                Console.ForegroundColor = previousColor;
                return;
            }
            if (editors.TryGetValue(arguments[0], out IEditor editor)) 
            {
                editor.Edit(arguments);
            }
            else 
            {
                ConsoleColor previousColor = Console.ForegroundColor;
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Class `{arguments[0]}` is not found");
                Console.ForegroundColor = previousColor;
            }
        }
        public override string ToString()
        {
            string output = "edit";
            foreach (string argument in arguments)
                output += (" " + argument);
            return output;
        }

        public ICommand Clone() 
        {
            return new EditCommand(editors);
        }
    }

    public class DeleteCommand : ICommand 
    {
        private readonly Dictionary<string, IDelete> deletes;

        public DeleteCommand(Dictionary<string, IDelete> deletes)
        {
            this.deletes = deletes;
        }

        private string[] arguments;

        public void ArgumentsSetter(string[] arguments)
        {
            this.arguments = arguments;
        }

        public void Execute()
        {
            if(arguments.Length < 2) 
            {
                ConsoleColor previousColor = Console.ForegroundColor;
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Usage : delete <name_of_the_class> [<requirement> …]");
                Console.ForegroundColor = previousColor;
            }
            else if (deletes.TryGetValue(arguments[0], out IDelete delete)) 
            {
                delete.Delete(arguments);
                ConsoleColor previousColor = Console.ForegroundColor;
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Delete was successful");
                Console.ForegroundColor = previousColor;
            }
            else 
            {
                ConsoleColor previousColor = Console.ForegroundColor;
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Class {arguments[0]} is not found!");
                Console.ForegroundColor = previousColor;
            }
        }
        public override string ToString()
        {
            string output = "delete";
            foreach (string argument in arguments)
                output += (" " + argument);
            return output;
        }

        public ICommand Clone() 
        {
            return new DeleteCommand(deletes);
        }
    }

    public abstract class QueueCommand : ICommand
    {
        protected string[] arguments;
        protected Queue<ICommand> command_queue;
        public void ArgumentsSetter(string[] arguments) 
        {
            this.arguments = arguments;
        }
        public abstract void Execute();
        public abstract ICommand Clone();
    }

    public class QueuePrintCommand : QueueCommand
    {
        public QueuePrintCommand(Queue<ICommand> command_queue) 
        {
            this.command_queue = command_queue;
        }
        public override ICommand Clone()
        {
            throw new NotImplementedException(); // Because we do not need to clone QueueCommands as they are executed immediately, and we do not need several instances of them.
        }

        public override void Execute()
        {
            if (arguments.Length != 0)
            {
                ConsoleColor previousColor = Console.ForegroundColor;
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Usage: queue print");
                Console.ForegroundColor = previousColor;
            }
            else if (command_queue.Count == 0)
            {
                ConsoleColor previousColor = Console.ForegroundColor;
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("There is no command to print");
                Console.ForegroundColor = previousColor;
            }
            else
            {
                foreach (ICommand command in command_queue)
                {
                    ConsoleColor previousColor = Console.ForegroundColor;
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine(command);
                    Console.ForegroundColor = previousColor;
                }
            }
        }
    }

    public class QueueCommitCommand : QueueCommand 
    {
        public QueueCommitCommand(Queue<ICommand> command_queue) 
        {
            this.command_queue = command_queue;
        }

        public override ICommand Clone()
        {
            throw new NotImplementedException();
        }

        public override void Execute()
        {
            if (arguments.Length != 0)
            {
                ConsoleColor previousColor = Console.ForegroundColor;
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Usage: queue commit");
                Console.ForegroundColor = previousColor;
                return;
            }
            if (command_queue.Count == 0) 
            {
                ConsoleColor previousColor = Console.ForegroundColor;
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("There is no command to execute");
                Console.ForegroundColor = previousColor;
                return;
            }
            foreach (ICommand command in command_queue)
            {
                ConsoleColor previousColor = Console.ForegroundColor;
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.WriteLine(command);
                Console.ForegroundColor = previousColor;

                command.Execute();
                Console.WriteLine();
            }
            command_queue.Clear();
        }
    }

    public class QueueExportCommand : QueueCommand
    {
        public QueueExportCommand(Queue<ICommand> command_queue) 
        {
            this.command_queue = command_queue;
        }
        public override ICommand Clone()
        {
            throw new NotImplementedException();
        }
        
        public override void Execute()
        {
            if(arguments.Length == 0 || arguments.Length > 2) 
            {
                ConsoleColor previousColor = Console.ForegroundColor;
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Usage: queue export {filename} [format]");
                Console.ForegroundColor = previousColor;
                return;
            }
            string filename = arguments[0];
            string format = "XML";
            if (arguments[1] is not null) 
            {
                format = arguments[1];
            }
            if (format == "plaintext") 
            {
                using (StreamWriter sw = new StreamWriter(filename + ".txt")) 
                {
                    foreach(ICommand command in command_queue) 
                    {
                        sw.WriteLine(command.ToString());
                    }
                }
            }
            else 
            {
                using (XmlWriter xw = XmlWriter.Create(filename + ".xml")) 
                {
                    xw.WriteStartDocument();
                    xw.WriteStartElement("Commands");

                    foreach(ICommand command in command_queue) 
                    {
                        xw.WriteStartElement("Command");
                        xw.WriteString(command.ToString());
                        xw.WriteEndElement();
                    }
                }
            }
        }
    }

    public class QueueDismissCommand : QueueCommand
    {

        public QueueDismissCommand(Queue<ICommand> command_queue)
        {
            this.command_queue = command_queue;
        }
        public override ICommand Clone()
        {
            throw new NotImplementedException();
        }

        public override void Execute()
        {
            if (arguments.Length != 0)
            {
                ConsoleColor previousColor = Console.ForegroundColor;
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Usage: queue dismiss");
                Console.ForegroundColor = previousColor;
            }
            else if (command_queue.Count == 0)
            {
                ConsoleColor previousColor = Console.ForegroundColor;
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("There is no command to dismiss");
                Console.ForegroundColor = previousColor;
            }
            else
            {
                command_queue.Clear();
            }

        }
    }

    public class QueueLoadCommand : QueueCommand 
    {
        private readonly Dictionary<string, ICommand> commands;
        public QueueLoadCommand(Queue<ICommand> command_queue, Dictionary<string, ICommand> commands) 
        {
            this.command_queue = command_queue;
            this.commands = commands;
        }

        public override ICommand Clone()
        {
            throw new NotImplementedException();
        }

        public override void Execute()
        {
            if (arguments.Length != 1) 
            {
                ConsoleColor previousColor = Console.ForegroundColor;
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Usage: queue load {filename}");
                Console.ForegroundColor = previousColor;
            }
            else 
            {
                string file_name = arguments[0];
                string file_extension = Path.GetExtension(file_name);
                if (file_extension == ".txt") 
                    ParseCommandFromPlainText(file_name);
                else
                    ParseCommandFromXML(file_name);
            }
        }

        private void ParseCommandFromPlainText(string file_name)
        {
            try
            {
                string[] lines = File.ReadAllLines(file_name);

                foreach (string line in lines)
                {
                    string trimmedLine = line.Trim();
                    if (string.IsNullOrEmpty(trimmedLine))
                        continue;

                    string[] tokens = trimmedLine.Split(" ");
                    string command_string = tokens[0];
                    string[] arguments = tokens.Skip(1).ToArray();
                    if (command_string.StartsWith("queue"))
                    {
                        command_string += (" " + tokens[1]);
                        arguments = arguments.Skip(1).ToArray();
                    }
                    if (commands.TryGetValue(command_string, out ICommand command))
                    {
                        ICommand clonedCommand = command.Clone();
                        clonedCommand.ArgumentsSetter(arguments);
                        command_queue.Enqueue(clonedCommand);
                    }
                    else
                    {
                        ConsoleColor previousColor = Console.ForegroundColor;
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Unrecognized command found on the file");
                        Console.ForegroundColor = previousColor;
                    }
                }
            }
            catch (Exception ex)
            {
                ConsoleColor previousColor = Console.ForegroundColor;
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(ex.Message);
                Console.ForegroundColor = previousColor;
            }
        }

        private void ParseCommandFromXML(string xml)
        {
            try
            {
                XDocument doc = XDocument.Load(xml);
                XElement rootElement = doc.Root;

                foreach(XElement commandElement in rootElement.Elements("Command"))
                {
                    string line = commandElement.Value.Trim();
                    string[] tokens = line.Split(" ");
                    string command_string = tokens[0];
                    string[] arguments = tokens.Skip(1).ToArray();
                    if (command_string.StartsWith("queue"))
                    {
                        command_string += (" " + tokens[1]);
                        arguments = arguments.Skip(1).ToArray();
                    }
                    if (commands.TryGetValue(command_string, out ICommand command))
                    {
                        ICommand clonedCommand = command.Clone();
                        clonedCommand.ArgumentsSetter(arguments);
                        command_queue.Enqueue(clonedCommand);
                    }
                    else
                    {
                        ConsoleColor previousColor = Console.ForegroundColor;
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Unrecognized command found on the file");
                        Console.ForegroundColor = previousColor;
                    }
                }
            }
            catch (Exception ex)
            {
                ConsoleColor previousColor = Console.ForegroundColor;
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(ex.Message);
                Console.ForegroundColor = previousColor;
            }
        }
    }

    public class CommandInvoker 
    {
        public readonly Dictionary<string, ICommand> commands;
        private Queue<ICommand> command_queue;

        public CommandInvoker() 
        {
            commands = new Dictionary<string, ICommand>();
        }

        public void RegisterCommand(string commandName, ICommand command)
        {
            commands[commandName] = command;
        }

        public void RegisterQueue(Queue<ICommand > command_queue) 
        {
            this.command_queue = command_queue;
        }

        public void ExecuteCommand(string input) 
        {
            string[] tokens = input.Split(" ");
            string command_string = tokens[0];
            string[] arguments = tokens.Skip(1).ToArray();
            if (command_string.StartsWith("queue"))
            {
                command_string += (" " + tokens[1]);
                arguments = arguments.Skip(1).ToArray();
            }

            if (command_string.StartsWith("queue") || command_string == "exit" || command_string == "clear")
            {
                if (commands.TryGetValue(command_string, out ICommand com))
                {
                    com.ArgumentsSetter(arguments);
                    com.Execute();
                }
            }
            else if (!command_string.StartsWith("queue") && command_string != "exit" && command_string != "clear")
            {
                if(commands.TryGetValue(command_string, out ICommand com)) 
                {
                    ICommand clonedCommand = com.Clone();
                    clonedCommand.ArgumentsSetter(arguments);
                    command_queue.Enqueue(clonedCommand);
                }
            }

            else if(input == string.Empty) 
            {
                Console.Write("");
            }
            else 
            {
                ConsoleColor previousColor = Console.ForegroundColor;
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Command {command_string} not recognized.");
                Console.ForegroundColor = previousColor;
            }
        }
    }

}
