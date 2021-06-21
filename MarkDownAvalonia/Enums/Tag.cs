using System;

namespace MarkDownAvalonia.Enums
{
    /**
     * abstract tag class
     */
    public struct Tag
    {
        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="prefix"></param>
        /// <param name="suffix"></param>
        public Tag(string prefix, string suffix)
        {
            this.Prefix = prefix;
            this.Suffix = suffix;
        }

        /// <summary>
        /// prefix
        /// </summary>
        public string Prefix { get; }

        /// <summary>
        /// suffix
        /// </summary>
        public string Suffix { get; }
    }
}