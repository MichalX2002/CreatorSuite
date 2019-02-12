using System.Threading.Tasks;

namespace CreatorSuite
{
    /// <summary>
    ///  Definition of the (<see cref="ChatHub"/>) client's API.
    /// </summary>
    public interface IChatClient
    {
        Task ReceiveMessageFrom(string user, string message);
        Task ReceiveMessage(string message);
    }

}
