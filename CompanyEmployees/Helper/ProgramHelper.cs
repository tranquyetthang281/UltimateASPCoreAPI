using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace CompanyEmployees.Helper
{
    public static class ProgramHelper
    {
        public static NewtonsoftJsonPatchInputFormatter GetJsonPatchInputFormatter() 
            => new ServiceCollection()
            .AddLogging()
            .AddMvc()
            .AddNewtonsoftJson().Services
            .BuildServiceProvider()
            .GetRequiredService<IOptions<MvcOptions>>().Value.InputFormatters
            .OfType<NewtonsoftJsonPatchInputFormatter>()
            .First();
    }
}
