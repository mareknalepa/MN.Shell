using MN.Shell.Framework.Menu;

namespace MN.Shell.UnitTests.Framework.Menu
{
    public sealed class MenuManagerTests
    {
        [Fact]
        public void AddItem_AddsMenuItem()
        {
            var menuManager = new MenuManagerWrapper();

            menuManager.RootItemDefinition.ShouldNotBeNull();
            menuManager.RootItemDefinition.SubItems.ShouldNotBeNull();
            menuManager.RootItemDefinition.SubItems.ShouldBeEmpty();

            menuManager.AddItem("Submenu", "Submenu localized name");

            menuManager.RootItemDefinition.SubItems.Count.ShouldBe(1);

            var submenu = menuManager.RootItemDefinition.SubItems.First();

            submenu.ShouldNotBeNull();
            submenu.Name.ShouldBe("Submenu");
            submenu.LocalizedName.ShouldBe("Submenu localized name");
        }

        [Fact]
        public void AddItem_RemovesLeadingSlash()
        {
            var menuManager = new MenuManagerWrapper();

            menuManager.AddItem("/Submenu", "Submenu localized name");

            menuManager.RootItemDefinition.SubItems.Count.ShouldBe(1);

            var submenu = menuManager.RootItemDefinition.SubItems.First();

            submenu.ShouldNotBeNull();
            submenu.Name.ShouldBe("Submenu");
            submenu.LocalizedName.ShouldBe("Submenu localized name");
            submenu.SubItems.Count.ShouldBe(0);
        }

        [Fact]
        public void AddItem_RemovesTrailingSlash()
        {
            var menuManager = new MenuManagerWrapper();

            menuManager.AddItem("Submenu/", "Submenu localized name");

            menuManager.RootItemDefinition.SubItems.Count.ShouldBe(1);

            var submenu = menuManager.RootItemDefinition.SubItems.First();

            submenu.ShouldNotBeNull();
            submenu.Name.ShouldBe("Submenu");
            submenu.LocalizedName.ShouldBe("Submenu localized name");
            submenu.SubItems.Count.ShouldBe(0);
        }

        [Fact]
        public void AddItem_Throws_WhenPathIsEmpty()
        {
            var menuManager = new MenuManagerWrapper();

            var act = () => menuManager.AddItem("", "");
            act.ShouldThrow<ArgumentException>();
            menuManager.RootItemDefinition.SubItems.Count.ShouldBe(0);
        }

        [Fact]
        public void AddItem_AddsMultipleSiblings()
        {
            var menuManager = new MenuManagerWrapper();

            menuManager.AddItem("Submenu", "Submenu localized name");
            menuManager.AddItem("Submenu/Submenu1", "Submenu1 localized name");
            menuManager.AddItem("Submenu/Submenu2", "Submenu2 localized name");

            menuManager.RootItemDefinition.SubItems.Count.ShouldBe(1);

            var submenu = menuManager.RootItemDefinition.SubItems.First();

            submenu.ShouldNotBeNull();
            submenu.Name.ShouldBe("Submenu");
            submenu.LocalizedName.ShouldBe("Submenu localized name");
            submenu.SubItems.Count.ShouldBe(2);

            var submenu1 = submenu.SubItems[0];

            submenu1.ShouldNotBeNull();
            submenu1.Name.ShouldBe("Submenu1");
            submenu1.LocalizedName.ShouldBe("Submenu1 localized name");

            var submenu2 = submenu.SubItems[1];

            submenu2.ShouldNotBeNull();
            submenu2.Name.ShouldBe("Submenu2");
            submenu2.LocalizedName.ShouldBe("Submenu2 localized name");
        }

