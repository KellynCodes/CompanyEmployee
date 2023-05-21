namespace Entities.Exceptions
{
    public abstract class BadRequestException : Exception
    {
        protected BadRequestException(string message)
        : base(message)
        {
        }
    }

    public abstract class NotFoundException : Exception
    {
        protected NotFoundException(string message) : base(message)
        {

        }
    }

    public sealed class CompanyNotFoundException : NotFoundException
    {
        public CompanyNotFoundException(Guid companyId) : base($"The company with id: {companyId} doesn't exist in the database.") { }
        public CompanyNotFoundException() : base($"ErrorFetching company") { }
    }

    public class EmployeeNotFoundException : NotFoundException
    {
        public EmployeeNotFoundException(Guid employeeId) : base($"Employee with id: {employeeId} doesn't exist in the database.")
        {
        }
    }


    public sealed class IdParametersBadRequestException : BadRequestException
    {
        public IdParametersBadRequestException()
        : base("Parameter ids is null")
        {
        }
    }
    public sealed class CollectionByIdsBadRequestException : BadRequestException
    {
        public CollectionByIdsBadRequestException()
        : base("Collection count mismatch comparing to ids.")
        {
        }
    }


    public sealed class CompanyCollectionBadRequest : BadRequestException
    {
        public CompanyCollectionBadRequest()
        : base("Company collection sent from a client is null.") { }
    }
    public sealed class MaxAgeRangeBadRequestException : BadRequestException
    {
        public MaxAgeRangeBadRequestException()
        : base("Max age can't be less than min age.")
        {
        }
    }

}
