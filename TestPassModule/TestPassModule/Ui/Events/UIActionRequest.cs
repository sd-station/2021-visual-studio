using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestPassModule.Ui.Events {

    public class Events {

        public enum EditorRequests {
            Save, Close
        }
        public enum ItemRequest {
            Reload,
            Selection,
            Position,
            Delete
        }

    }
}
