using FunctionalTester.InterpComponents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Antlr4.Runtime.Misc;
using System.Text.RegularExpressions;

namespace FunctionalTester
{
    class TranslateVisitor : TesterBaseVisitor<InterpBase>
    {
        private Dictionary<string, InterpBase> m_core;

        public IDictionary<string, InterpBase> Functions {
            get
            {
                return m_core;
            }
        }

        public InterpEnvironment BaseEnvironment { get; private set; }

        public TranslateVisitor()
        {
            m_core = new Dictionary<string, InterpBase>();
            BaseEnvironment = new InterpEnvironment();
        }

        #region Base expressions

        public override InterpBase VisitBoolExpr([NotNull] TesterParser.BoolExprContext context)
        {
            return new InterpBool(bool.Parse(context.GetText()));
        }

        public override InterpBase VisitIntExpr([NotNull] TesterParser.IntExprContext context)
        {
            return new InterpInteger(int.Parse(context.GetText()));
        }

        public override InterpBase VisitStringExpr([NotNull] TesterParser.StringExprContext context)
        {
            return new InterpString(Regex.Unescape(context.GetText().Trim('"')));
        }

        public override InterpBase VisitIdentExpr([NotNull] TesterParser.IdentExprContext context)
        {
            return new InterpIdentifier(context.Identifier().GetText());
        }

        #endregion

        #region Nested Expressions

        public override InterpBase VisitRunExpr([NotNull] TesterParser.RunExprContext context)
        {
            var nameChild = context.GetChild(1).Accept(this);
            InterpBase argsChild = null;
            if (context.ChildCount > 2)
                argsChild = context.GetChild(2).Accept(this);

            return new InterpRun(nameChild, argsChild);
        }

        public override InterpBase VisitParenExpr([NotNull] TesterParser.ParenExprContext context)
        {
            return context.expr().Accept(this);
        }

        public override InterpBase VisitShellExpr([NotNull] TesterParser.ShellExprContext context)
        {
            var command = context.expr().Accept(this);
            return new InterpShell(command);
        }

        public override InterpBase VisitOutputExpr([NotNull] TesterParser.OutputExprContext context)
        {
            var proc = context.expr().Accept(this);
            return new InterpOutput(proc);
        }

        public override InterpBase VisitWaitExpr([NotNull] TesterParser.WaitExprContext context)
        {
            var children = new List<InterpBase>();
            for (int i = 1; i < context.ChildCount; i++)
                children.Add(context.GetChild(i).Accept(this));

            return new InterpWait(children);
        }

        public override InterpBase VisitEqualExpr([NotNull] TesterParser.EqualExprContext context)
        {
            var left = context.GetChild(1).Accept(this);
            var right = context.GetChild(2).Accept(this);

            return new InterpEqual(left, right);
        }

        public override InterpBase VisitConnectExpr([NotNull] TesterParser.ConnectExprContext context)
        {
            var addr = context.GetChild(1).Accept(this);
            if(addr is InterpString)
            {

            }                

            return new InterpConnect(addr);
        }

        #endregion

        #region Statements

        public override InterpBase VisitAssignStatement([NotNull] TesterParser.AssignStatementContext context)
        {
            string id = context.Identifier().GetText();
            InterpBase value = context.expr().Accept(this);

            return new InterpAssignment(id, value);
        }

        public override InterpBase VisitAssertStatement([NotNull] TesterParser.AssertStatementContext context)
        {
            var child = context.expr().Accept(this);
            return new InterpAssertion(child);
        }

        public override InterpBase VisitExprStatement([NotNull] TesterParser.ExprStatementContext context)
        {
            return context.expr().Accept(this);
        }

        #endregion

        public override InterpBase VisitFunctionTop([NotNull] TesterParser.FunctionTopContext context)
        {
            var ident = context.Identifier().GetText();
            var body = context.functionBody().Accept(this);

            m_core.Add(ident, body);

            return null;
        }

        public override InterpBase VisitFunctionBody([NotNull] TesterParser.FunctionBodyContext context)
        {
            var children = new List<InterpBase>();

            for(int i = 0; i < context.ChildCount; i++)
            {
                children.Add(context.GetChild(i).Accept(this));
            }

            return new InterpList(children);
        }
    }
}
