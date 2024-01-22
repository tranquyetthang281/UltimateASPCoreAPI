using Contracts;
using Entities.LinkModels;
using Microsoft.Net.Http.Headers;
using Shared.DataTransferObjects;

namespace CompanyEmployees.Utility
{
    public class EmployeeLinks : IEmployeeLinks
    {
        private readonly LinkGenerator _linkGenerator;

        public EmployeeLinks(LinkGenerator linkGenerator)
        {
            _linkGenerator = linkGenerator;
        }   

        public LinkResponse<LinkedEmployee, EmployeeDto> TryGenerateLinks(IEnumerable<EmployeeDto> employeesDto,
            Guid companyId, HttpContext httpContext)
        {
            if (ShouldGenerateLinks(httpContext))
                return ReturnLinkdedEmployees(employeesDto, companyId, httpContext);
            return ReturnEmployees(employeesDto.ToList());
        }

        private bool ShouldGenerateLinks(HttpContext httpContext)
        {
            if(httpContext.Items.TryGetValue("AcceptHeaderMediaType", out object? mediaType) 
                && mediaType is MediaTypeHeaderValue mediaTypeValue)
            {
                    return mediaTypeValue
                        .SubTypeWithoutSuffix.EndsWith("hateoas", StringComparison.InvariantCultureIgnoreCase);
            }
            return false;
        }

        private LinkResponse<LinkedEmployee, EmployeeDto> ReturnEmployees(List<EmployeeDto> employees) =>
            new LinkResponse<LinkedEmployee, EmployeeDto> { Entities = employees };

        private LinkResponse<LinkedEmployee, EmployeeDto> ReturnLinkdedEmployees(IEnumerable<EmployeeDto> employeesDtoList,
            Guid companyId, HttpContext httpContext)
        {
            var linkedEmployees = employeesDtoList.Select(e => new LinkedEmployee()
            {
                Id = e.Id,
                Age = e.Age,
                Name = e.Name,
                Position = e.Position,
                Links = CreateLinksForEmployee(httpContext, companyId, e.Id)
            }).ToList();

            var employeeCollection = new LinkCollectionWrapper<LinkedEmployee>(linkedEmployees);
            var linkedCompanyEmployees = CreateLinksForCompanyEmployees(httpContext, employeeCollection);
            return new LinkResponse<LinkedEmployee, EmployeeDto> { HasLinks = true, LinkedEntities = linkedCompanyEmployees };
        }


        private List<Link> CreateLinksForEmployee(HttpContext httpContext, Guid companyId, Guid id)
        {
            var links = new List<Link>
            {
                new Link()
                {
                    Href = _linkGenerator.GetUriByAction
                        (httpContext, "GetEmployeeForCompany", values: new { companyId, id }),
                    Rel = "self",
                    Method = "GET"
                },
                new Link()
                {
                    Href = _linkGenerator.GetUriByAction
                        (httpContext, "DeleteEmployeeForCompany", values: new { companyId, id }),
                    Rel = "delete_employee",
                    Method = "DELETE"
                },
                new Link()
                {
                    Href = _linkGenerator.GetUriByAction
                        (httpContext, "UpdateEmployeeForCompany", values: new { companyId, id }),
                    Rel = "update_employee",
                    Method = "PUT"
                },
                new Link()
                {
                    Href = _linkGenerator.GetUriByAction
                        (httpContext, "PartiallyUpdateEmployeeForCompany", values: new { companyId, id }),
                    Rel = "partially_update_employee",
                    Method = "PATCH"
                },
            };

            return links;
        }

        private LinkCollectionWrapper<LinkedEmployee> CreateLinksForCompanyEmployees(HttpContext httpContext,
            LinkCollectionWrapper<LinkedEmployee> employeesWrapper)
        {
            employeesWrapper.Links.Add(new Link()
            {
                Href = _linkGenerator.GetUriByAction(
                    httpContext, "GetEmployeesForCompany", values: new { }),
                Rel = "self",
                Method = "GET"
            });
            return employeesWrapper;
        }
    }

}
