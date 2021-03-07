namespace Reflyn.Expressions
{
    // This is directly passed CodeDOM expression, which is not great...
    // TODO: Should this just become CodeSnippets? I don't know if Roslyn has the concept of Native Expressions...
    /*public class NativeExpression : Expression
    {
        public CodeExpression Expression { get; }

        public NativeExpression(CodeExpression expression)
        {
            this.Expression = expression ?? throw new ArgumentNullException(nameof(expression));
        }

        public override ExpressionSyntax ToSyntax()
        {
            throw new NotImplementedException();
        }

        /*public override CodeExpression ToCodeDom()
        {
            return Expression;
        }#1#
    }*/
}
