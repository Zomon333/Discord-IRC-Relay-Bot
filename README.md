# Discord-IRC-Relay-Bot
Code base for CSI-180 interface project; linking two notably different systems.

As stated in the title, this bot links an IRC channel to a discord server.
Currently, it's configured to my (now public) test server, and #nom on esper.net
Join the test server here: https://discord.gg/RJB2UTT

Instructions:
-Before launch, you should update the code to have the token for your personal bot. 
-Then, update the IRC channel info and server info to target the specific channels you want to link.
  (In the future, this may be done through a text file.)
-Launch the bot, and wait to see when it's online.
-Use the '~register' command in both discord and IRC to have the bot save you as the default owner for that instance.
-Once registered, the bot should relay text.

Current commands:
'~register' , sets default owner for that instance. IRC/Discord owners are separate for unique use cases.
'~makebreakfast' , sends an egg emoji to discord so that you can make breakfast.
