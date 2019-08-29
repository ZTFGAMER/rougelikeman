namespace PureMVC.Interfaces
{
    using System;

    public interface INotification
    {
        string ToString();

        string Name { get; }

        object Body { get; set; }

        string Type { get; set; }

        string FileName { get; }

        string FuncName { get; }

        int LineNumber { get; }
    }
}

