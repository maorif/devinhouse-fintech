using Classlib.Entities;
using Classlib.Enums;
using Classlib.Repository;

namespace App.Fintech
{
    public class Fintech
    {
        public DateOnly Data { get; protected set; }
        public Conta ContaLogada { get;  protected set; }
        public RepositorioContas RepositorioContas { get; private set; }
        public RepositorioTransferencias RepositorioTransferencias { get; protected set; }

        public RepositorioInvestimentos RepositorioInvestimentos { get; protected set; }

        public Fintech(){
            this.RepositorioContas = new RepositorioContas();
            this.RepositorioTransferencias = new RepositorioTransferencias();
            this.RepositorioInvestimentos = new RepositorioInvestimentos();
            this.Data = DateOnly.FromDateTime(DateTime.Now);
            this.ContaLogada = null;
        }

        public void CriarConta(string nome, string endereco, string cpf, decimal rendaMensal, AgenciaEnum agencia, TipoContaEnum tipoConta){

            var id = new Guid().ToString();
            int numConta = this.RepositorioContas.Elementos.Count() + 1;

            switch (tipoConta)
            {
                case TipoContaEnum.Corrente:
                    var contaCorrente = new ContaCorrente(id, nome, cpf, endereco, rendaMensal, numConta, agencia);
                    this.RepositorioContas.AdicionarElemento(contaCorrente);
                    break;

                case TipoContaEnum.Poupanca:
                    var contaPoupanca = new ContaPoupanca(id, nome, cpf, endereco, rendaMensal, numConta, agencia, 0.01m);
                    this.RepositorioContas.AdicionarElemento(contaPoupanca);
                    break;

                case TipoContaEnum.Investimento:
                    var contaInvestimento = new ContaInvestimento(id, nome, cpf, endereco, rendaMensal, numConta, agencia);
                    this.RepositorioContas.AdicionarElemento(contaInvestimento);
                    break;
                default:
                    break;
            }
        }

        public void AcessarConta(){
            Console.WriteLine("Digite o número da conta que deseja acessar:");
            var numConta = int.Parse(Console.ReadLine());

            try
            {
                Conta contaLogada = this.RepositorioContas.Elementos.FirstOrDefault(c => c.NumeroConta == numConta);
                this.ContaLogada = contaLogada;
                Console.WriteLine("Conta acessada com sucesso.");
            }
            catch (System.Exception)
            {
                throw;
            }
        }

        public Transacao Saque(){

            Console.WriteLine("Digite quanto deseja sacar:");
            var valorSaque = Console.ReadLine();

            while(valorSaque is null || decimal.Parse(valorSaque) <= decimal.Zero){
                Console.WriteLine("Valor inválido, digite novamente:");
                valorSaque = Console.ReadLine();
            }

            try{
                Transacao saque = this.ContaLogada.Saque(decimal.Parse(valorSaque));
                Console.WriteLine("Saque realizado com sucesso!");
                return saque;
            }
            catch (System.Exception){
                throw;
            }
        }

        public Transacao Deposito(){
            Console.WriteLine("Digite o valor que deseja depositar:");
            var valorDeposito = Console.ReadLine();

            while(valorDeposito is null || decimal.Parse(valorDeposito) <= decimal.Zero){
                Console.WriteLine("Valor inválido, digite novamente:");
                valorDeposito = Console.ReadLine();
            }

            try
            {
                Transacao deposito = ContaLogada.Deposito(decimal.Parse(valorDeposito));
                return deposito;
            }
            catch (System.Exception)
            {
                throw;
            }
        }

