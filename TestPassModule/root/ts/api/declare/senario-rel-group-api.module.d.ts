declare module "senario-rel-group-api" {
export interface IRelGroup {
    title: string;
    create: string;
    update: string;
    id:     number;
  
    bound:  Bound;
}

export interface Bound {
    left:   number;
    top:    number;
    bottom: number;
    right:  number;
}
 

}