import { AppConfig } from '../../api/data/AppConfig.js';
export class ModuleDrawer {
    canvas;
    ctx;
    Options;
    constructor(canvas, opt) {
        this.canvas = canvas;
        this.ctx = canvas.getContext("2d");
        this.Options = opt;
    }
    get Left() {
        return (this.Options.start - 1) * AppConfig.width + AppConfig.width / 2;
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
    DrawDashLine() {
        this.ctx.strokeStyle = this.Options.color;
        this.ctx.setLineDash([3, 3]);
        this.ctx.beginPath();
        this.ctx.moveTo(this.Left, 30);
        this.ctx.lineTo(this.Left, this.canvas.height - 40);
        this.ctx.stroke();
        this.ctx.setLineDash([]);
    }
    DrawBox(Left, Top) {
        this.ctx.clearRect(Left, Top, 140, 30);
        this.ctx.fillStyle = this.Options.color + "22";
        this.ctx.fillRect(Left, Top, 140, 30);
        this.ctx.fillStyle = this.Options.color;
        this.ctx.strokeRect(Left, Top, 140, 30);
    }
    DrawBoxTitle(Left, Top) {
        this.ctx.textAlign = "center";
        this.ctx.font = "14px segoe ui";
        this.ctx.fillText(this.Options.Title, Left, Top, 130);
    }
}