        [Fact]
        public void AddItem_AddsNested()
        {
            var menuManager = new MenuManagerWrapper();

            menuManager.AddItem("Submenu/SubmenuA/SubmenuB/SubmenuC", "SubmenuC localized name");

            var submenu = menuManager.RootItemDefinition.SubItems.FirstOrDefault();
            submenu.ShouldNotBeNull();
            submenu?.Name.ShouldBe("Submenu");
            submenu?.LocalizedName.ShouldBe("");
            submenu?.SubItems.Count.ShouldBe(1);

            var submenuA = submenu?.SubItems.FirstOrDefault();
            submenuA.ShouldNotBeNull();
            submenuA?.Name.ShouldBe("SubmenuA");
            submenuA?.LocalizedName.ShouldBe("");
            submenuA?.SubItems.Count.ShouldBe(1);

            var submenuB = submenuA?.SubItems.FirstOrDefault();
            submenuB.ShouldNotBeNull();
            submenuB?.Name.ShouldBe("SubmenuB");
            submenuB?.LocalizedName.ShouldBe("");
            submenuB?.SubItems.Count.ShouldBe(1);

            var submenuC = submenuB?.SubItems.FirstOrDefault();
            submenuC.ShouldNotBeNull();
            submenuC?.Name.ShouldBe("SubmenuC");
            submenuC?.LocalizedName.ShouldBe("SubmenuC localized name");
        }

        [Fact]
        public void AddItem_UpdatesLocalizedName()
        {
            var menuManager = new MenuManagerWrapper();

            menuManager.AddItem("SubmenuA/SubmenuB/SubmenuC", "SubmenuC localized name");

            var submenuA = menuManager.RootItemDefinition.SubItems.FirstOrDefault();
            submenuA.ShouldNotBeNull();
            submenuA?.Name.ShouldBe("SubmenuA");
            submenuA?.LocalizedName.ShouldBe("");

            var submenuB = submenuA?.SubItems.FirstOrDefault();
            submenuB.ShouldNotBeNull();
            submenuB?.Name.ShouldBe("SubmenuB");
            submenuB?.LocalizedName.ShouldBe("");

            var submenuC = submenuB?.SubItems.FirstOrDefault();
            submenuC.ShouldNotBeNull();
            submenuC?.Name.ShouldBe("SubmenuC");
            submenuC?.LocalizedName.ShouldBe("SubmenuC localized name");

            menuManager.AddItem("SubmenuA", "SubmenuA localized name");

            submenuA = menuManager.RootItemDefinition.SubItems.FirstOrDefault();
            submenuA.ShouldNotBeNull();
            submenuA?.Name.ShouldBe("SubmenuA");
            submenuA?.LocalizedName.ShouldBe("SubmenuA localized name");

            submenuB = submenuA?.SubItems.FirstOrDefault();
            submenuB.ShouldNotBeNull();
            submenuB?.Name.ShouldBe("SubmenuB");
            submenuB?.LocalizedName.ShouldBe("");

            submenuC = submenuB?.SubItems.FirstOrDefault();
            submenuC.ShouldNotBeNull();
            submenuC?.Name.ShouldBe("SubmenuC");
            submenuC?.LocalizedName.ShouldBe("SubmenuC localized name");

            menuManager.AddItem("SubmenuA/SubmenuB", "SubmenuB localized name");

            submenuA = menuManager.RootItemDefinition.SubItems.FirstOrDefault();
            submenuA.ShouldNotBeNull();
            submenuA?.Name.ShouldBe("SubmenuA");
            submenuA?.LocalizedName.ShouldBe("SubmenuA localized name");

            submenuB = submenuA?.SubItems.FirstOrDefault();
            submenuB.ShouldNotBeNull();
            submenuB?.Name.ShouldBe("SubmenuB");
            submenuB?.LocalizedName.ShouldBe("SubmenuB localized name");

            submenuC = submenuB?.SubItems.FirstOrDefault();
            submenuC.ShouldNotBeNull();
            submenuC?.Name.ShouldBe("SubmenuC");
            submenuC?.LocalizedName.ShouldBe("SubmenuC localized name");

            menuManager.AddItem("SubmenuA", "Overridden SubmenuA localized name");

            submenuA = menuManager.RootItemDefinition.SubItems.FirstOrDefault();
            submenuA.ShouldNotBeNull();
            submenuA?.Name.ShouldBe("SubmenuA");
            submenuA?.LocalizedName.ShouldBe("Overridden SubmenuA localized name");

            submenuB = submenuA?.SubItems.FirstOrDefault();
            submenuB.ShouldNotBeNull();
            submenuB?.Name.ShouldBe("SubmenuB");
            submenuB?.LocalizedName.ShouldBe("SubmenuB localized name");

            submenuC = submenuB?.SubItems.FirstOrDefault();
            submenuC.ShouldNotBeNull();
            submenuC?.Name.ShouldBe("SubmenuC");
            submenuC?.LocalizedName.ShouldBe("SubmenuC localized name");
        }

