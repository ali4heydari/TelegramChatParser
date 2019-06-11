using System;
using System.IO;
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
                            if (!File.Exists(o.InputHtmlPath))
                            {
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.WriteLine("Error: inputHtml dose not exists! aborting...");
                                Console.ForegroundColor = ConsoleColor.White;
                                return;
                            }
                            
                            TgParser tgParser = new TgParser(o.InputHtmlPath, o.CsvFilePath, o.Verbose, o.Append);

                            tgParser.CreateCsv();
                        });
            }
            catch (Exception e)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Exception thrown: \nMessage -> {e.Message}\nStackTrace -> {e.StackTrace}");
                Console.ForegroundColor = ConsoleColor.White;
            }
        }
    }
}