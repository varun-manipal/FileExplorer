namespace FileExplorer.Model;

public class FileItem
{
    public string Name { get; set; } = string.Empty;
    
    public string FullPath { get; set; } = string.Empty;
    public bool IsDirectory { get; set; }
    
    public DateTime DateModified { get; set; }
    
    public string Type { get; set; } = string.Empty;
    public long SizeBytes { get; set; }
    
    public string Icon { get; set; } = "📄";

    public string SizeDisplay => IsDirectory ? "" : FormatSize(SizeBytes);

    private static string FormatSize(long bytes)
    {
        string[] units = { "B", "KB", "MB", "GB", "TB" };
        double size = bytes;
        int unit = 0;
        while (size >= 1024 && unit < units.Length - 1) { size /= 1024; unit++; }
        return $"{size:0.##} {units[unit]}";
    }
}