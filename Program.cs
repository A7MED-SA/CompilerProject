using System.Text;
using MainConsole.Servises;
using MainConsole.DataStructure;

namespace MainConsole
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;
            Console.InputEncoding = Encoding.UTF8;


            string sourceCode = @"عيلة برنامجلينا 
{
    جاعد حيسبة سامو عليكم () 
    {
        رقم سن = ٢٥ ; 
        5رقم سن = 79 ;
        كلام اسم = ""محمود"" ; 
        اكتوب ( "" السلام عليكم يا جماعة "" ); 
        اكتوب ( "" أنا "" + اسم + "" وعندي "" + سن + "" سنة. "" ); 

        لو (سن >= ۱۸) 
        {
            اكتوب ( "" انت راجل خلاص يا "" + اسم + ""!"" ); 
        }
        والا 
        {
            اكتوب ( ""روح ذاكر الأول يا "" + اسم + ""!"" ); 
        }

        رقم العد = 0;
        علطول (العد < ۳) 
        {
            اكتوب (""لفة رقم "" + العد); 
            العد = العد + 1; 
        }
        
        لف (رقم i = 0; i < 2; i++) 
        {
            اكتوب("" لف يا واد "" + i); 
        }

        الجوف ; 
    }

    جاعد حيسبة احمد () 
    {
        رقم ول = ٢٥ ; 

    }
}";

            Scanner scanner = new Scanner(sourceCode);
            List<Token> tokens = scanner.ScanTokens();

            Console.WriteLine("--- [ Wah Ya Saeidi Scanner Output ] ---");
            foreach (Token token in tokens)
            {
                Console.WriteLine(token);
            }
            Console.WriteLine("--------------------------------------");

            Parser parser = new Parser(tokens);
            bool success = parser.Parse();


            Console.WriteLine(parser.GetASTString());
            Console.WriteLine();

            Console.WriteLine(parser.GetErrorsString());
            Console.WriteLine();

            Console.WriteLine(parser.GetWarningsString());

            if (success)
            {
                Console.WriteLine($"\n✓ عدد الجمل: {parser.AST.Statements.Count}");
            }
        }
    }
}