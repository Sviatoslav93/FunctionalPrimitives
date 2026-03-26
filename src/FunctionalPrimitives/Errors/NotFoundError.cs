namespace FunctionalPrimitives.Errors;

public sealed record NotFoundError(
    string Message,
    string Code = "") : Error(Message, Code)
{
    public override string Type => "not-found";
}
