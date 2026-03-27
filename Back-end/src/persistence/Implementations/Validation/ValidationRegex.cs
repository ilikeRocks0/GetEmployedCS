using System.Text.RegularExpressions;

namespace Back_end.Persistence.Implementations.Validation;

public abstract class ValidationRegex
{
    public static readonly Regex linkRegex = new("^(https://)?(\\w+)\\.(\\w+)(\\.{1}\\w+)*(/\\S*)*$");
    public static readonly Regex emailRegex = new Regex("^[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?$");


    public static bool IsValidEmail(string? email) =>
        !string.IsNullOrWhiteSpace(email) && emailRegex.IsMatch(email.Trim());
}