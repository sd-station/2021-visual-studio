import { ActiveData } from "./api/data/ActiveData.js";
var moduleContainer = document.querySelector("#page-modules");
var MainArticle = document.querySelector("#main-article");
fetch("uml.json").then(t => t.json()).then(t => {
    ActiveData.General.Modules = t;
    ActiveData.General.Modules.forEach(mdl => {
        var el = document.createElement("module-item");
        el.style.borderColor = mdl.theme;
        el.style.color = mdl.theme;
        el.textContent = mdl.title;
        moduleContainer.appendChild(el);
    });
    LoadSenario();
});
function LoadSenario() {
    fetch("senario.json").then(t => t.json()).then(t => {
        ActiveData.General.Senario = t;
        var datac = ActiveData.General.Senario;
        if (window.location.search.length > 0) {
            if (window.location.search.indexOf("senario=") >= 0) {
                datac = datac.filter(k => window.location.search.indexOf(k.id) > 0);
            }
        }
        datac.forEach(sen => {
            var h3 = document.createElement("h3");
            h3.textContent = sen.name;
            MainArticle.appendChild(h3);
            var el = document.createElement("senario-package");
            el.setAttribute("data-id", sen.id);
            MainArticle.appendChild(el);
        });
    });
}
window.addEventListener("beforeprint", function (event) {
    document.querySelectorAll("details").forEach(g => {
        g.open = true;
    });
    var content = document.querySelectorAll("senario-drawer");
    content.forEach(el => el.Print());
});
setTimeout(() => {
    document.querySelectorAll("details").forEach(g => {
        g.open = true;
    });
}, 2000);
