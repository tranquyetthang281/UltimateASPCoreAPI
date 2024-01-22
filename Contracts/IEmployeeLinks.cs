using Entities.LinkModels;
using Microsoft.AspNetCore.Http;
using Shared.DataTransferObjects;

namespace Contracts
{
    public interface IEmployeeLinks
    {
        LinkResponse<LinkedEmployee, EmployeeDto> TryGenerateLinks(IEnumerable<EmployeeDto> employeesDto,
            Guid companyId, HttpContext httpContext);
    }

}
