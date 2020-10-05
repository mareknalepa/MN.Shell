using MN.Shell.PluginContracts;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace MN.Shell.Framework.Menu
{
    public class MenuItem : IMenuItem
    {
        public string Name { get; set; }

        public IEnumerable<string> Path { get; set; }

        public int GroupId { get; set; }

        public int GroupOrder { get; set; }

        public Uri Icon { get; set; }

        public bool IsCheckable { get; set; }

        public Action<bool> OnIsCheckedChanged { get; set; }

        public ICommand Command { get; set; }

        public override string ToString()
        {
            StringBuilder stringBuilder = new StringBuilder();
            if (Path != null)
            {
                foreach (var pathComponent in Path)
                {
                    stringBuilder.Append($"{pathComponent}/");
                }
            }

            stringBuilder.Append(Name);
            stringBuilder.Append($" [gid: {GroupId}, order: {GroupOrder}");

            return stringBuilder.ToString();
        }
    }
}
