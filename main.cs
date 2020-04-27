using Discord;
using ChatSharp;
using System;
using Discord.WebSocket;
using Discord.Commands;
using System.Threading.Tasks;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
/*
 * OKAY SO, This is mine. I made it. It's SO VERY MESSY but I guess that's just how you *know* it's mine.
 * I know C++, and can follow the coding standards for that, but it won't take very long for you to recognize that this is C#.
 * I cannot follow the C++ coding standards in C#; I just don't know how, I've never had formal teaching.
 * This is an almalgamation of everything I've learned from both languages, a bit of documentation reading, a bit of stackoverflow, and some long drawn out periods of exasperation.
 * 
 * I hate async things.
 * This is as organized as I can physically get my code and I don't have a copy of the certificate of authenticity on me.
 * 
 * Code may be messy, so to compensate I commented to explain what the heck I'm doing.
 */

namespace NomBot
{

    class Program
    {
        #region Static Member Variables
        static string originalUser = ""; //Used for IRC ownership tracking
        static bool hasOwner = false; //Used for IRC ownership tracking
        const string TOKEN = ""; //BOT TOKEN, DO NOT RELEASE PUBLICLY

        const long DISCORD_SERVER_ID = 299653823503400960;
        static SocketUser originalDiscordUser; //Used for Discord ownership tracking
        static bool hasOwnerDiscord = false; //Used for Discord ownership tracking
        static string loginstate; //Used to determine login state as a non-async method


        static DiscordSocketClient NBCD = new DiscordSocketClient(new DiscordSocketConfig
        {
            LogLevel = LogSeverity.Debug,
            DefaultRetryMode = RetryMode.AlwaysRetry,
        });

        static CommandService Commands = new CommandService(new CommandServiceConfig
        {
            CaseSensitiveCommands = true,
            DefaultRunMode = RunMode.Async,
            LogLevel = LogSeverity.Debug
        });

        public static IrcChannel BoundChannel;

        #endregion


        //Discord message processing, relaying, & ownership.
        private async static Task<Task> NBCD_MessageRecieved(SocketMessage arg)
        {
            //This doesn't do anything, but the Discord.Net library demands I return a task and I don't know enough about async stuff to tell it to not.
            //It took me like two hours and fifteen websites to understand enough about it to get this written. I've seen this before,
            //but I normally just use pre-made ones for commands. It's easier for me that way. This one isn't pre-made, so that it's acceptable by coding standards.
            Action<Discord.WebSocket.SocketMessage> thingsDone = (arg2) => { };
            Task task = Task.Run(() => thingsDone);

            //A small pseudocommand, not actually using command interface, to set ownerhsip of the bot.
            if (arg.Content.ToString().Contains("~register") && Program.hasOwnerDiscord == false)
            {
                //Program.defaultGuild;
                //arg.Channel.Id;
                ulong id = arg.Id;
                int i = 0;
                while(i<NBCD.Guilds.Count)
                {
                    if(NBCD.Guilds.ElementAt(i).DefaultChannel.GetMessagesAsync(1)==arg)
                    {
                        Globals.defaultGuildiD = NBCD.Guilds.ElementAt(i).Id;
                        Console.WriteLine("Guild found. User has registered " + NBCD.GetGuild(Globals.defaultGuildiD) + " as the defaultGuild.");
                        i = NBCD.Guilds.Count;
                    }
                    i++;
                }
                i--;
                Globals.defaultGuildiD = NBCD.Guilds.ElementAt(i).Id;
                Console.WriteLine(Globals.defaultGuildiD);

                Program.hasOwnerDiscord = true;
                Program.originalDiscordUser = arg.Author;
                Console.WriteLine("Discord ownership registered to " + arg.Author.Username.ToString());
            }

            //Custom command for legolopi
            if (arg.Content.ToString().Contains("~make breakfast"))
            {
                arg.Channel.SendMessageAsync(":egg:");
            }

            //Updating the global message with a new copy.
            Globals.discordMessage = new TransferMessage(arg.Author.Username, arg.Channel.Name, "DSC", arg.Content.ToString(), false, true);

            //Relaying the message if it's not already in IRC and if it's not from NomBot.
            if (Globals.discordMessage.inIRC == false && hasOwner && hasOwnerDiscord && Globals.discordMessage.user != "NomBot")
            {
                Globals.discordMessage.inIRC = true;
                BoundChannel.SendMessage(Globals.discordMessage.getPrint().ToString());
            }

            //Write the message out to the console, for error checking purposes.
            Console.WriteLine(Globals.discordMessage.getPrint().ToString());
            return task;
        }

