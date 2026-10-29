using Conde.Noivados;
using Xunit;

namespace Conde.Noivados.Testes;

public class EstabilidadeTestes
{
    /// <summary>
    /// O exemplo clássico com três de cada lado em que existem exatamente três
    /// emparelhamentos estáveis: cada lado tem um favorito e há um do meio.
    /// </summary>
    private static Preferencias Tres() => new(
        [[0, 1, 2], [1, 2, 0], [2, 0, 1]],
        [[1, 2, 0], [2, 0, 1], [0, 1, 2]]);

    [Fact]
    public void OEmparelhamentoPrecisaSerBijecao()
    {
        Assert.Throws<ArgumentException>(() => new Emparelhamento([0, 0]));
        Assert.Throws<ArgumentException>(() => new Emparelhamento([0, 5]));
    }

    [Fact]
    public void AVoltaDoEmparelhamentoBateComAIda()
    {
        var e = new Emparelhamento([2, 0, 1]);
        for (var h = 0; h < e.N; h++)
        {
            Assert.Equal(h, e.HomemDe(e.MulherDo(h)));
        }
    }

    [Fact]
    public void SemParBloqueadorEEstavel()
    {
        var p = Tres();
        foreach (var e in Estabilidade.Todos(p))
        {
            Assert.Empty(Estabilidade.Bloqueadores(p, e));
            Assert.True(Estabilidade.Estavel(p, e));
        }
    }

    [Fact]
    public void ComParBloqueadorNaoEEstavel()
    {
        var p = Tres();
        var instaveis = Estabilidade.Permutacoes(3)
            .Select(x => new Emparelhamento(x))
            .Where(e => !Estabilidade.Estavel(p, e))
            .ToList();

        Assert.NotEmpty(instaveis);
        Assert.All(instaveis, e => Assert.NotEmpty(Estabilidade.Bloqueadores(p, e)));
    }

    [Fact]
    public void OExemploClassicoTemTresEstaveis()
    {
        Assert.Equal(3, Estabilidade.Todos(Tres()).Count);
    }

    [Fact]
    public void TodaRodadaTemPeloMenosUmEstavel()
    {
        foreach (var p in Rodadas.Todas(3))
        {
            Assert.NotEmpty(Estabilidade.Todos(p));
        }
    }

    [Fact]
    public void QuandoTodosQueremAMesmaPessoaSoExisteUmEstavel()
    {
        var p = new Preferencias(
            [[0, 1, 2], [0, 1, 2], [0, 1, 2]],
            [[0, 1, 2], [0, 1, 2], [0, 1, 2]]);

        var estaveis = Estabilidade.Todos(p);
        Assert.Single(estaveis);
        Assert.Equal("0 1 2", estaveis[0].ToString());
    }

    [Theory]
    [InlineData(1, 1)]
    [InlineData(2, 2)]
    [InlineData(3, 6)]
    [InlineData(4, 24)]
    [InlineData(5, 120)]
    public void AsPermutacoesSaoFatorialDeN(int n, int esperado)
    {
        var todas = Estabilidade.Permutacoes(n).ToList();
        Assert.Equal(esperado, todas.Count);
        Assert.Equal(esperado, todas.Select(x => string.Join(",", x)).Distinct().Count());
    }

    [Fact]
    public void OCustoDeUmLadoEASomaDosPostos()
    {
        var p = Tres();
        var e = new Emparelhamento([0, 1, 2]);
        Assert.Equal(0, e.CustoDosHomens(p));
        Assert.Equal(6, e.CustoDasMulheres(p));
    }

    [Fact]
    public void EmparelhamentosIguaisSaoIguais()
    {
        Assert.Equal(new Emparelhamento([1, 0]), new Emparelhamento([1, 0]));
        Assert.Equal(new Emparelhamento([1, 0]).GetHashCode(), new Emparelhamento([1, 0]).GetHashCode());
        Assert.NotEqual(new Emparelhamento([1, 0]), new Emparelhamento([0, 1]));
        Assert.False(new Emparelhamento([0]).Equals(null));
        Assert.False(new Emparelhamento([0]).Equals("texto"));
    }

    [Fact]
    public void NuloNaoEAceito()
    {
        Assert.Throws<ArgumentNullException>(() => new Emparelhamento(null!));
        Assert.Throws<ArgumentNullException>(() => Estabilidade.Todos(null!));
    }
}
