using Classlib.Enums;

namespace Classlib.Entities
{
    public class Investimento : Transacao
    {
        public TipoInvestimentoEnum TipoInvestimento { get; protected set; }
        public decimal ValorAtual { get; private set; }
        public int MesesDeAplicacao { get; private set; }
        public decimal RendimentoPrevisto { get; private set; }


        public Investimento(string Id, string Descricao, int NumContaOrigem, decimal Valor, int MesesDeAplicacao, DateOnly Data, TipoTransacaoEnum TipoTransacao, TipoInvestimentoEnum TipoInvestimento) : base(Id, Descricao, NumContaOrigem, Valor, Data, TipoTransacao)
        {
            this.TipoInvestimento = TipoInvestimento;
            this.ValorAtual = Valor;
            this.MesesDeAplicacao = MesesDeAplicacao;
            this.RendimentoPrevisto = PreverRendimento(MesesDeAplicacao);
        }

        public decimal PreverRendimento(int meses){

            decimal rendimento = this.ValorAtual;

            for(int i = 0; i < meses; i++){
                rendimento += rendimento * ((decimal)this.TipoInvestimento)/100;
            }

            return rendimento;
        }

        public decimal CalcularRendimentoDiario(){
            
            decimal rendimentoDiario = ((decimal)this.TipoInvestimento)/(100 * 30); //valor do rendimento diário

            return rendimentoDiario;
        }

        public void AtualizaValorAtual(){

            this.ValorAtual = this.ValorAtual * CalcularRendimentoDiario();
        }
    }
}