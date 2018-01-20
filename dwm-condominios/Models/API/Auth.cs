namespace DWM.Models.API
{
    public class Auth
    {
        public int Code { get; set; }
        public string Mensagem { get; set; }
        public string Token { get; set; }
        public int UsuarioID { get; set; }
        public string Avatar { get; set; }
    }
}