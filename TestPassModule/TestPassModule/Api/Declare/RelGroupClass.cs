using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestPassModule.Api.Data;
using TestPassModule.Stage.TemplateItems;

namespace TestPassModule.Api.Declare;

public class RelGroupClass {
    public int id { get; set; }
    public DateTime create { get; set; }
    public DateTime update { get; set; }
    public BoundClass bound { get; set; }
    public string title { get;   set; }
}


public class BoundClass {
    public int Left { get; set; }
    public int Top { get; set; }

    public int Bottom { get; set; }
    public int Right { get; set; }
}





