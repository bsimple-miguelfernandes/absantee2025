using Application.DTO.TrainingModule;
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

    public TrainingModuleService(ITrainingModuleRepository trainingModuleRepository, ITrainingModuleFactory trainingModuleFactory, IMapper mapper)
    {
        _trainingModuleRepository = trainingModuleRepository;
        _trainingModuleFactory = trainingModuleFactory;
        _mapper = mapper;
    }
    /*     public async Task<Result<TrainingModuleDTO>> Add(AddTrainingModuleDTO tmDTO)
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

            var result = _mapper.Map<TrainingModule, TrainingModuleDTO>((TrainingModule)tm);
            if (result == null)
            {
                return Result<TrainingModuleDTO>.Failure(Error.InternalServerError("Mapping failed"));
            }
            return Result<TrainingModuleDTO>.Success(result);
        } */
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
    public async Task<Result<IEnumerable<TrainingModuleDTO>>> GetAll()
    {
        var trainingModules = await _trainingModuleRepository.GetAllAsync();
        var trainingModuleDtos = trainingModules.Select(_mapper.Map<TrainingModuleDTO>);

        return Result<IEnumerable<TrainingModuleDTO>>.Success(trainingModuleDtos);

    }

    public async Task<Result<TrainingModuleDTO>> GetById(Guid id)
    {
        try
        {
            var trainingModule = await _trainingModuleRepository.GetByIdAsync(id);
            if (trainingModule == null)
                return Result<TrainingModuleDTO>.Failure(Error.NotFound("TrainingModule not found"));

            var dto = _mapper.Map<TrainingModuleDTO>(trainingModule);
            return Result<TrainingModuleDTO>.Success(dto);
        }
        catch (Exception e)
        {
            return Result<TrainingModuleDTO>.Failure(Error.InternalServerError(e.Message));
        }
    }
}