namespace questioneer.Core.Entities.Events
{
    public delegate void VersionMismatchHandler(int currentVersion, int newestVersion);
}