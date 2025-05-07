using System.ComponentModel.DataAnnotations;

public class GreaterThanAttribute : ValidationAttribute
{
    private readonly string _comparisonProperty;

    public GreaterThanAttribute(string comparisonProperty)
    {
        _comparisonProperty = comparisonProperty;
    }

    protected override ValidationResult IsValid(object? value, ValidationContext validationContext)
    {
        var property = validationContext.ObjectType.GetProperty(_comparisonProperty);

        if (property == null)
        {
            return new ValidationResult($"Property {_comparisonProperty} not found.");
        }

        var comparisonValue = property.GetValue(validationContext.ObjectInstance);

        return (int)value <= (int)comparisonValue ? new ValidationResult($"{validationContext.DisplayName} must be greater than {_comparisonProperty}.") : ValidationResult.Success;
    }
}