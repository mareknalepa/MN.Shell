namespace MN.Shell.Framework.MessageBox
{
    public interface IMessageBoxManager
    {
        bool? Show(string title, string msg, MessageBoxType type = MessageBoxType.None,
            MessageBoxButtons buttons = MessageBoxButtons.Ok);
    }
}
