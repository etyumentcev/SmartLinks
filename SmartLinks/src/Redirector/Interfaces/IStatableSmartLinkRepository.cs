namespace Redirector;

public interface IStatableSmartLinkRepository
{
  Task<IStatableSmartLink?> Read();
}
