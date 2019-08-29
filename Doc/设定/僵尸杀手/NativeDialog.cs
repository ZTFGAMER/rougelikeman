using System;

public class NativeDialog
{
    private string title;
    private string message;
    private string yesButton;
    private string noButton;
    public string urlString;

    public NativeDialog(string title, string message)
    {
        this.title = title;
        this.message = message;
        this.yesButton = "Yes";
        this.noButton = "No";
    }

    public NativeDialog(string title, string message, string yesButtonText, string noButtonText)
    {
        this.title = title;
        this.message = message;
        this.yesButton = yesButtonText;
        this.noButton = noButtonText;
    }

    public void init()
    {
        AndroidDialog.Create(this.title, this.message, this.yesButton, this.noButton).urlString = this.urlString;
    }

    public void SetUrlString(string urlString)
    {
        this.urlString = urlString;
    }
}

