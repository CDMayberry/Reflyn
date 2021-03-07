using System;

namespace Reflyn.Declarations
{
    public sealed class StringEventArgs : EventArgs
    {
        public string Value { get; }

        public StringEventArgs(string value)
        {
            this.Value = value;
        }
    }
}
