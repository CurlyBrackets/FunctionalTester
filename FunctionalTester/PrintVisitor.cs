using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Antlr4.Runtime.Misc;
using Antlr4.Runtime.Tree;
using System.Text.RegularExpressions;

namespace FunctionalTester
{
    class PrintVisitor : TesterBaseVisitor<string>
    {
        private int IndentLevel { get; set; }

        public PrintVisitor()
        {

        }

        private string Indent()
        {
            return new string('\t', IndentLevel);
        }

        #region Visits

        protected override string AggregateResult(string a, string b)
        {
            return a + b;
        }

        #region Statements

        public override string VisitExprStatement([NotNull] TesterParser.ExprStatementContext context)
        {
            IndentLevel++;
            var expr = VisitChildren(context);
            IndentLevel--;

            return string.Join(Environment.NewLine,
                Indent() + "ExprStatement: ",
                expr);
        }

        public override string VisitAssignStatement([NotNull] TesterParser.AssignStatementContext context)
        {
            IndentLevel += 2;
            var expr = context.expr().Accept(this);
            IndentLevel -= 2;

            IndentLevel++;
            var id = Indent() + "Identifier: " + context.Identifier().GetText();
            var exprText = Indent() + "Expression: ";
            IndentLevel--;

            return string.Join(Environment.NewLine,
                Indent() + "AssignStatement: ",
                id,
                exprText,
                expr);
        }

        public override string VisitAssertStatement([NotNull] TesterParser.AssertStatementContext context)
        {
            IndentLevel++;
            var expr = VisitChildren(context);
            IndentLevel--;

            return string.Join(Environment.NewLine,
                Indent() + "AssertStatement: ",
                expr);
        }

        #endregion

        public override string Visit(IParseTree tree)
        {
            return base.Visit(tree);
        }

        public override string VisitFunctionTop([NotNull] TesterParser.FunctionTopContext context)
        {
            IndentLevel++;
            var temp = VisitChildren(context);
            IndentLevel--;

            return string.Join(Environment.NewLine,
                Indent() + "Function: " + context.Identifier().GetText(),
                temp);
        }

        public override string VisitIdentExpr([NotNull] TesterParser.IdentExprContext context)
        {
            return Indent() + "Identifier: " + context.GetText() + Environment.NewLine;
        }

        #region Expr

        public override string VisitIntExpr([NotNull] TesterParser.IntExprContext context)
        {
            return Indent() + "IntegerLiteral: " + context.IntegerLiteral().GetText() + Environment.NewLine;
        }

        public override string VisitStringExpr([NotNull] TesterParser.StringExprContext context)
        {
            return Indent() + "StringLiteral: " + context.StringLiteral().GetText() + Environment.NewLine;
        }

        public override string VisitBoolExpr([NotNull] TesterParser.BoolExprContext context)
        {
            return Indent() + "BooleanLiteral: " + context.BooleanLiteral().GetText() + Environment.NewLine;
        }

        public override string VisitParenExpr([NotNull] TesterParser.ParenExprContext context)
        {
            IndentLevel++;
            var temp = VisitChildren(context);
            IndentLevel--;

            return string.Join(Environment.NewLine,
                Indent() + "ParenExpr: ",
                temp);
        }

        public override string VisitShellExpr([NotNull] TesterParser.ShellExprContext context)
        {
            IndentLevel++;
            var temp = VisitChildren(context);
            IndentLevel--;

            return string.Join(Environment.NewLine,
                Indent() + "ShellExpr: ",
                temp);
        }

        public override string VisitRunExpr([NotNull] TesterParser.RunExprContext context)
        {
            IndentLevel++;
            var temp = VisitChildren(context);
            IndentLevel--;

            return string.Join(Environment.NewLine,
                Indent() + "RunExpr: ",
                temp);
        }

        public override string VisitEqualExpr([NotNull] TesterParser.EqualExprContext context)
        {
            IndentLevel += 2;
            var left = context.GetChild(1).Accept(this);
            var right = context.GetChild(2).Accept(this);
            IndentLevel -= 2;

            IndentLevel++;
            var leftText = Indent() + "Left: ";
            var rightText = Indent() + "Right: ";
            IndentLevel--;

            return string.Join(Environment.NewLine,
                Indent() + "EqualExpr: ",
                leftText,
                left,
                rightText,
                right);
        }

        public override string VisitOutputExpr([NotNull] TesterParser.OutputExprContext context)
        {
            IndentLevel++;
            var temp = VisitChildren(context);
            IndentLevel--;

            return string.Join(Environment.NewLine,
                Indent() + "OutputExpr: ",
                temp);
        }

        public override string VisitWaitExpr([NotNull] TesterParser.WaitExprContext context)
        {
            IndentLevel++;
            var temp = VisitChildren(context);
            IndentLevel--;

            return string.Join(Environment.NewLine,
                Indent() + "WaitExpr: ",
                temp);
        }

        public override string VisitConnectExpr([NotNull] TesterParser.ConnectExprContext context)
        {
            IndentLevel++;
            var temp = VisitChildren(context);
            IndentLevel--;

            return string.Join(Environment.NewLine,
                Indent() + "ConnectExpr: ",
                temp);
        }

        public override string VisitDisconnectExpr([NotNull] TesterParser.DisconnectExprContext context)
        {
            IndentLevel++;
            var temp = VisitChildren(context);
            IndentLevel--;

            return string.Join(Environment.NewLine,
                Indent() + "DisconnectExpr: ",
                temp);
        }

        public override string VisitScpExpr([NotNull] TesterParser.ScpExprContext context)
        {
            IndentLevel++;
            var temp = VisitChildren(context);
            IndentLevel--;

            return string.Join(Environment.NewLine,
                Indent() + "ScpExpr: ",
                temp);
        }

        public override string VisitNotExpr([NotNull] TesterParser.NotExprContext context)
        {
            IndentLevel++;
            var temp = VisitChildren(context);
            IndentLevel--;

            return string.Join(Environment.NewLine,
                Indent() + "NotExpr: ",
                temp);
        }

        public override string VisitAssignmentTop([NotNull] TesterParser.AssignmentTopContext context)
        {
            IndentLevel++;
            var name = Indent() + "Identifier: " + context.Identifier().GetText();
            var temp = VisitChildren(context);
            IndentLevel--;

            return string.Join(Environment.NewLine,
                Indent() + "AssignmentTop",
                name,
                temp);
        }

        public override string VisitMultilineExpr([NotNull] TesterParser.MultilineExprContext context)
        {
            return Indent() + context.GetText();
        }

        #endregion

        #endregion
    }
}
