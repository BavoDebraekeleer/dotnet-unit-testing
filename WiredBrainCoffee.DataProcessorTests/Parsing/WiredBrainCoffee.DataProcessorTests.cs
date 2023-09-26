using System.Globalization;

namespace WiredBrainCoffee.DataProcessor.Parsing;

public class CsvLineParserTests
{
    [Fact]
    public void ShouldParseValidLine()
    {
        // 1. Arrange
        string[] csvLines = new[] { "Espresso;10/27/2022 8:07:34 AM" };

        // 2. Act
        var machineDataItems = CsvLineParser.Parse(csvLines);

        // 3. Assert
        Assert.NotNull(machineDataItems);
        Assert.Single(machineDataItems);
        Assert.Equal("Espresso", machineDataItems[0].CoffeeType);
        Assert.Equal(new DateTime(2022, 10, 27, 8, 07, 34), machineDataItems[0].CreatedAt);
    }

    [Fact]
    public void ShouldSkipEmptyLines()
    {
        // 1. Arrange
        string[] csvLines = new[] { "", "   " };

        // 2. Act
        var machineDataItems = CsvLineParser.Parse(csvLines);

        // 3. Assert
        Assert.NotNull(machineDataItems);
        Assert.Empty(machineDataItems);
    }

    [Fact]
    public void ShouldThrowExceptionForInvalidLineWhenNotTwoItems()
    {
        // 1. Arrange
        var csvLine = "Cappucciono";
        string[] csvLines = new[] { csvLine };

        // 2. Act & 3. Assert
        var exception = Assert.Throws<Exception>(() => CsvLineParser.Parse(csvLines));
        Assert.Equal($"Invalid csv line: {csvLine}", exception.Message);
    }

    [Fact]
    public void ShouldThrowExceptionForInvalidLineWhenInvalidDateTime()
    {
        // 1. Arrange
        var csvLine = "Cappucciono;InvalidDateTime";
        string[] csvLines = new[] { csvLine };

        // 2. Act & 3. Assert
        var exception = Assert.Throws<Exception>(() => CsvLineParser.Parse(csvLines));
        Assert.Equal($"Invalid DateTime in csv line: {csvLine}", exception.Message);
    }

    // Combines the two test above into one.
    [Theory]
    [InlineData("Cappucciono", "Invalid csv line")]
    [InlineData("Cappucciono;InvalidDateTime", "Invalid DateTime in csv line")]
    public void ShouldThrowExceptionForInvalidLine(string csvLine, string expectedMessagePrefix)
    {
        // 1. Arrange
        string[] csvLines = new[] { csvLine };

        // 2. Act & 3. Assert
        var exception = Assert.Throws<Exception>(() => CsvLineParser.Parse(csvLines));
        Assert.Equal($"{expectedMessagePrefix}: {csvLine}", exception.Message);
    }
}
