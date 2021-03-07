namespace Reflyn.Declarations
{
    public struct FileName
    {
        public string Namespace;

        public string Name;

        public string FullName => $"{Namespace}\\{Name}";

        public FileName(string @namespace, string name)
        {
            Namespace = @namespace;
            Name = name;
        }
    }
}
