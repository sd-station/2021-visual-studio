export class ConfigClass {
    padding = { x: 60, y: 30 };
    boxsize = { x: 30, y: 30 };
    gap = { x: 200, y: 100 };
}
export const Config = new ConfigClass();
class IDP {
    x;
    y;
}
export class CRelationShipDraw {
    ctx;
    LineColor = "#a0a0cc";
    MarkColor = "#a0a0cc";
    Arrow = {
        StartPoint: {},
        EndPoint: {},
        theta: 0,
        sint: 0,
        cost: 0,
        Head: {
            M: {},
            WingR: {},
            WingL: {},
            Forward: {},
            Backward: {},
        },
        HeadConfig: {
            WingSize: 16,
            ArrowSize: 11
        }
    };
    ArrowHead() {
        let ctx = this.ctx;
        let EndPoint = this.Arrow.EndPoint;
        this.Arrow.theta = Math.atan2(this.Arrow.Head.M.y - this.Arrow.EndPoint.y, this.Arrow.Head.M.x - this.Arrow.EndPoint.x);
        let sint = Math.sin(this.Arrow.theta);
        this.Arrow.sint = sint;
        let cost = Math.cos(this.Arrow.theta);
        this.Arrow.cost = cost;
        this.Arrow.Head.Forward = new IDP();
        this.Arrow.Head.Forward.x = this.Arrow.EndPoint.x;
        this.Arrow.Head.Forward.y = this.Arrow.EndPoint.y;
        this.Arrow.Head.Backward = new IDP();
        this.Arrow.Head.Backward.x = this.Arrow.EndPoint.x;
        this.Arrow.Head.Backward.y = this.Arrow.EndPoint.y;
        if (this.Arrow.Head.M.x == EndPoint.x) {
            this.Arrow.Head.Backward.y -= this.Arrow.HeadConfig.ArrowSize;
        }
        else if (this.Arrow.Head.M.y == EndPoint.y) {
            this.Arrow.Head.Backward.x -= this.Arrow.HeadConfig.ArrowSize;
        }
        else {
            this.Arrow.Head.Backward.x += this.Arrow.HeadConfig.ArrowSize * cost;
            this.Arrow.Head.Backward.y += this.Arrow.HeadConfig.ArrowSize * sint;
        }
        this.Arrow.Head.WingR = new IDP();
        this.Arrow.Head.WingR.x = this.Arrow.EndPoint.x + (this.Arrow.HeadConfig.WingSize * cost - this.Arrow.HeadConfig.WingSize * sint);
        this.Arrow.Head.WingR.y = this.Arrow.EndPoint.y + (this.Arrow.HeadConfig.WingSize * sint + this.Arrow.HeadConfig.WingSize * cost);
        this.Arrow.Head.WingL = new IDP();
        this.Arrow.Head.WingL.x = this.Arrow.EndPoint.x + (this.Arrow.HeadConfig.WingSize * cost + this.Arrow.HeadConfig.WingSize * sint);
        this.Arrow.Head.WingL.y = this.Arrow.EndPoint.y + (this.Arrow.HeadConfig.WingSize * sint - this.Arrow.HeadConfig.WingSize * cost);
        ctx.fillStyle = this.MarkColor;
        ctx.beginPath();
        ctx.moveTo(this.Arrow.Head.Forward.x, this.Arrow.Head.Forward.y);
        ctx.lineTo(this.Arrow.Head.WingR.x, this.Arrow.Head.WingR.y);
        ctx.lineTo(this.Arrow.Head.Backward.x, this.Arrow.Head.Backward.y);
        ctx.lineTo(this.Arrow.Head.WingL.x, this.Arrow.Head.WingL.y);
        ctx.closePath();
    }
}
