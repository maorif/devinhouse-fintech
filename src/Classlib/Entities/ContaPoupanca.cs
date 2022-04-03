using Classlib.Enums;

namespace Classlib.Entities
{
    public class ContaPoupanca : Conta
    {
        public decimal TaxaRendimento { get; private set; }
        public override TipoContaEnum TipoConta { get; protected set; } = TipoContaEnum.Poupanca;

        public ContaPoupanca(string Id, string Nome, string Cpf, object Endereco, decimal RendaMensal, int NumeroConta, AgenciaEnum Agencia, decimal TaxaRendimento) : base(Id, Nome, Cpf, Endereco, RendaMensal, NumeroConta, Agencia)
        {
            this.TaxaRendimento = TaxaRendimento;
        }

        public decimal SimulaRendimento(int meses, decimal rentabilidade)
        {
            decimal rendimento = this.Saldo;

            for(int i = 0; i < meses; i++)
            {
                rendimento += rendimento * rentabilidade;
            }

            return rendimento;
        }
    }
}