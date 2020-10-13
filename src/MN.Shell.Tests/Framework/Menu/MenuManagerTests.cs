using MN.Shell.Framework.Menu;
using MN.Shell.PluginContracts;
using NUnit.Framework;
using System.Linq;

namespace MN.Shell.Tests.Framework.Menu
{
    [TestFixture]
    public class MenuManagerTests
    {
        [Test]
        public void AddItemBasicTest()
        {
            var menuManager = new MenuManagerWrapper();

            Assert.NotNull(menuManager.RootItemDefinition);
            Assert.NotNull(menuManager.RootItemDefinition.SubItems);
            Assert.IsEmpty(menuManager.RootItemDefinition.SubItems);

            menuManager.AddItem("Submenu");

            Assert.AreEqual(1, menuManager.RootItemDefinition.SubItems.Count);
            Assert.True(menuManager.RootItemDefinition.SubItems.Any(d => d.Name == "Submenu"));
        }

        [Test]
        public void AddItemLeadingSlashTest()
        {
            var menuManager = new MenuManagerWrapper();

            menuManager.AddItem("/Submenu");

            Assert.AreEqual(1, menuManager.RootItemDefinition.SubItems.Count);

            var submenu = menuManager.RootItemDefinition.SubItems.First();

            Assert.AreEqual("Submenu", submenu.Name);
            Assert.AreEqual(0, submenu.SubItems.Count);
        }

        [Test]
        public void AddItemTrailingSlashTest()
        {
            var menuManager = new MenuManagerWrapper();

            menuManager.AddItem("Submenu/");

            Assert.AreEqual(1, menuManager.RootItemDefinition.SubItems.Count);

            var submenu = menuManager.RootItemDefinition.SubItems.First();

            Assert.AreEqual("Submenu", submenu.Name);
            Assert.AreEqual(0, submenu.SubItems.Count);
        }

        [Test]
        public void AddItemMultipleSiblingsTest()
        {
            var menuManager = new MenuManagerWrapper();

            menuManager.AddItem("Submenu");
            menuManager.AddItem("Submenu/Submenu1");
            menuManager.AddItem("Submenu/Submenu2");

            Assert.AreEqual(1, menuManager.RootItemDefinition.SubItems.Count);
            Assert.True(menuManager.RootItemDefinition.SubItems.Any(d => d.Name == "Submenu"));

            Assert.AreEqual(2, menuManager.RootItemDefinition.SubItems.First().SubItems.Count);
            Assert.True(menuManager.RootItemDefinition.SubItems.First().SubItems.Any(d => d.Name == "Submenu1"));
            Assert.True(menuManager.RootItemDefinition.SubItems.First().SubItems.Any(d => d.Name == "Submenu2"));
        }

        [Test]
        public void AddItemNestedTest()
        {
            var menuManager = new MenuManagerWrapper();

            menuManager.AddItem("Submenu/SubmenuA/SubmenuB/SubmenuC");

            var submenu = menuManager.RootItemDefinition.SubItems.FirstOrDefault();
            Assert.NotNull(submenu);
            Assert.AreEqual("Submenu", submenu.Name);
            Assert.AreEqual(1, submenu.SubItems.Count);

            var submenuA = submenu.SubItems.FirstOrDefault();
            Assert.NotNull(submenuA);
            Assert.AreEqual("SubmenuA", submenuA.Name);
            Assert.AreEqual(1, submenuA.SubItems.Count);

            var submenuB = submenuA.SubItems.FirstOrDefault();
            Assert.NotNull(submenuB);
            Assert.AreEqual("SubmenuB", submenuB.Name);
            Assert.AreEqual(1, submenuB.SubItems.Count);

            var submenuC = submenuB.SubItems.FirstOrDefault();
            Assert.NotNull(submenuC);
            Assert.AreEqual("SubmenuC", submenuC.Name);
        }

        [Test]
        public void RemoveItemBasicTest()
        {
            var menuManager = new MenuManagerWrapper();

            menuManager.AddItem("Submenu");

            Assert.AreEqual(1, menuManager.RootItemDefinition.SubItems.Count);

            menuManager.RemoveItem("Submenu");

            Assert.AreEqual(0, menuManager.RootItemDefinition.SubItems.Count);
        }

        [Test]
        public void RemoveItemNotExistingTest()
        {
            var menuManager = new MenuManagerWrapper();

            menuManager.AddItem("Submenu");

            Assert.AreEqual(1, menuManager.RootItemDefinition.SubItems.Count);

            menuManager.RemoveItem("SubmenuNotExisting");

            Assert.AreEqual(1, menuManager.RootItemDefinition.SubItems.Count);
        }

