using Application.DTO.AssociationTrainingModuleCollaborator;
using Application.DTO.TrainingModule;
using Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

[Route("api/trainingmodules")]
[ApiController]
public class TrainingModuleController : ControllerBase
{
    private readonly TrainingModuleService _trainingModuleService;
    private readonly AssociationTrainingModuleCollaboratorService _assocTMCService;
    private readonly CollaboratorService _collaboratorService;

    public TrainingModuleController(TrainingModuleService trainingModuleService, AssociationTrainingModuleCollaboratorService assocTMCService, CollaboratorService collaboratorService)
    {
        _trainingModuleService = trainingModuleService;
        _assocTMCService = assocTMCService;
        _collaboratorService = collaboratorService;
    }

    // UC32: Como Gestor de Formação, quero listar todos os colaboradores ativos que não têm qualquer formação terminada em determinado tema
    // GET api/trainingmodules/{id}/collaborators/active/no-training-done
    [HttpGet("not-completed/subjects/{id}/collaborators/active/")]
    public async Task<ActionResult<ICollection<Guid>>> GetActiveCollaboratorsWithNoTrainingDoneForSubject(Guid id)
    {
        var collabs = await _collaboratorService.GetActiveCollaboratorsWithNoTrainingModuleFinishedInSubject(id);

        return collabs.ToActionResult();
    }

    // UC33: Como Gestor de Formação, quero listar todos os colaboradores que têm 
    //       formação terminada em determinado tema depois de uma determinada data
    // Get 
    [HttpGet("completed/subjects/{id}/collaborators")]
    public async Task<ActionResult<ICollection<Guid>>> GetAllCollaboratorsWithTrainingDoneInSubjectAfterDate(Guid id, [FromQuery] DateTime fromDate)
    {
        var collabs = await _collaboratorService.GetCompletedTrainingsAsync(id, fromDate);

        return collabs.ToActionResult();
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
