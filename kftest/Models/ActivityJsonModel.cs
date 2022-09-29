using System.Collections.Generic;

namespace kftest.Models
{
    public class ActivityJsonModel
    {
        public List<Activity> activity { get; set; }
    }
    public class Activity
    {
        public string type { get; set; }
        public string displayName { get; set; }
        public string description { get; set; }
        public string runtimeDescription { get; set; }
        public string category { get; set; }
        public string icon { get; set; }
        public string[] outcomes { get; set; }
        public List<Property> properties { get; set; }
    }

    public class PropertyOptions
    {
        public List<object> items { get; set; }
        public bool? multiline { get; set; }
    }

    public class Property
    {
        public string name { get; set; }
        public string type { get; set; }
        public string label { get; set; }
        public string hint { get; set; }
        //public PropertyOptions options { get; set; }
    }

}
