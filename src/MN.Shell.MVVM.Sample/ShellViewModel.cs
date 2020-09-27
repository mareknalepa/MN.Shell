namespace MN.Shell.MVVM.Sample
{
    public class ShellViewModel : ItemsConductorOneActive<Screen>
    {
        public ShellViewModel()
        {
            Title = "MN.Shell.MVVM sample application";

            ItemsCollection.Add(new CommandsSampleViewModel());
            ItemsCollection.Add(new SampleDocumentViewModel("Document 1"));
            ItemsCollection.Add(new SampleDocumentViewModel("Document 2"));

            ActivateItem(ItemsCollection[0]);

            Tool = new SampleToolViewModel();
        }

        public SampleToolViewModel Tool { get; }
    }
}
