using AutoMapper;
using Domain.Factory;
using Domain.Interfaces;
using Domain.IRepository;
using Domain.Models;
using Infrastructure.DataModel;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class TrainingModuleRepositoryEF : GenericRepositoryEF<ITrainingModule, TrainingModule, TrainingModuleDataModel>, ITrainingModuleRepository
{
    private readonly IMapper _mapper;

    public TrainingModuleRepositoryEF(AbsanteeContext context, IMapper mapper) : base(context, mapper)
    {
        _mapper = mapper;
    }

    public TrainingModuleRepositoryEF(DbContext context, IMapper mapper) : base(context, mapper)
    {
    }

    public override ITrainingModule? GetById(Guid id)
    {
        var trainingModuleDM = _context.Set<TrainingModuleDataModel>()
                                    .FirstOrDefault(t => t.Id == id);

        if (trainingModuleDM == null)
            return null;

        return _mapper.Map<TrainingModuleDataModel, TrainingModule>(trainingModuleDM);
    }

    public override async Task<ITrainingModule?> GetByIdAsync(Guid id)
    {
        var trainingModuleDM = await _context.Set<TrainingModuleDataModel>()
                                    .FirstOrDefaultAsync(t => t.Id == id);

        if (trainingModuleDM == null)
            return null;

        return _mapper.Map<TrainingModuleDataModel, TrainingModule>(trainingModuleDM);
    }

    public async Task<ITrainingModule> UpdateAsync(ITrainingModule trainingModule)
    {
        var trainingModuleDM = await _context.Set<TrainingModuleDataModel>().FirstOrDefaultAsync(m => m.Id == trainingModule.Id);
        if (trainingModuleDM == null) return null;

        trainingModuleDM.Id = trainingModule.Id;
        trainingModuleDM.TrainingSubjectId = trainingModule.TrainingSubjectId;
        trainingModuleDM.Periods = trainingModule.Periods;

        _context.Set<TrainingModuleDataModel>().Update(trainingModuleDM);
        _context.SaveChanges();
        return _mapper.Map<TrainingModule>(trainingModuleDM);
    }
    public async Task<IEnumerable<TrainingModule>> GetBySubjectIdAndFinished(Guid subjectId, DateTime date)
    {
        var modules = await _context.Set<TrainingModuleDataModel>()
                               .Where(t => t.TrainingSubjectId == subjectId
                               && t.Periods.All(p => p._finalDate <= date))
                               .ToListAsync();

        var trainingModules = modules.Select(_mapper.Map<TrainingModuleDataModel, TrainingModule>);

        return trainingModules;
    }

    //Metodo para encontrar os TrainingModules de um dados subject que terminaram depois de uma determinada data
    public async Task<IEnumerable<TrainingModule>> GetBySubjectAndAfterDateFinished(Guid subjectId, DateTime date)
    {
        var trainingModulesDMs = await _context.Set<TrainingModuleDataModel>()
                                        .Where(t => t.TrainingSubjectId == subjectId
                                                && t.Periods.All(p => p._finalDate <= date))
                                        .ToListAsync();

        var trainingModules = trainingModulesDMs.Select(t => _mapper.Map<TrainingModuleDataModel, TrainingModule>(t));

        return trainingModules;
    }

    public async Task<bool> HasOverlappingPeriodsAsync(Guid trainingSubjectId, List<PeriodDateTime> newPeriods)
    {
        var existingModules = await _context.Set<TrainingModuleDataModel>()
                                            .Where(t => t.TrainingSubjectId == trainingSubjectId)
                                            .ToListAsync();

        foreach (var module in existingModules)
        {
            foreach (var existingPeriod in module.Periods)
            {
                foreach (var newPeriod in newPeriods)
                {
                    bool overlaps = newPeriod._initDate <= existingPeriod._finalDate &&
                                    existingPeriod._initDate <= newPeriod._finalDate;

                    if (overlaps)
                    {
                        return true;
                    }
                }
            }
        }

        return false;
    }
}