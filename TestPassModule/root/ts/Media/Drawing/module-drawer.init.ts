import { IModuleDraw } from "module-drawer";
import { AppConfig } from '../../api/data/AppConfig.js';

class ModuleDrawerBasic {
    canvas: HTMLCanvasElement;
    ctx: CanvasRenderingContext2D;
    Options: IModuleDraw;
    constructor(canvas: HTMLCanvasElement, opt: IModuleDraw) {
        this.canvas = canvas;
        this.ctx = canvas.getContext("2d")!;
        this.Options = opt

    }

    public get Left(): number {
        return (this.Options.start - 1) * AppConfig.width + AppConfig.width / 2;
    }

}

export class ModuleDrawerInit extends ModuleDrawerBasic {

    constructor(canvas: HTMLCanvasElement, opt: IModuleDraw) {
        super(canvas, opt)

    }

    DrawDashLine() {
        this.ctx.strokeStyle = this.Options.BorderColor + "66";
         
        this.ctx.lineWidth = 2;
        this.ctx.setLineDash([this.ctx.lineWidth * 2, this.ctx.lineWidth * 2]);
        this.ctx.beginPath();
        this.ctx.moveTo(this.Left, 30);
        this.ctx.lineTo(this.Left, this.canvas.height - 40);
        this.ctx.stroke();
        this.ctx.setLineDash([]);
    }
    DrawBox(Left: number, Top: number) {
        this.ctx.clearRect(Left, Top, 140, 30);
        this.ctx.fillStyle = this.Options.BorderColor;
        this.ctx.fillRect(Left, Top, 140, 30);
        this.ctx.strokeStyle = this.Options.BorderColor;
        this.ctx.strokeRect(Left, Top, 140, 30);



    }
    DrawBoxTitle(Left: number, Top: number) {
        this.ctx.textAlign = "center";
        this.ctx.font = "14px segoe ui";
        this.ctx.fillStyle = "#000000";
        this.ctx.shadowColor = "black";
        if (this.Options.TextColor == "#000000") {
            this.ctx.fillStyle = "#ffffff";
            this.ctx.shadowColor = "#fffff";
        }
        this.ctx.shadowBlur = 2;
        var lv = this.ctx.lineWidth;
        this.ctx.lineWidth = 2;
        this.ctx.strokeText(this.Options.Title, Left, Top, 130);
        this.ctx.shadowBlur = 0;
        this.ctx.fillStyle = this.Options.TextColor;

        this.ctx.fillText(this.Options.Title, Left, Top, 130);
        this.ctx.lineWidth = lv;

    }

}
