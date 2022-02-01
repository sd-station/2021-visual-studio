import { IModule, ISenario } from "module-api";
 
class ActiveDatageneralClass{
    Modules: IModule[] = [];
    Senario: ISenario[] = [];

}
class ActiveDataClass{

    General = new ActiveDatageneralClass ();

}

export const ActiveData= new ActiveDataClass();