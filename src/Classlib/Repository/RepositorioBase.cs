using Classlib.Entities;
using Classlib.Enums;

namespace Classlib.Repository
{
    public class RepositorioBase<T> where T : EntidadeBase
    {
        public List<T> Elementos { get; set; }

        public RepositorioBase() => Elementos = new List<T>();

        public void AdicionarElemento(T elemento) => Elementos.Add(elemento);

        public void ApagarElemento(string id) => Elementos.Remove(RetornarElemento(id));

        public T RetornarElemento(string id) =>
                Elementos.FirstOrDefault(e => e.Id == id)
             ?? throw new Exception($"O elemento com id {id} não foi encontrado ou não existe.");
    }

}