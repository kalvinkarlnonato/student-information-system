namespace App.Contracts
{
    public interface IPrintingService
    {
        void SaveFile(string filename, string contentType, MemoryStream stream);
    }
}