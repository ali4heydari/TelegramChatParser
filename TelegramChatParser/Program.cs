using System;
using CommandLine;
using E2.Utils;

namespace TelegramChatParser
{
    public class Program
    {
        public static void Main(string[] args)
        {
            try
            {
                ParserResult<TgParser> parserResult =
                    Parser
                        .Default
                        .ParseArguments<TgParser>(args)
                        .WithParsed<TgParser>(o =>
                        {
                            TgParser tgParser = new TgParser(o.InputHtmlPath, o.CsvFilePath, o.Verbose, o.Append);

                            tgParser.CreateCsv();
                        });
            }
            catch (Exception e)
            {
                Console.BackgroundColor = ConsoleColor.Red;
                Console.WriteLine($"Exception thrown: \nMessage -> {e.Message}\nStackTrace -> {e.StackTrace}");
                Console.BackgroundColor = ConsoleColor.Black;
            }
        }
    }
}