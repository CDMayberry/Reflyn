using Microsoft.CodeAnalysis.CSharp.Syntax;
using Reflyn.Expressions;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace Reflyn.Statements
{
    public class ContinueStatement : ExpressionStatement
    {
        public ContinueStatement() : base(new SnippetExpression(""))
        {
        }

        public ContinueStatement(Expression expression) : base(expression)
        {
        }

        public override StatementSyntax ToSyntax()
        {
            return ContinueStatement();
        }
    }
}
