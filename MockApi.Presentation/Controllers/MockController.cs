using Microsoft.AspNetCore.Mvc;
using MockApi.Application.Dto;
using MockApi.Application.Services.Abstractions;

namespace MockApi.Presentation.Controllers;

[ApiController]
[Route("[controller]")]
public class MockController(IMockService mockService) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> GetMockData([FromBody] MockDataRequest request)
    {
        var result = await mockService.GenerateMockDataWithAiAsync(request.Schema);
        return Ok(result);
    }
}