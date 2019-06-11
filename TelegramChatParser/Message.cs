using System;

namespace E2.Utils
{
    public class Message
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public string Author { get; set; }
        public DateTime DateTime { get; set; }
        public int? ReplyMessageId { get; set; }

        public Message(string content, string author, string dateTime, int? replyMessageId, int id)
        {
            string[] date = dateTime
                .Trim()
                .Split(' ')[0]
                .Split('.');

            string[] time = dateTime
                .Trim()
                .Split(' ')[1]
                .Split(':');

            DateTime = new DateTime(
                int.Parse(date[2]),
                int.Parse(date[1]),
                int.Parse(date[0]),
                int.Parse(time[0]),
                int.Parse(time[1]),
                int.Parse(time[2]));
            Id = id;
            Content = content;
            Author = author;
            ReplyMessageId = replyMessageId;
        }

        public override string ToString() =>
            $"{nameof(Id)}: {Id}, {nameof(ReplyMessageId)}: {ReplyMessageId}, {nameof(Author)}: {Author}, {nameof(DateTime)}: {DateTime}, {nameof(Content)}: {Content}";


        public string ToCsv() =>
            $"{Id},{ReplyMessageId},{Author},{DateTime},{Content?.Replace('\n', ' ')}";
    }
}