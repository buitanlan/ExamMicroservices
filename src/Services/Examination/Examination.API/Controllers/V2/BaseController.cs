using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Examination.API.Controllers.V2;

[Route("api/v{version:apiVersion}/[controller]")]
[ApiController]
[Authorize]
public class BaseController: ControllerBase;