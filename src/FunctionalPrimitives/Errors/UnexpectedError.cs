namespace FunctionalPrimitives.Errors;

public sealed record UnexpectedError(
    string Message,
    string Code = "") : Error(Message, Code)
{
    public override string Type => "unexpected";

    public static implicit operator UnexpectedError(Exception ex) => new(ex.Message, ex.GetType().Name);
}