        public void Extrato(){
            Console.WriteLine("Digite a data inicial: (AAAA/MM/DD)");
            var dataInicial = Console.ReadLine();
            while(dataInicial is null){
                Console.WriteLine("Data inválida, digite novamente:");
                dataInicial = Console.ReadLine();
            }

            Console.WriteLine("Digite a data final: (AAAA/MM/DD)");
            var dataFinal = Console.ReadLine();
            while(dataFinal is null){
                Console.WriteLine("Data inválida, digite novamente:");
                dataFinal = Console.ReadLine();
            }

            try
            {
                List<Transacao> extrato = this.ContaLogada.Extrato(DateOnly.Parse(dataInicial), DateOnly.Parse(dataFinal));
                foreach (var transacao in extrato)
                {
                    Console.WriteLine($"{transacao.Data} - {transacao.Descricao} - {transacao.Valor}");
                }
            }
            catch (System.Exception)
            {
                
                throw;
            }
        }

        public void Transferencia(){
            Console.WriteLine("Digite o número da conta de destino:");
            var numContaDestino = int.Parse(Console.ReadLine());
            var contaDestino = this.RepositorioContas.RetornaContaPeloNumero(numContaDestino);

            while( contaDestino is null || contaDestino.Id == this.ContaLogada.Id){
                Console.WriteLine("Você não pode transferir para esta conta porque ela não existe ou é a sua prórpria conta. Digite novamente:");
                numContaDestino = int.Parse(Console.ReadLine());
                contaDestino = this.RepositorioContas.RetornaContaPeloNumero(numContaDestino);
            }

            Console.WriteLine($"Digite o valor que deseja transferir para a conta {contaDestino.NumeroConta}:");
            var valorTransferencia = Console.ReadLine();

            while(valorTransferencia is null || decimal.Parse(valorTransferencia) <= 0){
                Console.WriteLine("Valor inválido, digite novamente:");
                valorTransferencia = Console.ReadLine();
            }

            Console.WriteLine($"Conta destino: {contaDestino.NumeroConta}\nValor: {valorTransferencia}");
            
            try
            {
                Transferencia transferenciaEnviada = this.ContaLogada.EnviaTransferencia(numContaDestino, decimal.Parse(valorTransferencia));
                this.RepositorioTransferencias.AdicionarElemento(transferenciaEnviada);
                Transferencia transferenciaRecebida = contaDestino.RecebeTransferencia(this.ContaLogada.NumeroConta, decimal.Parse(valorTransferencia));
            }
            catch (System.Exception)
            {
                throw;
            }

        }

        public void ListarContas(){
            foreach (var conta in this.RepositorioContas.Elementos)
            {
                Console.WriteLine($"Conta: {conta.NumeroConta} - Nome: {conta.Nome} - Tipo: {conta.TipoConta}");
            }
        }

        public void ListarContasComSaldoNegativo(){
            foreach (var conta in this.RepositorioContas.Elementos.Where(c => c.Saldo < 0))
            {
                Console.WriteLine($"Conta: {conta.NumeroConta} - Saldo: {conta.SaldoAtual()} - Tipo de Conta: {conta.TipoConta}");
            }
        }
        public void ListarTransferencias(){
            foreach (var transferencia in this.RepositorioTransferencias.Elementos)
            {
                Console.WriteLine($"Data: {transferencia.Data} - Conta Origem: {transferencia.NumContaOrigem} - Conta Destino: {transferencia.NumContaDestino} - Descrição: {transferencia.Descricao} - Valor: {transferencia.Valor}");
            }
        }

        public decimal RetornaTotalInvestido(){
            decimal totalInvestido = this.RepositorioTransferencias.Elementos.Sum(t => t.Valor);
            Console.WriteLine($"Total investido: {totalInvestido}");
            return totalInvestido;
        }

        public void ListarTransacoesCliente(){
            Console.WriteLine("Digite o número da conta que deseja ver as transações:");
            var numConta = int.Parse(Console.ReadLine());

            try
            {
                var conta = this.RepositorioContas.RetornaContaPeloNumero(numConta);
                Console.WriteLine("Lista de transações:");
                foreach (var transacao in conta.Transacoes)
                {
                    Console.WriteLine($"Data: {transacao.Data} - Descrição: {transacao.Descricao} - Valor: {transacao.Valor}");
                }
            }
            catch (System.Exception)
            {
                
                throw;
            }
        }
    }
}