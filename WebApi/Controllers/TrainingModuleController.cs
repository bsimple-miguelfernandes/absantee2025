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

    // US 30: Como Gestor de Formação, quero definir um Módulo de Formação (Training Module): 
    //        - assunto, 
    //        - lista de vários períodos (com horas)
    // POST api/trainingmodules
    [HttpPost]
    public async Task<ActionResult<TrainingModuleDTO>> AddTrainingModule(AddTrainingModuleDTO tmDTO)
    {
        var addedTS = await _trainingModuleService.Add(tmDTO);

        return addedTS.ToActionResult();
    }
    [HttpPut]
    public async Task<ActionResult<UpdatedTrainingModuleDTO>> UpdateTrainingModule([FromBody] UpdateTrainingModuleDTO newModule)
    {
        if (newModule.Id == Guid.Empty)
            return BadRequest("Id is required");

        var result = await _trainingModuleService.UpdateTrainingModule(newModule);

        if (result == null) return BadRequest("Invalid arguments");
        return Ok(result);
    }
}
