namespace FunctionalPrimitives.Errors;

public sealed record ConflictError(
    string Message,
    string Code = "") : Error(Message, Code)
{
    public override string Type => "conflict";
}
