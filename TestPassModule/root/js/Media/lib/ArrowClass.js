import { AppConfig } from "../../api/data/AppConfig.js";
export class ArrowDrawer {
    width;
    left;
    top;
    label;
    marked;
    to;
    from;
    color;
    line;
    padding = { top: 20 };
    SetPosition(startline, endline, top) {
        this.left = Math.min(...[startline, endline]) * AppConfig.width + AppConfig.width / 2;
        this.width = Math.abs(startline - endline) * AppConfig.width;
        this.top = top * AppConfig.height + AppConfig.height / 2;
        this.from = {
            x: startline * AppConfig.width + 1 + AppConfig.width / 2,
            y: this.padding.top + top * AppConfig.height + 1
        };
        this.to = {
            x: endline * AppConfig.width + 1 + AppConfig.width / 2,
            y: this.padding.top + top * AppConfig.height + 1
        };
    }
    ctx;
    constructor(canvas) {
        this.ctx = canvas.getContext("2d");
        return this;
    }
    Draw() {
        new ArrowClass(this);
    }
}
class ArrowClass {
    padding = { top: 20 };
    config;
    constructor(config) {
        this.config = config;
        this.Draw();
    }
    Draw() {
        const ctx = this.config.ctx;
        ctx.lineWidth = 2;
        if (this.config.line == "dashed")
            ctx.setLineDash([10, 6]);
        else
            ctx.setLineDash([]);
        ctx.strokeStyle = this.config.color;
        this.canvas_arrow();
        this.label_draw();
    }
    canvas_arrow() {
        const ctx = this.config.ctx;
        const from = this.config.from;
        const to = this.config.to;
        var headlen = 12; // length of head in pixels
        var dx = to.x - from.x;
        var dy = to.y - from.y;
        var angle = Math.atan2(dy, dx);
        ctx.beginPath();
        ctx.moveTo(from.x, from.y);
        ctx.lineTo(to.x, to.y);
        ctx.stroke();
        ctx.setLineDash([]);
        ctx.beginPath();
        ctx.moveTo(to.x, to.y);
        ctx.lineTo(to.x - headlen * Math.cos(angle - Math.PI / 6), to.y - headlen * Math.sin(angle - Math.PI / 6));
        ctx.moveTo(to.x, to.y);
        ctx.lineTo(to.x - headlen * Math.cos(angle + Math.PI / 6), to.y - headlen * Math.sin(angle + Math.PI / 6));
        ctx.stroke();
    }
    label_draw() {
        const ctx = this.config.ctx;
        ctx.fillStyle = "#000";
        if (this.config.marked)
            ctx.fillStyle = "#4caf50";
        ctx.font = "14px segoe ui";
        ctx.fillText(this.config.label, this.config.left + this.config.width / 2, this.padding.top + this.config.top);
    }
}
