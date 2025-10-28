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

            if (args.Length == 0)
            {
                Console.WriteLine("الاستخدام: MainConsole.exe <filename.wys>");
                Console.WriteLine("مثال: MainConsole.exe example.wys");
                return;
            }

            string filePath = args[0];
            
            if (!File.Exists(filePath))
            {
                Console.WriteLine($"خطأ: الملف '{filePath}' غير موجود!");
                return;
            }

            string sourceCode = File.ReadAllText(filePath, Encoding.UTF8);

            Scanner scanner = new Scanner(sourceCode);

            List<Token> tokens = scanner.ScanTokens();

            Console.WriteLine("--- [ Wah Ya Saeidi Scanner Output ] ---");
            foreach (Token token in tokens)
            {
                Console.WriteLine(token);
            }
            Console.WriteLine("--------------------------------------");
        }
    }
}
