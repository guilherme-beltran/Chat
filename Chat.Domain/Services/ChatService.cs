using Chat.Domain.Interfaces;

namespace Chat.Domain.Services
{
    internal class ChatService : IChatServices
    {
        private readonly static Dictionary<string, string> Users = new Dictionary<string, string>();

        public bool AddUserToList(string userToAdd)
        {
            lock(Users)
            {
                foreach(var user in Users)
                {
                    if (user.Key.ToLower() == userToAdd.ToLower())
                    {
                        return false;
                    }
                }

                Users.Add(userToAdd, null);
                return true;
            }
        }

        public async Task AddUserConnectionId(string connectionId, string user)
        {
            lock(Users)
            {
                if (Users.ContainsKey(user))
                {
                    Users[user] = connectionId;
                }
            }
        }

        public string GetUserByConnectionId(string connectionId)
        {
            lock(Users)
            {
                return Users.Where(x => x.Value.Equals(connectionId)).Select(x => x.Key).FirstOrDefault();
            }
        }

        public string GetConnectionIdByUser(string user)
        {
            lock (Users)
            {
                return Users.Where(x => x.Key.Equals(user)).Select(x => x.Value).FirstOrDefault();
            }
        }

        public async Task RemoveUser(string user)
        {
            lock (Users)
            {
                Users.Remove(user);
            }
        }

        public string[] GetOnlineUsers()
        {
            lock (Users)
            {
                return Users.OrderBy(x => x.Key).Select(x => x.Key).ToArray();
            }
        }
    }
}
