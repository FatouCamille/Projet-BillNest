namespace BillNest.Models
{
    public class Client
    {
        public int Id { get; set; }
        public string Nom { get; set; }
        public string Prenom { get; set; }
        public string? Email { get; set; }     // Le ? permet d'accepter une valeur vide
        public string? Telephone { get; set; }
    }
}