using Application.DTO.TrainingModule;
using Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

[Route("api/trainingmodules")]
[ApiController]
public class TrainingModuleController : ControllerBase
{
    private readonly TrainingModuleService _trainingModuleService;

    public TrainingModuleController(TrainingModuleService trainingModuleService)
    {
        _trainingModuleService = trainingModuleService;
    }
    [HttpGet]
    public async Task<ActionResult<IEnumerable<TrainingModuleDTO>>> Get()
    {
        var result = await _trainingModuleService.GetAll();
        return result.ToActionResult();
    }

    // GET: api/trainingmodules/{id}
    [HttpGet("{id}")]
    public async Task<ActionResult<TrainingModuleDTO>> GetById(Guid id)
    {
        var result = await _trainingModuleService.GetById(id);
        return result.ToActionResult();
    }
}
