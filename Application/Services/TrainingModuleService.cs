using Application.DTO.TrainingModule;
using Application.IPublisher;
using AutoMapper;
using Domain.Factory;
using Domain.Interfaces;
using Domain.IRepository;
using Domain.Models;

namespace Application.Services;

public class TrainingModuleService
{
    private readonly ITrainingModuleRepository _trainingModuleRepository;
    private readonly ITrainingModuleFactory _trainingModuleFactory;
    private readonly IMapper _mapper;
    private readonly IMessagePublisher _publisher;


    public TrainingModuleService(ITrainingModuleRepository trainingModuleRepository, ITrainingModuleFactory trainingModuleFactory, IMapper mapper, IMessagePublisher publisher)
    {
        _trainingModuleRepository = trainingModuleRepository;
        _trainingModuleFactory = trainingModuleFactory;
        _mapper = mapper;
        _publisher = publisher;
    }

    public async Task<Result<TrainingModuleDTO>> Add(AddTrainingModuleDTO tmDTO)
    {
        ITrainingModule tm;

        try
        {
            tm = await _trainingModuleFactory.Create(tmDTO.TrainingSubjectId, tmDTO.Periods);
            tm = await _trainingModuleRepository.AddAsync(tm);
        }
        catch (ArgumentException a)
        {
            return Result<TrainingModuleDTO>.Failure(Error.BadRequest(a.Message));
        }
        catch (Exception e)
        {
            return Result<TrainingModuleDTO>.Failure(Error.BadRequest(e.Message));
        }
        await _publisher.PublishCreatedTrainingModuleMessageAsync(tm.Id, tm.TrainingSubjectId, tm.Periods);


        var result = _mapper.Map<TrainingModule, TrainingModuleDTO>((TrainingModule)tm);

        return Result<TrainingModuleDTO>.Success(result);
    }
    public async Task<Result<TrainingModuleDTO>> SubmitAsync(Guid Id, Guid subjectId, List<PeriodDateTime> periods)
    {
        var exists = await _trainingModuleRepository.ExistsAsync(Id);
        if (exists)
        {
            throw new ArgumentException($"Training subject with name {subjectId} already exists.");
        }

        var trainingModule = await _trainingModuleFactory.Create(
            subjectId,
            periods
        );

        var addedTrainingModule = await _trainingModuleRepository.AddAsync(trainingModule);
        // await _repository.SaveChangesAsync();

        var dto = _mapper.Map<TrainingModuleDTO>(addedTrainingModule);
        return Result<TrainingModuleDTO>.Success(dto);
    }
    public async Task<Result<UpdatedTrainingModuleDTO?>> UpdateTrainingModule(UpdateTrainingModuleDTO dto)
    {
        var trainingModule = await _trainingModuleRepository.GetByIdAsync(dto.Id);
        if (trainingModule is not TrainingModule concreteTM)
            return Result<UpdatedTrainingModuleDTO?>.Failure(Error.NotFound("TrainingModule not found."));

        try
        {
            concreteTM.UpdateTrainingSubjectId(dto.TrainingSubjectId);
            concreteTM.UpdatePeriods(dto.Periods);

            var updated = await _trainingModuleRepository.UpdateTrainingModule(concreteTM);
            if (updated == null)
                return Result<UpdatedTrainingModuleDTO?>.Failure(Error.InternalServerError("Failed to update TrainingModule."));

            var updatedDto = _mapper.Map<UpdatedTrainingModuleDTO>(updated);
            await _publisher.PublishUpdatedTrainingModuleMessageAsync(updated.Id, updated.TrainingSubjectId, updated.Periods);

            return Result<UpdatedTrainingModuleDTO?>.Success(updatedDto);
        }
        catch (Exception e)
        {
            return Result<UpdatedTrainingModuleDTO?>.Failure(Error.InternalServerError(e.Message));
        }
    }

    public async Task<Result<TrainingModuleDTO?>> SubmitUpdateAsync(Guid id, Guid subjectId, List<PeriodDateTime> periods)
    {
        var trainingModule = await _trainingModuleRepository.GetByIdAsync(id);
        if (trainingModule is not TrainingModule concreteTM)
            return Result<TrainingModuleDTO?>.Failure(Error.NotFound("TrainingModule not found."));

        try
        {
            concreteTM.UpdateTrainingSubjectId(subjectId);
            concreteTM.UpdatePeriods(periods);

            var updated = await _trainingModuleRepository.UpdateTrainingModule(concreteTM);
            if (updated == null)
                return Result<TrainingModuleDTO?>.Failure(Error.InternalServerError("Failed to update TrainingModule."));

            var updatedDto = _mapper.Map<TrainingModuleDTO>(updated);
            return Result<TrainingModuleDTO?>.Success(updatedDto);
        }
        catch (Exception e)
        {
            return Result<TrainingModuleDTO?>.Failure(Error.InternalServerError(e.Message));
        }
    }

}