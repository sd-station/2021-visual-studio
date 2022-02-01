declare module "senario-rel-api" {
  
    export interface IRelationship {
        id: number;
        title: string;
        create: string;
        actionmode: number;
        position: Position;
        runtime: Runtime;
        doc: Doc;
        group: Group;
    }

    export interface Doc {
        actions: string[];
    }

    export interface Group {
        id: number;
        caption: string;
    }

    export interface Position {
        parent: number;
        isContinue: boolean;
        index: number;
        from: number;
        to: number;
    }

    export interface Runtime {
        row: number;
        active: boolean;
        state?: "pending" | "success" | "failed" | "stop";
        theme: string;
    }

 

    

}