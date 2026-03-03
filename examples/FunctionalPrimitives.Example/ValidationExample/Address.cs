using FunctionalPrimitives.Extensions;

namespace FunctionalPrimitives.Example;

public class Address
{
    private Address(string street, string city, string state, string zipCode)
    {
        Street = street;
        City = city;
        State = state;
        ZipCode = zipCode;
    }

    public string Street { get; }

    public string City { get; }

    public string State { get; }

    public string ZipCode { get; }

    public static Result<Address> Create(string street, string city, string state, string zipCode)
    {
        var validationResult =
            from streetValidation in street
                .Should(x => !string.IsNullOrEmpty(x), new Error("street is required", "street"))
                .Should(x => x.Length <= 500, new Error("street is too long", "street"))
            from cityValidation in city
                .Should(x => !string.IsNullOrEmpty(x), new Error("city is required", "city"))
                .Should(x => x.Length <= 500, new Error("city is too long", "city"))
            from stateValidation in state
                .Should(x => !string.IsNullOrEmpty(x), new Error("state is required", "state"))
                .Should(x => x.Length <= 500, new Error("state is too long", "state"))
            from zipValidation in zipCode
                .Should(x => !string.IsNullOrEmpty(x), new Error("zip code is required", "zipCode"))
                .Should(x => x.Length == 10, new Error("zip code is not correct", "zipCode"))
            select Result.Success(None.Value);

        if (validationResult.IsFailure)
        {
            return validationResult.Errors.ToArray();
        }

        return new Address(street, city, state, zipCode);
    }
}
