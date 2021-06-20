namespace MarkDownAvalonia.Data
{
    /**
     * application directory config
     */
    public class Configuration
    {
        /**
         * root directory
         */
        public string RootDirectory { get; set; }

        /**
         * git address
         */
        public string GitAddress { get; set; }

        /**
         * post storage directory
         */
        public string PostDirectory { get; set; }
    }
}