using Classlib.Enums;
using Classlib.Entities;
using Classlib.Repository;

var fintech = new App.Fintech.Fintech();

// Contas para teste
fintech.RepositorioContas.AdicionarElemento(new ContaInvestimento(new Guid().ToString(), "Maori", "888.888.888-88", "Rua da SC401", 1000, 1, AgenciaEnum.Florianópolis));
fintech.RepositorioContas.AdicionarElemento(new ContaCorrente(new Guid().ToString(), "Laura", "777.777.777-77", "Rua do forte", 2000, 2, AgenciaEnum.Florianópolis));
fintech.RepositorioContas.AdicionarElemento(new ContaPoupanca(new Guid().ToString(), "Cris", "999.999.999-99", "Rua principal", 1500, 3, AgenciaEnum.SãoJosé, 0.01m));
fintech.RepositorioContas.AdicionarElemento(new ContaInvestimento(new Guid().ToString(), "Fulano", "111.111.111-11", "Rua segunda", 1200, 4, AgenciaEnum.Biguaçu));

fintech.MenuInicial();