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
        public static botmanager Bot;
        static void makelangenglish()
        {
            Bot.mydata.setlang("English");
            Bot.mydata.addresponse("hi", @"hi 'user'[FirstName] ","hi");//'user'[LastName]
            Bot.mydata.resps[Bot.mydata.resps.Count - 1].setkey("~s~ 'bot'[Name] ~s~");
            Bot.mydata.addmoretoresponse("hi", "hello");
            Bot.mydata.addmoretoresponse("hi", "'bot'[Name]", "hello");
            Bot.mydata.addmoretoresponse("hi", "" ,"hi");
          //  Bot.mydata.addmoretoresponse("hi", "" , @"hello 'user'[FirstName] 'user'[LastName]");
            Bot.mydata.addmoretoresponse("hi", "" , @"hello 'user'[FirstName]");
            Bot.mydata.addmoretoresponse("hi", "" , @"hi 'user'[FirstName]");
            Bot.mydata.errs.Add("Only leaders can do this");
            Bot.mydata.errs.Add("Type correct password 'data'{openresponse}<privateaddleader-1,id>");
            Bot.mydata.errs.Add("");
            Bot.mydata.errs.Add("Invalid Message");
            Bot.mydata.errs.Add("Invalid Message");
            Bot.mydata.errs.Add("Invalid password");
            Bot.mydata.sucess.Add("Reply added successfully");
            Bot.mydata.sucess.Add("Leader added successfully");
            Bot.mydata.sucess.Add("Leader added successfully");
            Bot.mydata.sucess.Add("Gender modifed to 'bot'[sgender]");
            Bot.mydata.sucess.Add("Request added successfully");
            Bot.mydata.sucess.Add("Request added successfully");
            Bot.mydata.sucess.Add("Timed message created :\n'data'[lasttimed_reply]\neach 'data'[stime] seconds");
            Bot.mydata.infos.Add("");
            Bot.mydata.infos.Add("Leader already exists");
            Bot.mydata.addresponse("make me leader ~s~", "'bot'{addleader}<^'~s~',id>", "hi2");
            Bot.mydata.resps[Bot.mydata.resps.Count - 1].setkey("~s~ 'bot'[Name] ~s~");
            Bot.mydata.addresponse("say hi", "'data'{runresponse}<hi-1,id>", "hn");
            Bot.mydata.resps[Bot.mydata.resps.Count - 1].setkey("~s~ 'bot'[Name] ~s~");
            Bot.mydata.addresponse("gender", "(&I'm Boy,I'm girl&)", "hn");
            Bot.mydata.resps[Bot.mydata.resps.Count - 1].setkey("~s~ 'bot'[Name] ~s~");
            Bot.mydata.addresponse("~s~", "'bot'{addleaderwithoutcopy}<^'~s~',id>", "privateaddleader");
            Bot.mydata.resps[Bot.mydata.resps.Count - 1].type = 2;
          //  Bot.mydata.resps[Bot.mydata.resps.Count - 1].leader_resp = true;
            Bot.mydata.addresponse("add response ~s,s,s~", "'data'{addresponse}<^'~s~',^'~s~',^'~s~'>","response add");
            Bot.mydata.resps[Bot.mydata.resps.Count - 1].setkey("~s~ 'bot'[Name] ~s~");
            Bot.mydata.resps[Bot.mydata.resps.Count - 1].leader_resp = true;
            Bot.mydata.resps[Bot.mydata.resps.Count - 1].seterrmessage(Bot.mydata.errs.Count.ToString());
            Bot.mydata.errs.Add("Enter request message  'data'{openresponse}<setreq-1,id>");
            Bot.mydata.resps[Bot.mydata.resps.Count - 1].leader_resp = true;
            Bot.mydata.addresponse("~s~", "Enter reply message  'data'{setrequest}<^'~s~'> 'data'{openresponse}<setresp-1,id>", "setreq");
            Bot.mydata.resps[Bot.mydata.resps.Count - 1].leader_resp = true;
            Bot.mydata.resps[Bot.mydata.resps.Count - 1].type = 2;
            Bot.mydata.addresponse("~s~", "Enter Key  'data'{setreply}<^'~s~'> 'data'{openresponse}<assure-1,id>", "setresp");
            Bot.mydata.resps[Bot.mydata.resps.Count - 1].leader_resp = true;
            Bot.mydata.resps[Bot.mydata.resps.Count - 1].type = 2;
            Bot.mydata.addresponse("~s~", "Assure your request \nrequest message : \"'data'[prerequest]\"\nreply message : \"'data'[preresponse]\"\nkey message : \"'data'[prekey]\" \n yes to aply , no to cancel 'data'{setrkey}<^'~s~'> 'data'{openresponse}<finalone-2,id>", "assure");
            Bot.mydata.resps[Bot.mydata.resps.Count - 1].leader_resp = true;
            Bot.mydata.resps[Bot.mydata.resps.Count - 1].type = 2;
            Bot.mydata.addresponse("yes", "'data'{quickaddrequest}<>", "finalone");
            Bot.mydata.addmoretoresponse("finalone", "no", "Request canceled");
            Bot.mydata.resps[Bot.mydata.resps.Count - 1].type = 2;
            Bot.mydata.resps[Bot.mydata.resps.Count - 1].response_rule = 1;
            Bot.mydata.resps[Bot.mydata.resps.Count - 1].leader_resp = true;
            Bot.mydata.addresponse("set timed response ~s~,~s~", "'data'{addtimereq}<^~s~,^~s~,chatid>", "ftime");
            Bot.mydata.resps[Bot.mydata.resps.Count - 1].leader_resp = true;
            Bot.mydata.errs.Add("write it correctly \"set timed response responsekey,seconds\"");
            Bot.mydata.resps[Bot.mydata.resps.Count - 1].errmessage = Bot.mydata.errs.Count - 1;
            Bot.mydata.addresponse("Throw dice", @"<b>I will try my luck</b> 'data'{runresponse}<dice1-1,id>", "dice");//"<html>< head >< title > Telegram </ title ></ head >< body >< form method = \"GET\" action = \"https://api.telegram.org/bot(token)/sendMessage\" >< input type = \"hidden\" name = \"chat_id\" value = \"@testadminch\" >< input type = \"hidden\" name = \"parse_mod\" value = \"markdown\" >< textarea name = \"text\" ></ textarea >< input type = \"submit\" value = \"Submit\" ></ form ></ body ></ html >"
            Bot.mydata.resps[Bot.mydata.resps.Count - 1].setkey("~s~ 'bot'[Name] ~s~");
            Bot.mydata.addresponse("", "[1]", "dice1");
            Bot.mydata.resps[Bot.mydata.resps.Count - 1].type = 2;
            Bot.mydata.addmoretoresponse("dice1", "", "[2]");
            Bot.mydata.addmoretoresponse("dice1", "", "[3]");
            Bot.mydata.addmoretoresponse("dice1", "", "[4]");
            Bot.mydata.addmoretoresponse("dice1", "", "[5]");
            Bot.mydata.addmoretoresponse("dice1", "", "[6]");

            //   Bot.mydata.addresponse("~s~ 'bot'[Name] ~s~", "'data'{runresponse}<hi-1,id>", "hipublic");
            //   Bot.mydata.resps[Bot.mydata.resps.Count - 1].type = 1;
            Bot.setlang("English");
        }
        [Obsolete]
        static void Main(string[] args)
        {
            bot = new TelegramBotClient("5560478258:AAE1lzNAMgi4-kGAUswRXnrGAa-zJ5wflbI");
            string username="admin";
            string password="admin";
            Bot = new botmanager(username,password);
            Bot = botmanager.load(path);
            for (int i = 0; i < Bot.mydata.timreqs.Count; i++)
            {
                 Bot.mydata.timreqs[i].nexttime=0;
            }
            Bot.getpass();
            makelangenglish();
            //read updates
           // bot = new TelegramBotClient("5560478258:AAE1lzNAMgi4-kGAUswRXnrGAa-zJ5wflbI");
            bot.StartReceiving();
            
            bot.OnMessage += Bot_message;
            bot.OnUpdate += Bot_update;
            ThreadStart thr = new ThreadStart(backgroundmessaging);
            Thread th = new Thread(thr);
            th.Start();
            Console.ReadLine();
        }
        //kick user found , ban user found
        [Obsolete]
        private static void Bot_update(object sender, UpdateEventArgs e)
        {
            var s = e.Update.Message.From.Id;
            
            if (e.Update.Message.ReplyToMessage!=null && e.Update.Message.ReplyToMessage.From.Id == bot.BotId)
            {
                //bot.KickChatMemberAsync
                string texto = "<i> italic </i><img src=\"https://www.gravatar.com/avatar/270943dfff93c34049177c40a2cbd061?s=64&d=identicon&r=PG\" alt=\"Girl in a jacket\" width=\"500\" height=\"600\">";
               //@"<i> italic </i>, <em> italic </em>" +
              // @"<a href = 'http://www.example.com/'> inline URL </a>" +
              // @"<a href = 'tg://user?id=123456789'> inline mention of a user</a>" +
              // @"<code> inline fixed-width code </code>" + @"<pre> pre - formatted fixed-width code block</pre>";
                string sj = "<h1>go<h1>";
                //bot.SendTextMessageAsync(e.Update.Message.Chat.Id, texto,ParseMode.Html);
                //bot.SendVideoAsync(e.Update.Message.Chat.Id, video: "https://www.youtube.com/watch?v=kZIXa-JNawY");
               // bot.SendPhotoAsync(e.Update.Message.Chat.Id,photo: @"https://upload.wikimedia.org/wikipedia/commons/thumb/b/b6/Image_created_with_a_mobile_phone.png/1200px-Image_created_with_a_mobile_phone.png");
               bot.SendTextMessageAsync(e.Update.Message.Chat.Id, Bot.recievereply(e.Update.Message.Text,e.Update.Message.From,e.Update) ,ParseMode.Html);
               bot.SendTextMessageAsync(e.Update.Message.Chat.Id, Bot.mydata.getrunresponse(e.Update.Message.From));
            }
            else
            {
                bot.SendTextMessageAsync(e.Update.Message.Chat.Id, Bot.recievereply(e.Update.Message.Text, e.Update.Message.From,e.Update,mention:false), ParseMode.Html);
                bot.SendTextMessageAsync(e.Update.Message.Chat.Id, Bot.mydata.getrunresponse(e.Update.Message.From));
            }
            
        }
        public static int now_secs = 0;
        public static void backgroundmessaging()
        {
            while(true){
                for (int i=0;i< Bot.mydata.timreqs.Count;i++)
                {
                    var l = Bot.mydata.timreqs[i];
                    // Console.WriteLine(l.resp.uniquekey);
                    //   l.activate(now_secs, new User(), Bot, Bot.mydata);
                    User uu = new User();
                    bot.SendTextMessageAsync(l.chatid, l.activate(now_secs,uu, Bot, Bot.mydata) + " ");
                    bot.SendTextMessageAsync(l.chatid, Bot.mydata.getrunresponse(uu));
                }
                now_secs++;
                Thread.Sleep(1000);
                Bot.saveme(path);
            }
        }
        static string path = "Botdata.x"; 
        [Obsolete]
        private static void Bot_message(object sender, MessageEventArgs e)
        {
           // bot.SendTextMessageAsync(e.Message.Chat.Id, "hello "+ e.Message.Chat.LastName);
           // ChatMember ch = new ChatMember();// e.Message.Chat.Username;
           
        }
    }
}
