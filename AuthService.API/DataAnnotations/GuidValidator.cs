using System.ComponentModel.DataAnnotations;

namespace AuthService.API.DataAnnotations;

public class GuidValidator : ValidationAttribute
{
    public override bool IsValid(object? value)
    {
        if (value == null || string.IsNullOrEmpty(value.ToString()))
            return true;
        
        return Guid.TryParse(value.ToString(), out _);
    }
}