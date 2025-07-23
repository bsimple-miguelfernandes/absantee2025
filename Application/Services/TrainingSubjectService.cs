using Application.DTO.TrainingSubject;
using AutoMapper;
using Domain.Factory;
using Domain.IRepository;
using Domain.Models;
using Application.IPublisher;
namespace Application.Services;

public class TrainingSubjectService
{
    private readonly ITrainingSubjectRepository _trainingSubjectRepository;
    private readonly ITrainingSubjectFactory _trainingSubjectFactory;
    private readonly IMessagePublisher _publisher;
    private readonly IMapper _mapper;

    public TrainingSubjectService(ITrainingSubjectRepository trainingSubjectRepository, ITrainingSubjectFactory trainingSubjectFactory, IMapper mapper, IMessagePublisher publisher)

    {
        _trainingSubjectRepository = trainingSubjectRepository;
        _trainingSubjectFactory = trainingSubjectFactory;
        _mapper = mapper;
        _publisher = publisher;
    }

    public async Task<Result<TrainingSubjectDTO>> Add(AddTrainingSubjectDTO tsDTO)
    {
        TrainingSubject ts;

        try
        {
            ts = await _trainingSubjectFactory.Create(tsDTO.Subject, tsDTO.Description);
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

        await _publisher.PublishCreatedTrainingSubjectMessageAsync(ts.Id, ts.Subject, ts.Description);

        var result = _mapper.Map<TrainingSubject, TrainingSubjectDTO>(ts);
        return Result<TrainingSubjectDTO>.Success(result);
    }
    public async Task<Result<TrainingSubjectDTO>> SubmitAsync(Guid Id, string subject, string description)
    {

        var exists = await _trainingSubjectRepository.ExistsAsync(Id);
        if (exists)
        {
            throw new ArgumentException($"Training subject with name {subject} already exists.");
        }

        var TrainingSubject = await _trainingSubjectFactory.Create(
            subject,
            description
        );

        var addedSubject = await _trainingSubjectRepository.AddAsync(TrainingSubject);
        // await _repository.SaveChangesAsync();

        var dto = _mapper.Map<TrainingSubjectDTO>(addedSubject);
        return Result<TrainingSubjectDTO>.Success(dto);
    }
    public async Task<Result<UpdatedTrainingSubjectDTO?>> UpdateTrainingSubject(UpdateTrainingSubjectDTO dto)
    {
        var trainingSubject = await _trainingSubjectRepository.GetByIdAsync(dto.Id);
        if (trainingSubject == null)
            return Result<UpdatedTrainingSubjectDTO?>.Failure(Error.NotFound("TrainingSubject not found."));

        trainingSubject.UpdateSubject(dto.Subject);
        trainingSubject.UpdateDescription(dto.Description);

        var updated = await _trainingSubjectRepository.UpdateTrainingSubject(trainingSubject);
        if (updated == null)
            return Result<UpdatedTrainingSubjectDTO?>.Failure(Error.InternalServerError("Failed to update TrainingSubject."));

        var updatedDto = _mapper.Map<UpdatedTrainingSubjectDTO>(updated);

        //Publish message to message broker
        await _publisher.PublishUpdatedTrainingSubjectMessageAsync(updated.Id, updated.Subject, updated.Description);

        return Result<UpdatedTrainingSubjectDTO?>.Success(updatedDto);
    }

    public async Task<Result<TrainingSubjectDTO?>> SubmitUpdateAsync(Guid id, string subject, string description)
    {
        var trainingSubject = await _trainingSubjectRepository.GetByIdAsync(id);
        if (trainingSubject == null)
            return Result<TrainingSubjectDTO?>.Failure(Error.NotFound("TrainingSubject not found."));

        trainingSubject.UpdateSubject(subject);
        trainingSubject.UpdateDescription(description);

        var updated = await _trainingSubjectRepository.UpdateTrainingSubject(trainingSubject);
        if (updated == null)
            return Result<TrainingSubjectDTO?>.Failure(Error.InternalServerError("Failed to update TrainingSubject."));

        var updatedDto = _mapper.Map<TrainingSubjectDTO>(updated);
        return Result<TrainingSubjectDTO?>.Success(updatedDto);
    }
}