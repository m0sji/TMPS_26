using FragranceComposer.Approvals;
using FragranceComposer.Builders;
using FragranceComposer.Factories;
using FragranceComposer.Interfaces;
using FragranceComposer.Models;
using FragranceComposer.Observers;
using FragranceComposer.Repositories;
namespace FragranceComposer.Client;

public class InteractiveMode
{
    private static readonly Dictionary<string, Action<ProductionOrder>> OrderActions = new()
    {
        ["approve"] = order => order.Approve(),
        ["macerate"] = order => order.StartMaceration(),
        ["bottle"] = order => order.Bottle(),
        ["ship"] = order => order.Ship(),
        ["cancel"] = order => order.Cancel()
    };

    private readonly Dictionary<string, IProductLineFactory> _lines = new()
    {
        ["luxury"] = new LuxuryLineFactory(),
        ["classic"] = new ClassicLineFactory(),
        ["travel"] = new TravelLineFactory()
    };

    private readonly IBottleInventory _inventory = new InMemoryBottleInventory();
    private readonly Dictionary<string, Func<IConcentration, Fragrance>> _recipes;
    private readonly Dictionary<string, IOrderObserver> _observers;
    private readonly List<ProductionOrder> _orders = new();
    private bool _safetyCheck = true;
    private bool _stockCheck = true;

    public InteractiveMode()
    {
        var director = new FragranceDirector(new FragranceBuilder(new InMemoryIngredientRepository()));
        _recipes = new()
        {
            ["aquatic"] = concentration => director.BuildAquatic("Black Sea Breeze", concentration),
            ["gourmand"] = concentration => director.BuildGourmand("Vanilla Night", concentration)
        };
        _observers = new()
        {
            ["audit"] = new AuditLogObserver(),
            ["stock"] = new BottleStockObserver(_inventory),
            ["customer"] = new CustomerNotifier("Riviera boutique")
        };
    }

    public void Run()
    {
        PrintHelp();
        while (true)
        {
            Console.Write("\n> ");
            string? line = Console.ReadLine();
            if (line is null)
                return;
            string[] words = line.Trim().ToLowerInvariant().Split(' ', StringSplitOptions.RemoveEmptyEntries);
            if (words.Length == 0)
                continue;
            if (words[0] == "exit")
                return;
            try
            {
                Execute(words);
            }
            catch (Exception ex) when (ex is ArgumentException or InvalidOperationException)
            {
                Console.WriteLine($"REJECTED: {ex.Message}");
            }
        }
    }

    private void Execute(string[] words)
    {
        if (OrderActions.TryGetValue(words[0], out var action) && words.Length == 2)
        {
            ProductionOrder order = FindOrder(words[1]);
            action(order);
            Console.WriteLine($"{order.Id} {words[0]} -> {order.StateName}");
            if (words[0] == "approve")
                Console.WriteLine($"   {order.ApprovalNote}");
            return;
        }

        switch (words[0])
        {
            case "new" when words.Length == 4:
                CreateOrder(words[1], words[2], words[3]);
                break;
            case "subscribe" when words.Length == 3:
                FindOrder(words[1]).Subscribe(FindObserver(words[2]));
                Console.WriteLine($"{words[2]} subscribed to {words[1].ToUpperInvariant()}");
                break;
            case "unsubscribe" when words.Length == 3:
                FindOrder(words[1]).Unsubscribe(FindObserver(words[2]));
                Console.WriteLine($"{words[2]} unsubscribed from {words[1].ToUpperInvariant()}");
                break;
            case "toggle" when words.Length == 2:
                Toggle(words[1]);
                break;
            case "list":
                PrintStatus();
                break;
            default:
                PrintHelp();
                break;
        }
    }

    private void CreateOrder(string lineName, string recipe, string quantityText)
    {
        if (!_lines.TryGetValue(lineName, out var line))
            throw new ArgumentException($"unknown line '{lineName}', use luxury, classic or travel");
        if (!_recipes.TryGetValue(recipe, out var build))
            throw new ArgumentException($"unknown recipe '{recipe}', use aquatic or gourmand");
        if (!int.TryParse(quantityText, out int quantity))
            throw new ArgumentException($"'{quantityText}' is not a number");

        var product = new Product(line.LineName, build(line.CreateConcentration()), line.CreateBottle(), line.CreatePackaging());
        var order = new ProductionOrder($"PO-{_orders.Count + 1}", product, quantity, BuildApprovalChain());
        order.Subscribe(_observers["audit"]);
        order.Subscribe(_observers["stock"]);
        _orders.Add(order);
        Console.WriteLine($"{order.Id}: {quantity} x {product.Description}, {order.TotalCost:F2} EUR [{order.StateName}]");
        Console.WriteLine("   subscribed: audit, stock");
    }

    private IApprovalHandler BuildApprovalChain()
    {
        var handlers = new List<IApprovalHandler>();
        if (_safetyCheck)
            handlers.Add(new SafetyLimitHandler(new Dictionary<string, double> { ["Jasmine"] = 20, ["Rose"] = 30 }));
        if (_stockCheck)
            handlers.Add(new BottleStockHandler(_inventory));
        handlers.Add(new CostApprovalHandler("Perfumer", 500m));
        handlers.Add(new CostApprovalHandler("Lab manager", 1200m));
        handlers.Add(new CostApprovalHandler("Director", 2000m));

        for (int i = 0; i < handlers.Count - 1; i++)
            handlers[i].SetNext(handlers[i + 1]);
        return handlers[0];
    }

    private void Toggle(string check)
    {
        if (check == "safety")
            _safetyCheck = !_safetyCheck;
        else if (check == "stock")
            _stockCheck = !_stockCheck;
        else
            throw new ArgumentException($"unknown check '{check}', use safety or stock");
        Console.WriteLine($"safety check {OnOff(_safetyCheck)}, stock check {OnOff(_stockCheck)} (used by orders created from now on)");
    }

    private ProductionOrder FindOrder(string id) =>
        _orders.FirstOrDefault(order => order.Id.Equals(id, StringComparison.OrdinalIgnoreCase))
        ?? throw new ArgumentException($"no order with id '{id}'");

    private IOrderObserver FindObserver(string name) =>
        _observers.TryGetValue(name, out var observer)
            ? observer
            : throw new ArgumentException($"unknown observer '{name}', use audit, stock or customer");

    private void PrintStatus()
    {
        foreach (var order in _orders)
            Console.WriteLine($"{order.Id} [{order.StateName}] {order.Quantity} x {order.Product.Scent.Name} ({order.Product.LineName}), {order.TotalCost:F2} EUR");
        foreach (var line in _lines.Values)
        {
            string bottle = line.CreateBottle().Name;
            Console.WriteLine($"{bottle}: {_inventory.Available(bottle)} in stock");
        }
        Console.WriteLine($"safety check {OnOff(_safetyCheck)}, stock check {OnOff(_stockCheck)}");
    }

    private static string OnOff(bool enabled) => enabled ? "ON" : "OFF";

    private static void PrintHelp()
    {
        Console.WriteLine("Commands:");
        Console.WriteLine("  new <luxury|classic|travel> <aquatic|gourmand> <quantity>");
        Console.WriteLine("  approve | macerate | bottle | ship | cancel <order id>");
        Console.WriteLine("  subscribe | unsubscribe <order id> <audit|stock|customer>");
        Console.WriteLine("  toggle <safety|stock>");
        Console.WriteLine("  list");
        Console.WriteLine("  exit");
    }
}