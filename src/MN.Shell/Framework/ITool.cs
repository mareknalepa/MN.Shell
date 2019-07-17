namespace MN.Shell.Framework
{
    public interface ITool : ILayoutModule
    {
        bool IsVisible { get; set; }
        ToolPosition InitialPosition { get; }
        double InitialMinWidth { get; }
        double InitialMinHeight { get; }
        double AutoHideMinWidth { get; }
        double AutoHideMinHeight { get; }
    }
}
