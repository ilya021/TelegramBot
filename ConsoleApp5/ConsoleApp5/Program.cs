using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using Telegram.Bot.Types.InlineKeyboardButtons;
namespace ConsoleApp5
{
    class Program
    {
        //static TelegramBotClient Bot;
        static readonly TelegramBotClient Bot = new TelegramBotClient("727563345:AAF-KfCs3m5ndYzia2ECTCEGf-AH9WuGsMQ");

        static void Main(string[] args)
        {
            //Bot = new TelegramBotClient("727563345:AAF-KfCs3m5ndYzia2ECTCEGf-AH9WuGsMQ");

            Bot.OnMessage += BotOnMessageReceived; //для получения сообщений от пользователя
            Bot.OnCallbackQuery += BotOnCallbackQueryReceived;

            //var me = Bot.GetMeAsync().Result; //сохранение имени бота
            //Console.WriteLine(me.FirstName); //вывод в консоль имени бота

            Bot.StartReceiving();
            Console.ReadLine();
            Bot.StopReceiving();
        }

        private static async void BotOnCallbackQueryReceived(object sender, Telegram.Bot.Args.CallbackQueryEventArgs e)
        {
            string buttonText = e.CallbackQuery.Data; //выбранная кнопка
            string name = $"{e.CallbackQuery.From.FirstName}{e.CallbackQuery.From.LastName}";
            Console.WriteLine($"{name} нажал кнопку {buttonText}");

            await Bot.AnswerCallbackQueryAsync(e.CallbackQuery.Id, $"Вы нажали кнопку {buttonText}");
            
        }

        private static async void BotOnMessageReceived(object sender, Telegram.Bot.Args.MessageEventArgs e)
        {
            var message = e.Message; //сообщение от пользователя

            if (message == null || message.Type != MessageType.TextMessage)//если сообщение не текст - выходим из функции
                return;

            string name = $"{message.From.FirstName} {message.From.LastName}"; //имя пользователя
            Console.WriteLine($"{name} отправил сообщение {message.Text}");

            switch(message.Text)
            {
                case "/start":
                    string text =
@"Cписок команд:
/start - запуск бота
/inline - вывод меню 
/keyboard - вывод клавиатуры";
                    await Bot.SendTextMessageAsync(message.From.Id, text); //чат 
                    break;
                case "/inline":
                    //кнопки
                    var inlineKeyboard = new InlineKeyboardMarkup(new[]
                    {
                        new[]
                        {
                            InlineKeyboardButton.WithCallbackData("Пункт 1.1"),
                            InlineKeyboardButton.WithCallbackData("Пункт 1.2")
                        },
                        new[]
                        {
                            InlineKeyboardButton.WithCallbackData("Пункт 2.1"),
                            InlineKeyboardButton.WithCallbackData("Пункт 2.2")
                        }


                    });
                    await Bot.SendTextMessageAsync(message.From.Id, "Выберите", replyMarkup: inlineKeyboard);
                    break;
                case "/keyboard":
                    var replyKeyboard = new ReplyKeyboardMarkup(new[]
                    {
                        new KeyboardButton("Хочу узнать погоду")
                    });
                    await Bot.SendTextMessageAsync(message.Chat.Id, "Сообщение ", replyMarkup: replyKeyboard);
                    break;
                default:
                    break;


            }

        }
    }
}
