using System;
using System.Text;
using System.Windows.Input;

namespace MN.Shell.Framework.Menu
{
    public class MenuItem
    {
        public string Name { get; set; }

        public string[] Path { get; set; }

        public int GroupId { get; set; }

        public int GroupOrder { get; set; }

        public ICommand Command { get; set; }

        public Uri Icon { get; set; }

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
