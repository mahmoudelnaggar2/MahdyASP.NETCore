namespace MahdyASP.NETCore;

public class AttachmentOptions
{
    public string AllowedExtenstions { get; set; }
    public int MaxSizeInMegaBytes { get; set; }
    public bool EnableCompression { get; set; }
}
