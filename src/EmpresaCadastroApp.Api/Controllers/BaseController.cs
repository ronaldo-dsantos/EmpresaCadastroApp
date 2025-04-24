using EmpresaCadastroApp.Application.Utils;
using Microsoft.AspNetCore.Mvc;

namespace EmpresaCadastroApp.Api.Controllers
{
    [ApiController]
    public abstract class BaseController : ControllerBase
    {
        protected IActionResult FromResult<T>(Result<T> result)
        {
            if (result == null)
                return StatusCode(500, new { success = false, errors = new[] { "Ocorreu um erro inesperado." } });

            if (result.Success)
                return Ok(result);

            return BadRequest(result);
        }
    }
}
