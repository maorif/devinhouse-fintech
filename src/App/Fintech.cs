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

        public void CriarConta(){
            Console.WriteLine("Criando conta...");
            Console.WriteLine("Digite o nome: ");
            var nome = Console.ReadLine();
            nome = GetInput(nome);

            Console.WriteLine("Digite o endereço: ");
            var endereco = Console.ReadLine();
            endereco = GetInput(endereco);

            Console.WriteLine("Digite o cpf: ");
            var cpf = Console.ReadLine();
            cpf = GetInput(cpf);

            Console.WriteLine("Digite a renda mensal: ");
            var rendaMensalString = Console.ReadLine();
            decimal rendaMensal = decimal.Parse(GetInput(rendaMensalString));

            Console.WriteLine("Digite a opção de agencia (1-Florianópolis, 2-São José, 3-Biguaçu): ");
            string opcaoAgencia = Console.ReadLine().ToString();
            while(opcaoAgencia == "" || !(opcaoAgencia == "1" || opcaoAgencia == "2" || opcaoAgencia == "3")){
                Console.WriteLine("Opção inválida, digite novamente: ");
                opcaoAgencia = Console.ReadLine();
            }
            AgenciaEnum agencia = AgenciaEnum.Florianópolis;

            if(opcaoAgencia == "1"){
                agencia = AgenciaEnum.Florianópolis;
            }
            else if(opcaoAgencia == "2"){
                agencia = AgenciaEnum.SãoJosé;
            }
            else if(opcaoAgencia == "3"){
                agencia = AgenciaEnum.Biguaçu;
            }
            
            Console.WriteLine("Digite a opção de conta (1-Corrente, 2-Poupança, 3-Investimento): ");
            var opcaoConta = Console.ReadLine();
            while(opcaoConta == "" || !(opcaoConta == "1" || opcaoConta == "2" || opcaoConta == "3")){
                Console.WriteLine("Opção inválida, digite novamente: ");
                opcaoConta = Console.ReadLine();
            }

            var id = new Guid().ToString();
            int numConta = this.RepositorioContas.Elementos.Count() + 1;
            Console.WriteLine("-----" + opcaoConta+ "-----");

            if(opcaoConta == "1"){
                var contaCorrente = new ContaCorrente(id, nome, cpf, endereco, rendaMensal, numConta, agencia);
                this.RepositorioContas.AdicionarElemento(contaCorrente);
                Console.WriteLine("Conta criada com sucesso!");
                this.MenuInicial();
            }
            else if(opcaoConta == "2"){
                var contaPoupanca = new ContaPoupanca(id, nome, cpf, endereco, rendaMensal, numConta, agencia, 0.01m);
                this.RepositorioContas.AdicionarElemento(contaPoupanca);
                Console.WriteLine("Conta criada com sucesso!");
                this.MenuInicial();
            }
            else if(opcaoConta == "3"){
                var contaInvestimento = new ContaInvestimento(id, nome, cpf, endereco, rendaMensal, numConta, agencia);
                this.RepositorioContas.AdicionarElemento(contaInvestimento);
                Console.WriteLine("Conta criada com sucesso!");
                this.MenuInicial();
            }

            

            // switch (tipoConta)
            // {
            //     case TipoContaEnum.Corrente:
            //         var contaCorrente = new ContaCorrente(id, nome, cpf, endereco, rendaMensal, numConta, agencia);
            //         this.RepositorioContas.AdicionarElemento(contaCorrente);
            //         Console.WriteLine("Conta criada com sucesso!");
            //         this.MenuInicial();
            //         break;

            //     case TipoContaEnum.Poupanca:
            //         var contaPoupanca = new ContaPoupanca(id, nome, cpf, endereco, rendaMensal, numConta, agencia, 0.01m);
            //         this.RepositorioContas.AdicionarElemento(contaPoupanca);
            //         Console.WriteLine("Conta criada com sucesso!");
            //         this.MenuInicial();
            //         break;

            //     case TipoContaEnum.Investimento:
            //         var contaInvestimento = new ContaInvestimento(id, nome, cpf, endereco, rendaMensal, numConta, agencia);
            //         this.RepositorioContas.AdicionarElemento(contaInvestimento);
            //         Console.WriteLine("Conta criada com sucesso!");
            //         this.MenuInicial();
            //         break;

            //     default:
            //         break;
            // }

            string GetInput(string input){

                while(input is null || input == ""){
                    Console.WriteLine("Digite um valor válido.");
                    input = Console.ReadLine();
                }

                return input;
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
                this.Voltar();
                
            }
            catch (System.Exception)
            {
                throw;
            }
        }

        public void Saldo(){
            Console.WriteLine("Saldo: R$" + this.ContaLogada.SaldoAtual());
            this.Voltar();
        }

        public void Saque(){

            Console.WriteLine("Digite quanto deseja sacar:");
            var valorSaque = Console.ReadLine();

            while(valorSaque is null || valorSaque == "" || decimal.Parse(valorSaque) <= decimal.Zero){
                Console.WriteLine("Valor inválido, digite novamente:");
                valorSaque = Console.ReadLine();
            }

            try{
                Transacao saque = this.ContaLogada.Saque(decimal.Parse(valorSaque));
                Console.WriteLine($"Saque no valor de {valorSaque} realizado com sucesso!");
                this.Voltar();
                
            }
            catch (System.Exception){
                throw;
            }
        }

        public void Deposito(){
            Console.WriteLine("Digite o valor que deseja depositar:");
            var valorDeposito = Console.ReadLine();

            while(valorDeposito is null || valorDeposito == "" || decimal.Parse(valorDeposito) <= decimal.Zero){
                Console.WriteLine("Valor inválido, digite novamente:");
                valorDeposito = Console.ReadLine();
            }

            try
            {
                Transacao deposito = ContaLogada.Deposito(decimal.Parse(valorDeposito));
                Console.WriteLine($"Depósito no valor de {valorDeposito} realizado com sucesso!");
                this.Voltar();
            }
            catch (System.Exception)
            {
                throw;
            }
        }

        public void Extrato(){
            Console.WriteLine("Digite a data inicial: (AAAA/MM/DD)");
            var dataInicial = Console.ReadLine();
            while(dataInicial is null || dataInicial == ""){
                Console.WriteLine("Data inválida, digite novamente:");
                dataInicial = Console.ReadLine();
            }

            Console.WriteLine("Digite a data final: (AAAA/MM/DD)");
            var dataFinal = Console.ReadLine();
            while(dataFinal is null || dataFinal == ""){
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
                this.Voltar();
            }
            catch (System.Exception)
            {
                
                throw;
            }
        }

        public void ListarTransacoes(){
            foreach (var transacao in this.ContaLogada.Transacoes)
            {
                Console.WriteLine($"Data: {transacao.Data} - Valor: {transacao.Valor} - Tipo: {transacao.TipoTransacao}");
            }
            this.Voltar();
        }

        public void Transferencia(){
            Console.WriteLine("Digite o número da conta de destino:");
            var numContaDestino = int.Parse(Console.ReadLine());
            var contaDestino = this.RepositorioContas.RetornaContaPeloNumero(numContaDestino);

            while( contaDestino is null || contaDestino.NumeroConta == this.ContaLogada.NumeroConta){
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
                this.Voltar();
            }
            catch (System.Exception)
            {
                throw;
            }

        }

        public void RetornaInvestimento(){
            Console.WriteLine("Digite o tipo de investimento:1- LCI: 8% ao ano, 2- LCA: 9% ao ano, 3- CDB: 10% ao ano");
            var tipoInvestimento = Console.ReadLine();
            while(!(tipoInvestimento == "1" || tipoInvestimento == "2" || tipoInvestimento == "3")){
                Console.WriteLine("Digite um valor válido:");
                tipoInvestimento = Console.ReadLine();
            }
            TipoInvestimentoEnum investimento = TipoInvestimentoEnum.LCI;
            if(tipoInvestimento == "1") {investimento = TipoInvestimentoEnum.LCI;}
            else if(tipoInvestimento == "2") {investimento = TipoInvestimentoEnum.LCA;}
            else if(tipoInvestimento == "3") {investimento = TipoInvestimentoEnum.CDB;}

            Console.WriteLine("Digite o valor do investimento:");
            var valorInvestimento = Console.ReadLine();
            while(valorInvestimento is null || decimal.Parse(valorInvestimento) <= 0){
                Console.WriteLine("Valor inválido, digite novamente:");
                valorInvestimento = Console.ReadLine();
            }
            Console.WriteLine("Digite quantos meses deseja investir: (1-100)");
            var mesesInvestimento = Console.ReadLine();
            while(mesesInvestimento is null || int.Parse(mesesInvestimento) <= 0 || int.Parse(mesesInvestimento) > 100){
                Console.WriteLine("Valor inválido, digite novamente:");
                mesesInvestimento = Console.ReadLine();
            }

            try
            {
                Investimento novoInvestimento = this.ContaLogada.Investimento(decimal.Parse(valorInvestimento), investimento, int.Parse(mesesInvestimento));
                Console.WriteLine($"Investimento realizado com sucesso! Valor: {valorInvestimento} Duração: {mesesInvestimento} Meses");
                this.RepositorioInvestimentos.AdicionarElemento(novoInvestimento);
                this.MenuContaInvestimento();
            }
            catch (System.Exception)
            {
                
                throw;
            }
        }

        public void RetornaSimulacaoInvestimento(){
            Console.WriteLine("Vamos simular um investimento!");
            Console.WriteLine("Digite o tipo de investimento:1- LCI: 8% ao ano, 2- LCA: 9% ao ano, 3- CDB: 10% ao ano");
            var tipoInvestimento = Console.ReadLine();
            while(!(tipoInvestimento == "1" || tipoInvestimento == "2" || tipoInvestimento == "3")){
                Console.WriteLine("Digite um valor válido:");
                tipoInvestimento = Console.ReadLine();
            }
            TipoInvestimentoEnum investimento = TipoInvestimentoEnum.LCI;
            if(tipoInvestimento == "1") {investimento = TipoInvestimentoEnum.LCI;}
            else if(tipoInvestimento == "2") {investimento = TipoInvestimentoEnum.LCA;}
            else if(tipoInvestimento == "3") {investimento = TipoInvestimentoEnum.CDB;}

            Console.WriteLine("Digite o valor do investimento:");
            var valorInvestimento = Console.ReadLine();
            while(valorInvestimento is null || decimal.Parse(valorInvestimento) <= 0){
                Console.WriteLine("Valor inválido, digite novamente:");
                valorInvestimento = Console.ReadLine();
            }
            Console.WriteLine("Digite quantos meses deseja investir: (1-100)");
            var mesesInvestimento = Console.ReadLine();
            while(mesesInvestimento is null || int.Parse(mesesInvestimento) <= 0 || int.Parse(mesesInvestimento) > 100){
                Console.WriteLine("Valor inválido, digite novamente:");
                mesesInvestimento = Console.ReadLine();
            }

            try
            {
                var rendimento = this.ContaLogada.SimulacaoInvestimento(decimal.Parse(valorInvestimento), investimento, int.Parse(mesesInvestimento));
                Console.WriteLine($"Simulação realizada com sucesso! Valor inicial: {valorInvestimento} Valor final: {rendimento}");
                Console.WriteLine("Deseja realizar este investimento? (S/N)");
                var resposta = Console.ReadLine();
                while(!(resposta == "S" || resposta == "N")){
                    Console.WriteLine("Digite um valor válido:");
                    resposta = Console.ReadLine();
                }
                if(resposta == "N"){
                    this.MenuContaInvestimento();
                }
                else{
                    Investimento novoInvestimento = this.ContaLogada.Investimento(decimal.Parse(valorInvestimento), investimento, int.Parse(mesesInvestimento));
                    Console.WriteLine($"Investimento realizado com sucesso! Valor: {valorInvestimento} Duração: {mesesInvestimento} Meses");
                    this.RepositorioInvestimentos.AdicionarElemento(novoInvestimento);
                    this.MenuContaInvestimento();
                }

            }
            catch (System.Exception)
            {
                
                throw;
            }

        }

        public void SimularRendimento(){
            Console.WriteLine("Digite o número de meses que deseja simular:");
            var mesesSimulacao = Console.ReadLine();
            while(mesesSimulacao is null || int.Parse(mesesSimulacao) <= 0){
                Console.WriteLine("Valor inválido, digite novamente:");
                mesesSimulacao = Console.ReadLine();
            }

            Console.WriteLine("Digite a rentabilidade da poupança atual:");
            var rentabilidade = Console.ReadLine();
            while(rentabilidade is null || decimal.Parse(rentabilidade) <= 0){
                Console.WriteLine("Valor inválido, digite novamente:");
                rentabilidade = Console.ReadLine();
            }

            this.ContaLogada.SimularRendimento(int.Parse(mesesSimulacao), decimal.Parse(rentabilidade));
            this.MenuContaCorrente();
        }

        public void MudarDadosCadastrados(){
            Console.WriteLine("Digite um novo nome:");
            var nome = Console.ReadLine();
            if(nome != null){
                nome = Console.ReadLine();
            }

            Console.WriteLine("Digite um novo endereço:");
            var endereco = Console.ReadLine();
            if(endereco != null){
                endereco = Console.ReadLine();
            }

            Console.WriteLine("Digite uma nova renda mensal:");
            var rendaMensal = Console.ReadLine();
            if(rendaMensal != null || decimal.Parse(rendaMensal) <= decimal.Zero){
                rendaMensal = Console.ReadLine();
            }

            this.ContaLogada.MudarDadosCadastrais(nome, endereco, decimal.Parse(rendaMensal));
            this.Voltar();
        }

        public void ListarContas(){
            foreach (var conta in this.RepositorioContas.Elementos)
            {
                Console.WriteLine($"Conta: {conta.NumeroConta} - Nome: {conta.Nome} - Tipo: {conta.TipoConta}");
            }
            this.BancoDeDados();
        }

        public void ListarContasCorrente(){
            foreach (var conta in this.RepositorioContas.Elementos.Where(c => c.TipoConta == TipoContaEnum.Corrente))
            {
                Console.WriteLine($"Conta: {conta.NumeroConta} - Nome: {conta.Nome} - Tipo: {conta.TipoConta}");
            }
            this.BancoDeDados();
        }

        public void ListarContasInvestimento(){
            foreach (var conta in this.RepositorioContas.Elementos.Where(c => c.TipoConta == TipoContaEnum.Investimento))
            {
                Console.WriteLine($"Conta: {conta.NumeroConta} - Nome: {conta.Nome} - Tipo: {conta.TipoConta}");
            }
            this.BancoDeDados();
        }

        public void ListarContasPoupanca(){
            foreach (var conta in this.RepositorioContas.Elementos.Where(c => c.TipoConta == TipoContaEnum.Poupanca))
            {
                Console.WriteLine($"Conta: {conta.NumeroConta} - Nome: {conta.Nome} - Tipo: {conta.TipoConta}");
            }
            this.BancoDeDados();
        }

        public void ListarContasComSaldoNegativo(){
            foreach (var conta in this.RepositorioContas.Elementos.Where(c => c.Saldo < 0))
            {
                Console.WriteLine($"Conta: {conta.NumeroConta} - Saldo: {conta.SaldoAtual()} - Tipo de Conta: {conta.TipoConta}");
            }
            this.BancoDeDados();
        }
        public void ListarTransferencias(){
            foreach (var transferencia in this.RepositorioTransferencias.Elementos)
            {
                Console.WriteLine($"Data: {transferencia.Data} - Conta Origem: {transferencia.NumContaOrigem} - Conta Destino: {transferencia.NumContaDestino} - Descrição: {transferencia.Descricao} - Valor: {transferencia.Valor}");
            }
            this.BancoDeDados();
        }

        public void RetornaTotalInvestido(){
            decimal totalInvestido = this.RepositorioInvestimentos.Elementos.Sum(t => t.ValorAtual);
            Console.WriteLine($"Total investido: {totalInvestido}");
            this.BancoDeDados();
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
                this.BancoDeDados();
            }
            catch (System.Exception)
            {
                
                throw;
            }
        }
        
        public void BancoDeDados(){
            Console.WriteLine("Digite a opção desejada:");
            Console.WriteLine("1- Lista de transferências");
            Console.WriteLine("2- Lista de todas as contas");
            Console.WriteLine("3- Lista de contas corrente");
            Console.WriteLine("4- Lista de contas poupança");
            Console.WriteLine("5- Lista de contas investimento");
            Console.WriteLine("6- Lista de contas negativadas");
            Console.WriteLine("7- Total investido");
            Console.WriteLine("8- Trasações de uma conta");
            Console.WriteLine("0- Voltar");

            var opcao = Console.ReadLine();

            switch (opcao)
            {
                case "1":
                    this.ListarTransferencias();
                    break;
                case "2":
                    this.ListarContas();
                    break;
                case "3":
                    this.ListarContasCorrente();
                    break;
                case "4":
                    this.ListarContasPoupanca();
                    break;
                case "5":
                    this.ListarContasInvestimento();
                    break;
                case "6":
                    this.ListarContasComSaldoNegativo();
                    break;
                case "7":
                    this.RetornaTotalInvestido();
                    break;
                case "8":
                    this.ListarTransacoesCliente();
                    break;
                case "0":
                    this.MenuInicial();
                    break;
                default:
                    break;
            }
        }

        public void MenuInicial(){
            Console.WriteLine("Bem vida/o a Dev-in-House Fintech!");
            Console.WriteLine("Digite o onde pretende ir:\n1- Acessar Conta\n2- Criar Conta\n3- Visualisar banco de dados\n0- Sair");
            var opcao = Console.ReadLine();
            switch (opcao)
            {
                case "1":
                    this.AcessarConta();
                    break;
                case "2":
                    this.CriarConta();
                    break;
                case "3":
                    this.BancoDeDados();
                    break;
                case "0":
                    Console.WriteLine("Volte sempre!");
                    Environment.Exit(0);
                    break;
                default:
                    Console.WriteLine("Opção inválida!\n");
                    this.MenuInicial();
                    break;

            }
        }

        public void MenuContaCorrente(){
            Console.WriteLine("Digite a opção desejada:");
            Console.WriteLine("1- Depositar\n2- Sacar\n3- Transferir\n4- Visualizar saldo\n5- Visualizar extrato\n6- Visualizar transações\n0- Sair");
            var opcao = Console.ReadLine();

            switch (opcao)
            {
                case "1":
                    this.Deposito();
                    break;
                case "2":
                    this.Saque();
                    break;
                case "3":
                    this.Transferencia();
                    break;
                case "4":
                    this.Saldo();
                    break;
                case "5":
                    this.Extrato();
                    break;
                case "6":
                    this.ListarTransacoes();
                    break;
                case "0":
                    this.MenuInicial();
                    break;
                default:
                    Console.WriteLine("Opção inválida!\n");
                    this.MenuContaCorrente();
                    break;
            }
        }

         public void MenuContaPoupanca(){
            Console.WriteLine("Digite a opção desejada:");
            Console.WriteLine("1- Depositar\n2- Sacar\n3- Transferir\n4- Visualizar saldo\n5- Visualizar extrato\n6- Visualizar transações\n7- Simular Rendimento\n0- Sair");
            var opcao = Console.ReadLine();

            switch (opcao)
            {
                case "1":
                    this.Deposito();
                    break;
                case "2":
                    this.Saque();
                    break;
                case "3":
                    this.Transferencia();
                    break;
                case "4":
                    this.Saldo();
                    break;
                case "5":
                    this.Extrato();
                    break;
                case "6":
                    this.ListarTransacoes();
                    break;
                case "7":
                    this.SimularRendimento();
                    break;
                case "0":
                    this.MenuInicial();
                    break;
                default:
                    Console.WriteLine("Opção inválida!\n");
                    this.MenuContaPoupanca();
                    break;
            }
        }

        public void MenuContaInvestimento(){
            Console.WriteLine("Digite a opção desejada:");
            Console.WriteLine("1- Depositar\n2- Sacar\n3- Transferir\n4- Visualizar saldo\n5- Visualizar extrato\n6- Visualizar transações\n7- Investir\n8- Simular rendimento\n0- Sair");
            var opcao = Console.ReadLine();

            switch (opcao)
            {
                case "1":
                    this.Deposito();
                    break;
                case "2":
                    this.Saque();
                    break;
                case "3":
                    this.Transferencia();
                    break;
                case "4":
                    this.Saldo();
                    break;
                case "5":
                    this.Extrato();
                    break;
                case "6":
                    this.ListarTransacoes();
                    break;
                case "7":
                    this.RetornaInvestimento();
                    break;
                case "8":
                    this.RetornaSimulacaoInvestimento();
                    break;
                case "0":
                    this.MenuInicial();
                    break;
                default:
                    Console.WriteLine("Opção inválida");
                    this.MenuContaInvestimento();
                    break;
            }
        }

        public void Voltar(){
            if(this.ContaLogada.TipoConta == TipoContaEnum.Corrente){this.MenuContaCorrente();}
            else if(this.ContaLogada.TipoConta == TipoContaEnum.Poupanca){this.MenuContaPoupanca();}
            else if(this.ContaLogada.TipoConta == TipoContaEnum.Investimento){this.MenuContaInvestimento();}
        }
    }
}