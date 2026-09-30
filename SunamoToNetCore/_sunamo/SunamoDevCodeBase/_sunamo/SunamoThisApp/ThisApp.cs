namespace SunamoToNetCore._sunamo.SunamoDevCodeBase._sunamo.SunamoThisApp;

internal class ThisApp
{

    internal static bool Check;

    /// <summary>
    /// Error.
    /// </summary>
    internal static void Error(string message, params string[] args)
    {
        SetStatus(TypeOfMessageShared.Error, message, args);
    }

    /// <summary>
    /// Set status.
    /// </summary>
    internal static void SetStatus(TypeOfMessageShared messageType, string status, params string[] args)
    {
        var formattedMessage = string.Format(status, args);
        if (formattedMessage.Trim() != string.Empty)
        {
            if (StatusSetted == null)
            {
                // For unit tests - no handler attached
            }
            else
            {
                StatusSetted(messageType, formattedMessage);
            }
        }
    }

    internal static event Action<TypeOfMessageShared, string>? StatusSetted;
}