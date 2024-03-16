using Cinema.Application.Utils.Errors;

namespace Cinema.Application.Utils;

public class CinemaError : Exception
{
    public CinemaErrorType Type { get; private set; }

    public CinemaError(CinemaErrorType type, string message) : base(message)
    {
        Type = type;
    }
}
