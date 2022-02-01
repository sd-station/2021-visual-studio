import { IModuleDraw } from "module-drawer";
import { IRelationship } from "senario-rel-api";
import { AppConfig } from "../../api/data/AppConfig.js";
import { ModuleDrawerInit } from "./module-drawer.init.js";

export class ModuleDrawer extends ModuleDrawerInit {
    ActivePoint(In: IRelationship[], Out: IRelationship[]) {
        console.log("------- ----- ");
        Out.forEach(r => {
            if (r.runtime.row == 1) return;
            console.log("out" , r.position.from, "row" ,   r.runtime.row,   r.title);
            
           // this.ctx.fillStyle = r.runtime.theme;
            var pv = r.runtime.row - 1;
            var h = AppConfig.height;
            while (pv > 1 && In.filter(k => k.runtime.row == pv).length == 0 && Out.filter(k => k.runtime.row == pv).length == 0) {
               // if (Out.filter(k => k.runtime.row == pv).length > 0) this.ctx.fillStyle = this.Options.BorderColor;
                pv -= 1;
                h += AppConfig.height;
            }
           // if (In.filter(k => k.runtime.row == pv).length > 0)
            // this.ctx.fillStyle = In.filter(k => k.runtime.row == pv)[0].runtime.theme;
            // this.ctx.fillStyle = "#cfcfcf";
           this.ctx.fillStyle = this.Options.BorderColor + "a0";
            var y = pv * AppConfig.height + AppConfig.height / 2 - 5;

            this.ctx.fillRect(this.Left - 3, y + 2, 6, h);
        })
    }

    constructor(canvas: HTMLCanvasElement, opt: IModuleDraw) {
        super(canvas, opt)
        this.canvas = canvas;
        this.ctx = canvas.getContext("2d")!;
        this.Options = opt

    }

    Draw() {

        this.DrawDashLine();

        if (this.Options.Title.length > 0) {

            this.DrawBox(this.Left - 70, 10);
            this.DrawBox(this.Left - 70, this.canvas.height - 10 - 30);
            this.DrawBoxTitle(this.Left, 10 + 20)
            this.DrawBoxTitle(this.Left, this.canvas.height - 10 - 30 + 20)
        }

    }

}
