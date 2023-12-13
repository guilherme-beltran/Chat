namespace Chat.Domain.Interfaces
{
    internal interface IChatServices
    {
        bool AddUserToList(string userToAdd);
        Task AddUserConnectionId(string user, string connectionId);
        string GetUserByConnectionId(string connectionId);
        string GetConnectionIdByUser(string user);
        Task RemoveUser(string user);
        string[] GetOnlineUsers();
    }
}
