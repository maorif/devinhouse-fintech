using Classlib.Entities;

namespace Classlib.Repository
{
    public class RepositorioContas : RepositorioBase<Conta>
    {
        public Conta RetornaContaPeloNumero(int numConta) =>
            Elementos.FirstOrDefault(c => c.NumeroConta == numConta);
    }
}
