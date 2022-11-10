namespace Domain.Exceptions
{
    public sealed class NumberAndProviderIdIsNotUnique : BadRequestException
    {
        public NumberAndProviderIdIsNotUnique(string sameOrderId)
            : base($"{sameOrderId} has the same order number and order provider id, " +
                   $"order number and order provider id must be unique")
        { }
    }
}