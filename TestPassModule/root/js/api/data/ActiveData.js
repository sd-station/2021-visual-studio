class ActiveDatageneralClass {
    Modules = [];
    Senario = [];
}
class ActiveDataClass {
    General = new ActiveDatageneralClass();
}
export const ActiveData = new ActiveDataClass();
