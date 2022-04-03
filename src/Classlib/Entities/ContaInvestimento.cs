using Classlib.Enums;
using System.Collections.Generic;

namespace Classlib.Entities
{
    public class ContaInvestimento : Conta
    {
        public override TipoContaEnum TipoConta { get; protected set; } = TipoContaEnum.Investimento;

        public ContaInvestimento(string Id, string Nome, string Cpf, object Endereco, decimal RendaMensal, int NumeroConta, AgenciaEnum Agencia) : base(Id, Nome, Cpf, Endereco, RendaMensal, NumeroConta, Agencia) { }

        public Investimento Investimento(decimal valor, TipoInvestimentoEnum tipoInvestimento, int meses) {

            var investimentoOk = ChecaPossivelInvestimento(meses, tipoInvestimento);
            if(!investimentoOk) {
                throw new ArgumentException("Investimento não pode ser realizado.");
            }
            
            var investimento = new Investimento(new Guid().ToString(), $"Investimento {tipoInvestimento}", this.NumeroConta, valor, meses, DateOnly.FromDateTime(DateTime.Now), TipoTransacaoEnum.despesa, tipoInvestimento);
            this.Transacoes.Add(investimento);
            
            return investimento;
        }

        public decimal SimulacaoInvestimento(decimal valor, TipoInvestimentoEnum tipoInvestimento, int meses) {

            var investimentoOk = ChecaPossivelInvestimento(meses, tipoInvestimento);
            if(!investimentoOk) {
                throw new ArgumentException("Investimento não pode ser realizado.");
            }

            var rendimento = valor;

            for(int i=0; i<meses; i++){
                rendimento += rendimento * (((decimal)tipoInvestimento) / 100);
            }

            return rendimento;
        }

        private bool ChecaPossivelInvestimento(int meses, TipoInvestimentoEnum tipoInvestimento) {            
            if(tipoInvestimento == TipoInvestimentoEnum.LCI && meses < 6) {
                return false;
            }

            if(tipoInvestimento == TipoInvestimentoEnum.LCA && meses < 12) {
                return false;
            }

            if(tipoInvestimento == TipoInvestimentoEnum.CDB && meses < 24) {
                return false;
            }

            return true;
        }
        
    }
}