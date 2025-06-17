using System.Text.RegularExpressions;
using IMDBLite.API.Exceptions;
using IMDBLite.API.Models.DB;

namespace IMDBLite.API.Validations;

public static class BaseValidator
{
    public static void ValidateName(string name)
    {
        if (name.Any(ch => !(char.IsLetter(ch) || char.IsWhiteSpace(ch) || ch == '.')))
            throw new CustomExceptions.InvalidNameException(
                "Name cannot contain special characters or numbers other than spaces and periods.");

        if (name.Length < 3)
            throw new CustomExceptions.InvalidNameException("Name must be at least 3 characters long.");

        if (name.Length >= 30)
            throw new CustomExceptions.InvalidNameException("Name must not be greater than 30 characters long.");

        if (name.Contains("  "))
            throw new CustomExceptions.InvalidNameException("Name cannot contain consecutive spaces.");
    }

    public static void ValidateDOB(DateOnly dob)
    {
        var today = DateOnly.FromDateTime(DateTime.Today);

        if (dob >= today)
            throw new CustomExceptions.InvalidDOBException("DOB must be in the past.");

        if (dob.Month == 2 && dob.Day == 29 && !DateTime.IsLeapYear(dob.Year))
            throw new CustomExceptions.InvalidDOBException(
                "The date February 29th is invalid for a non-leap year.");

        var age = today.Year - dob.Year;

        if (today.Month < dob.Month || (today.Month == dob.Month && today.Day < dob.Day))
            age--;

        if (age < 5)
            throw new CustomExceptions.InvalidDOBException("Age can't be less than 5.");

        if (age > 100)
            throw new CustomExceptions.InvalidDOBException("Age can't be greater than 100.");
    }

    public static void ValidateGender(Gender? gender)
    {
        if (!Enum.IsDefined(typeof(Gender), gender))
            throw new CustomExceptions.InvalidGenderException(
                "Invalid gender specified,Provide valid gender like Male,Female or Others.");
    }

    public static void ValidateBio(string bio)
    {
        if (bio.Length < 10)
            throw new CustomExceptions.InvalidBioException("Bio should be at least 10 characters long.");

        if (bio.Length > 1000)
            throw new CustomExceptions.InvalidBioException("Bio should not exceed 1000 characters.");

        if (bio.Contains("  "))
            throw new CustomExceptions.InvalidBioException("Bio cannot contain consecutive spaces.");

        if (bio == bio.ToUpper() || bio == bio.ToLower())
            throw new CustomExceptions.InvalidBioException("Bio should be a proper sentence with mixed casing.");
    }

    public static void ValidatePlot(string plot)
    {
        if (!char.IsUpper(plot[0]))
            throw new CustomExceptions.InvalidMovieException("Plot must start with a capital letter.");

        if (!plot.EndsWith("."))
            throw new CustomExceptions.InvalidMovieException("Plot must end with a period.");

        const int minLength = 6;
        const int maxLength = 100;

        if (plot.Length < minLength)
            throw new CustomExceptions.InvalidMovieException($"Plot must be at least {minLength} characters long.");

        if (plot.Length > maxLength)
            throw new CustomExceptions.InvalidMovieException($"Plot cannot exceed {maxLength} characters.");

        if (plot.Any(ch =>
                !(char.IsLetterOrDigit(ch) || char.IsWhiteSpace(ch) || ch == '.' || ch == ',' || ch == '!' ||
                  ch == '?' || ch == ';' || ch == ':')))
            throw new CustomExceptions.InvalidMovieException("Plot contains unsupported special characters.");

        if (plot.Contains("  "))
            throw new CustomExceptions.InvalidMovieException("Plot cannot contain multiple consecutive spaces.");
    }

    public static void ValidateYearOfRelease(int year)
    {
        if (year < 1700 || year > DateTime.Now.Year)
            throw new CustomExceptions.InvalidMovieException(
                "Invalid year of release.The year should be between 1700 and the current year.");
    }

    public static void ValidateNameLength(string name)
    {
        if (name.Length < 2 || name.Length > 100)
            throw new CustomExceptions.InvalidGenreException("Genre name must be between 2 and 100 characters.");
    }

    public static void ValidateSpecialCharacters(string name)
    {
        if (!Regex.IsMatch(name, @"^[a-zA-Z\s'-]+$"))
            throw new CustomExceptions.InvalidGenreException(
                "Genre name can only contain letters, spaces, apostrophes, and hyphens.");
    }

    public static void ValidateMessageLength(string msg)
    {
        if (msg.Length < 10 || msg.Length > 100)
            throw new CustomExceptions.InvalidReviewException(
                "Review message must be between 10 and 1000 characters.");
    }


    public static void ValidateMovieName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new CustomExceptions.InvalidMovieException("Movie name cannot be empty.");
        if (name.Any(ch => !(char.IsLetterOrDigit(ch) || char.IsWhiteSpace(ch))))
            throw new CustomExceptions.InvalidMovieException("Movie name cannot contain special characters.");
        if (name.Length > 30)
            throw new CustomExceptions.InvalidMovieException("Movie name cannot be longer than 30 characters.");
    }
}