using Antlr4.Runtime;
using Antlr4.Runtime.Tree;
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
    [Flags]
    enum DisplayMode
    {
        None        = 0,
        Errors      = 1,
        Exceptions  = 2,
        Success     = 4,
        All         = (Errors | Exceptions | Success)
    }

    class Program
    {
        const string PrerunFunction = "PreRun";
        const string PostrunFunction = "PostRun";

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

            IParseTree root = null;
            using (var fileStream = new StreamReader(args[0]))
            {
                var inputStream = new AntlrInputStream(fileStream);
                var lexer = new TesterLexer(inputStream);
                var tokens = new CommonTokenStream(lexer);
                var parser = new TesterParser(tokens);

                root = parser.prog();

            }

            /* var printer = new PrintVisitor();
            Console.WriteLine(printer.Visit(root)); */

            var authManager = new SshAuthManager();
            authManager.SetUserPrompt(GetUser);
            authManager.SetPasswordPrompt(GetPassword);
            authManager.SetRepeatPrompt(GetRepeat);
            authManager.SetCachePrompt(GetCache);

            var translator = new TranslateVisitor(authManager);
            translator.Visit(root);

            Run(translator.Functions, translator.BaseEnvironment);
        }

        #region Running

        static void Run(IDictionary<string, InterpBase> functions, InterpEnvironment baseEnv)
        {
            if (functions.ContainsKey(PrerunFunction))
                Run(PrerunFunction, functions[PrerunFunction], baseEnv, DisplayMode.Errors | DisplayMode.Exceptions);

            int passed = 0, total = 0;

            foreach(var function in functions)
            {
                if (ShouldRun(function.Key))
                {
                    if (Run(function.Key, function.Value, baseEnv.Clone()))
                        passed++;
                    total++;
                }
            }

            if (functions.ContainsKey(PostrunFunction))
                Run(PrerunFunction, functions[PostrunFunction], baseEnv, DisplayMode.Errors | DisplayMode.Exceptions);

            if(total != 0)
            {
                var temp = Console.ForegroundColor;
                if (passed == total)
                    Console.ForegroundColor = GoodColour;
                else if (passed / (double)total > 0.75)
                    Console.ForegroundColor = IndeterminateColour;
                else
                    Console.ForegroundColor = FailColour;

                Console.WriteLine();
                Console.WriteLine($"\t{passed} / {total} ({passed / (double)total * 100}%) tests passed");
                Console.ForegroundColor = temp;
            }
        }

        static bool ShouldRun(string name)
        {
            return name.StartsWith("Test");
        }

        static bool Run(string name, InterpBase func, InterpEnvironment env, DisplayMode display = DisplayMode.All)
        {
            try
            {
                func.Interp(env);

                if (display.HasFlag(DisplayMode.Success))
                {
                    Console.WriteLine(name);
                    WriteColour("\t[Passed]", GoodColour);
                }

                return true;
            }
            catch(AssertFailException afe)
            {
                if (display.HasFlag(DisplayMode.Errors))
                {
                    Console.WriteLine(name);
                    WriteColour("\t[Failed]: " + afe.Message, FailColour);
                }
            }
            catch(WrongTypeException wte)
            {
                if (display.HasFlag(DisplayMode.Errors))
                {
                    Console.WriteLine(name);
                    WriteColour("\t[Failed]: " + wte.Message, IndeterminateColour);
                }
            }
            catch(UndefinedIdentifierException uie)
            {
                if (display.HasFlag(DisplayMode.Errors))
                {
                    Console.WriteLine(name);
                    WriteColour("\t[Failed]: " + uie.Message, IndeterminateColour);
                }
            }
            catch(Exception ex)
            {
                if (display.HasFlag(DisplayMode.Errors))
                {
                    Console.WriteLine(name);
                    WriteColour("\t[Failed]: " + ex.Message, IndeterminateColour);
                }
            }

            return false;
        }

        static void WriteColour(string text, ConsoleColor col)
        {
            var temp = Console.ForegroundColor;
            Console.ForegroundColor = col;

            Console.WriteLine(text);

            Console.ForegroundColor = temp;
        }

        #endregion

        #region Prompts

        static string GetUser(string server)
        {
            Console.Write("Username for {0}: ", server);
            return Console.ReadLine();
        }

        static byte[] GetPassword(string server)
        {
            Console.Write("Password: ");
            var ret = new List<byte>();

            ConsoleKeyInfo info;
            do
            {
                info = Console.ReadKey(true);
                if (info.Key != ConsoleKey.Enter)
                    ret.Add(Convert.ToByte(info.KeyChar));
            } while (info.Key != ConsoleKey.Enter);
            Console.WriteLine();

            return ret.ToArray();
        }

        static bool GetRepeat(string newHost, string oldHost)
        {
            while (true) {
                Console.WriteLine("{0} uses the same domain a {1}. Reuse authentication? [Y/n] ", newHost, oldHost);
                var read = Console.ReadLine();
                if (read == string.Empty)
                    return true;
                else if (read.Equals("y", StringComparison.InvariantCultureIgnoreCase))
                    return true;
                else if (read.Equals("n", StringComparison.InvariantCultureIgnoreCase))
                    return false;
            }
        }

        static bool GetCache()
        {
            while (true)
            {
                Console.WriteLine("Would you like to cache this info? [Y/n] ");
                var read = Console.ReadLine();
                if (read == string.Empty)
                    return true;
                else if (read.Equals("y", StringComparison.InvariantCultureIgnoreCase))
                    return true;
                else if (read.Equals("n", StringComparison.InvariantCultureIgnoreCase))
                    return false;
            }
        }

        #endregion
    }
}
