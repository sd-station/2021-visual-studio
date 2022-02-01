import { ModuleDrawerInit } from "./module-drawer.init.js";
export class ModuleDrawer extends ModuleDrawerInit {
    constructor(canvas, opt) {
        super(canvas, opt);
        this.canvas = canvas;
        this.ctx = canvas.getContext("2d");
        this.Options = opt;
    }
    Draw() {
        this.DrawDashLine();
        if (this.Options.Title.length > 0) {
            this.DrawBox(this.Left - 70, 10);
            this.DrawBox(this.Left - 70, this.canvas.height - 10 - 30);
            this.DrawBoxTitle(this.Left, 10 + 20);
            this.DrawBoxTitle(this.Left, this.canvas.height - 10 - 30 + 20);
        }
    }
}
