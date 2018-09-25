namespace muzi
{
    public enum DownloadState : byte
    {
        NotDownloaded,
        Downloading,
        Download,
        Unziping,
        Compete,
        Pause,
    }
}