using MN.Shell.Framework.Menu;
using MN.Shell.PluginContracts;
using NUnit.Framework;
using System;
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

            menuManager.AddItem("Submenu", "Submenu localized name");

            Assert.AreEqual(1, menuManager.RootItemDefinition.SubItems.Count);

            var submenu = menuManager.RootItemDefinition.SubItems.First();

            Assert.NotNull(submenu);
            Assert.AreEqual("Submenu", submenu.Name);
            Assert.AreEqual("Submenu localized name", submenu.LocalizedName);
        }

        [Test]
        public void AddItemLeadingSlashTest()
        {
            var menuManager = new MenuManagerWrapper();

            menuManager.AddItem("/Submenu", "Submenu localized name");

            Assert.AreEqual(1, menuManager.RootItemDefinition.SubItems.Count);

            var submenu = menuManager.RootItemDefinition.SubItems.First();

            Assert.NotNull(submenu);
            Assert.AreEqual("Submenu", submenu.Name);
            Assert.AreEqual("Submenu localized name", submenu.LocalizedName);
            Assert.AreEqual(0, submenu.SubItems.Count);
        }

        [Test]
        public void AddItemTrailingSlashTest()
        {
            var menuManager = new MenuManagerWrapper();

            menuManager.AddItem("Submenu/", "Submenu localized name");

            Assert.AreEqual(1, menuManager.RootItemDefinition.SubItems.Count);

            var submenu = menuManager.RootItemDefinition.SubItems.First();

            Assert.NotNull(submenu);
            Assert.AreEqual("Submenu", submenu.Name);
            Assert.AreEqual("Submenu localized name", submenu.LocalizedName);
            Assert.AreEqual(0, submenu.SubItems.Count);
        }

        [Test]
        public void AddItemEmptyPathTest()
        {
            var menuManager = new MenuManagerWrapper();

            Assert.Throws<ArgumentException>(() => menuManager.AddItem("", ""));
            Assert.AreEqual(0, menuManager.RootItemDefinition.SubItems.Count);
        }

        [Test]
        public void AddItemMultipleSiblingsTest()
        {
            var menuManager = new MenuManagerWrapper();

            menuManager.AddItem("Submenu", "Submenu localized name");
            menuManager.AddItem("Submenu/Submenu1", "Submenu1 localized name");
            menuManager.AddItem("Submenu/Submenu2", "Submenu2 localized name");

            Assert.AreEqual(1, menuManager.RootItemDefinition.SubItems.Count);

            var submenu = menuManager.RootItemDefinition.SubItems.First();

            Assert.NotNull(submenu);
            Assert.AreEqual("Submenu", submenu.Name);
            Assert.AreEqual("Submenu localized name", submenu.LocalizedName);
            Assert.AreEqual(2, submenu.SubItems.Count);

            var submenu1 = submenu.SubItems[0];

            Assert.NotNull(submenu1);
            Assert.AreEqual("Submenu1", submenu1.Name);
            Assert.AreEqual("Submenu1 localized name", submenu1.LocalizedName);

            var submenu2 = submenu.SubItems[1];

            Assert.NotNull(submenu2);
            Assert.AreEqual("Submenu2", submenu2.Name);
            Assert.AreEqual("Submenu2 localized name", submenu2.LocalizedName);
        }

        [Test]
        public void AddItemNestedTest()
        {
            var menuManager = new MenuManagerWrapper();

            menuManager.AddItem("Submenu/SubmenuA/SubmenuB/SubmenuC", "SubmenuC localized name");

            var submenu = menuManager.RootItemDefinition.SubItems.FirstOrDefault();
            Assert.NotNull(submenu);
            Assert.AreEqual("Submenu", submenu.Name);
            Assert.AreEqual("", submenu.LocalizedName);
            Assert.AreEqual(1, submenu.SubItems.Count);

            var submenuA = submenu.SubItems.FirstOrDefault();
            Assert.NotNull(submenuA);
            Assert.AreEqual("SubmenuA", submenuA.Name);
            Assert.AreEqual("", submenuA.LocalizedName);
            Assert.AreEqual(1, submenuA.SubItems.Count);

            var submenuB = submenuA.SubItems.FirstOrDefault();
            Assert.NotNull(submenuB);
            Assert.AreEqual("SubmenuB", submenuB.Name);
            Assert.AreEqual("", submenuB.LocalizedName);
            Assert.AreEqual(1, submenuB.SubItems.Count);

            var submenuC = submenuB.SubItems.FirstOrDefault();
            Assert.NotNull(submenuC);
            Assert.AreEqual("SubmenuC", submenuC.Name);
            Assert.AreEqual("SubmenuC localized name", submenuC.LocalizedName);
        }

        [Test]
        public void AddItemUpdateLocalizedNameTest()
        {
            var menuManager = new MenuManagerWrapper();

            menuManager.AddItem("SubmenuA/SubmenuB/SubmenuC", "SubmenuC localized name");

            var submenuA = menuManager.RootItemDefinition.SubItems.FirstOrDefault();
            Assert.NotNull(submenuA);
            Assert.AreEqual("SubmenuA", submenuA.Name);
            Assert.AreEqual("", submenuA.LocalizedName);

            var submenuB = submenuA.SubItems.FirstOrDefault();
            Assert.NotNull(submenuB);
            Assert.AreEqual("SubmenuB", submenuB.Name);
            Assert.AreEqual("", submenuB.LocalizedName);

            var submenuC = submenuB.SubItems.FirstOrDefault();
            Assert.NotNull(submenuC);
            Assert.AreEqual("SubmenuC", submenuC.Name);
            Assert.AreEqual("SubmenuC localized name", submenuC.LocalizedName);

            menuManager.AddItem("SubmenuA", "SubmenuA localized name");

            submenuA = menuManager.RootItemDefinition.SubItems.FirstOrDefault();
            Assert.NotNull(submenuA);
            Assert.AreEqual("SubmenuA", submenuA.Name);
            Assert.AreEqual("SubmenuA localized name", submenuA.LocalizedName);

            submenuB = submenuA.SubItems.FirstOrDefault();
            Assert.NotNull(submenuB);
            Assert.AreEqual("SubmenuB", submenuB.Name);
            Assert.AreEqual("", submenuB.LocalizedName);

            submenuC = submenuB.SubItems.FirstOrDefault();
            Assert.NotNull(submenuC);
            Assert.AreEqual("SubmenuC", submenuC.Name);
            Assert.AreEqual("SubmenuC localized name", submenuC.LocalizedName);

            menuManager.AddItem("SubmenuA/SubmenuB", "SubmenuB localized name");

            submenuA = menuManager.RootItemDefinition.SubItems.FirstOrDefault();
            Assert.NotNull(submenuA);
            Assert.AreEqual("SubmenuA", submenuA.Name);
            Assert.AreEqual("SubmenuA localized name", submenuA.LocalizedName);

            submenuB = submenuA.SubItems.FirstOrDefault();
            Assert.NotNull(submenuB);
            Assert.AreEqual("SubmenuB", submenuB.Name);
            Assert.AreEqual("SubmenuB localized name", submenuB.LocalizedName);

            submenuC = submenuB.SubItems.FirstOrDefault();
            Assert.NotNull(submenuC);
            Assert.AreEqual("SubmenuC", submenuC.Name);
            Assert.AreEqual("SubmenuC localized name", submenuC.LocalizedName);

            menuManager.AddItem("SubmenuA", "Overridden SubmenuA localized name");

            submenuA = menuManager.RootItemDefinition.SubItems.FirstOrDefault();
            Assert.NotNull(submenuA);
            Assert.AreEqual("SubmenuA", submenuA.Name);
            Assert.AreEqual("Overridden SubmenuA localized name", submenuA.LocalizedName);

            submenuB = submenuA.SubItems.FirstOrDefault();
            Assert.NotNull(submenuB);
            Assert.AreEqual("SubmenuB", submenuB.Name);
            Assert.AreEqual("SubmenuB localized name", submenuB.LocalizedName);

            submenuC = submenuB.SubItems.FirstOrDefault();
            Assert.NotNull(submenuC);
            Assert.AreEqual("SubmenuC", submenuC.Name);
            Assert.AreEqual("SubmenuC localized name", submenuC.LocalizedName);
        }

        [Test]
        public void AddItemUpdatePlacementTest()
        {
            var menuManager = new MenuManagerWrapper();

            menuManager.AddItem("SubmenuA/SubmenuB/SubmenuC", "").SetPlacement(1, 2);

            var submenuA = menuManager.RootItemDefinition.SubItems.FirstOrDefault();
            Assert.AreEqual(0, submenuA.Section);
            Assert.AreEqual(0, submenuA.Order);

            var submenuB = submenuA.SubItems.FirstOrDefault();
            Assert.AreEqual(0, submenuB.Section);
            Assert.AreEqual(0, submenuB.Order);

            var submenuC = submenuB.SubItems.FirstOrDefault();
            Assert.AreEqual(1, submenuC.Section);
            Assert.AreEqual(2, submenuC.Order);

            menuManager.AddItem("SubmenuA", "").SetPlacement(3, 4);

            submenuA = menuManager.RootItemDefinition.SubItems.FirstOrDefault();
            Assert.AreEqual(3, submenuA.Section);
            Assert.AreEqual(4, submenuA.Order);

            submenuB = submenuA.SubItems.FirstOrDefault();
            Assert.AreEqual(0, submenuB.Section);
            Assert.AreEqual(0, submenuB.Order);

            submenuC = submenuB.SubItems.FirstOrDefault();
            Assert.AreEqual(1, submenuC.Section);
            Assert.AreEqual(2, submenuC.Order);

            menuManager.AddItem("SubmenuA/SubmenuB", "").SetPlacement(5, 6);

            submenuA = menuManager.RootItemDefinition.SubItems.FirstOrDefault();
            Assert.AreEqual(3, submenuA.Section);
            Assert.AreEqual(4, submenuA.Order);

            submenuB = submenuA.SubItems.FirstOrDefault();
            Assert.AreEqual(5, submenuB.Section);
            Assert.AreEqual(6, submenuB.Order);

            submenuC = submenuB.SubItems.FirstOrDefault();
            Assert.AreEqual(1, submenuC.Section);
            Assert.AreEqual(2, submenuC.Order);

            menuManager.AddItem("SubmenuA", "").SetPlacement(7, 8);

            submenuA = menuManager.RootItemDefinition.SubItems.FirstOrDefault();
            Assert.AreEqual(7, submenuA.Section);
            Assert.AreEqual(8, submenuA.Order);

            submenuB = submenuA.SubItems.FirstOrDefault();
            Assert.AreEqual(5, submenuB.Section);
            Assert.AreEqual(6, submenuB.Order);

            submenuC = submenuB.SubItems.FirstOrDefault();
            Assert.AreEqual(1, submenuC.Section);
            Assert.AreEqual(2, submenuC.Order);
        }

        [Test]
        public void RemoveItemBasicTest()
        {
            var menuManager = new MenuManagerWrapper();

            menuManager.AddItem("Submenu", "Submenu localized name");

            Assert.AreEqual(1, menuManager.RootItemDefinition.SubItems.Count);

            menuManager.RemoveItem("Submenu");

            Assert.AreEqual(0, menuManager.RootItemDefinition.SubItems.Count);
        }

        [Test]
        public void RemoveItemNotExistingTest()
        {
            var menuManager = new MenuManagerWrapper();

            menuManager.AddItem("Submenu", "Submenu localized name");

            Assert.AreEqual(1, menuManager.RootItemDefinition.SubItems.Count);

            menuManager.RemoveItem("SubmenuNotExisting");

            Assert.AreEqual(1, menuManager.RootItemDefinition.SubItems.Count);
        }

        [Test]
        public void RemoveItemMultipleSiblingsTest()
        {
            var menuManager = new MenuManagerWrapper();

            menuManager.AddItem("Submenu", "Submenu localized name");
            menuManager.AddItem("Submenu/Submenu1", "Submenu1 localized name");
            menuManager.AddItem("Submenu/Submenu2", "Submenu2 localized name");

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

            menuManager.AddItem("Submenu/SubmenuA/SubmenuB/SubmenuC", "SubmenuC localized name");

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

            menuManager.AddItem("Submenu/SubmenuA/SubmenuB/SubmenuC", "SubmenuC localized name");

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

            menuManager.AddItem("File/Exit", "Exit").SetPlacement(100, 100);
            menuManager.AddItem("File/New Project...", "New Project...").SetPlacement(10, 20);
            menuManager.AddItem("File/Open Project...", "Open Project...").SetPlacement(10, 30);
            menuManager.AddItem("View/Tools", "Tools").SetPlacement(10, 10);
            menuManager.AddItem("View/Advanced mode", "Advanced mode").SetPlacement(20, 10);

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
