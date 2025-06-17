namespace IMDBLite.API.Exceptions;

public class CustomExceptions
{
    public class InvalidNameException : Exception
    {
        public InvalidNameException(string message) : base(message)
        {
        }
    }

    public class InvalidDOBException : Exception
    {
        public InvalidDOBException(string message) : base(message)
        {
        }
    }

    public class InvalidGenderException : Exception
    {
        public InvalidGenderException(string message) : base(message)
        {
        }
    }

    public class InvalidBioException : Exception
    {
        public InvalidBioException(string message) : base(message)
        {
        }
    }

    public class InvalidActorException : Exception
    {
        public InvalidActorException(string message) : base(message)
        {
        }
    }

    public class InvalidProducerException : Exception
    {
        public InvalidProducerException(string message) : base(message)
        {
        }
    }

    public class InvalidMovieException : Exception
    {
        public InvalidMovieException(string message) : base(message)
        {
        }
    }

    public class InvalidGenreException : Exception
    {
        public InvalidGenreException(string message) : base(message)
        {
        }
    }

    public class InvalidReviewException : Exception
    {
        public InvalidReviewException(string message) : base(message)
        {
        }
    }

    public class InvalidAuthException : Exception
    {
        public InvalidAuthException(string message) : base(message)
        {
        }
    }
}