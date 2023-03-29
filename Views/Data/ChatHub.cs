using ArmsModels.BaseModels;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace Views.Data
{
    public class ChatHub:Hub
    {
        public async Task SendMessages(int MessageID,BranchModel InitiateBranch, BranchModel ReceivedBranch, string MessageTitle, string MessageBody, int RedirectedTo, string MsgDatest)
        {
            await  Clients.All.SendAsync("ReceiveMessage", MessageID, InitiateBranch, ReceivedBranch, MessageTitle, MessageBody, RedirectedTo, MsgDatest);
        }
    }
}
