namespace MN.Shell.Framework.MessageBox
{
    public interface IMessageBoxManager
    {
        bool? Show(string title, string msg, MessageBoxType type = MessageBoxType.None,
            MessageBoxButtonSet buttons = MessageBoxButtonSet.Ok);
    }
}
