namespace MarkDownAvalonia.Data
{
    public class Configuration
    {
        private string rootDirectory;
        private string gitAddress;

        public string RootDirectory
        {
            get => rootDirectory;
            set => rootDirectory = value;
        }

        public string GitAddress
        {
            get => gitAddress;
            set => gitAddress = value;
        }
    }
}