        [Test]
        public void RemoveItemMultipleSiblingsTest()
        {
            var menuManager = new MenuManagerWrapper();

            menuManager.AddItem("Submenu");
            menuManager.AddItem("Submenu/Submenu1");
            menuManager.AddItem("Submenu/Submenu2");

            var submenu = menuManager.RootItemDefinition.SubItems.First();

            Assert.NotNull(submenu);
            Assert.AreEqual("Submenu", submenu.Name);
            Assert.AreEqual(2, submenu.SubItems.Count);
            Assert.True(submenu.SubItems.Any(d => d.Name == "Submenu1"));
            Assert.True(submenu.SubItems.Any(d => d.Name == "Submenu2"));

            menuManager.RemoveItem("Submenu/Submenu1");

            submenu = menuManager.RootItemDefinition.SubItems.First();

            Assert.NotNull(submenu);
            Assert.AreEqual("Submenu", submenu.Name);
            Assert.AreEqual(1, submenu.SubItems.Count);
            Assert.False(submenu.SubItems.Any(d => d.Name == "Submenu1"));
            Assert.True(submenu.SubItems.Any(d => d.Name == "Submenu2"));
        }

        [Test]
        public void RemoveItemNestedTest()
        {
            var menuManager = new MenuManagerWrapper();

            menuManager.AddItem("Submenu/SubmenuA/SubmenuB/SubmenuC");

            var submenuB = menuManager.RootItemDefinition.SubItems.First()
                .SubItems.First()
                .SubItems.First();

            Assert.NotNull(submenuB);
            Assert.AreEqual("SubmenuB", submenuB.Name);
            Assert.AreEqual(1, submenuB.SubItems.Count);

            menuManager.RemoveItem("Submenu/SubmenuA/SubmenuB/SubmenuC");

            submenuB = menuManager.RootItemDefinition.SubItems.First()
                .SubItems.First()
                .SubItems.First();

            Assert.NotNull(submenuB);
            Assert.AreEqual("SubmenuB", submenuB.Name);
            Assert.AreEqual(0, submenuB.SubItems.Count);
        }

        [Test]
        public void RemoveItemContainingSubItemsTest()
        {
            var menuManager = new MenuManagerWrapper();

            menuManager.AddItem("Submenu/SubmenuA/SubmenuB/SubmenuC");

            var submenu = menuManager.RootItemDefinition.SubItems.First();

            Assert.NotNull(submenu);
            Assert.AreEqual("Submenu", submenu.Name);
            Assert.AreEqual(1, submenu.SubItems.Count);

            menuManager.RemoveItem("Submenu/SubmenuA");

            submenu = menuManager.RootItemDefinition.SubItems.First();

            Assert.NotNull(submenu);
            Assert.AreEqual("Submenu", submenu.Name);
            Assert.AreEqual(1, submenu.SubItems.Count);

            menuManager.RemoveItem("Submenu/SubmenuA", forceRemoveIfNonEmpty: true);

            submenu = menuManager.RootItemDefinition.SubItems.First();

            Assert.NotNull(submenu);
            Assert.AreEqual("Submenu", submenu.Name);
            Assert.AreEqual(0, submenu.SubItems.Count);
        }

        [Test]
        public void CompileMenuSimpleTest()
        {
            var menuManager = new MenuManagerWrapper();

            menuManager.AddItem("File/Exit").SetPlacement(100, 100);
            menuManager.AddItem("File/New Project...").SetPlacement(10, 20);
            menuManager.AddItem("File/Open Project...").SetPlacement(10, 30);
            menuManager.AddItem("View/Tools").SetPlacement(10, 10);
            menuManager.AddItem("View/Advanced mode").SetPlacement(20, 10);

            menuManager.CompileMenu();

            Assert.AreEqual(2, menuManager.MenuItems.Count);

            var fileMenu = menuManager.MenuItems[0];
            Assert.AreEqual("File", fileMenu.Name);
            Assert.AreEqual(4, fileMenu.SubItems.Count);
            Assert.AreEqual("New Project...", fileMenu.SubItems[0].Name);
            Assert.AreEqual("Open Project...", fileMenu.SubItems[1].Name);
            Assert.True(fileMenu.SubItems[2].IsSeparator);
            Assert.AreEqual("Exit", fileMenu.SubItems[3].Name);

            var viewMenu = menuManager.MenuItems[1];
            Assert.AreEqual("View", viewMenu.Name);
            Assert.AreEqual(3, viewMenu.SubItems.Count);
            Assert.AreEqual("Tools", viewMenu.SubItems[0].Name);
            Assert.True(viewMenu.SubItems[1].IsSeparator);
            Assert.AreEqual("Advanced mode", viewMenu.SubItems[2].Name);
        }

        private class MenuManagerWrapper : MenuManager
        {
            public MenuManagerWrapper() : base(Enumerable.Empty<IMenuProvider>()) { }

            public new MenuItemDefinition RootItemDefinition => base.RootItemDefinition;
        }
    }
}
