using MN.Shell.Framework.Dialogs;
using System.Globalization;

namespace MN.Shell.Tests.Framework.Dialogs
{
    public sealed class DialogButtonTests : IDisposable
    {
        private readonly CultureInfo _originalCulture;
        private readonly CultureInfo _originalUiCulture;

        public DialogButtonTests()
        {
            _originalCulture = Thread.CurrentThread.CurrentCulture;
            _originalUiCulture = Thread.CurrentThread.CurrentUICulture;

            Thread.CurrentThread.CurrentCulture = new CultureInfo("");
            Thread.CurrentThread.CurrentUICulture = new CultureInfo("");
        }

        public void Dispose()
        {
            Thread.CurrentThread.CurrentCulture = _originalCulture;
            Thread.CurrentThread.CurrentUICulture = _originalUiCulture;
        }

        [Theory]
        [InlineData(DialogButtonType.Ok)]
        [InlineData(DialogButtonType.Cancel)]
        [InlineData(DialogButtonType.Yes)]
        [InlineData(DialogButtonType.No)]
        [InlineData(DialogButtonType.Custom)]
        public void Create_ReturnsCorrectResult(DialogButtonType type)
        {
            var dialogButton = DialogButton.Create(type);

            dialogButton.ShouldNotBeNull();
            dialogButton.Type.ShouldBe(type);

            if (type != DialogButtonType.Custom)
            {
                dialogButton.Caption.ToUpperInvariant().ShouldBe(type.ToString().ToUpperInvariant());
            }

            dialogButton.IsDefault.ShouldBe(type == DialogButtonType.Ok || type == DialogButtonType.Yes || type == DialogButtonType.Custom);
            dialogButton.IsCancel.ShouldBe(type == DialogButtonType.Cancel);
        }

        [Theory]
        [InlineData("Caption 1")]
        [InlineData("Caption 2")]
        public void Create_ReturnsCorrectResult_ForCustomButton(string caption)
        {
            var dialogButton = DialogButton.Create(DialogButtonType.Custom, caption);

            dialogButton.Type.ShouldBe(DialogButtonType.Custom);
            dialogButton.Caption.ShouldBe(caption);
        }

        [Fact]
        public void Create_Throws_ForInvalidType()
        {
            var act = () => DialogButton.Create(DialogButtonType.Unknown);
            act.ShouldThrow<ArgumentException>();
        }
    }
}
