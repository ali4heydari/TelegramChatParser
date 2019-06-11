using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using CommandLine;
using HtmlAgilityPack;

namespace E2.Utils
{
    public class TgParser
    {
        private const string MESSAGES_QUERY_SELECTOR = "div.default[id^='message']";
        private const string MESSAGE_TEXT_QUERY_SELECTOR = "div.body > div.text";
        private const string MESSAGE_DATE_QUERY_SELECTOR = "div.body > div.pull_right.date.details";
        private const string MESSAGE_FROM_NAME_QUERY_SELECTOR = "div.body > div.from_name";
        private const string MESSAGE_REPLY_QUERY_SELECTOR = "div.body > div.reply_to.details > a";


        [Option(shortName: 'v', longName: "verbose", Default = false,
            HelpText = "Set output to verbose messages.")]
        public bool Verbose { get; set; }

        [Option(shortName: 'i', longName: "html-path", Required = true, HelpText = "Set html file path to parse.")]
        public string InputHtmlPath { get; set; }

        [Option(shortName: 'o', longName: "csv-path", Required = true,
            HelpText = "Set csv file path with it's name to save. ex: C:\\Users\\Ali\\Desktop\\chats.csv")]
        public string CsvFilePath { get; set; }

        [Option(shortName: 'a', longName: "append", HelpText = "Append to existing csv file.")]
        public bool Append { get; set; }


        public TgParser()
        {
        }

        public TgParser(string inputHtmlPath, string csvFilePath, bool verbose = false, bool append = false)
        {
            if (append == true && !File.Exists(csvFilePath))
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("Warning: Csv file not exists, Creating new file...");
                Console.ForegroundColor = ConsoleColor.White;
            }

            Verbose = verbose;
            InputHtmlPath = inputHtmlPath;
            CsvFilePath = csvFilePath;
            Append = append;
        }

        private List<Message> GetMessages()
        {
            var document = new HtmlDocument();
            document.Load(InputHtmlPath, encoding: Encoding.UTF8);

            var messageNodes = document.QuerySelectorAll(MESSAGES_QUERY_SELECTOR);

            Stack<Message> messagesStack = new Stack<Message>();


            try
            {
                foreach (var messageNode in messageNodes)
                {
                    int messageId = int.Parse(
                        messageNode
                            .Attributes["id"]
                            .Value.Substring(7));

                    // text can be null
                    string text = messageNode
                        .QuerySelector(MESSAGE_TEXT_QUERY_SELECTOR)
                        ?.InnerText.Trim();

                    string date = messageNode
                        .QuerySelector(MESSAGE_DATE_QUERY_SELECTOR)
                        .Attributes["title"].Value;

                    string author = messageNode
                        .QuerySelector(MESSAGE_FROM_NAME_QUERY_SELECTOR)
                        ?.InnerText.Trim();

                    int? replyMessageId = null;
                    if (messageNode.QuerySelector(MESSAGE_REPLY_QUERY_SELECTOR) != null)
                    {
                        replyMessageId = int.Parse(messageNode
                                                       .QuerySelector(MESSAGE_REPLY_QUERY_SELECTOR)
                                                       ?.Attributes["href"].Value.Substring(14) ??
                                                   throw new Exception());
                    }

                    messagesStack.Push(author != null
                        ? new Message(text, author, date, replyMessageId, messageId)
                        : new Message(text, messagesStack.Peek().Author, date, replyMessageId, messageId));
                }
            }
            catch (Exception e)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Exception thrown: \nMessage -> {e.Message}\nStackTrace -> {e.StackTrace}");
                Console.ForegroundColor = ConsoleColor.White;
            }

            return messagesStack.ToList();
        }


        public void CreateCsv()
        {
            List<Message> messages = GetMessages();

            using (StreamWriter sw = new StreamWriter(CsvFilePath, Append, encoding: Encoding.UTF8))
            {
                sw.WriteLine("MessageId,ReplyMessageId,Author,DateTime,Content");
                foreach (Message message in messages)
                {
                    if (Verbose)
                        Console.WriteLine(message.ToString());
                    sw.WriteLine(message.ToCsv());
                }
            }
        }
    }
}