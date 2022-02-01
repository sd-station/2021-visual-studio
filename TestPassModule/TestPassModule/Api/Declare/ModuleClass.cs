using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestPassModule.Api.Declare {
    public class ModuleClass : IThemeColor {
        public int id { get; set; }       
        public string title { get; set; }
        public DateTime create { get; set; }
        public DateTime update { get; set; }
        public string theme { get; set; }       
        public ModuleModes  mode { get; set; }

    }

  

    public enum ModuleModes {
        ///>> None
        None = 0,

        ///>> Person
        Role_User = 101,
        Role_Customer = 102,
        Role_Visitor = 103,
        Role_Admin = 104,
        Role_Owner = 105, 
        Role_System = 106, // scheduler 
        Role_Sub_System = 107, // timer

        ///>> Environmet
        Env_OperationSystem = 200,// website 
        Env_Application = 201, // server 
        Env_Project = 202, // calendar 
        Env_Module = 203, // start page
        Env_Class = 204, // AuthData 
        Env_Function = 205, // Auth Reader
        Env_Interface = 206, // Auth Rule

        ///>> User Interface
        Ui_Main_Page = 301, // Main Window
        Ui_Index_Page = 302, // Product Index
        Ui_Preview_Page = 303, // Product View
        Ui_User_Control = 304, // Product Line
        Ui_Editor = 305, // Product Editor
        Ui_Other = 306, // Product title element

        ///>> Data Manager
        Data_Holder = 400, // active data       
        Data_Local_Handler = 401, // session storage
        Data_Api_External = 402, // statistics    
        Data_Api_Server = 403,   // Acccount      
        Data_Server = 404, // Sql Server

        ///>> Provider
        Provide_Data_Loader = 501, // Post / Get Conver
        Provide_Data_Checker = 502, // Auth Require
        Provide_Data_Router = 503, // Auth Require
        Provide_Content = 504, // CDN
        Provide_Data_Saver = 505, // Post / Get Conver
        Provide_Data_Sender = 506, // Post / Get Conver
         
    }

    public static class ModuleModesNames {
        public static string GetCategory(this ModuleModes value) {
            var txt = value.ToString().Split('_').First();

            if (txt == "Provider") return "Data / Service Provider";
            if (txt == "Data") return "Data Store";
            if (txt == "Ui") return "Front End";
            if (txt == "Env") return "Code Behind";
            if (txt == "Role") return "Role Player";

            return txt;
        }
        public static string GetTitle(this ModuleModes value) {
            var txt = value.ToString();
            if (txt.IndexOf("_") > 0) txt = txt.Substring(txt.IndexOf("_") + 1);
            return txt.Replace("_", " ");
        }
    }
}
