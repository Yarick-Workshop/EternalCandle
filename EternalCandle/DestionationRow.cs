using CsvHelper.Configuration.Attributes;

public class DestionationRow : SourceRow
{
    public DestionationRow(SourceRow sourceRow) 
        : base(sourceRow)
    {
    }

    public DestionationRow(SourceRow currentRow, SourceRow previousRow)
        : this(currentRow)
    {
        this.TimeFromPrevious = currentRow.DateTime - previousRow.DateTime;
        this.VoltageDecrease = previousRow.Voltage - currentRow.Voltage;
        this.NormalizedVoltageDecrease = Math.Round(this.VoltageDecrease/ (decimal)this.TimeFromPrevious.TotalDays, 4);
    }    

    [Name("Прошло времени от предыдущего")]
    [Optional]
    public TimeSpan TimeFromPrevious { get; set; }

    [Name("Уменьшение напряжения, В")]
    [Optional]
    public decimal VoltageDecrease { get; set; }

    [Name("Уменьшение напряжения (нормализированное), В")]
    [Optional]
    public decimal NormalizedVoltageDecrease { get; set; }
    
}