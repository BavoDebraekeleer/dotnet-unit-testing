using System.Globalization;
using WiredBrainCoffee.DataProcessor.Model;

namespace WiredBrainCoffee.DataProcessor.Parsing;

public class CsvLineParser
{
    public static MachineDataItem[] Parse(string[] csvlines)
    {
        var machineDataItems = new List<MachineDataItem>();

        foreach (var csvLine in csvlines)
        {
            if (!string.IsNullOrWhiteSpace(csvLine))
            {
                var machineDataItem = Parse(csvLine);

                machineDataItems.Add(machineDataItem);
            }
        }

        return machineDataItems.ToArray();
    }

    private static MachineDataItem Parse(string csvLine)
    {
        var lineItems = csvLine.Split(';');
        string[] formats = new[] { "MM/dd/yyyy h:m:s tt" };

        if (lineItems.Length != 2)
        {
            throw new Exception($"Invalid csv line: {csvLine}");
        }

        if (!DateTime.TryParseExact(
            lineItems[1], 
            formats, 
            CultureInfo.CreateSpecificCulture("en-US"), 
            DateTimeStyles.None, 
            out DateTime dateTime))
        {
            //throw new Exception($"Invalid DateTime in csv line: {csvLine}");
            throw new Exception($"Invalid DateTime: {csvLine}"); // Fault on purpose to fail test.
        }

        return new MachineDataItem(lineItems[0], dateTime);
        //lineItems[0], DateTime.Parse(lineItems[1], CultureInfo.CreateSpecificCulture("en-US")));
    }
}
