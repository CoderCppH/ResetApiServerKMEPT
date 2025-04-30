namespace Orm.Type {
    public class Message {
        public int id { get; set; }
        public int user_from_id { get; set; }
        public int user_to_id { get; set; }
        public string message { get; set; } = "";
        public DateTime timestamp { get; set; }
    }
}