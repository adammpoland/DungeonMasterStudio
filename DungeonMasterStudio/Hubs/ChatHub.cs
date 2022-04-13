using DungeonMasterStudio.Data;
using DungeonMasterStudio.Models;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace DungeonMasterStudio.Hubs
{
    public class ChatHub : Hub
    {
        private readonly ApplicationDbContext _context;

        public ChatHub(ApplicationDbContext context)
        {
            _context = context;
        }
        public void SendChatMessage(string who, string message, string fileData)
        {
            var name = Context.User.Identity.Name;

            var user = _context.Users.Where(x => x.UserName == who).FirstOrDefault();

            if (user == null)
            {
                Clients.Caller.SendAsync("showErrorMessage");
            }
            else
            {
                _context.Entry(user)
                    .Collection(u => u.Connections)
                    .Query()
                    .Where(c => c.Connected == true)
                    .Load();

                if (user.Connections == null)
                {
                    Clients.Caller.SendAsync("showErrorMessage");
                }
                else
                {
                    foreach (var connection in user.Connections)
                    {
                        Clients.Client(connection.ConnectionID).SendAsync("ReceiveMessage", Context.User.Identity.Name, message, fileData);
                    }
                }
            }
        }

        public async Task AddToGroup(string groupName)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);

            await Clients.Group(groupName).SendAsync("Send", $"{Context.ConnectionId} has joined the group {groupName}.");
        }

        public async Task RemoveFromGroup(string groupName)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupName);

            await Clients.Group(groupName).SendAsync("Send", $"{Context.ConnectionId} has left the group {groupName}.");
        }

        //public void AttackEnemy(string enemyID, string message)
        //{
        //    var name = Context.User.Identity.Name;

        //    var Enemy = _context.Enemies.Where(x => x.EnemyID == enemy)

        //    if (Enemy == null)
        //    {
        //        Clients.Caller.SendAsync("NoEnemy");
        //    }
        //    else
        //    {
        //        //You will still need to have the user info for the dungeonmaster so you can post results to their feed
        //        _context.Entry(user)
        //            .Collection(u => u.Connections)
        //            .Query()
        //            .Where(c => c.Connected == true)
        //            .Load();

        //        if (user.Connections == null)
        //        {
        //            Clients.Caller.SendAsync("showErrorMessage");
        //        }
        //        else
        //        {
        //            foreach (var connection in user.Connections)
        //            {
        //                Clients.Client(connection.ConnectionID).SendAsync("ReceiveMessage", user.UserName, message);
        //            }
        //        }
        //    }

        //}

        public override Task OnConnectedAsync()
        {
            var name = Context.User.Identity.Name;

            var user = _context.Users
                .Include(u => u.Connections)
                .SingleOrDefault(u => u.UserName == name);

            if (user == null)
            {
                user = new ApplicationUser
                {
                    UserName = name,
                    Connections = new List<Connection>()
                };
                _context.Users.Add(user);
            }

            user.Connections.Add(new Connection
            {
                ConnectionID = Context.ConnectionId,
                UserAgent = Context.UserIdentifier,
                Connected = true
            });
            _context.SaveChanges();

            ApplicationUser partyMember = _context.Users.Where(x => x.Id == user.Id).FirstOrDefault();
            Party party = _context.Parties.Where(x => x.Members.Contains(partyMember)).FirstOrDefault();
            AddToGroup(party.PartyID.ToString());   
            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception e)
        {

            var connection = _context.Connections.Find(Context.ConnectionId);
            connection.Connected = false;
            _context.SaveChanges();

            return base.OnDisconnectedAsync(e);
        }
    }
}
