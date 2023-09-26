using WiredBrainCoffee.DataProcessor.Data;
using WiredBrainCoffee.DataProcessor.Model;

namespace WiredBrainCoffee.DataProcessor.Processing;

public class MachineDataProcessor
{
    private readonly Dictionary<string, int> _countPerCoffeeType = new();
    private readonly ICoffeeCountStore _coffeeCountStore;

    public MachineDataProcessor(ICoffeeCountStore coffeeCountStore) // type "ctor" to create a constructor
    {
        _coffeeCountStore = coffeeCountStore;
    }

    public void ProcessItems(MachineDataItem[] dataItems)
    {
        _countPerCoffeeType.Clear();

        foreach (var dataItem in dataItems)
        {
            ProcessItem(dataItem);
        }

        SaveCountPerCoffeeType();
    }

    private void ProcessItem(MachineDataItem dataItem)
    {
        if (!_countPerCoffeeType.ContainsKey(dataItem.CoffeeType))
        {
            _countPerCoffeeType.Add(dataItem.CoffeeType, 1);
        }
        else
        {
            _countPerCoffeeType[dataItem.CoffeeType]++;
        }
    }

    private void SaveCountPerCoffeeType()
    {
        foreach (var entry in _countPerCoffeeType)
        {
            _coffeeCountStore.Save(new CoffeeCountItem(entry.Key, entry.Value));
        }
    }
}
