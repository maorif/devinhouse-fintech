using Classlib.Enums;

namespace Classlib.Entities
{
    public class Conta : EntidadeBase
    {
        public string Nome { get; protected set; }
        public string Cpf { get; protected set; }
        public object Endereco { get; protected set; }

        public decimal RendaMensal { get; protected set; }

        public int NumeroConta { get; protected set; }

        public AgenciaEnum Agencia { get; protected set; }
        public decimal Saldo { get; protected set; }
        public List<Transacao> Transacoes { get; protected set; }
        public virtual TipoContaEnum TipoConta { get; protected set; }

        public Conta(string Id, string Nome, string Cpf, object Endereco, decimal RendaMensal, int NumeroConta, AgenciaEnum Agencia) : base(Id)
        {
            this.Nome = Nome;
            this.Cpf = Cpf;
            this.Endereco = Endereco;
            this.RendaMensal = RendaMensal;
            this.NumeroConta = NumeroConta;
            this.Agencia = Agencia;
            this.Saldo = decimal.Zero;

            this.Transacoes = new List<Transacao>();
        }
        
        public Transacao Saque(decimal valorSaque){
            
            if(!ChecaPossivelTransacao(valorSaque)){
                throw new Exception("Saldo insuficiente.");
            }

            if(valorSaque < decimal.Zero){
                throw new Exception("Valor de saque inválido.");
            }

            Saldo -= valorSaque;
            string id = new Guid().ToString();
            var saque = new Transacao(id, "Saque", this.NumeroConta, valorSaque, DateOnly.FromDateTime(DateTime.Now), TipoTransacaoEnum.despesa);
            this.Transacoes.Add(saque);

            return saque;
        }

        public Transacao Deposito(decimal valorDeposito){

            if(valorDeposito <= decimal.Zero){
                throw new Exception("Valor inválido.");
            }

            Saldo += valorDeposito;
            string id = new Guid().ToString();
            var deposito = new Transacao(id, "Depósito", this.NumeroConta, valorDeposito, DateOnly.FromDateTime(DateTime.Now), TipoTransacaoEnum.receita);
            this.Transacoes.Add(deposito);

            return deposito;
        }

        public decimal SaldoAtual(){

            return this.Saldo;
        }

        public List<Transacao> Extrato(DateOnly dataInicial, DateOnly dataFinal){

            return this.Transacoes.Where(t => t.Data >= dataInicial && t.Data <= dataFinal).ToList() ?? throw new Exception("Nenhuma transação encontrada.");
        }

        public Transferencia EnviaTransferencia(int numContaDestino, decimal valorTransferencia){

            if(!ChecaPossivelTransacao(valorTransferencia)){
                throw new Exception("Saldo insuficiente.");
            }

            this.Saldo -= valorTransferencia;
            string id = new Guid().ToString();
            var transferencia = new Transferencia(id, "Transferência", this.NumeroConta, numContaDestino, valorTransferencia, DateOnly.FromDateTime(DateTime.Now), TipoTransacaoEnum.despesa);
            this.Transacoes.Add(transferencia);
            
            return transferencia;
        }

        public Transferencia RecebeTransferencia(int numContaOrigem, decimal valorTransferencia){

            this.Saldo += valorTransferencia;
            string id = new Guid().ToString();
            var transferencia = new Transferencia(id, "Transferência", numContaOrigem, this.NumeroConta, valorTransferencia, DateOnly.FromDateTime(DateTime.Now), TipoTransacaoEnum.receita);
            this.Transacoes.Add(transferencia);

            return transferencia;
        }

        public string MudarDadosCadastrais(string nome, object endereco, decimal rendaMensal){
            this.Nome = nome;
            this.Endereco = endereco;
            this.RendaMensal = rendaMensal;

            return "Dados alterados com sucesso!";
        }

        public virtual bool ChecaPossivelTransacao(decimal valor) => valor <= this.Saldo ? true : false;
    }
}