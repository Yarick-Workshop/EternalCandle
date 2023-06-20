using CsvHelper.Configuration.Attributes;

public class SourceRow
{
    public SourceRow()
    {

    }

    public SourceRow(SourceRow sourceRow)
    {
        DateTime = sourceRow.DateTime;
        Voltage = sourceRow.Voltage;
        IsAverage = sourceRow.IsAverage;
        Notes = sourceRow.Notes;
    }

    [Name("Дата / Время")]
    [Format("yyyy-MM-dd HH:mm:ss")]
    public DateTime DateTime { get; set; }

    [Name("Напряжение, В")]
    public decimal Voltage { get; set; }
    
    [Name("Усредненное значение напряжения")]
    public string IsAverage { get; set; }

    [Name("Примечания")]
    public string Notes { get; set; }
}
