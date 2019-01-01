public class FriendMessages
{
    public string friendName;
    public Msgs messages;

    public void AddMessage (Msg msg)
    {
        messages.messages.Add(msg);
    }
}
