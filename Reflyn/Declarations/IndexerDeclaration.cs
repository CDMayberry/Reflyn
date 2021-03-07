using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Reflyn.Collections;
using Reflyn.Mixins;

namespace Reflyn.Declarations
{
    public class IndexerDeclaration : MemberDeclaration, ScopeMixin<IndexerDeclaration>
    {
        public MethodSignature Signature { get; } = new MethodSignature();

        public StatementList Get { get; } = new StatementList();

        public ThrowedExceptionDeclarationList GetExceptions { get; } = new ThrowedExceptionDeclarationList();

        public StatementList Set { get; } = new StatementList();

        public ThrowedExceptionDeclarationList SetExceptions { get; } = new ThrowedExceptionDeclarationList();

        internal IndexerDeclaration(Declaration declaringType, ITypeDeclaration returnType)
            : base("Item", declaringType)
        {
            Signature.ReturnType = returnType;
            this.ToPublic();
            // TODO: Why isn't ScopeMixin throwing errors...?
            //base.Attributes = MemberAttributes.Public;
        }
        
        public override MemberDeclarationSyntax ToSyntax()
        {
            throw new System.NotImplementedException();
        }

        public SyntaxToken? ScopeModifier { get; set; }
    }
}
