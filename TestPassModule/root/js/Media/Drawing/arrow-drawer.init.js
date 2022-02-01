import { AppConfig } from "../../api/data/AppConfig.js";
export class ArrowDrawer {
    padding = { top: 20 };
    Options;
    get Left() {
        return Math.min(...[this.Options.start, this.Options.end]) * AppConfig.width + AppConfig.width / 2;
    }
    get Top() {
        return this.Options.top * AppConfig.height + AppConfig.height / 2 - 5;
    }
    get From() {
        return {
            x: this.Options.start * AppConfig.width + 1 + AppConfig.width / 2,
            y: this.padding.top + this.Options.top * AppConfig.height + 1
        };
    }
    get To() {
        return {
            x: this.Options.end * AppConfig.width + 1 + AppConfig.width / 2,
            y: this.padding.top + this.Options.top * AppConfig.height + 1
        };
    }
    get Width() {
        return Math.abs(this.Options.start - this.Options.end) * AppConfig.width;
    }
    constructor(canvas, opt) {
        this.ctx = canvas.getContext("2d");
        this.Options = opt;
    }
    ctx;
    Draw() {
        new ArrowClass(this);
    }
}
class ArrowClass {
    padding = { top: 20 };
    Config;
    constructor(config) {
        this.Config = config;
        this.Draw();
    }
    Draw() {
        const ctx = this.Config.ctx;
        ctx.lineWidth = 2;
        if (this.Config.Options.LineStyle == "dashed")
            ctx.setLineDash([10, 6]);
        else
            ctx.setLineDash([]);
        ctx.strokeStyle = this.Config.Options.BorderColor;
        this.canvas_arrow();
        this.label_draw();
    }
    canvas_arrow() {
        const ctx = this.Config.ctx;
        const from = this.Config.From;
        const to = this.Config.To;
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
        const ctx = this.Config.ctx;
        ctx.fillStyle = this.Config.Options.TextColor;
        if (this.Config.Options.IsMarked)
            ctx.fillStyle = this.Config.Options.BorderColor;
        ctx.font = "14px segoe ui";
        ctx.fillText(this.Config.Options.Title, this.Config.Left + this.Config.Width / 2, this.padding.top + this.Config.Top);
        if (this.Config.Options.IsMarked && this.Config.Options.BorderColor == "#000000")
            this.underline(ctx, this.Config.Options.Title, this.Config.Left + this.Config.Width / 2, this.padding.top + this.Config.Top);
    }
    measureText = (ctx, text) => {
        let metrics = ctx.measureText(text);
        return {
            width: Math.floor(metrics.width),
            //  height: Math.floor(metrics.fontBoundingBoxAscent + metrics.fontBoundingBoxDescent),
            actualHeight: Math.floor(metrics.actualBoundingBoxAscent + metrics.actualBoundingBoxDescent)
        };
    };
    underline(ctx, text, x, y) {
        let metrics = this.measureText(ctx, text);
        let fontSize = Math.floor(metrics.actualHeight * 1.4); // 140% the height 
        switch (ctx.textAlign) {
            case "center":
                x -= (metrics.width / 2);
                break;
            case "right":
                x -= metrics.width;
                break;
        }
        switch (ctx.textBaseline) {
            case "top":
                y += (fontSize);
                break;
            case "middle":
                y += (fontSize / 2);
                break;
        }
        y += 3;
        ctx.save();
        ctx.beginPath();
        //ctx.strokeStyle = ctx.fillStyle
        ctx.lineWidth = 1;
        ctx.moveTo(x, y);
        ctx.lineTo(x + metrics.width, y);
        ctx.stroke();
        ctx.restore();
    }
}
