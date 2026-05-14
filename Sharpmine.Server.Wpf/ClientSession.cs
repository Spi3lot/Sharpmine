using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Sharpmine.Server.Wpf;

public record ClientSession(Guid Id, string Ip) : INotifyPropertyChanged
{

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
