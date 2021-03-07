using System;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Reflyn.Statements
{
    public class LabeledStatement : Statement
    {
        private readonly string _label;

        private readonly Statement _statement;

        public LabeledStatement(string label, Statement statement)
        {
            _label = label ?? throw new ArgumentNullException(nameof(label));
            _statement = statement ?? throw new ArgumentNullException(nameof(statement));
        }

        public override StatementSyntax ToSyntax()
        {
            throw new NotImplementedException();
        }
    }
}
