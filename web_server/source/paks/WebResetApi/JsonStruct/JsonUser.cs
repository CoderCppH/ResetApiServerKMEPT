namespace web_server
{
    class JsonUser
    {
        public int id { get; set; }            
        public string fullname { get; set; } = string.Empty;
        public string email { get; set; } = string.Empty;
        public string password { get; set; } = string.Empty;
    }
}