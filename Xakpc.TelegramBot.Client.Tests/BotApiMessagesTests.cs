﻿// Copyright(C) 2015 by Pavel Osadchuk <xakz.pc@gmail.com>

//  This file is part of Xakpc.TelegramBot.Client.Tests.

//  Xakpc.TelegramBot.Client.Tests is free software: you can redistribute it and/or modify
//  it under the terms of the GNU General Public License as published by
//  the Free Software Foundation, either version 3 of the License, or
//  (at your option) any later version.
//
//  Xakpc.TelegramBot.Client.Tests is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.See the
//  GNU General Public License for more details.
//
//  You should have received a copy of the GNU General Public License
//  along with Xakpc.TelegramBot.Client.Tests. If not, see <http://www.gnu.org/licenses/>.

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NUnit.Framework;
using Xakpc.TelegramBot.Model;

namespace Xakpc.TelegramBot.Client.Tests
{
    [TestFixture]
    public class BotApiMessagesTests : BotApiTests
    {
        [Test]
        public async Task SendMessageAsync_Called_SuccesfullySentMessage()
        {
            var apiClient = ConstructClient();
            const string messageText = "SendMessageAsync_Called_SuccesfullySentMessage";

            var message = await apiClient.SendMessageAsync(TestChatId, messageText, null, null, null);

            Assert.That(message.Text, Is.EqualTo(messageText));
        }

        [Test]
        public async Task SendMessageAsyncWithBracket_Called_SuccesfullySentMessage()
        {
            var apiClient = ConstructClient();
            const string messageText = "[ru-test] [Hi] [Dog] SendMessageAsyncCalledSuccesfullySentMessage";

            dynamic t =
                JObject.Parse(
                    "{\"TalkId\":51432,\"Messages\":[{\"MessageId\":1101243430,\"Text\":\"Выберите проект:\",\"Photo\":null,\"LinkedEntities\":null}]}");

            var list = JsonConvert.DeserializeObject < List<List<string>>>("[[\"docxFiledfsdfsdf\"],[\"docxFileук6куенке\"],[\"[Project guide-DIN] [ru] [Перевод] Ольга Топор\"],[\"sample\"],[\"Приключения крошки енота 5\"],[\"Приключения крошки енота 4\"],[\"20161107-10\"],[\"20161107-9\"],[\"20161107-1\"],[\"Привет_тест\"],[\"Показать еще\"],[\"В меню\"]]");


            var reply = new ReplyKeyboardMarkup()
            {
                Keyboard = list,
                OneTimeKeyboard = true                
            };

            var message = await apiClient.SendMessageAsync(TestChatId, messageText, null, null, reply);

            Assert.That(message.Text, Is.EqualTo(messageText));
        }

        [Test]
        public async Task SendFormattedMessageAsync_Called_SuccesfullySentMessage()
        {
            var apiClient = ConstructClient();
            const string messageText = "SendMessageAsync_Called_SuccesfullySentMessage";

            var message = await apiClient.SendMessageAsync(TestChatId, $"*{messageText}*", "Markdown", null, null, null);

            Assert.That(message.Text, Is.EqualTo(messageText));
        }

        [Test]
        public async Task SendFormattedComplexMessageAsync_Called_SuccesfullySentMessage()
        {
            var apiClient = ConstructClient();
            const string messageText = @"ChatBots Hub
[Grupo en Telegram](https://telegram.me/joinchat/AdBIOgbEcVYDywy_Ri1KTw) 
[Canal de Noticias de ChatBots en Telegram](https://telegram.me/chatbotshub)
[Grupo en Facebook](https://www.facebook.com/groups/chatbotshub)";

            var message = await apiClient.SendMessageAsync(TestChatId, $"{messageText}", "Markdown", null, null, null);

            Assert.That(message.Text, Is.EqualTo(messageText));
        }

        [Test]
        public async Task SendMessageAsync_WithKeyboardHide_SuccesfullyHideKeyboard()
        {
            var apiClient = ConstructClient();
            const string messageText = "SendMessageAsync_WithKeyboardHide_SuccesfullyHideKeyboard";
            var tempKeyboard = new ReplyKeyboardHide() { HideKeyboard = true };

            var message = await apiClient.SendMessageAsync(TestChatId, messageText, null, null, tempKeyboard);

            Assert.That(message.Text, Is.EqualTo(messageText));
        }

        [Test]
        public async Task SendMessageAsync_WithKeyboardMarkup_SuccesfullySentMessage()
        {
            var apiClient = ConstructClient();
            const string messageText = "SendMessageAsync_WithKeyboardMarkup_SuccesfullySentMessage";
            var tempKeyboard = new ReplyKeyboardMarkup
            {
                Keyboard = new List<List<string>>
                {
                    new List<string> {"A", "B"},
                    new List<string> {"C", "D"}
                },

                Selective = true
            };

            var message = await apiClient.SendMessageAsync(TestChatId, messageText, null, null, tempKeyboard);

            Assert.That(message.Text, Is.EqualTo(messageText));
        }

        [Test]
        public async Task ForwardMessageAsync_Called_SuccesfullyForwarded()
        {
            var apiClient = ConstructClient();
            var incoming = await apiClient.GetUpdatesAsync(timeout: 60); // need some mesage to determine chat
            Assert.That(incoming, Has.Count.AtLeast(1), "need some incoming mesage to determine chat");
            var lastMessage = incoming.Last();

            var message = await apiClient.ForwardMessageAsync(lastMessage.Message.Chat.Id, lastMessage.Message.Chat.Id, lastMessage.Message.MessageId);

            Assert.That(message.Text, Is.EqualTo(lastMessage.Message.Text));
        }

        [Test]
        public async Task SendMessageAsync_WithInlineKeyboardMarkup_SuccessfullySentMessage()
        {
            var apiClient = ConstructClient();
            const string messageText = "SendMessageAsync_WithInlineKeyboardMarkup_SuccessfullySentMessage";
            var tempKeyboard = new InlineKeyboardMarkup
            {
                InlineKeyboard = new List<List<InlineKeyboardButton>>
                {
                    new List<InlineKeyboardButton>
                    {
                        new InlineKeyboardButton { Text = "A", Url = "https://telegram.org/" },
                        new InlineKeyboardButton { Text = "B", Url = "https://telegram.org/" }
                    },
                    new List<InlineKeyboardButton>
                    {
                        new InlineKeyboardButton { Text = "C", CallbackData = "callback1" },
                        new InlineKeyboardButton { Text = "D", CallbackData = "callback2" }
                    }
                }
            };

            var message = await apiClient.SendMessageAsync(TestChatId, messageText, null, null, tempKeyboard);

            Assert.That(message.Text, Is.EqualTo(messageText));
        }
    }
}