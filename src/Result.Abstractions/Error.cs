namespace Result.Abstractions;

public class Error(string message)
{
    public string Message { get; } = message;

    public string? Code { get; init; }

    public Exception? Exception { get; init; }

    public Dictionary<string, object>? Metadata { get; init; }

    public static Error Create(string message) => new(message);

    public static Error Create(string message, string code) => new(message) { Code = code };

    public static Error Create(string message, Exception exception) => new(message) { Exception = exception };

    public static Error Create(string message, string code, Exception exception) => new(message) { Code = code, Exception = exception };

    public Error WithCode(string code) => new(Message) { Code = code, Exception = Exception, Metadata = Metadata };

    public Error WithException(Exception exception) => new(Message) { Code = Code, Exception = exception, Metadata = Metadata };

    public Error WithMetadata(string key, object value)
    {
        var metadata = Metadata?.ToDictionary(kvp => kvp.Key, kvp => kvp.Value) ?? new Dictionary<string, object>();
        metadata[key] = value;
        return new(Message) { Code = Code, Exception = Exception, Metadata = metadata };
    }

    public override string ToString()
    {
        return Message;
    }
}
