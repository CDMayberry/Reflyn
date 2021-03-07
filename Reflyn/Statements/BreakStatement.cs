using Microsoft.CodeAnalysis.CSharp.Syntax;
using Reflyn.Expressions;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace Reflyn.Statements
{
    public class BreakStatement : ExpressionStatement
    {
        public BreakStatement() : base(new SnippetExpression(""))
        {
        }

        public BreakStatement(Expression expression) : base(expression)
        {
        }

        public override StatementSyntax ToSyntax()
        {
            return BreakStatement();
        }
    }
}
