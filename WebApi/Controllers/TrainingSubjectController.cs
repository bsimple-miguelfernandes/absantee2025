using Application.DTO.TrainingSubject;
using Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

[Route("api/trainingsubjects")]
[ApiController]
public class TrainingSubjectController : ControllerBase
{
    private readonly TrainingSubjectService _trainingSubjectService;

    public TrainingSubjectController(TrainingSubjectService trainingSubjectService)
    {
        _trainingSubjectService = trainingSubjectService;
    }

    //US 7: Como Gestor de Formação, quero definir tema/assunto de formação (Subject): 
    //      - título (não nulo, max. 20 carateres alfanuméricos),
    //      - descrição (não nulo, max. 100 carateres alfanuméricos)
    // POST  api/TrainingSubject
    [HttpPost]
    public async Task<ActionResult<TrainingSubjectDTO>> AddTrainingSubject([FromBody] AddTrainingSubjectDTO tsDTO)
    {
        var addedTS = await _trainingSubjectService.Add(tsDTO);

        return addedTS.ToActionResult();
    }
    [HttpPut]
    public async Task<ActionResult<UpdatedTrainingSubjectDTO>> UpdateTrainingSubject([FromBody] UpdateTrainingSubjectDTO newSubject)
    {
        if (newSubject.Id == Guid.Empty)
            return BadRequest("Id is required");

        var result = await _trainingSubjectService.UpdateTrainingSubject(newSubject);

        if (result == null) return BadRequest("Invalid arguments");
        return Ok(result);
    }
}
