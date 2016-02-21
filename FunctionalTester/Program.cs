using Antlr4.Runtime;
using FunctionalTester.Exceptions;
using FunctionalTester.InterpComponents;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FunctionalTester
{
    class Program
    {
        public const string PrerunFunction = "PreRun";
        private const ConsoleColor GoodColour = ConsoleColor.Green;
        private const ConsoleColor FailColour = ConsoleColor.Red;
        private const ConsoleColor IndeterminateColour = ConsoleColor.Yellow;

        static void Main(string[] args)
        {
            if (args.Length != 1)
            {
                Console.WriteLine("Usage: FunctionalTester <filename>");
                return;
            }

            using (var fileStream = new StreamReader(args[0]))
            {
                var inputStream = new AntlrInputStream(fileStream);
                var lexer = new TesterLexer(inputStream);
                var tokens = new CommonTokenStream(lexer);
                var parser = new TesterParser(tokens);

                var tree = parser.prog();

                var translator = new TranslateVisitor();
                translator.Visit(tree);

                Run(translator.Functions, translator.BaseEnvironment);
            }
        }

        static void Run(IDictionary<string, InterpBase> functions, InterpEnvironment baseEnv)
        {
            if (functions.ContainsKey(PrerunFunction))
                Run(PrerunFunction, functions[PrerunFunction], baseEnv);

            foreach(var function in functions)
            {
                if(ShouldRun(function.Key))
                    Run(function.Key, function.Value, baseEnv);
            }
        }

        static bool ShouldRun(string name)
        {
            return name.StartsWith("Test");
        }

        static void Run(string name, InterpBase func, InterpEnvironment baseEnv)
        {
            Console.WriteLine(name);
            try
            {
                func.Interp(baseEnv.Clone());

                WriteColour("\t[Passed]", GoodColour);
            }
            catch(AssertFailException afe)
            {
                WriteColour("\t[Failed]: " + afe.Message, FailColour);
            }
            catch(WrongTypeException wte)
            {
                WriteColour("\t[Failed]: " + wte.Message, IndeterminateColour);
            }
            catch(UndefinedIdentifierException uie)
            {
                WriteColour("\t[Failed]: " + uie.Message, IndeterminateColour);
            }
            catch(Exception ex)
            {
                WriteColour("\t[Failed]: " + ex.Message, IndeterminateColour);
            }
        }

        static void WriteColour(string text, ConsoleColor col)
        {
            var temp = Console.ForegroundColor;
            Console.ForegroundColor = col;

            Console.WriteLine(text);

            Console.ForegroundColor = temp;
        }
    }
}
