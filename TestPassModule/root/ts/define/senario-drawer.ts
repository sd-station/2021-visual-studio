import { IModuleDraw } from "module-drawer";
import { ISenarioModule } from "senario-data-api";
import { IRelationship } from "senario-rel-api";
import { AppConfig } from "../api/data/AppConfig.js";
import { ArrowDrawer } from "../Media/Drawing/arrow-drawer.js";
import { ModuleDrawer } from "../Media/Drawing/module-draw.js";
import { ActiveData } from './../api/data/ActiveData.js';

export class csSenarioDrawer extends HTMLElement {

    AddCaption(title: string, iii: HTMLDivElement[]) {
        var d = document.createElement("details");
        d.style.marginLeft = "150px";
        this.appendChild(d);
        var s = document.createElement("summary");
        s.textContent = title;
        d.appendChild(s)
        iii.forEach(p => {
            d.appendChild(p)
        })



    }

    items: IRelationship[] = [];



    mermaid!: HTMLPreElement;
    Columns!: ISenarioModule[];

    SetupModules() {
        if (!this.Columns) return;




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
    Display_Data(mode: "colorful" | "black-white" = "colorful"): any {
        var canvas = document.createElement("canvas");
        if (mode == "black-white") canvas.style.backgroundColor = "#ffffff";
        this.appendChild(canvas);

        var mx = 1;
        this.items.sort((a, b) => a.position.index - b.position.index).forEach(itm => {
             itm.runtime.row = mx++;
        })

        this.Columns = JSON.parse(JSON.stringify(this.Columns));

        var inx = 1;
        this.Columns.sort((a, b) => a.index - b.index).forEach(itm => {
            var underuse = this.items.find(h => h.position.from == itm.id || h.position.to == itm.id);
            if (underuse) {
                itm.underuse = true;
                itm.index = inx++;
            }

        })
        if (this.Columns.filter(o => o.underuse).length == 0) {
            this.removeChild(canvas);
            return;
        }

        canvas.style.height = this.items.length * AppConfig.height + 2 * AppConfig.height + "px";
        canvas.style.width = this.Columns.filter(o => o.underuse).length * AppConfig.width + "px";
        canvas.height = canvas.clientHeight;
        canvas.width = canvas.clientWidth;

         


        this.Columns.filter(o => o.underuse).forEach(itm => {
            var r = {} as IModuleDraw;
            r.Title = "";
            r.start = itm.index;
            r.BorderColor = "#000000";
            r.TextColor = "#ffffff";
            r.height = canvas.height;
            if (mode == "colorful") {
                r.BorderColor = itm.theme;
            }
            r.Title = itm.title;
            var h = new ModuleDrawer(canvas, r)
            h.Draw();            
        })

       
        
     
        
        this.Columns.filter(o => o.underuse).forEach(itm => {
            if(itm.module <=0) return;
            var mdl = ActiveData.General.Modules.find(p => p.id == itm.module);
            if(!mdl || mdl.mode < 10) return;
            var r = {} as IModuleDraw;
            r.Title = "";
            r.start = itm.index;
            r.BorderColor = "#000000";
            r.TextColor = "#ffffff";
            r.height = canvas.height;
            if (mode == "colorful") {
                r.BorderColor = itm.theme;
            }

            var h = new ModuleDrawer(canvas, r)
            h.ActivePoint(this.items.filter(u => u.position.to == itm.id) ,this.items.filter(u => u.position.from == itm.id)  );
        })

        this.items.forEach(itm => {
          
            
            var start = this.Columns.find(u => u.id == itm.position.from);
            var end = this.Columns.find(u => u.id == itm.position.to);
            console.log("SDACC" ,itm.title , itm.position.from , itm.position.to);

            if (!start || !end) return;  

            new ArrowDrawer(canvas, {
                start: start.index - 1,
                end: end.index - 1,
                top: itm.runtime.row,
                Title: itm.title,
                LineStyle: itm.actionmode == 1 ? "dashed" : "solid",
                BorderColor: mode == "colorful" ? itm.runtime.theme : "#000000",
                TextColor: mode == "colorful" ? "#ffffff" : "#000000",
                IsMarked: itm.group.id > 0 && itm.runtime.active
            }).Draw();

        })

        var img = document.createElement("img");
        img.classList.add(mode)
        img.src = canvas.toDataURL();
        this.appendChild(img);
        this.removeChild(canvas);
    }

    Print(): void {
        if (!this.querySelector("img.black-white")) this.Display_Data("black-white")
    }


    constructor() {

        super();
        /// uncomment to use shadow
        //this.InitalizeComponent();
    }
}

window.customElements.define("senario-drawer", csSenarioDrawer)