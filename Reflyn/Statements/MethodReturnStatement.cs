using Microsoft.CodeAnalysis.CSharp.Syntax;
using Reflyn.Expressions;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace Reflyn.Statements
{
    public class MethodReturnStatement : ExpressionStatement
    {
        private bool _isEmpty;
        public MethodReturnStatement()
            : base(new SnippetExpression(""))
        {
            _isEmpty = true;
        }

        public MethodReturnStatement(Expression expression)
            : base(expression)
        {
        }

        public override StatementSyntax ToSyntax()
        {
            return ReturnStatement(_isEmpty ? null : Expression.ToSyntax());
        }
    }
}
