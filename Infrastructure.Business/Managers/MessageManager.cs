using AutoMapper;
using Domain.Interfaces.Repositories;
using Infrastructure.Business.Hubs;
using Infrastructure.Business.Interfaces;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Business.Managers
{
    public class MessageManager : BaseManager, IMessageManager
    {
        protected readonly IHubContext<MessageHub> messageHub;

        public MessageManager(IHubContext<MessageHub> hubcontext, IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
        {
            this.messageHub = hubcontext;
        }

        public void ShowMessage(string name, string user, string message)
        {
            messageHub.Clients.All.SendAsync("ShowToastMessage", user, message);
        }
    }
}