        static async Task<int> Main(string[] args) //Don't question what this stands for, because it works and intellisense clearly knows what to suggest better than me.
        {
            //Initializing and/or resetting the messages so that we don't need to worry about leftover data, hopefully.
            Globals.discordMessage = new TransferMessage();
            Globals.ircMessage = new TransferMessage();
            
            #region Discord Login

            //Enables asynchronous functions & stuff
            await NBCD.StartAsync();

            //Logs in asynchronously
            loginstate = NBCD.LoginState.ToString(); //Get loginstate as string
            if (loginstate == "LoggedOut") await NBCD.LoginAsync(TokenType.Bot, TOKEN, true); //Check if logged out, if so then login

            //Adds the command interface that I normally need but didn't use.
            await Commands.AddModulesAsync(Assembly.GetEntryAssembly(), null);
            #endregion

            #region IRC Login
            //Declares a new IRC Bot
            var NBCIRC = new IrcClient("irc.esper.net", new IrcUser("NomBot", "NomBot"));

            //Waits until it can connect, and then connects to #nom
            NBCIRC.ConnectionComplete += (s, e) =>
            {
                NBCIRC.JoinChannel("#nom");
            };
            #endregion

            #region IRC Message Handling

            //Upon recieving a message, it breaks it down into local variables that are easier to work with.
            NBCIRC.ChannelMessageRecieved += (s, e) =>
            {

                string message = e.PrivateMessage.Message.ToString();
                string channel = NBCIRC.Channels[e.PrivateMessage.Source].Name.ToString();
                string user = e.PrivateMessage.User.Nick.ToString();
                string userRaw = e.PrivateMessage.User.ToString();

                //Again; not using command interface, a pseudocommand for registering the bot before use.
                if (!hasOwner && message.Contains("~register"))
                {
                    BoundChannel = NBCIRC.Channels[e.PrivateMessage.Source];
                    Console.WriteLine("This instance has no owner! Giving user " + userRaw + " owner priviledge by first use.");
                    NBCIRC.Channels[e.PrivateMessage.Source].SendMessage(userRaw+" registered to instance owner!");
                    hasOwner = true;
                    originalUser = userRaw;
                }

                

                //ircMessage() = "<" + channel + " | " + user + "> " + message;
                Globals.ircMessage = new TransferMessage(user, channel, "IRC", message, true, false);
                Console.WriteLine(Globals.ircMessage.getPrint());
                
                //A rudimentary command not using command interface in order for the owner to manually request the bot to join new channels.
                //Requires console verification for safety.
                if(userRaw == originalUser && message.Contains("~join "))
                {
                    message = message.Remove(0, 5);
                    NBCIRC.Channels[e.PrivateMessage.Source].SendMessage("Joining channel " + message + ". Please confirm in console.");
                    Console.WriteLine("Join "+message+"? Y/N: ");
                    string choice = "";
                    choice = Console.ReadLine();
                    if(choice=="Y")
                    {
                        NBCIRC.Channels[e.PrivateMessage.Source].SendMessage("Join confirmed. Joining channel " + message+".");
                        Console.WriteLine("Joining " + message + ".");
                        NBCIRC.JoinChannel(message);
                    }
                    else
                    {
                        NBCIRC.Channels[e.PrivateMessage.Source].SendMessage("Join request denied.");
                    }
                }

                //Relays messages from IRC to discord if they haven't been already and they weren't sent from the bot itself.
                if (hasOwnerDiscord && !Globals.ircMessage.inDiscord && hasOwner && Globals.ircMessage.user != "NomBot")
                {
                    Globals.ircMessage.inDiscord = true;
                    NBCD.GetGuild(Globals.defaultGuildiD).DefaultChannel.SendMessageAsync(Globals.ircMessage.getPrint().ToString(), false, null, null);
                }
            };
            #endregion

            #region Discord Message Handling

            //Calls the async nonsense at the head of the program
            NBCD.MessageReceived += NBCD_MessageRecieved;

            #endregion

            //Makes sure the bot stays connected
            NBCIRC.ConnectAsync();

            //Sets the bot's status for amusement
            await NBCD.SetGameAsync("NomBot Incorporated");
            while (true) ;
        }

         
    }
}



