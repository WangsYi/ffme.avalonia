using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;

namespace FFME.Avalonia.Sample;

public static class Utils
{
    public static IClassicDesktopStyleApplicationLifetime? GetDesktop()
    {
        return (Application.Current!.ApplicationLifetime as IClassicDesktopStyleApplicationLifetime);
    }
    
}