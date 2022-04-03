using Classlib.Entities;

namespace Classlib.Interfaces
{
    public interface IRepositorioBase<T> where T : EntidadeBase
    {
        public void AdicionarElemento(T elemento);

        public void ApagarElemento(string id);

        public T RetornarElemento(string id);
    }
}