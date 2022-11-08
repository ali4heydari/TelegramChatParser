using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
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

                string[] input_files = Directory.GetFiles(Environment.CurrentDirectory, "messages*.html", SearchOption.AllDirectories);
                int files = 0;
                if (input_files.Length != 0) //try parse all file from Environment.CurrentDirectory
                {
                    files = input_files.Length;
                }
                if (files == 0) //parse from arg
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
                else //parse all files in Environment.CurrentDirectory
                {
                    for (int i = 0; i < files; i++)
                    {
                        TgParser tgParser = new TgParser(@input_files[i], @input_files[i].Replace(".html", ".csv"), false, true); //Verbose=false, Append=true
                        tgParser.CreateCsv();
                    }
                }
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