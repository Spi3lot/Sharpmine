namespace Sharpmine.Domain;

public interface IProvider<out T>
{

    T Get();

}
