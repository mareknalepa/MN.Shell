﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MN.Shell.Framework.Menu
{
    public interface IMenuAggregator
    {
        IEnumerable<MenuItemViewModel> ComposeMenu(IEnumerable<IMenuProvider> menuProviders);
    }
}
