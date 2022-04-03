using Classlib.Enums;

namespace Classlib.Entities
{
    public class Transacao : EntidadeBase
    {  
        public string Descricao { get; protected set; }
        public int NumContaOrigem { get; protected set; }
        public decimal Valor { get; protected set;}
        public DateOnly Data { get; protected set; }
        public TipoTransacaoEnum TipoTransacao { get; protected set; }

        public Transacao(string Id, string Descricao, int NumContaOrigem, decimal Valor, DateOnly Data, TipoTransacaoEnum TipoTransacao) : base(Id)
        {
            this.Descricao = Descricao;
            this.NumContaOrigem = NumContaOrigem;
            this.Valor = Valor;
            this.Data = Data;
            this.TipoTransacao = TipoTransacao;
        }
    }
}
