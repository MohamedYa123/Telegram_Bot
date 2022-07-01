using System;
using Telegram.Bot;
using System.IO;
using System.Threading;
using Telegram.Bot.Types;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.InlineQueryResults;
using Telegram.Bot.Types.InputFiles;
using Telegram.Bot.Types.ReplyMarkups;
using System.Threading.Tasks;
using System.Collections.Generic;
using Newtonsoft.Json;
using Telegram.Bot.Args;

namespace teleBot
{
    struct BotUpdate
    {
        public string text;
        public long id;
        public string? username;
    }
    class Program
    {

        public static TelegramBotClient bot;
        private static string filename = "updates.json";
        static List<BotUpdate> BotUpdates = new List<BotUpdate>();

        [Obsolete]
        static void Main(string[] args)
        {
            bot = new TelegramBotClient("5560478258:AAE1lzNAMgi4-kGAUswRXnrGAa-zJ5wflbI");
            //read updates
            try
            {
                var botupdatesstring = System.IO.File.ReadAllText(filename);
                BotUpdates = JsonConvert.DeserializeObject<List<BotUpdate>>(botupdatesstring) ?? BotUpdates;
                
            }
            catch(Exception ex) {
                Console.WriteLine(ex.Message);
            }
           // bot = new TelegramBotClient("5560478258:AAE1lzNAMgi4-kGAUswRXnrGAa-zJ5wflbI");
            bot.StartReceiving();
            bot.OnMessage += Bot_message;
            Console.ReadLine();
        }

        [Obsolete]
        private static void Bot_message(object sender, MessageEventArgs e)
        {
            bot.SendTextMessageAsync(e.Message.Chat.Id, "hello"+e.Message.Chat.Username);
        }
    }
}
