using Conde.Noivados;
using Xunit;

namespace Conde.Noivados.Testes;

/// <summary>
/// Os números do README presos aqui. Só entra o que eu vi o medidor imprimir.
/// </summary>
public class MedidasTestes
{
    [Fact]
    public void ATabelaDeQuantosEstaveisComDoisDeCadaLado()
    {
        var linhas = Medidas.Quantos(2);
        Assert.Equal(2, linhas.Count);
        Assert.Equal(1, linhas[0].Estaveis);
        Assert.Equal(14, linhas[0].Rodadas);
        Assert.Equal(87.5, linhas[0].Fracao);
        Assert.Equal(2, linhas[1].Estaveis);
        Assert.Equal(2, linhas[1].Rodadas);
        Assert.Equal(12.5, linhas[1].Fracao);
    }

    [Fact]
    public void ATabelaDeQuantosEstaveisComTresDeCadaLado()
    {
        var linhas = Medidas.Quantos(3);
        Assert.Equal(3, linhas.Count);
        Assert.Equal(34080, linhas[0].Rodadas);
        Assert.Equal(73.05, linhas[0].Fracao);
        Assert.Equal(11484, linhas[1].Rodadas);
        Assert.Equal(24.61, linhas[1].Fracao);
        Assert.Equal(1092, linhas[2].Rodadas);
        Assert.Equal(2.34, linhas[2].Fracao);
        Assert.Equal(46656, linhas.Sum(l => l.Rodadas));
    }

    [Fact]
    public void NuncaHaMaisDeTresEstaveisComTresDeCadaLado()
    {
        Assert.Equal(3, Medidas.Quantos(3).Max(l => l.Estaveis));
    }

    [Fact]
    public void QuemPedeLevaOMelhorEmTodasAsRodadas()
    {
        var c = Medidas.Confronto(3);
        Assert.Equal(46656, c.Rodadas);
        Assert.Equal(46656, c.HomensOtimoParaEles);
        Assert.Equal(46656, c.HomensPessimoParaElas);
        Assert.Equal(46656, c.GaleShapleyAcertou);
    }

    [Fact]
    public void ComDoisDeCadaLadoTambemNaoHaExcecao()
    {
        var c = Medidas.Confronto(2);
        Assert.Equal(16, c.Rodadas);
        Assert.Equal(16, c.HomensOtimoParaEles);
        Assert.Equal(16, c.HomensPessimoParaElas);
        Assert.Equal(14, c.UmSoEstavel);
        Assert.Equal(14, c.OsDoisLadosIguais);
    }

    [Fact]
    public void PedirOuReceberSoMudaONdeHaMaisDeUmEstavel()
    {
        // Os dois numeros sao o mesmo, e nao por pouco: sao a mesma contagem.
        var c = Medidas.Confronto(3);
        Assert.Equal(34080, c.UmSoEstavel);
        Assert.Equal(34080, c.OsDoisLadosIguais);
        Assert.Equal(c.UmSoEstavel, c.OsDoisLadosIguais);
    }

    [Theory]
    [InlineData(3, 10368)]
    [InlineData(4, 15552)]
    [InlineData(5, 14256)]
    [InlineData(6, 5832)]
    [InlineData(7, 648)]
    public void ATabelaDosPedidos(int pedidos, long quantas)
    {
        var linha = Medidas.Pedidos(3).Single(l => l.Pedidos == pedidos);
        Assert.Equal(quantas, linha.Quantas);
    }

    [Fact]
    public void OLimiteDePedidosEAtingidoEmPoucasRodadas()
    {
        var linhas = Medidas.Pedidos(3);
        Assert.Equal(GaleShapley.LimiteDePedidos(3), linhas.Max(l => l.Pedidos));
        Assert.Equal(648, linhas.Single(l => l.Pedidos == 7).Quantas);
        Assert.Equal(46656, linhas.Sum(l => l.Quantas));
    }

    [Fact]
    public void ATabelaDosPedidosComDoisDeCadaLado()
    {
        var linhas = Medidas.Pedidos(2);
        Assert.Equal(2, linhas.Count);
        Assert.Equal(8, linhas[0].Quantas);
        Assert.Equal(8, linhas[1].Quantas);
        Assert.Equal(3, linhas[1].Pedidos);
    }

    [Fact]
    public void OReticuladoFechaSemUmaFalha()
    {
        var r = Medidas.Reticulado(3);
        Assert.Equal(46656, r.Rodadas);
        Assert.Equal(89844, r.ComJuncao);
        Assert.Equal(89844, r.ComEncontro);
        Assert.Equal(0, r.Falhas);
    }

    [Fact]
    public void OTotalDeParesDoReticuladoEASomaDosQuadrados()
    {
        var quantos = Medidas.Quantos(3);
        var esperado = quantos.Sum(l => l.Rodadas * l.Estaveis * l.Estaveis);
        Assert.Equal(89844, esperado);
        Assert.Equal(esperado, Medidas.Reticulado(3).ComJuncao);
    }

    [Fact]
    public void OReticuladoDeDoisTambemFecha()
    {
        var r = Medidas.Reticulado(2);
        Assert.Equal(22, r.ComJuncao);
        Assert.Equal(22, r.ComEncontro);
        Assert.Equal(0, r.Falhas);
    }
}
