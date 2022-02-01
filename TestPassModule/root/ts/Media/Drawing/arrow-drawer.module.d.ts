
declare module "arrow-drawer"{
    interface IArrow {
        IsMarked: boolean;
        BorderColor: string;
        TextColor: string;
        start: number;
        end: number;
        top: number;
        Title: string,
        LineStyle: "solid" | "dashed"
    }
    
}

