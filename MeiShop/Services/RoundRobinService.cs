namespace MeiShop.Services;

public class RoundRobinService
{
    private readonly List<string> _addresses;
    private int _currentIndex = -1;

    public RoundRobinService(IConfiguration config)
    {
        _addresses = config.GetSection("CreditcardServiceBaseAddresses").Get<List<string>>();
    }

    public string GetBaseAddress()
    {
        _currentIndex += 1;
        _currentIndex = _currentIndex > _addresses.Count - 1 ? 0 : _currentIndex;

        string address = _addresses[_currentIndex];

        return address;
    }
}
