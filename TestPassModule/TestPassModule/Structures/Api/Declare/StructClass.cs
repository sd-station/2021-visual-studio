using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestPassModule.Api.Declare
{
    public class StructClass
    {
        /// <summary>
        /// id of this
        /// </summary>
        public int id { get; set; }
        /// <summary>
        /// Short Name 
        /// </summary>
        public string name { get; set; }
        /// <summary>
        /// Title of Item
        /// </summary>
        public string title { get; set; }

        /// <summary>
        /// About this Element 
        /// </summary>
        public string description { get; set; }

        /// <summary>
        ///  none , private ,public , caller, ui , code , physical 
        /// </summary>
        public string mode { get; set; }
        /// <summary>
        /// type of this element None Method Class Property Etc.
        /// </summary>
        public string type { get; set; }
        /// <summary>
        /// Color of this Element
        /// </summary>
        public string theme { get; set; }

        /// <summary>
        /// Original Name space
        /// </summary>
        public string scope { get; set; }

        public DateTime create { get; set; }
        public DateTime update { get; set; }
        public StructPosition position { get; set; }

        //>> old version
        public int parent { get; set; }
    }


    public class StructPosition
    {
        /// <summary>
        /// Identify position of item
        /// </summary>
        public string path { get; set; }

        /// <summary>
        /// Level of Item
        /// </summary>
        public int level { get; set; }
        /// <summary>
        /// Posion in Collection
        /// </summary>
        public int index { get; set; }
        /// <summary>
        /// Parent Item
        /// </summary>
        public int parent { get; set; }
    }
}
