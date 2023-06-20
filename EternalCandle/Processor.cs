using System.Globalization;
using CsvHelper;
using OxyPlot;

public class Processor
{
    private string sourceFile { get; }


    public Processor(string sourceFile)
    {
        this.sourceFile = sourceFile;
    }


    public void Process()
    {
        var sourceRows = GetSourceRows();

        var destionationRows = ConvertToDestinationRow(sourceRows);
        
        SaveToDestinationRow("LogResult.csv", destionationRows);

        SavePlots(destionationRows);
    }

    private void SavePlots(List<DestionationRow> rows)
    {
        Console.WriteLine($"Saving charts ...");

        var firstDt = rows.First().DateTime;

        var days = rows
            .Select(x => (x.DateTime - firstDt).TotalDays)
            .ToArray();

        var values = rows
            .Select(x => (double)x.VoltageDecrease)
            .ToArray();

        var values1 = rows
            .Select(x => (double)x.NormalizedVoltageDecrease)
            .ToArray();

        var decreasePlot = new PlotProcessor("График падения напряжения по дням", "Дни", "Напряжение, В");

        decreasePlot.Save(rows, 
                        new [] {
                            (DestionationRow row) => 
                            {
                                return new DataPoint((row.DateTime - firstDt).TotalDays, (double)row.VoltageDecrease);
                            },
                            (DestionationRow row) => 
                            {
                                return new DataPoint((row.DateTime - firstDt).TotalDays, (double)row.NormalizedVoltageDecrease);
                            },
                         },
                        new [] 
                        {
                            "Падение напряжения",
                            "Падение напряжения (нормализированное)"
                        },
                        "VoltageDecrease.png");

        var voltagePlot = new PlotProcessor("График замеров напряжения по дням", "Дни", "Напряжение, В");

        voltagePlot.Save(
            rows, 
            new [] {
                (DestionationRow row) => 
                {
                    return new DataPoint((row.DateTime - firstDt).TotalDays, (double)row.Voltage);
                }
                },
            new [] 
            {
                "Напряжение на 1хАА"
            },
            "Voltage.png"
        );


        Console.WriteLine("Done.");
    }

    private static void SaveToDestinationRow(string destinationFile, List<DestionationRow> destionationRows)
    {
        Console.WriteLine($"Saving to {destinationFile} ...");

        using (var writer = new StreamWriter(destinationFile))
        using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
        {
            csv.WriteRecords(destionationRows);
        }

        Console.WriteLine("Saved.");
    }

    private static List<DestionationRow> ConvertToDestinationRow(List<SourceRow> sourceRows)
    {
        Console.WriteLine("Converting...");

        var destionationRows = new List<DestionationRow>(sourceRows.Count);
        destionationRows.Add(new DestionationRow(sourceRows[0]));

        for (var i = 1; i < sourceRows.Count; i++)
        {
            destionationRows.Add(new DestionationRow(sourceRows[i], sourceRows[i - 1]));
        }

        Console.WriteLine($"Converting finished.");
        return destionationRows;
    }

    private List<SourceRow> GetSourceRows()
    {

        using (var reader = new StreamReader(sourceFile))
        using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
        {
            Console.WriteLine($"Reading from {sourceFile} ...");
            var sourceRows = csv.GetRecords<SourceRow>()
                .ToList();;

            Console.WriteLine($"Reading finished with {sourceRows.Count} rows.");

            return sourceRows;
        }        
    }
}    
