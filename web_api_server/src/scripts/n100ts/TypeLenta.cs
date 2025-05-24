namespace Orm.Type {
    public class Lenta
    {
        public int id { get; set; }
        public string name_post { get; set; } = "";
        public string description_post { get; set; } = "";
        public string image_post { get; set; } = "";
        public int id_user { get; set; }
    }
}