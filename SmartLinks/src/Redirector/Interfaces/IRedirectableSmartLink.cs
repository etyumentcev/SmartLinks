namespace Redirector;

public interface IRedirectableSmartLink
{
    bool IsLanguageAccepted(string language);

    bool IsInTime(DateTime start, DateTime end);
}