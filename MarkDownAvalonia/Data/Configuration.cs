namespace MarkDownAvalonia.Data
{
    public class Configuration
    {
        private string rootDirectory;
        private string gitAddress;
        private string postDirectory;

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

        public string PostDirectory
        {
            get => postDirectory;
            set => postDirectory = value;
        }
    }
}