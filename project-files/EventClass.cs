using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetManager
{
    public class InterfaceSelectedEventArgs : EventArgs
    {
        public string SelectedInterfaceContent { get; }

        public InterfaceSelectedEventArgs(string selectedInterfaceContent)
        {
            SelectedInterfaceContent = selectedInterfaceContent;
        }
    }
}
