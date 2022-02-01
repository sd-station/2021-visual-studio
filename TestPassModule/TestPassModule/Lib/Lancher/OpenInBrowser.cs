using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestPassModule.Lib.Lancher {
    internal class OpenInBrowser {

        public void OpenLink(string link) {
            ProcessStartInfo psi = new ProcessStartInfo {
                FileName = "cmd.exe",
                Arguments = $"/C start {link}",
                WindowStyle = ProcessWindowStyle.Hidden,
                CreateNoWindow = true
            };
            Process.Start(psi);
        }
    }
}
