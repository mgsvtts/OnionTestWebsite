namespace Domain.Exceptions
{
    public sealed class NumberAndProviderIdIsNotUniqueException : BadRequestException
    {
        public NumberAndProviderIdIsNotUniqueException(string sameOrderNumber)
            : base($"\"{sameOrderNumber}\" already exists, order number and order provider id must be unique")
        { }
    }
}