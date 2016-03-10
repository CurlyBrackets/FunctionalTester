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

        private SshAuthManager m_authManager;

        public InterpEnvironment BaseEnvironment { get; private set; }

        public TranslateVisitor(SshAuthManager authManager)
        {
            m_authManager = authManager;
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

        public override InterpBase VisitMultilineExpr([NotNull] TesterParser.MultilineExprContext context)
        {
            var text = context.GetText();

            return new InterpString(text.Substring(3, text.Length - 6).Replace(Environment.NewLine, "\n"));
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

        public override InterpBase VisitOsExpr([NotNull] TesterParser.OsExprContext context)
        {
            var left = context.GetChild(1).Accept(this);
            var right = context.GetChild(2).Accept(this);

            return new InterpOs(left, right);
        }

        public override InterpBase VisitWriteExpr([NotNull] TesterParser.WriteExprContext context)
        {
            var left = context.GetChild(1).Accept(this);
            var right = context.GetChild(2).Accept(this);

            return new InterpWrite(left, right);
        }

        public override InterpBase VisitReadExpr([NotNull] TesterParser.ReadExprContext context)
        {
            var file = context.expr().Accept(this);

            return new InterpRead(file);
        }

        public override InterpBase VisitKillExpr([NotNull] TesterParser.KillExprContext context)
        {
            var target = context.expr().Accept(this);
            return new InterpKill(target);
        }

        public override InterpBase VisitConnectExpr([NotNull] TesterParser.ConnectExprContext context)
        {
            var addr = context.GetChild(1).Accept(this);
            if(addr is InterpString)
            {
                //possibly add thing to m_authManager
            }
            InterpBase prepend = null;
            if (context.ChildCount > 2)
                prepend = context.GetChild(2).Accept(this);

            return new InterpConnect(m_authManager, addr, prepend);
        }

        public override InterpBase VisitDisconnectExpr([NotNull] TesterParser.DisconnectExprContext context)
        {
            var conn = context.GetChild(1).Accept(this);
            InterpBase cleanup = null;
            if (context.ChildCount > 2)
                cleanup = context.GetChild(2).Accept(this);

            return new InterpDisconnect(conn, cleanup);
        }

        public override InterpBase VisitSshExpr([NotNull] TesterParser.SshExprContext context)
        {
            var conn = context.GetChild(1).Accept(this);
            var command = context.GetChild(2).Accept(this);

            return new InterpSsh(conn, command);
        }

        public override InterpBase VisitScpExpr([NotNull] TesterParser.ScpExprContext context)
        {
            var conn = context.GetChild(1).Accept(this);
            var localFile = context.GetChild(2).Accept(this);
            InterpBase remoteFile = null;
            if (context.ChildCount > 3)
                remoteFile = context.GetChild(3).Accept(this);

            return new InterpScp(conn, localFile, remoteFile);
        }

        public override InterpBase VisitNotExpr([NotNull] TesterParser.NotExprContext context)
        {
            var child = context.expr().Accept(this);
            return new InterpNot(child);
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

        public override InterpBase VisitAssignmentTop([NotNull] TesterParser.AssignmentTopContext context)
        {
            var name = context.Identifier().GetText();
            var value = context.expr().Accept(this);

            var val = value.Interp(BaseEnvironment);
            BaseEnvironment[name] = val;

            return null;
        }
    }
}
