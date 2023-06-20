using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Legends;
using OxyPlot.Series;
using OxyPlot.SkiaSharp;


public class PlotProcessor
{
    public PlotProcessor(string plotName, string xAxisName, string yAxisName)
    {
        this.plotName = plotName;
        this.xAxisName = xAxisName;
        this.yAxisName = yAxisName;
    }

    private string plotName { get; }

    private string xAxisName { get; }

    private string yAxisName { get; }
    

    public void Save(IEnumerable<DestionationRow> rows, Func<DestionationRow, DataPoint>[] converters, string[] seriesNames, string filePath)
    {
        if (converters.Length != seriesNames.Length)
        {
            throw new ArgumentException($"{nameof(converters)} and {nameof(seriesNames)} have different lengths.");
        }

        var model = new PlotModel { Title = plotName, Background = OxyColors.White };

        for (var i = 0; i < converters.Length; i ++)
        {
            var lineSeries = new LineSeries
            {
                Title = seriesNames[i]
            };

            lineSeries.Points.AddRange(rows.Select(x => converters[i](x)));

            model.Series.Add(lineSeries);
        }

        var axisX = new LinearAxis
        {
            Position = AxisPosition.Left,
            Title = yAxisName
        };
        model.Axes.Add(axisX);

        var axisY = new LinearAxis
        {
            Position = AxisPosition.Bottom,
            Title = yAxisName
        };        
        model.Axes.Add(axisY);

        model.Legends.Add(new Legend() { 
            LegendPosition = LegendPosition.RightTop,
        });
        
        PngExporter.Export(model, filePath, 1280, 720);
    }
}