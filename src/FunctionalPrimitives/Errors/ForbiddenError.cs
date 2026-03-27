namespace FunctionalPrimitives.Errors;

public sealed record ForbiddenError(
    string Message,
    string Code = "") : Error(Message, Code)
{
    public override string Type => "forbidden";
}
