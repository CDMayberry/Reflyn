using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Reflyn.Mixins;
using Xunit;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace Reflyn.Test.Mixins
{
    public class ConstMixin_Test
    {
        private ConstMixin_Dummy dummy;

        public ConstMixin_Test()
        {
            dummy = new ConstMixin_Dummy();
        }

        [Fact]
        public void DefaultsToNull()
        {
            Assert.Null(dummy.ConstModifier);
        }

        [Fact]
        public void ToConst_SetsConstModifierToConstToken()
        {
            dummy.ToConst();
            Assert.Equal(Token(SyntaxKind.ConstKeyword), dummy.ConstModifier);
        }
    }

    public class ConstMixin_Dummy : ConstMixin<ConstMixin_Dummy>
    {
        public SyntaxToken? ConstModifier { get; set; }
    }

}
