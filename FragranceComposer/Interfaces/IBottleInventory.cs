namespace FragranceComposer.Interfaces;

public interface IBottleInventory
{
    int Available(string bottleName);
    void Take(string bottleName, int quantity);
}