namespace MameMiner.Service
{
    public interface IMameMinerSettingsService
    {

        string GetMameExecutablePath();

        string GetMameExportPath();

        string GetMameImportPath();


        void SetMameExecutablePath(string p);

        void SetMameExportPath(string p);

        void SetMameImportPath(string p);

    }
}