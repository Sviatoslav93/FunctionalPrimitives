using FunctionalPrimitives.Extensions;
using static FunctionalPrimitives.Result;

namespace FunctionalPrimitives.Example.ResultExamples.Validation;

public partial class ResultExample
{
    public static async Task Do()
    {
        var test = GetUserById(1)
            .BindAsync(user => user);

        var result = await (from user in GetUserById(1)
                            select user);
    }

    public static async Task<Result<User>> GetUserById(int id)
    {
        await Task.Yield();

        return Success(new User(id, "John", "some@gmail.com", DateTime.Now));
    }

    public static async Task<Result<Bill>> GetBillById(int id)
    {
        await Task.Yield();

        return Success(new Bill(id, 1, 100));
    }

    public record User(int Id, string Name, string Email, DateTime CreatedAt);

    public record Bill(int Id, int UserId, decimal Amount);
}
