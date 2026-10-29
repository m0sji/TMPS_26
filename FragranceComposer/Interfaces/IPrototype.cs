namespace FragranceComposer.Interfaces;

public interface IPrototype<T>
{
    T Clone();
}