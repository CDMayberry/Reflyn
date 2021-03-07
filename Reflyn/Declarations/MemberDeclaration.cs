namespace Reflyn.Declarations
{
    // T is the parent Declaring Type here, IE the Namespace or the class that this class is contained within.
    public abstract class MemberDeclaration : Declaration
    {
        public Declaration DeclaringType { get; }

        public override string FullName => $"{DeclaringType.FullName}.{base.Name}";

        protected MemberDeclaration(string name, Declaration declaringType) : base(name, declaringType.Conformer)
        {
            DeclaringType = declaringType;
        }
    }
}
