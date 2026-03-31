using Microsoft.AspNetCore.Mvc;
using MockApi.Application.Dto;
using MockApi.Application.Services.Abstractions;

namespace MockApi.Presentation.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MockController(IMockService mockService) : ControllerBase
{
    [HttpPost]
    public IActionResult GetMockData([FromBody] MockDataRequest request)
    {
        var result = mockService.GenerateMockData(request.Schema);
        return Ok(result);
    }
}