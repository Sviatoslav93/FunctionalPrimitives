namespace FunctionalPrimitives.Errors;

public sealed record InvalidStateError(
    string Message,
    string Code = "") : Error(Message, Code)
{
    public override string Type => "invalid-state";
}
