using Application.DTO.TrainingSubject;
using Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace InterfaceAdapters.Controllers;

[Route("api/trainingsubjects")]
[ApiController]
public class TrainingSubjectController : ControllerBase
{
    private readonly TrainingSubjectService _trainingSubjectService;

    public TrainingSubjectController(TrainingSubjectService trainingSubjectService)
    {
        _trainingSubjectService = trainingSubjectService;
    }
    [HttpGet]
    public async Task<ActionResult<IEnumerable<TrainingSubjectDTO>>> Get()
    {
        var trainingSubjectService = await _trainingSubjectService.GetAll();

        return trainingSubjectService.ToActionResult();
    }
    [HttpGet("{subjectId}")]
    public async Task<ActionResult<TrainingSubjectDTO>> GetById(Guid subjectId)
    {
        var trainingSubject = await _trainingSubjectService.GetById(subjectId);

        return trainingSubject.ToActionResult();
    }
}