        [Fact]
        public void AddItem_UpdatesPlacement()
        {
            var menuManager = new MenuManagerWrapper();

            menuManager.AddItem("SubmenuA/SubmenuB/SubmenuC", "").SetPlacement(1, 2);

            var submenuA = menuManager.RootItemDefinition.SubItems.FirstOrDefault();
            submenuA?.Section.ShouldBe(0);
            submenuA?.Order.ShouldBe(0);

            var submenuB = submenuA?.SubItems.FirstOrDefault();
            submenuB?.Section.ShouldBe(0);
            submenuB?.Order.ShouldBe(0);

            var submenuC = submenuB?.SubItems.FirstOrDefault();
            submenuC?.Section.ShouldBe(1);
            submenuC?.Order.ShouldBe(2);

            menuManager.AddItem("SubmenuA", "").SetPlacement(3, 4);

            submenuA = menuManager.RootItemDefinition.SubItems.FirstOrDefault();
            submenuA?.Section.ShouldBe(3);
            submenuA?.Order.ShouldBe(4);

            submenuB = submenuA?.SubItems.FirstOrDefault();
            submenuB?.Section.ShouldBe(0);
            submenuB?.Order.ShouldBe(0);

            submenuC = submenuB?.SubItems.FirstOrDefault();
            submenuC?.Section.ShouldBe(1);
            submenuC?.Order.ShouldBe(2);

            menuManager.AddItem("SubmenuA/SubmenuB", "").SetPlacement(5, 6);

            submenuA = menuManager.RootItemDefinition.SubItems.FirstOrDefault();
            submenuA?.Section.ShouldBe(3);
            submenuA?.Order.ShouldBe(4);

            submenuB = submenuA?.SubItems.FirstOrDefault();
            submenuB?.Section.ShouldBe(5);
            submenuB?.Order.ShouldBe(6);

            submenuC = submenuB?.SubItems.FirstOrDefault();
            submenuC?.Section.ShouldBe(1);
            submenuC?.Order.ShouldBe(2);

            menuManager.AddItem("SubmenuA", "").SetPlacement(7, 8);

            submenuA = menuManager.RootItemDefinition.SubItems.FirstOrDefault();
            submenuA?.Section.ShouldBe(7);
            submenuA?.Order.ShouldBe(8);

            submenuB = submenuA?.SubItems.FirstOrDefault();
            submenuB?.Section.ShouldBe(5);
            submenuB?.Order.ShouldBe(6);

            submenuC = submenuB?.SubItems.FirstOrDefault();
            submenuC?.Section.ShouldBe(1);
            submenuC?.Order.ShouldBe(2);
        }

        [Fact]
        public void RemoveItem_RemovesItem()
        {
            var menuManager = new MenuManagerWrapper();

            menuManager.AddItem("Submenu", "Submenu localized name");

            menuManager.RootItemDefinition.SubItems.Count.ShouldBe(1);

            menuManager.RemoveItem("Submenu");

            menuManager.RootItemDefinition.SubItems.Count.ShouldBe(0);
        }

        [Fact]
        public void RemoveItem_DoesNothing_ForNotExistingItem()
        {
            var menuManager = new MenuManagerWrapper();

            menuManager.AddItem("Submenu", "Submenu localized name");

            menuManager.RootItemDefinition.SubItems.Count.ShouldBe(1);

            menuManager.RemoveItem("SubmenuNotExisting");

            menuManager.RootItemDefinition.SubItems.Count.ShouldBe(1);
        }

        [Fact]
        public void RemoveItem_RemovesFromMultipleSiblings()
        {
            var menuManager = new MenuManagerWrapper();

            menuManager.AddItem("Submenu", "Submenu localized name");
            menuManager.AddItem("Submenu/Submenu1", "Submenu1 localized name");
            menuManager.AddItem("Submenu/Submenu2", "Submenu2 localized name");

            var submenu = menuManager.RootItemDefinition.SubItems.First();

            submenu.ShouldNotBeNull();
            submenu.Name.ShouldBe("Submenu");
            submenu.SubItems.Count.ShouldBe(2);
            submenu.SubItems.ShouldContain(d => d.Name == "Submenu1");
            submenu.SubItems.ShouldContain(d => d.Name == "Submenu2");

            menuManager.RemoveItem("Submenu/Submenu1");

            submenu = menuManager.RootItemDefinition.SubItems.First();

            submenu.ShouldNotBeNull();
            submenu.Name.ShouldBe("Submenu");
            submenu.SubItems.Count.ShouldBe(1);
            submenu.SubItems.ShouldNotContain(d => d.Name == "Submenu1");
            submenu.SubItems.ShouldContain(d => d.Name == "Submenu2");
        }

