using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Linq;

namespace dotnetfeatures.Models
{
 // Root form model must be declared in a C# class file and annotated with [ValidatableType]
 [ValidatableType]
 public class RegistrationModel : IValidatableObject
 {
 [Required]
 [MinLength(3)]
 [MaxLength(20)]
 [ReservedUserName]
 public string? UserName { get; set; }

 [Required]
 [EmailAddress]
 public string? Email { get; set; }

 [Required]
 [MinLength(8)]
 public string? Password { get; set; }

 [Required]
 [Compare(nameof(Password), ErrorMessage = "Passwords must match.")]
 public string? ConfirmPassword { get; set; }

 [Required]
 [Range(18,120)]
 public int? Age { get; set; }

 // Nested complex type (validated automatically)
 [Required]
 public Address Address { get; set; } = new();

 // Collection of complex types (each item validated automatically)
 public List<PhoneEntry> Phones { get; set; } = new();

 public string? NonValidatedNote { get; set; }

 public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
 {
 if (!string.IsNullOrEmpty(Password))
 {
 bool hasUpper = Password.Any(char.IsUpper);
 bool hasDigit = Password.Any(char.IsDigit);
 bool hasSymbol = Password.Any(ch => !char.IsLetterOrDigit(ch));
 if (!(hasUpper && hasDigit && hasSymbol))
 {
 yield return new ValidationResult(
 "Password must contain an upper-case letter, a digit, and a symbol.", new[] { nameof(Password) });
 }
 }
 if (Phones.Count ==0)
 {
 yield return new ValidationResult("At least one phone number is required.", new[] { nameof(Phones) });
 }
 }
 }

 public class Address
 {
 [Required]
 [MaxLength(100)]
 public string? Street { get; set; }

 [Required]
 [MaxLength(60)]
 public string? City { get; set; }

 [Required]
 [MaxLength(2)]
 public string? State { get; set; }

 [Required]
 [RegularExpression(@"^[0-9]{5}$", ErrorMessage = "Postal code must be5 digits.")]
 public string? PostalCode { get; set; }
 }

 public class PhoneEntry
 {
 [Required]
 [Phone]
 public string? Number { get; set; }

 [Required]
 [RegularExpression(@"^(Mobile|Home|Work)$", ErrorMessage = "Type must be Mobile, Home, or Work.")]
 public string? Type { get; set; }
 }

 [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
 public class ReservedUserNameAttribute : ValidationAttribute
 {
 private static readonly HashSet<string> Reserved = new(StringComparer.OrdinalIgnoreCase)
 { "admin", "root", "system" };
 protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
 {
 if (value is string s && Reserved.Contains(s))
 {
 return new ValidationResult($"'{s}' is a reserved user name.");
 }
 return ValidationResult.Success;
 }
 }
}
