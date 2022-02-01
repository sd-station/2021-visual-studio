export class csSenarioPackage extends HTMLElement {
    container;
    item;
    relations = [];
    relGroup = [];
    InitalizeComponent() {
        // Create a shadow root
        this.container = this.attachShadow({ mode: 'open' });
    }
    connectedCallback() {
        fetch(this.getAttribute("data-id") + "/module.info.json")
            .then(x => x.json())
            .then(x => {
            this.item = x;
        });
        fetch(this.getAttribute("data-id") + "/uml-rel.json")
            .then(x => x.json())
            .then(x => {
            this.relations = x;
        });
        fetch(this.getAttribute("data-id") + "/uml-rel-group.json")
            .then(x => x.json())
            .then(x => {
            this.relGroup = x;
        });
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
    Display_Data() {
        let R = 0;
        this.relGroup.forEach(itm => {
            itm.rels.forEach((A, i) => {
                var h4 = document.createElement("h4");
                h4.textContent = "Solution " + String.fromCharCode("A".charCodeAt(0) + R++);
                this.appendChild(h4);
                var n = document.createElement("senario-drawer");
                this.appendChild(n);
            });
        });
    }
    constructor() {
        +super();
        /// uncomment to use shadow
        //this.InitalizeComponent();
    }
}
window.customElements.define("senario-package", csSenarioPackage);
