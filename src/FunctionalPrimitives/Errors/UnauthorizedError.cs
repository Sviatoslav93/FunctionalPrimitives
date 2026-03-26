namespace FunctionalPrimitives.Errors;

public sealed record UnauthorizedError(
    string Message,
    string Code = "") : Error(Message, Code)
{
    public override string Type => "unauthorized";
}
