namespace Domain.Models;

using System.Text.Json.Serialization;


public class PeriodDateTime
{
    public DateTime _initDate { get; set; }
    public DateTime _finalDate { get; set; }

    public PeriodDateTime(DateTime initDate, DateTime finalDate)
    {
        if (CheckInputFields(initDate, finalDate))
        {
            _initDate = initDate;
            _finalDate = finalDate;
        }
        else
            throw new ArgumentException("Invalid Arguments");
    }

    public PeriodDateTime()
    {

    }

    public PeriodDateTime(PeriodDate periodDate) : this(
        periodDate.InitDate.ToDateTime(TimeOnly.MinValue),
        periodDate.FinalDate.ToDateTime(TimeOnly.MinValue))
    {
    }

    private bool CheckInputFields(DateTime initDate, DateTime finalDate)
    {
        if (initDate > finalDate)
            return false;

        return true;
    }

    public void SetFinalDate(DateTime finalDate)
    {
        this._finalDate = finalDate;
    }

    public bool IsFinalDateUndefined()
    {
        return _finalDate == DateTime.MaxValue;
    }

    public bool IsFinalDateSmallerThan(DateTime date)
    {
        return date > _finalDate;
    }

    public bool Contains(PeriodDateTime periodDateTime)
    {
        return _initDate <= periodDateTime._initDate
            && _finalDate >= periodDateTime._finalDate;
    }

    public bool Intersects(PeriodDateTime periodDateTime)
    {
        return _initDate <= periodDateTime._finalDate && periodDateTime._initDate <= _finalDate;
    }

    public override bool Equals(object? obj)
    {
        if (obj is not PeriodDateTime other)
            return false;

        return _initDate == other._initDate && _finalDate == other._finalDate;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(_initDate, _finalDate);
    }
}

