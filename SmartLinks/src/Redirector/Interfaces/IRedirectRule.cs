namespace Redirector;

public interface IRedirectRule
{
    DateTime Start
    {
        get; set;
    }
    DateTime End
    {
        get; set;
    }
    string Language
    {
        get; set;
    }
    string Redirect
    {
        get; set;
    }
}