using MN.Shell.Framework.Dialogs;
using NUnit.Framework;
using System;

namespace MN.Shell.Tests.Framework.Dialogs
{
    [TestFixture]
    public class DialogButtonTests
    {
        [Test]
        public void DialogButtonCreateTest(
            [Values(DialogButtonType.Ok, DialogButtonType.Cancel, DialogButtonType.Yes, DialogButtonType.No)]
            DialogButtonType dialogButtonType)
        {
            var dialogButton = DialogButton.Create(dialogButtonType);

            Assert.NotNull(dialogButton);
            Assert.AreEqual(dialogButtonType, dialogButton.Type);
            Assert.AreEqual(dialogButtonType.ToString().ToLower(), dialogButton.Caption.ToLower());

            Assert.AreEqual(dialogButtonType == DialogButtonType.Ok, dialogButton.IsDefault);
            Assert.AreEqual(dialogButtonType == DialogButtonType.Cancel, dialogButton.IsCancel);
        }

        [Test]
        public void DialogButtonCreateThrowsTest()
        {
            Assert.Throws<ArgumentException>(() => DialogButton.Create(DialogButtonType.Unknown));
            Assert.Throws<ArgumentException>(() => DialogButton.Create(DialogButtonType.Custom));
        }

        [Test, Pairwise]
        public void DialogButtonCreateCustomTest(
            [Values("Caption 1", "Caption 2")] string caption)
        {
            var dialogButton = DialogButton.CreateCustom(caption);

            Assert.AreEqual(DialogButtonType.Custom, dialogButton.Type);
            Assert.AreEqual(caption, dialogButton.Caption);
        }

        [Test]
        public void DialogButtonCreateCustomThrowsTest()
        {
            Assert.Throws<ArgumentException>(() => DialogButton.CreateCustom(null));
            Assert.Throws<ArgumentException>(() => DialogButton.CreateCustom(""));
        }
    }
}
