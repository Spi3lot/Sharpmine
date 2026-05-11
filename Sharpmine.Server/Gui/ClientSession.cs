using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Sharpmine.Server.Gui;

public class ClientSession(Guid id) : INotifyPropertyChanged
{

    public Guid Id { get; } = id;

    public bool IsConnected
    {
        get;
        set
        {
            field = value;
            OnPropertyChanged();
        }
    } = true;

    public event PropertyChangedEventHandler? PropertyChanged;

    protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

}
