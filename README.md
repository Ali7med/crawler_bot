
# Crawler C# with Telegram bot


Crawler C# console application that crawler page and send feed with telegram bot


## Environment Variables

To run this project, you will need to add the following environment variables to your Program.cs

`Telegram-bot-Token`




## Usage/Examples

Before you start, you need to talk to @BotFather on Telegram. Create a new bot, acquire the bot token and get back here.
Bot token is a key that required to authorize the bot and send requests to the Bot API. Keep your token secure and store it safely, it can be used to control your bot. It should look like this:

```
1234567:4TT8bAc8GHUspu3ERYn-KGcvsvGB9u_n4ddy
```
Now that you have a bot, it's time to bring it to life! Create a new console project for your bot. It could be a legacy project targeting .NET Framework 4.6.1-4.8 or a modern .NET Core 3.1-.NET 5+.
```
dotnet new console
```
Add a reference to Telegram.Bot package:
```
dotnet add package Telegram.Bot
```
This code fetches Bot information based on its access token by calling the Bot API getMe method. Open Program.cs and use the following content:
```
⚠️ Replace {YOUR_ACCESS_TOKEN_HERE} with your access token from the @BotFather.
```
Now take the YOUR_ACCESS_TOKEN_HERE and placed in 
```
var botClient = new TelegramBotClient("Telegram-bot-Token");
```
Now can send and receive the data through the app.

## Authors

- [@ali7med](https://github.com/Ali7med)



## License

[MIT](https://choosealicense.com/licenses/mit/)

