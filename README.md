# Discord-IRC-Relay-Bot
Code base for CSI-180 interface project; linking two notably different systems.<br>

As stated in the title, this bot links an IRC channel to a discord server.<br>
Currently, it's configured to my (now public) test server, and #nom on esper.net<br>
Join the test server here: https://discord.gg/RJB2UTT <br>
<br>
Instructions: <br>
-Before launch, you should update the code to have the token for your personal bot. <br>
-Then, update the IRC channel info and server info to target the specific channels you want to link.<br>
  (In the future, this may be done through a text file.)<br>
-Launch the bot, and wait to see when it's online.<br>
-Use the '~register' command in both discord and IRC to have the bot save you as the default owner for that instance.<br>
-Once registered, the bot should relay text.<br>
<br>
Current commands:<br>
<br>
'~register' , sets default owner for that instance. IRC/Discord owners are separate for unique use cases.<br>
'~makebreakfast' , sends an egg emoji to discord so that you can make breakfast.<br>
'~join #channel' , allows the bot to join a new IRC channel; will relay messages from the channel to the discord, but not across other IRC channels. Requires console verification.<br>
