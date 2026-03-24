using MN.Shell.Framework.Dialogs;
using NUnit.Framework;
using System;

namespace MN.Shell.Tests.Framework.Dialogs
{
    [TestFixture]
    public class DialogButtonTests
    {
        [Test, SetUICulture("")]
        public void DialogButtonCreateTest([Values(
            DialogButtonType.Ok,
            DialogButtonType.Cancel,
            DialogButtonType.Yes,
            DialogButtonType.No,
            DialogButtonType.Custom)] DialogButtonType type)
        {
            var dialogButton = DialogButton.Create(type);

            Assert.NotNull(dialogButton);
            Assert.AreEqual(type, dialogButton.Type);

            if (type != DialogButtonType.Custom)
                Assert.AreEqual(type.ToString().ToUpperInvariant(), dialogButton.Caption.ToUpperInvariant());

            Assert.AreEqual(type == DialogButtonType.Ok || type == DialogButtonType.Yes ||
                type == DialogButtonType.Custom, dialogButton.IsDefault);
            Assert.AreEqual(type == DialogButtonType.Cancel, dialogButton.IsCancel);
        }

        [Test]
        public void DialogButtonCreateCustomTest(
            [Values("Caption 1", "Caption 2")] string caption)
        {
            var dialogButton = DialogButton.Create(DialogButtonType.Custom, caption);

            Assert.AreEqual(DialogButtonType.Custom, dialogButton.Type);
            Assert.AreEqual(caption, dialogButton.Caption);
        }

        [Test]
        public void DialogButtonCreateThrowsTest()
        {
            Assert.Throws<ArgumentException>(() => DialogButton.Create(DialogButtonType.Unknown));
        }
    }
}
