using Microsoft.AspNetCore.Mvc;

namespace Examination.API.Controllers;
[Route("api/v{version:apiVersion}/[controller]")]
[ApiController]
public class BaseController: ControllerBase
{
    
}