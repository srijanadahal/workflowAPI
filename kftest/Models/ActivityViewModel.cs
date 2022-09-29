using System.Collections.Generic;

namespace kftest.Models
{
    public class ActivityViewModel
    {
        public int Id { get; set; }
        public string ActivityType { get; set; }
        public string DisplayName { get; set; }
        public string Description { get; set; }
        public string RuntimeDescription { get; set; }
        public string Category { get; set; }
        public string Icon { get; set; }
        public string[] Outcomes { get; set; }
        public List<string> properties { get; set; }
    }

    public class ActivityStringViewModel
    {
        public int Id { get; set; }
        public string ActivityType { get; set; }
        public string DisplayName { get; set; }
        public string Description { get; set; }
        public string RuntimeDescription { get; set; }
        public string Category { get; set; }
        public string Icon { get; set; }
        //public string[] Outcomes { get; set; }
        //public string Properties { get; set; }
    }

    public class ActivityJsonViewModel
    {
        public string type { get; set; }
        public string displayName { get; set; }
        public string description { get; set; }
        public string runtimeDescription { get; set; }
        public string category { get; set; }
        public string icon { get; set; }
        public string[] outcomes { get; set; }
        public List<PropertyJsonViewModel> properties { get; set; }
    }
    public class PropertyJsonViewModel
    {
        public string name { get; set; }
        public string type { get; set; }
        public string label { get; set; }
        public string hint { get; set; }
    }

    public class outcomeViewModel
    {
        public string outcome { get; set; }
    }

    public class ActivityViewModelJsonReturn
    {
        public string type { get; set; }
        public string displayName { get; set; }
        public string description { get; set; }
        public string runtimeDescription { get; set; }
        public string category { get; set; }
        public string icon { get; set; }
        public string[] outcome { get; set; }
        public List<PropertyJsonViewModel> properties { get; set; }
    }

}
