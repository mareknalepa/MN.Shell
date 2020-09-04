namespace MN.Shell.MVVM.Sample
{
    public class ShellViewModel : ItemsConductorOneActive<Screen>
    {
        public ShellViewModel()
        {
            Title = "MN.Shell.MVVM sample application";

            ActivateItem(new SampleDocumentViewModel("Document 1"));
            ActivateItem(new SampleDocumentViewModel("Document 2"));
            ActivateItem(new SampleDocumentViewModel("Document 3"));

            Tool = new SampleToolViewModel();
        }

        public SampleToolViewModel Tool { get; }
    }
}
