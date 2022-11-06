namespace education_project_backend.Models.Todo
{
    public class Workspace
    {
        public string id { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public Workspace()
        {

        }
        public Workspace(string id, string name, string description)
        {
            this.id = id;
            this.name = name;
            this.description = description;
        }
    }
}
