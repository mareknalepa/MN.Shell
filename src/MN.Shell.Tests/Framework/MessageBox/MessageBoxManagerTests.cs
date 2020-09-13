using MN.Shell.Framework.MessageBox;
using MN.Shell.Modules.MessageBox;
using MN.Shell.MVVM;
using Moq;
using NUnit.Framework;
using System;

namespace MN.Shell.Tests.Framework.MessageBox
{
    [TestFixture]
    public class MessageBoxManagerTests
    {
        private Mock<IWindowManager> _mockWindowManager;
        private IMessageBoxManager _messageBoxManager;

        private Action<object> OnDialogShown;
        private int _showWindowInvoked;

        [SetUp]
        public void SetUp()
        {
            OnDialogShown = null;
            _showWindowInvoked = 0;

            _mockWindowManager = new Mock<IWindowManager>(MockBehavior.Strict);
            _mockWindowManager.Setup(m => m.ShowDialog(It.IsAny<MessageBoxViewModel>()))
                .Callback<object>(viewModel =>
                {
                    ++_showWindowInvoked;
                    OnDialogShown?.Invoke(viewModel);
                })
                .Returns(true);

            _messageBoxManager = new MessageBoxManager(_mockWindowManager.Object);
        }

        [Test, Pairwise]
        public void MessageBoxManagerShowCaptionMessageTypeTest(
            [Values("", "Caption 1", "Caption 2")] string caption,
            [Values("", "Message 1", "Message 2")] string message,
            [Values] MessageBoxType type)
        {
            OnDialogShown = vm =>
            {
                if (!(vm is MessageBoxViewModel messageBoxViewModel))
                    throw new ArgumentException("Cannot handle view models other than MessageBoxViewModel");

                Assert.AreEqual(caption, messageBoxViewModel.Title);
                Assert.AreEqual(message, messageBoxViewModel.Message);
                Assert.AreEqual(type, messageBoxViewModel.Type);
            };

            _messageBoxManager.Show(caption, message, type);
            Assert.AreEqual(1, _showWindowInvoked);
        }

        [Test]
        public void MessageBoxManagerShowButtonsTest([Values] MessageBoxButtons buttons)
        {
            OnDialogShown = vm =>
            {
                if (!(vm is MessageBoxViewModel messageBoxViewModel))
                    throw new ArgumentException("Cannot handle view models other than MessageBoxViewModel");

                if (buttons == MessageBoxButtons.Ok)
                    Assert.That(messageBoxViewModel.Buttons, Has.Count.EqualTo(1));
                else if (buttons == MessageBoxButtons.OkCancel || buttons == MessageBoxButtons.YesNo)
                    Assert.That(messageBoxViewModel.Buttons, Has.Count.EqualTo(2));
                else if (buttons == MessageBoxButtons.YesNoCancel)
                    Assert.That(messageBoxViewModel.Buttons, Has.Count.EqualTo(3));
                else
                    Assert.Fail("Buttons argument out of scope");
            };

            _messageBoxManager.Show("Caption", "Message", MessageBoxType.None, buttons);
            Assert.AreEqual(1, _showWindowInvoked);
        }
    }
}
