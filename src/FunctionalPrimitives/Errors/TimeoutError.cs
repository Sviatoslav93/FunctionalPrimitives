namespace FunctionalPrimitives.Errors;

public sealed record TimeoutError(
    string Message,
    string Code = "") : Error(Message, Code)
{
    public override string Type => "timeout";
}
