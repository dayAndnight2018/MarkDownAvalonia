using System;

namespace MarkDownAvalonia.Enums
{
    public struct Tag
    {
        private String prefix;
        private String suffix;

        public Tag(String prefix, String suffix)
        {
            this.prefix = prefix;
            this.suffix = suffix;
        }

        public string Prefix
        {
            get => prefix;
        }

        public string Suffix
        {
            get => suffix;
        }
    }
}