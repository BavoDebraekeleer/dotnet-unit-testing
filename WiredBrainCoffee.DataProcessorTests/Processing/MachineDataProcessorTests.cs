using WiredBrainCoffee.DataProcessor.Data;
using WiredBrainCoffee.DataProcessor.Model;

namespace WiredBrainCoffee.DataProcessor.Processing;

public class MachineDataProcessorTests : IDisposable
{
    private readonly MachineDataProcessor _machineDataProcessor;
    private readonly FakeCoffeeCountStore _coffeeCountStore;

    public MachineDataProcessorTests()
    {
        _coffeeCountStore = new FakeCoffeeCountStore();
        _machineDataProcessor = new MachineDataProcessor(_coffeeCountStore);
    }

    [Fact]
    public void ShouldSaveCountPerCoffeeType()
    {
        // 1. Arrange
        var items = new[]
        {
            new MachineDataItem("Latte", new DateTime(2023, 09, 26, 14, 21, 56)),
            new MachineDataItem("Latte", new DateTime(2023, 09, 26, 10, 21, 56)),
            new MachineDataItem("Espresso", new DateTime(2023, 09, 26, 7, 21, 56)),
        };

        // 2. Act
        _machineDataProcessor.ProcessItems(items);

        // 3. Assert
        Assert.Equal(2, _coffeeCountStore.SavedItems.Count);

        var item = _coffeeCountStore.SavedItems[0]; // 0 for first saved item.
        Assert.Equal("Latte", item.CoffeeType); // The first item in items is "Latte".
        Assert.Equal(2, item.Count); // The first and second item in items is the same, so the count should be 2.

        item = _coffeeCountStore.SavedItems[1]; // 1 for second saved item.
        Assert.Equal("Espresso", item.CoffeeType); // The following unique name is "Espresso".
        Assert.Equal(1, item.Count); // There is only one "Espresso" item so the count should be 1.
    }

    [Fact]
    public void ShouldClearPreviousCoffeeCount()
    {
        // 1. Arrange
        var items = new[]
        {
            new MachineDataItem("Latte", new DateTime(2023, 09, 26, 14, 21, 56)),
        };

        // 2. Act
        _machineDataProcessor.ProcessItems(items);
        _machineDataProcessor.ProcessItems(items);

        // 3. Assert
        Assert.Equal(2, _coffeeCountStore.SavedItems.Count);
        foreach (var item in _coffeeCountStore.SavedItems)
        {
            Assert.Equal("Latte", item.CoffeeType);
            Assert.Equal(1, item.Count);
        }
    }

    public void Dispose()
    {
        // Clean up code after all tests have finished.
    }

    public class FakeCoffeeCountStore : ICoffeeCountStore
    {
        public List<CoffeeCountItem> SavedItems { get; } = new(); // Type "prop" and Tab

        public void Save(CoffeeCountItem item)
        {
            SavedItems.Add(item);
        }
    }
}
