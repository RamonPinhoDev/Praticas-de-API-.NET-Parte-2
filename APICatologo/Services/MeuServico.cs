namespace APICatologo.Services
{
    public class MeuServico : IMeuServoco
    {
        public string Saudacao(string nome)
        {
            nome = $"Oi, eu sou Goku! e vc quem?{nome}, e hj é {DateTime.UtcNow}";

            return nome; 
        }
    }
}
