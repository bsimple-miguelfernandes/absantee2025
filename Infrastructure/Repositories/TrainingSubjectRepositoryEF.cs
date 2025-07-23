using AutoMapper;
using Domain.Interfaces;
using Domain.IRepository;
using Domain.Models;
using Infrastructure.DataModel;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class TrainingSubjectRepositoryEF : GenericRepositoryEF<ITrainingSubject, TrainingSubject, TrainingSubjectDataModel>, ITrainingSubjectRepository
{
    private readonly IMapper _mapper;
    public TrainingSubjectRepositoryEF(AbsanteeContext context, IMapper mapper) : base(context, mapper)
    {
        _mapper = mapper;
    }

    public override ITrainingSubject? GetById(Guid id)
    {
        try
        {
            var tsDM = _context.Set<TrainingSubjectDataModel>()
                               .FirstOrDefault();

            if (tsDM == null)
                return null;

            var ts = _mapper.Map<TrainingSubjectDataModel, TrainingSubject>(tsDM);
            return ts;
        }
        catch
        {
            throw;
        }
    }
    public async Task<bool> ExistsAsync(Guid id)
    {
        return await _context.Set<TrainingSubjectDataModel>().AnyAsync(ts => ts.Id == id);
    }
    public override async Task<ITrainingSubject?> GetByIdAsync(Guid id)
    {
        try
        {
            var tsDM = await _context.Set<TrainingSubjectDataModel>()
                               .FirstOrDefaultAsync(ts => ts.Id == id);

            if (tsDM == null)
                return null;

            var ts = _mapper.Map<TrainingSubjectDataModel, TrainingSubject>(tsDM);
            return ts;
        }
        catch
        {
            throw;
        }
    }

    public async Task<bool> IsDuplicated(string subject)
    {
        return await _context.Set<TrainingSubjectDataModel>()
                       .AnyAsync(t => t.Subject.Equals(subject));
    }
    public async Task<TrainingSubject?> UpdateTrainingSubject(ITrainingSubject trainingSubject)
    {
        var trainingSubjectDM = await _context.Set<TrainingSubjectDataModel>()
            .FirstOrDefaultAsync(s => s.Id == trainingSubject.Id);

        if (trainingSubjectDM == null) return null;

        trainingSubjectDM.Subject = trainingSubject.Subject;
        trainingSubjectDM.Description = trainingSubject.Description;

        _context.Set<TrainingSubjectDataModel>().Update(trainingSubjectDM);
        await _context.SaveChangesAsync();
        return _mapper.Map<TrainingSubjectDataModel, TrainingSubject>(trainingSubjectDM);
    }
}
