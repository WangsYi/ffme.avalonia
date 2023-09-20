using Avalonia.Controls;

namespace FFME;

public class ImageHost:Image
{
    public ImageHost(): base()
    {
        
    }
    
    // todo 暂时不引入过多属性
    
    /// <summary>
    /// Gets or sets if the independent dispatcher should be prevented
    /// from being shutdown when the host visual is removed from the visual tree (unloaded).
    /// </summary>
    public bool PreventShutdown { get; set; }
}