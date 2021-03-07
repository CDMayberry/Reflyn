/*
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Text;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Reflyn.Refly.CodeDom.Statements;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace Refly.Refly.CodeDom.Statements
{
    public class CommentStatement : StatementWithExpression
    {
        private string[] _comment;
        

        public CommentStatement(string comment)
        {
            _comment = comment != null ? new []{ comment } : throw new ArgumentNullException(nameof(comment));
        }

        public CommentStatement(params string[] comments)
        {
            _comment = comments;
        }

        public override StatementSyntax ToSyntax()
        {
            return TriviaList(
                new[]
                {
                    Comment("// Test Comment"),
                    Comment("// Other comment"),
                }
            );

            // Technically multiple comments in a row would
        }

        public override void ToCodeDom(CodeStatementCollection statements)
        {
            throw new NotImplementedException();
        }

        public override ExpressionSyntax ToExpressionSyntax()
        {
            return TriviaList(
                new[]
                {
                    Comment("// Test Comment"),
                    Comment("// Other comment"),
                }
            );
        }
    }
}
*/
