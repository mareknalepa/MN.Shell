namespace MN.Shell.Framework
{
    public interface ITool : ILayoutModule
    {
        bool IsVisible { get; set; }
        ToolPosition InitialPosition { get; }
        double MinWidth { get; }
        double MinHeight { get; }
        double AutoHideMinWidth { get; }
        double AutoHideMinHeight { get; }
    }
}
