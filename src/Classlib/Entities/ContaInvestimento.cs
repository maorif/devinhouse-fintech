using Classlib.Enums;
using System.Collections.Generic;

namespace Classlib.Entities
{
    public class ContaInvestimento : Conta
    {
        public override TipoContaEnum TipoConta { get; protected set; } = TipoContaEnum.Investimento;

        public ContaInvestimento(string Id, string Nome, string Cpf, object Endereco, decimal RendaMensal, int NumeroConta, AgenciaEnum Agencia) : base(Id, Nome, Cpf, Endereco, RendaMensal, NumeroConta, Agencia) { }

        public override Investimento Investimento(decimal valor, TipoInvestimentoEnum tipoInvestimento, int meses) {

            var investimentoOk = ChecaPossivelInvestimento(valor, meses, tipoInvestimento);
            if(!investimentoOk) {
                throw new ArgumentException("Investimento não pode ser realizado.");
            }
            
            this.Saldo -= valor;
            var investimento = new Investimento(new Guid().ToString(), $"Investimento {tipoInvestimento}", this.NumeroConta, valor, meses, DateOnly.FromDateTime(DateTime.Now), TipoTransacaoEnum.despesa, tipoInvestimento);
            this.Transacoes.Add(investimento);
            
            return investimento;
        }

        public override decimal SimulacaoInvestimento(decimal valor, TipoInvestimentoEnum tipoInvestimento, int meses) {

            var investimentoOk = ChecaPossivelInvestimento(valor, meses, tipoInvestimento);
            if(!investimentoOk) {
                throw new ArgumentException("Investimento não pode ser realizado. Saldo ou número de meses insuficiente.");
            }

            var rendimento = valor;

            for(int i=0; i<meses; i++){
                rendimento += rendimento * (((decimal)tipoInvestimento) / 100);
            }

            return rendimento;
        }

        protected override bool ChecaPossivelInvestimento(decimal valor, int meses, TipoInvestimentoEnum tipoInvestimento) {            
            if(valor > this.Saldo) {
                return false;
            }
            if(tipoInvestimento == TipoInvestimentoEnum.LCI && meses < 6) {
                return false;
            }

            else if(tipoInvestimento == TipoInvestimentoEnum.LCA && meses < 12) {
                return false;
            }

            else if(tipoInvestimento == TipoInvestimentoEnum.CDB && meses < 24) {
                return false;
            }

            return true;
        }
        
    }
}