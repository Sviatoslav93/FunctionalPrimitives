namespace FunctionalPrimitives.Errors;

public sealed record ValidationError(
    string Message,
    string Code = "") : Error(Message, Code)
{
    public override string Type => "validation";
}
