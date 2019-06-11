# Telegram chat parser
create `csv` file from html file of exported telegram data.

## Dependencies
+ [Html Agility Pack](https://github.com/zzzprojects/html-agility-pack)
+ [Html Agility Pack CssSelector](https://github.com/hcesar/HtmlAgilityPack.CssSelector)
+ [Commandline Parser](https://github.com/commandlineparser/commandline)

## Quick start

```csharp
public class Program
  {
    public static void Main(string[] args)
    {
      string inputHtml = @"C:\Users\Ali\Desktop\messages.html";
      string outputCsv = @"C:\Users\Ali\Desktop\chats.csv";

      TgParser tgParser = new TgParser(inputHtml, outputCsv);

      tgParser.CreateCsv();
    }
  }
```


## How to use
Download binary file `TelegramChatParser.exe` from [releases](https://github.com/ali4heydari/TelegramChatParser/releases).
```
prompt>TelegramChatParser.exe --help
TelegramChatParser 1.0.0.0
Copyright ©  2019

  -v, --verbose      (Default: false) Set output to verbose messages.

  -i, --html-path    Required. Set html file path to parse.

  -o, --csv-path     Required. Set csv file path with it's name to save. ex: C:\Users\Ali\Desktop\chats.csv

  -a, --append       Append to existing csv file.

  --help             Display this help screen.

  --version          Display version information.
```
