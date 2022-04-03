using Classlib.Enums;
namespace Classlib.Entities
{
    public class Transferencia : Transacao
    {
        public int NumContaDestino { get; protected set; }

        public Transferencia(string Id, string Descricao, int NumContaOrigem, int NumContaDestino, decimal Valor, DateOnly Data, TipoTransacaoEnum TipoTransacao) : base(Id, Descricao, NumContaOrigem, Valor, Data, TipoTransacao)
        {
            this.NumContaDestino = NumContaDestino;
        }
    }
}