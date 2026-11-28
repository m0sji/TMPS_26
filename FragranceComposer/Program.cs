using FragranceComposer.Builders;
using FragranceComposer.Models;
using FragranceComposer.Services;
using FragranceComposer.Repositories;
using FragranceComposer.Interfaces;
using FragranceComposer.Concentrations;
using FragranceComposer.Factories;
using FragranceComposer.Prototypes;
using FragranceComposer.Composites;
using FragranceComposer.Proxies;
using FragranceComposer.Decorators;
using FragranceComposer.Approvals;
using FragranceComposer.Observers;

namespace FragranceComposer;

class Program
{
    static void Main(string[] args)
    {
       IIngredientRepository repository = new InMemoryIngredientRepository();
       var builder = new FragranceBuilder(repository);
       var director = new FragranceDirector(builder);
       var printer = new LabelPrinter();


       //1. Builder used directly
       var summer = builder
       .WithName("Moldavian Summer")
       .WithFamily(ScentFamily.Floral)
       .WithConcentration(new EauDeParfum())
       .AddNote("Bergamot", 25)
       .AddNote("Rose", 40)
       .AddNote("Vanilla", 35)
       .Build();
       printer.Print(summer);
       Console.WriteLine();


//2 Abstract Factory: the same receipt in 3 product lines
IProductLineFactory[] lines=
        {
            new LuxuryLineFactory(),
            new ClassicLineFactory(),
            new TravelLineFactory()
        };

        foreach (var line in lines)
        {
            var fragrance = director.BuildAquatic ("Black Sea Breeze", line.CreateConcentration());
            var product = new Product(line.LineName, fragrance, line.CreateBottle(),line.CreatePackaging());
            printer.Print(fragrance);
            printer.Print(product);
            Console.WriteLine();
        }


//3 Validation: this formula adds only to 90%
try
        {
            builder.WithName("Broken")
            .WithFamily(ScentFamily.Citrus)
            .WithConcentration(new EauDeToilette())
            .AddNote("Lemon", 50)
            .AddNote("Musk", 40)
            .Build();
        }

    catch (InvalidOperationException ex)
        {
            Console.WriteLine($"BUILD REJECTED: {ex.Message}");
        }


//4.Prototype: register originals, create variants by cloninig
Console.WriteLine("\n--- Prototype ---");
var registry = new FragranceRegistry();
registry.Register("summer", summer);
registry.Register("vanilla-night", director.BuildGourmand("Vanilla Night", new EauDeParfum()));

var summerCopy = registry.Get("summer");
Console.WriteLine($"Same object as original? {ReferenceEquals(summer,summerCopy)}");
Console.WriteLine($"Same formula instance? {ReferenceEquals(summer.Formula, summerCopy.Formula)}");

var summerIntense = registry.Get("summer").CloneAs("Moldavian Summer Intense", new Parfum());
printer.Print(summerIntense);
Console.WriteLine($"Original unchanged: {summer.Name} ({summer.Concentration.Name})\n");

//All 3 patterns together: cloned variant packaged by a product line
var luxury = new LuxuryLineFactory();
var extrait = registry.Get("vanilla-night").CloneAs("Vanilla Night Extrait", luxury.CreateConcentration());
var extraitProduct = new Product(luxury.LineName, extrait, luxury.CreateBottle(), luxury.CreatePackaging());
printer.Print(extrait);
printer.Print(extraitProduct);

//5. Composite: one fragrance and layered combination used through the same ISCent
Console.WriteLine("\n--- Composite ---");
var scentPrinter = new ScentPrinter();
var classic = new ClassicLineFactory();

IScent seaBreeze = director.BuildAquatic("Black Sea Breeze", classic.CreateConcentration());
IScent vanillaNight = registry.Get("vanilla-night");

IScent beachEvening = new LayeredScent("Beach Evening")
.Add(seaBreeze, 2)
.Add(vanillaNight, 1);

IScent weekendRitual = new LayeredScent("Weekend Ritual")
.Add(beachEvening, 3)
.Add(registry.Get("summer"), 1);

foreach(IScent scent in new[] { seaBreeze, beachEvening, weekendRitual})
scentPrinter.Print(scent);

//6. Proxy a secret formula everyone sees name and price only a perfumer sees the composition
Console.WriteLine("--- Proxy ---");
var session = new UserSession();
IScent secret = new ProtectedScent(registry.Get("vanilla-night").CloneAs("Maison Secret No. 1"), session);
IScent secretEvening = new LayeredScent("Secret Evening")
.Add(secret, 2)
.Add(seaBreeze, 1);

foreach (var role in new[] { UserRole.Customer, UserRole.Perfumer })
        {
            session.SignIn(role);
            Console.WriteLine($"[signed in as {role}]");
            foreach (IScent scent in new[] {secret, secretEvening})
            {
                Console.WriteLine($"{scent.Name} costs {scent.CostPerMl:F4} EUR per ml");
                try
                {
                    scentPrinter.Print(scent);
                }
                catch (UnauthorizedAccessException ex)
                {
                    Console.WriteLine($"ACCESS DENIED: {ex.Message}\n");
                }
            }
        }

//7. Decorator optional add ons stacjed on a product
//path: decorators -> Product -> LayeredScent(composite) -> ProtectedScent(proxy) -> Fragrance
Console.WriteLine("--- Decorator ---");
session.SignIn(UserRole.Customer);
IProduct plain = new Product(luxury.LineName, secretEvening, luxury.CreateBottle(), luxury.CreatePackaging());
printer.Print(plain);

IProduct gift = new GiftWrapDecorator(plain);
gift = new EngravingDecorator(gift, "For Ana");
gift = new SampleVialDecorator(gift, secret);
printer.Print(gift);

IProduct otherOrder =new GiftWrapDecorator(new EngravingDecorator(new SampleVialDecorator(plain, secret), "For Ana"));
printer.Print(otherOrder);
Console.WriteLine($"Same cost in any order? {gift.Cost == otherOrder.Cost}");

//Aproval chain for the production order in section 8 and 9, built by the client
IBottleInventory bottleInventory = new InMemoryBottleInventory();
IApprovalHandler approvalChain = new SafetyLimitHandler(new Dictionary<string, double>
{
    ["Jasmine"] = 20,
    ["Rose"] = 30
});
approvalChain
.SetNext(new BottleStockHandler(bottleInventory))
.SetNext(new CostApprovalHandler("Perfumer", 500m))
.SetNext(new CostApprovalHandler("Lab manager", 1200m))
.SetNext(new CostApprovalHandler("Director", 2000m));

//8.State: a production order moves through its lifecycle, and forbidden steps are rejected
Console.WriteLine("\n--- State ---");
var batch = new Product(classic.LineName, beachEvening, classic.CreateBottle(), classic.CreatePackaging());
var order = new ProductionOrder("PO-001", batch, 50, approvalChain);
Console.WriteLine($"{order.Id}: {order.Quantity} x {batch.Description} [{order.StateName}]");

void Try(ProductionOrder o, string action, Action step)
        {
            try
            {
                step();
                Console.WriteLine($"{o.Id} {action} -> {o.StateName}");
            }
            catch(InvalidOperationException ex)
            {
                Console.WriteLine($"{o.Id} {action} REJECTED: {ex.Message}");
            }
        }
Try(order, "ship", order.Ship);
Try(order, "approve", order.Approve);
Try(order, "start maceration", order.StartMaceration);
Try(order, "cancel", order.Cancel);
Try(order, "bottle", order.Bottle);
Try(order, "ship", order.Ship);
Try(order, "approve", order.Approve);

var smallOrder = new ProductionOrder("PO-002", batch, 20, approvalChain);
Try(smallOrder, "cancel", smallOrder.Cancel);
Try(smallOrder, "approve", smallOrder.Approve);

//9. Chain of Responsability : approving a draft sends it through the chain; the first handler that decides ends the request
Console.WriteLine("\n--- Chain of Responsibility ---");
var luxuryBatch = new Product(luxury.LineName, weekendRitual, luxury.CreateBottle(), luxury.CreatePackaging());
ProductionOrder[] drafts =
        {
            new("PO-003", batch, 20, approvalChain),
            new("PO-004", batch, 50, approvalChain),
            new("PO-005", batch, 110, approvalChain),
            new("PO-006", extraitProduct, 10, approvalChain),
            new("PO-007", luxuryBatch, 40, approvalChain),
            new("PO-008", luxuryBatch, 25, approvalChain)
        };

        foreach (var draft in drafts)
        {
            Console.WriteLine($"{draft.Id}: {draft.Quantity} x {draft.Product.Scent.Name} in {draft.Product.Bottle.Name}, {draft.TotalCost:F2} EUR");
            Try(draft, "approve", draft.Approve);
            Console.WriteLine($"   {draft.ApprovalNote}");
        }

//10. Observer: the order announces every state change to the observers subscribed at the moment
Console.WriteLine("\n--- Observer ---");
var audit = new AuditLogObserver();
var stockKeeper = new BottleStockObserver(bottleInventory);
var boutique = new CustomerNotifier("Riviera boutique");

var bigOrder = new ProductionOrder("PO-009", batch, 60, approvalChain);
bigOrder.Subscribe(audit);
bigOrder.Subscribe(stockKeeper);
bigOrder.Subscribe(boutique);
Console.WriteLine($"{bigOrder.Id}: {bigOrder.Quantity} x {batch.Scent.Name}, glass bottles in stock: {bottleInventory.Available("Glass bottle")}");

Try(bigOrder, "approve", bigOrder.Approve);
Try(bigOrder, "start maceration", bigOrder.StartMaceration);
bigOrder.Unsubscribe(audit);
Console.WriteLine("  (audit log unsubscribed)");
Try(bigOrder, "bottle", bigOrder.Bottle);
Try(bigOrder, "ship", bigOrder.Ship);

var nextOrder = new ProductionOrder("PO-010", batch, 80, approvalChain);
Try(nextOrder, "approve", nextOrder.Approve);
Console.WriteLine($"  {nextOrder.ApprovalNote}");


    }
}