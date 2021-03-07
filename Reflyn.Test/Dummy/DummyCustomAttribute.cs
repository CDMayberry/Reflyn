using System;

namespace Reflyn.Test.Dummy
{
    public class DummyCustomAttribute : Attribute
    {
    }

    public class DummyPositionalCustomAttribute : Attribute
    {
        private readonly string _str;

        public DummyPositionalCustomAttribute(string str)
        {
            _str = str;
        }
    }
    public class DummyNamedCustomAttribute : Attribute
    {
        public string Str;
    }
}
