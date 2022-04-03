using Classlib.Enums;

namespace Classlib.Entities
{
    public class ContaCorrente : Conta
    {
        public decimal TaxaChequeEspecial { get; private set; } = 0.01m;
        public decimal ChequeEspecial { get; private set; }
        public override TipoContaEnum TipoConta { get; protected set; } = TipoContaEnum.Corrente;

        public ContaCorrente(string Id, string Nome, string Cpf, object Endereco, decimal RendaMensal, int NumeroConta, AgenciaEnum Agencia) : base(Id, Nome, Cpf, Endereco, RendaMensal, NumeroConta, Agencia)
        {
            this.ChequeEspecial = this.RendaMensal * this.TaxaChequeEspecial;
        }

        public override bool ChecaPossivelTransacao(decimal valor) => valor <= (this.Saldo + this.ChequeEspecial) ? true : false;
    }
}
