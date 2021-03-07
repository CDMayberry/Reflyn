using Microsoft.CodeAnalysis.CSharp.Syntax;
using Reflyn.Declarations;
using Reflyn.Expressions;

namespace Reflyn.Statements
{
	public abstract class Statement<T> where T : StatementSyntax
    {
        public abstract T ToSyntax();

		/*public LabeledStatement Label(string label)
		{
			return new LabeledStatement(label, this);
		}*/
	}

    public abstract class Statement : Statement<StatementSyntax>
    {
        /*public override StatementSyntax ToSyntax()
        {
            throw new NotImplementedException();
		}*/

		public LabeledStatement Label(string label)
		{
			return new LabeledStatement(label, this);
		}
        
        public static implicit operator Statement(Expression val) => Stm.FromExpr(val);

        public static implicit operator Statement(bool val) => Expr.Prim(val);
        public static implicit operator Statement(short val) => Expr.Prim(val);
        public static implicit operator Statement(int val) => Expr.Prim(val);
        public static implicit operator Statement(long val) => Expr.Prim(val);
        public static implicit operator Statement(float val) => Expr.Prim(val);
        public static implicit operator Statement(double val) => Expr.Prim(val);
    }

    // Not sure if this is the best with but it's working atm.
    public abstract class StatementWithExpression : Statement
    {
        public abstract ExpressionSyntax ToExpressionSyntax();
        /*public override StatementSyntax ToSyntax()
        {
            throw new NotImplementedException();
        }*/

    }
}
