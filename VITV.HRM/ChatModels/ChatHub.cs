using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.Identity;
using VITV.HRM.Models;
using VITV.HRM.ChatModels;
using System.Collections.Generic;
using System;

namespace VITV.HRM
{
    public class MyIdProvider : IUserIdProvider
    {
        public string GetUserId(IRequest request)
        {
           return request.User.Identity.GetUserId();
        }
    }
    [Authorize]
    public class ChatHub: Hub
    {
        private static readonly List<ChatClient> clients = new List<ChatClient>();
        public override System.Threading.Tasks.Task OnConnected()
        {
            var userId = Context.User.Identity.GetUserId();
            var context = new VITVSecondContext();
            var user = context.Employees.Find(userId);
            var client = clients.Find(c => c.UserId == userId);
            if (client == null)
            {
                client = new ChatClient()
                {
                    UserId = userId,
                    DisplayName = user.Name,
                    Avatar = user.ProfilePicture,
                    ConnectionIds = new List<string> { Context.ConnectionId }
                };
                clients.Add(client);
            }
            else
                client.ConnectionIds.Add(Context.ConnectionId);
            
            Clients.Others.notifyUserOnline(client);
            return base.OnConnected();
        }
        public override System.Threading.Tasks.Task OnDisconnected(bool stopCalled)
        {
            var userId = Context.User.Identity.GetUserId();
            var context = new VITVSecondContext();
            var client = clients.Find(c => c.UserId == userId);
            if (client == null)
                return null;
            client.ConnectionIds.Remove(Context.ConnectionId);
            if (client.ConnectionIds.Count == 0)
            {
                Clients.Others.notifyUserOffline(client);
                clients.Remove(client);
            }
            return base.OnDisconnected(stopCalled);
        }
        public override System.Threading.Tasks.Task OnReconnected()
        {
            return base.OnReconnected();
        }

        public void GetOnlineUsers()
        {   
            Clients.Caller.receiveOnlineUsers(clients);
        }
        public void SendMessage(string userId, string message)
        {
            var senderId = Context.User.Identity.GetUserId();
            var sender = clients.Find(c=>c.UserId == senderId);
            Message mes = null;
            using (var context = new VITVSecondContext())
            {
                mes = new Message()
                    {
                        SenderId = senderId,
                        ReceiverId = userId,
                        MessageContent = message,
                        CreatedTime = DateTime.Now,
                    };
                context.Messages.Add(mes);
                context.SaveChanges();
            }
            Clients.User(userId).receiveMessage(senderId, sender.DisplayName, message, sender.Avatar, mes.CreatedTime.ToLongTimeString());
            Clients.Caller.sendMessageComplete(userId, sender.DisplayName, message, sender.Avatar, mes.CreatedTime.ToLongTimeString());
        }

        //public void SendGroupMessage(string message)
        //{
        //    var senderId = Context.User.Identity.GetUserId();
        //    var sender = clients.Find(c => c.UserId == senderId);
        //    using (var context = new VITVSecondContext())
        //    {
        //        context.GroupMessages.Add(new GroupMessage()
        //        {
        //            SenderId = senderId,
        //            MessageContent = message,
        //            CreatedTime = DateTime.Now,
        //        });
        //        context.SaveChanges();
        //    }
        //    Clients.All.receiveGroupMessage(senderId, sender.DisplayName, message);
        //}
    }
}