        [Fact]
        public void RemoveItem_RemovesFromNested()
        {
            var menuManager = new MenuManagerWrapper();

            menuManager.AddItem("Submenu/SubmenuA/SubmenuB/SubmenuC", "SubmenuC localized name");

            var submenuB = menuManager.RootItemDefinition.SubItems.First()
                .SubItems.First()
                .SubItems.First();

            submenuB.ShouldNotBeNull();
            submenuB?.Name.ShouldBe("SubmenuB");
            submenuB?.SubItems.Count.ShouldBe(1);

            menuManager.RemoveItem("Submenu/SubmenuA/SubmenuB/SubmenuC");

            submenuB = menuManager.RootItemDefinition.SubItems.First()
                .SubItems.First()
                .SubItems.First();

            submenuB.ShouldNotBeNull();
            submenuB?.Name.ShouldBe("SubmenuB");
            submenuB?.SubItems.Count.ShouldBe(0);
        }

        [Fact]
        public void RemoveItem_RemovesItemContainingSubItems()
        {
            var menuManager = new MenuManagerWrapper();

            menuManager.AddItem("Submenu/SubmenuA/SubmenuB/SubmenuC", "SubmenuC localized name");

            var submenu = menuManager.RootItemDefinition.SubItems.First();

            submenu.ShouldNotBeNull();
            submenu.Name.ShouldBe("Submenu");
            submenu.SubItems.Count.ShouldBe(1);

            menuManager.RemoveItem("Submenu/SubmenuA");

            submenu = menuManager.RootItemDefinition.SubItems.First();

            submenu.ShouldNotBeNull();
            submenu.Name.ShouldBe("Submenu");
            submenu.SubItems.Count.ShouldBe(1);

            menuManager.RemoveItem("Submenu/SubmenuA", forceRemoveIfNonEmpty: true);

            submenu = menuManager.RootItemDefinition.SubItems.First();

            submenu.ShouldNotBeNull();
            submenu.Name.ShouldBe("Submenu");
            submenu.SubItems.Count.ShouldBe(0);
        }

        [Fact]
        public void CompileMenu_CreatesMenuCorrectly()
        {
            var menuManager = new MenuManagerWrapper();

            menuManager.AddItem("File/Exit", "Exit").SetPlacement(100, 100);
            menuManager.AddItem("File/New Project...", "New Project...").SetPlacement(10, 20);
            menuManager.AddItem("File/Open Project...", "Open Project...").SetPlacement(10, 30);
            menuManager.AddItem("View/Tools", "Tools").SetPlacement(10, 10);
            menuManager.AddItem("View/Advanced mode", "Advanced mode").SetPlacement(20, 10);

            menuManager.CompileMenu();

            menuManager.MenuItems.Count.ShouldBe(2);

            var fileMenu = menuManager.MenuItems[0];
            fileMenu.Name.ShouldBe("File");
            fileMenu.SubItems.Count.ShouldBe(4);
            fileMenu.SubItems[0].Name.ShouldBe("New Project...");
            fileMenu.SubItems[1].Name.ShouldBe("Open Project...");
            fileMenu.SubItems[2].IsSeparator.ShouldBeTrue();
            fileMenu.SubItems[3].Name.ShouldBe("Exit");

            var viewMenu = menuManager.MenuItems[1];
            viewMenu.Name.ShouldBe("View");
            viewMenu.SubItems.Count.ShouldBe(3);
            viewMenu.SubItems[0].Name.ShouldBe("Tools");
            viewMenu.SubItems[1].IsSeparator.ShouldBeTrue();
            viewMenu.SubItems[2].Name.ShouldBe("Advanced mode");
        }

        private sealed class MenuManagerWrapper : MenuManager
        {
            public MenuManagerWrapper() : base([]) { }

            public new MenuItemDefinition RootItemDefinition => base.RootItemDefinition;
        }
    }
}
