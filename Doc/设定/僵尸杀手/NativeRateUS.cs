using System;

public class NativeRateUS
{
    public string title;
    public string message;
    public string yes;
    public string later;
    public string no;
    public string appLink;

    public NativeRateUS(string title, string message)
    {
        this.title = title;
        this.message = message;
        this.yes = "Rate app";
        this.later = "Later";
        this.no = "No, thanks";
    }

    public NativeRateUS(string title, string message, string yes, string later, string no)
    {
        this.title = title;
        this.message = message;
        this.yes = yes;
        this.later = later;
        this.no = no;
    }

    public void InitRateUS()
    {
        AndroidRateUsPopUp.Create(this.title, this.message, this.yes, this.later, this.no).appLink = this.appLink;
    }

    public void SetAppLink(string _appLink)
    {
        this.appLink = _appLink;
    }
}

