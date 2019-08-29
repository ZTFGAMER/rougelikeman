using System;

public class NativeMessage
{
    public NativeMessage(string title, string message)
    {
        this.init(title, message, "Ok");
    }

    public NativeMessage(string title, string message, string ok)
    {
        this.init(title, message, ok);
    }

    private void init(string title, string message, string ok)
    {
        AndroidMessage.Create(title, message, ok);
    }
}

