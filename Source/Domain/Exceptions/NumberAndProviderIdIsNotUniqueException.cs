namespace Domain.Exceptions
{
    public sealed class NumberAndProviderIdIsNotUniqueException : BadRequestException
    {
        public NumberAndProviderIdIsNotUniqueException(string sameOrderNumber)
            : base($"The order with number: {sameOrderNumber} already exists, order number and order provider id must be unique")
        { }
    }
}