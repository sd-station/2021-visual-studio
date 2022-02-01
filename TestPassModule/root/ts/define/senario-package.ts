import { ISenarioData } from "senario-data-api";
import { IRelationship } from "senario-rel-api";
import { IRelGroup } from "senario-rel-group-api";
import { csSenarioDrawer } from "./senario-drawer.js";


export class csSenarioPackage extends HTMLElement {
    container!: ShadowRoot;
    SenarioData!: ISenarioData;
    relations: IRelationship[] = [];
    relGroup: IRelGroup[] = [];
    InitalizeComponent(): any {
        // Create a shadow root
        this.container = this.attachShadow({ mode: 'open' });
    }
    connectedCallback() {
        fetch(this.getAttribute("data-id") + "/module.info.json")
            .then(x => x.json())
            .then(x => {
                this.SenarioData = x as ISenarioData;

            })

        fetch(this.getAttribute("data-id") + "/uml-rel.json")
            .then(x => x.json())
            .then(x => {
                this.relations = x as IRelationship[];

            })


        fetch(this.getAttribute("data-id") + "/uml-rel-group.json")
            .then(x => x.json())
            .then(x => {
                this.relGroup = x as IRelGroup[];

            })

        setTimeout(() => {
            this.Display_Data();
        }, 1000);

    }
    //disconnectedCallback() {
    //...
    //}
    //ributeChangedCallback(name, oldValue, newValue) {
    //...
    //
    //doptedCallback() {
    //...
    //

    /**
     * Display Item
     */

    Display_Data(): any {
        if (!this.SenarioData) return;
        let R = 0;

        this.relGroup.forEach(itm => {
            this.relations.filter(k => k.group.id == itm.id).forEach((u, i) => {
                u.runtime.active = i == 0 ? true : false;
            });
        });


        this.DrawASolution(this.sln++);

        if (this.groups.length > 0) {
            this.DrawGroup();
        }

    }
    sln = 0;
    DrawGroup() {


        //>> Find Last Group
        var FINDEX = this.groups.length - 1;
        while (FINDEX >= 0) {



            var TGroup = this.relGroup.find(y => y.id == this.groups[FINDEX])!;
            //>> Find Active Index
            var ax = -1;
            var rels = this.relations.filter(k => k.group.id == TGroup.id);
            rels.forEach((itm, i) => {
                if (itm.runtime.active) ax = i;
            })


            if (ax < rels.length - 1) {
                //>> Inactive Item in Ax
                this.relations.find(k => k.id == rels[ax].id)!.runtime.active = false;
                //>> Active Item in Ax +1
                this.relations.find(k => k.id == rels[ax + 1].id)!.runtime.active = true;
                this.DrawASolution(this.sln++);
                this.DrawGroup();
                return;
            }
            FINDEX--;
        }

    }
    DrawASolution(R: number) {
        var h4 = document.createElement("h4");
        h4.textContent = "Solution " + String.fromCharCode("A".charCodeAt(0) + R);
        this.appendChild(h4);

        var n = document.createElement("senario-drawer") as csSenarioDrawer;
        this.appendChild(n);

        n.Columns = this.SenarioData.modules;
        var xc = [] as IRelationship[];
        this.AddRelChilds(xc, 0);
        n.items = JSON.parse(JSON.stringify(xc));

        n.SetupModules();
        n.Display_Data();

        n.AddCaption("Conditions", this.StateOfGroups());
    }
    StateOfGroups(): HTMLDivElement[] {

        var res = [] as HTMLDivElement[];
        this.relGroup.forEach(p => {

            var act = this.relations.filter(k => k.group.id == p.id).map((itm, i) => {
                return `${itm.runtime.active ? "✔️" : "❌"} ${itm.group.caption.length > 0 ? itm.group.caption : itm.title}`
            })
            if (act.length == 0) return;
            var dv = document.createElement("div");
            dv.classList.add("detail-content-box")
           
            var k = document.createElement("h6")
            k.innerHTML = `Alt ${p.id} ${p.title}`;
            dv.append(k);
            var pre = document.createElement("pre")
            pre.innerHTML = `${act.join("\n")}`;
            dv.append(pre);
            res.push(dv)
        })

        return res;
    }
    AddRelChilds(n: IRelationship[], arg0: number) {
        var starters = this.relations.filter(x => x.position.parent == arg0);
        starters.forEach(rel => {

            if (rel.group.id > 0) {
                this.FindGroup(rel.group.id);
            }
            if (rel.group.id <= 0 || (rel.group.id > 0 && rel.runtime.active)) {
                n.push(rel);
                this.AddRelChilds(n, rel.id);


                if (rel.doc && rel.doc.actions && rel.doc.actions.length > 0) {

                    rel.doc.actions.forEach(c => {
                        if (c.startsWith("next")) {
                            var k = c.substring(4).trim();
                            if (k.startsWith("to")) k = k.substring(2).trim();
                            var u = parseInt(k);

                            if (u > 0) {
                                var sp = this.relations.find(n22 => n22.id == u);
                                if (sp != null) {

                                    n.push(sp);
                                    this.AddRelChilds(n, sp.id);
                                }
                            }
                        }
                    });
                }
            }



        })
    }
    groups = [] as number[];
    FindGroup(group: number) {
        if (this.groups.indexOf(group) < 0 && this.relGroup.find(u => u.id == group)) this.groups.push(group);
    }


    constructor() {
        +
            super();
        /// uncomment to use shadow
        //this.InitalizeComponent();
    }
}

window.customElements.define("senario-package", csSenarioPackage)