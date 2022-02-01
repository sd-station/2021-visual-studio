using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using Nancy.Json;

namespace Data.Recent {


    internal class RecentDir {
        /// <summary> Save Category</summary>
        private string Category;

        public ItemMode ItemMode { get; private set; }

        /// <summary>
        /// Get Recent File Path Based On 
        /// User appData Folder And Current Project Name
        /// </summary>
        private FileInfo RecentFilePath { get => new FileInfo(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "savedata", System.Diagnostics.Process.GetCurrentProcess().ProcessName, "recent", $"recent-{Category}.json")); }

        /// <summary> Storeable Object </summary>
        public StoreFile Store { get; private set; }


        /// <summary></summary>
        /// <param name="Category"> Part Of Saved File Like "last-dir" , etc </param>
        public RecentDir(string Category = "last-dir", ItemMode mode = ItemMode.Directory) {
            this.Category = Category;
            this.ItemMode = mode;
            Load();
        }

        internal bool HasLastDirectory() {
            //>> IS EMPTY
            if (string.IsNullOrWhiteSpace(Store.path)) return false;
            if (ItemMode == ItemMode.Directory ) return Directory.Exists(Store.path);
            if (ItemMode == ItemMode.File ) return File.Exists(Store.path);

            return true;
        }


        private void Load() {
            if (RecentFilePath.Exists) {
                Store = new JavaScriptSerializer().Deserialize<StoreFile>(File.ReadAllText(RecentFilePath.FullName));
            } else {
                Store = new StoreFile();
                Store.path = "";
                Store.Items = new List<StoreItem>();
            }
        }

        private void Save() {
            if (RecentFilePath.Directory == null) return;
            if (!RecentFilePath.Directory.Exists) RecentFilePath.Directory.Create();
            File.WriteAllText(RecentFilePath.FullName, new JavaScriptSerializer().Serialize(Store));
        }




        /// <summary> Last Item</summary>
        public string Last {
            get => Store.path;
            set {
                Store.path = value;
                Store.use = DateTime.UtcNow;

                var target = Store.Items.FirstOrDefault(p => p.path == Store.path);
                if (target == null) {
                    target = new StoreItem();
                    target.path = Store.path;
                    target.runs = new List<RunStat>();
                    Store.Items.Add(target);
                }
                target.lastrun = Store.use;

                var Stat = target.runs.FirstOrDefault(p => p.d.Date == target.lastrun.Date);
                if (Stat == null) {
                    Stat = new RunStat();
                    Stat.d = target.lastrun.Date;
                    Stat.c = 0;
                    target.runs.Add(Stat);
                }
                Stat.c += 1;

                Save();
            }
        }


        /// <summary>
        /// is User Use This in hours age
        /// </summary>
        /// <param name="hours"></param>
        /// <returns></returns>
        public string LastTimeUsed(int hours) {

            var validItems = Store.Items.Where(m => File.Exists(m.path)).OrderByDescending(t => t.run).ToList();

            if (!validItems.Any()) return "";

            var b = validItems.First();
            if ((int)(DateTime.UtcNow - b.run).TotalHours < hours) return b.path;

            return "";
        }
    }

    public enum ItemMode {
        All, Directory, File
    }

    public class RunStat {
        public DateTime d { get; set; }
        public int c { get; set; }
    }
    public class StoreItem {
        public string path { get; set; }
        public DateTime run { get; set; }
        public List<RunStat> runs { get; set; }
        public DateTime lastrun { get; set; }
    }

    public class StoreFile {
        public string path { get; set; }
        public DateTime use { get; set; }
        public List<StoreItem> Items { get; set; }
    }
}





