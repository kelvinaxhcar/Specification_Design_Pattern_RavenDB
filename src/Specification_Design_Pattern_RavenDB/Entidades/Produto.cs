namespace Specification_Design_Pattern_RavenDB.Entidades
{
    public class Produto
    {
        public string Id { get; set; }
        public string Nome { get; set; }
        public string Cidade { get; set; }
        public decimal Preco { get; set; }
        public string Marca { get; set; }
        public DateTime Data { get; set; }
        public Tipo Tipo { get; set; }
    }

    public enum Tipo
    {
        Teste,
        Producao
    }
}
