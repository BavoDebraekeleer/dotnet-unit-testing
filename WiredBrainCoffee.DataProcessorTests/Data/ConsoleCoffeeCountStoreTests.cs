using WiredBrainCoffee.DataProcessor.Model;

namespace WiredBrainCoffee.DataProcessor.Data;

public class ConsoleCoffeeCountStoreTests
{
    [Fact]
    public void ShouldWriteOutputToConsole()
    {
        // 1. Arrange
        var coffeeCountItem = new CoffeeCountItem("Latte", 5);
        var stringWriter = new StringWriter(); // StringWriter inherits from TextWriter, but is specifically for strings.
        var consoleCoffeeCountStore = new ConsoleCoffeeCountStore(stringWriter);

        // 2. Act
        consoleCoffeeCountStore.Save(coffeeCountItem);

        // 3. Assert
        var result = stringWriter.ToString();
        Assert.Equal(
            $"{coffeeCountItem.CoffeeType}:{coffeeCountItem.Count}{Environment.NewLine}",
            result ); // "Latte:5/r/n"
    }
}
