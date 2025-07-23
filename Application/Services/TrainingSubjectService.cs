using Application.DTO.TrainingSubject;
using AutoMapper;
using Domain.Factory;
using Domain.Interfaces;
using Domain.IRepository;
using Domain.Models;

namespace Application.Services;

public class TrainingSubjectService
{
    private readonly ITrainingSubjectRepository _trainingSubjectRepository;
    private readonly ITrainingSubjectFactory _trainingSubjectFactory;
    private readonly IMapper _mapper;

    public TrainingSubjectService(ITrainingSubjectRepository trainingSubjectRepository, ITrainingSubjectFactory trainingSubjectFactory, IMapper mapper)
    {
        _trainingSubjectRepository = trainingSubjectRepository;
        _trainingSubjectFactory = trainingSubjectFactory;
        _mapper = mapper;
    }
    public async Task<Result<TrainingSubjectDTO>> Add(AddTrainingSubjectDTO tsDTO)
    {
        TrainingSubject ts;

        try
        {
            if (await _trainingSubjectRepository.IsDuplicated(tsDTO.Subject))
            {
                return Result<TrainingSubjectDTO>.Failure(Error.BadRequest("Subject must be unique"));
            }
            ts = await _trainingSubjectFactory.Create(tsDTO.id, tsDTO.Subject, tsDTO.Description);
            await _trainingSubjectRepository.AddAsync(ts);
        }
        catch (ArgumentException a)
        {
            return Result<TrainingSubjectDTO>.Failure(Error.BadRequest(a.Message));
        }
        catch (Exception e)
        {
            return Result<TrainingSubjectDTO>.Failure(Error.InternalServerError(e.Message));
        }

        var result = _mapper.Map<TrainingSubject, TrainingSubjectDTO>(ts);
        return Result<TrainingSubjectDTO>.Success(result);
    }
    public async Task SubmitAsync(Guid id, string subject, string description)
    {
        var TrainingSubject = await _trainingSubjectFactory.Create(
            id,
            subject,
            description
        );

        await _trainingSubjectRepository.AddAsync(TrainingSubject);
        await _trainingSubjectRepository.SaveChangesAsync();
    }
    public async Task<Result<TrainingSubjectDTO?>> SubmitUpdateAsync(Guid id, string subject, string description)
    {
        var existingSubject = await _trainingSubjectRepository.GetByIdAsync(id);
        if (existingSubject == null)
            return Result<TrainingSubjectDTO?>.Failure(Error.NotFound("TrainingSubject not found."));

        try
        {
            var updatedTrainingSubject = await _trainingSubjectFactory.Create(id, subject, description);

            var updated = await _trainingSubjectRepository.UpdateAsync(updatedTrainingSubject);

            var updatedDto = _mapper.Map<TrainingSubjectDTO>(updated);
            return Result<TrainingSubjectDTO?>.Success(updatedDto);
        }
        catch (Exception e)
        {
            return Result<TrainingSubjectDTO?>.Failure(Error.InternalServerError(e.Message));
        }
    }

    public async Task<Result<IEnumerable<TrainingSubjectDTO>>> GetAll()
    {
        var trainingSubjects = await _trainingSubjectRepository.GetAllAsync();
        var trainingsubjectIds = trainingSubjects.Select(_mapper.Map<TrainingSubjectDTO>);

        return Result<IEnumerable<TrainingSubjectDTO>>.Success(trainingsubjectIds);

    }
    public async Task<Result<TrainingSubjectDTO>> GetById(Guid id)
    {
        try
        {
            var trainingSubject = await _trainingSubjectRepository.GetByIdAsync(id);
            if (trainingSubject == null)
                return Result<TrainingSubjectDTO>.Failure(Error.NotFound("Subject not found"));
            var result = _mapper.Map<TrainingSubjectDTO>(trainingSubject);

            return Result<TrainingSubjectDTO>.Success(result);

        }
        catch (Exception e)
        {
            return Result<TrainingSubjectDTO>.Failure(Error.InternalServerError(e.Message));
        }
    }


}