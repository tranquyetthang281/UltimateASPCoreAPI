using Microsoft.AspNetCore.Mvc;
using Service.Contracts;

namespace CompanyEmployees.Presentation.Controllers
{
    [Route("api/companies")]
    [ApiController]
    public class CompaniesController : ControllerBase
    {
        private readonly IServiceManager _servive;

        public CompaniesController(IServiceManager service)
        {
            _servive = service;
        }

        [HttpGet]
        public IActionResult GetCompanies()
        {
            try
            {
                var companies = _servive.CompanyService.GetAllCompanies(trackChanges: false);

                return Ok(companies);
            }
            catch
            {
                return StatusCode(500, "Internal server error");
            }
        }
    }
}
