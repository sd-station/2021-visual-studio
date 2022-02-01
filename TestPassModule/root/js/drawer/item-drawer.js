import { AppConfig } from './../api/data/AppConfig.js';
import { ActiveData } from './../api/data/ActiveData.js';
export class itemDrawer {
    canvas;
    DrawColumn(m, h) {
        var startX = (m.index - 1) * AppConfig.width + AppConfig.width / 2;
        var itm = ActiveData.General.Modules.find(y => y.id == m.module);
        this.ctx.strokeStyle = "#000";
        this.ctx.fillStyle = "#000";
        if (itm) {
            this.ctx.strokeStyle = itm.theme;
            this.ctx.fillStyle = itm.theme;
        }
        this.ctx.setLineDash([3, 3]);
        this.ctx.beginPath();
        this.ctx.moveTo(startX, 30);
        this.ctx.lineTo(startX, this.canvas.height - 40);
        this.ctx.stroke();
        this.ctx.setLineDash([]);
        if (itm) {
            this.ctx.textAlign = "center";
            this.ctx.font = "14px segoe ui";
            this.ctx.clearRect(startX - 70, 10, 140, 30);
            this.ctx.strokeRect(startX - 70, 10, 140, 30);
            this.ctx.fillText(itm.title, startX, 10 + 20, 130);
            this.ctx.clearRect(startX - 70, this.canvas.height - 10 - 30, 140, 30);
            this.ctx.strokeRect(startX - 70, this.canvas.height - 10 - 30, 140, 30);
            this.ctx.fillText(itm.title, startX, this.canvas.height - 10 - 30 + 20, 130);
        }
    }
    padding = { top: 20 };
    ctx;
    constructor(c, cx) {
        this.ctx = c;
        this.canvas = cx;
    }
}
