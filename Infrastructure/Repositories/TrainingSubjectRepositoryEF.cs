using AutoMapper;
using Domain.Factory;
using Domain.Interfaces;
using Domain.IRepository;
using Domain.Models;
using Infrastructure.DataModel;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class TrainingSubjectRepositoryEF : GenericRepositoryEF<ITrainingSubject, TrainingSubject, TrainingSubjectDataModel>, ITrainingSubjectRepository
{
    private readonly IMapper _mapper;
    private readonly ITrainingSubjectFactory _trainingSubjectFactory;
    public TrainingSubjectRepositoryEF(AbsanteeContext context, IMapper mapper, ITrainingSubjectFactory trainingSubjectFactory) : base(context, mapper)
    {
        _mapper = mapper;
        _trainingSubjectFactory = trainingSubjectFactory;
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
    public async Task<ITrainingSubject> UpdateAsync(ITrainingSubject trainingSubject)
    {
        var trainingSubjectDM = await _context.Set<TrainingSubjectDataModel>().FirstOrDefaultAsync(m => m.Id == trainingSubject.Id);
        if (trainingSubjectDM == null) return null;

        trainingSubjectDM.Id = trainingSubject.Id;
        trainingSubjectDM.Description = trainingSubject.Description;
        trainingSubjectDM.Subject = trainingSubject.Subject;

        _context.Set<TrainingSubjectDataModel>().Update(trainingSubjectDM);
        _context.SaveChanges();
        return _mapper.Map<TrainingSubject>(trainingSubjectDM);
    }
}
