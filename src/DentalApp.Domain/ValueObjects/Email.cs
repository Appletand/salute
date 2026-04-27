namespace DentalApp.Domain.ValueObjects;

public record Email
{
    public string Value { get; }
    public Email(string value)
    {
        if (string.IsNullOrWhiteSpace(value) || !value.Contains("@"))
            throw new ArgumentException("Invalid email");
        Value = value;
    }
}