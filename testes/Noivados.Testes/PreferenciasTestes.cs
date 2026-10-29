using Conde.Noivados;
using Xunit;

namespace Conde.Noivados.Testes;

public class PreferenciasTestes
{
    private static Preferencias Exemplo() => new(
        [[0, 1, 2], [1, 0, 2], [2, 1, 0]],
        [[2, 1, 0], [0, 2, 1], [1, 0, 2]]);

    [Fact]
    public void OPostoZeroEOFavorito()
    {
        var p = Exemplo();
        Assert.Equal(0, p.PostoDoHomem(0, 0));
        Assert.Equal(0, p.PostoDoHomem(1, 1));
        Assert.Equal(0, p.PostoDaMulher(0, 2));
    }

    [Fact]
    public void OPostoEOInversoDaLista()
    {
        var p = Exemplo();
        for (var h = 0; h < p.N; h++)
        {
            for (var posicao = 0; posicao < p.N; posicao++)
            {
                Assert.Equal(posicao, p.PostoDoHomem(h, p.ListaDosHomens[h][posicao]));
            }
        }
    }

    [Fact]
    public void PreferirEComparacaoDePosto()
    {
        var p = Exemplo();
        Assert.True(p.HomemPrefere(0, 0, 1));
        Assert.False(p.HomemPrefere(0, 1, 0));
        Assert.True(p.MulherPrefere(0, 2, 1));
        Assert.False(p.MulherPrefere(0, 0, 2));
    }

    [Fact]
    public void NinguemPrefereAlguemASiMesmoNaPosicao()
    {
        var p = Exemplo();
        for (var h = 0; h < p.N; h++)
        {
            for (var m = 0; m < p.N; m++)
            {
                Assert.False(p.HomemPrefere(h, m, m));
            }
        }
    }

    [Fact]
    public void ListaIncompletaNaoEAceita()
    {
        Assert.Throws<ArgumentException>(() => new Preferencias(
            [[0, 1], [1, 0]],
            [[0], [1, 0]]));
    }

    [Fact]
    public void ListaComRepetidoNaoEAceita()
    {
        Assert.Throws<ArgumentException>(() => new Preferencias(
            [[0, 0], [1, 0]],
            [[0, 1], [1, 0]]));
    }

    [Fact]
    public void ListaComNomeForaDaFaixaNaoEAceita()
    {
        Assert.Throws<ArgumentException>(() => new Preferencias(
            [[0, 5], [1, 0]],
            [[0, 1], [1, 0]]));
    }

    [Fact]
    public void LadosDeTamanhosDiferentesNaoSaoAceitos()
    {
        Assert.Throws<ArgumentException>(() => new Preferencias(
            [[0, 1], [1, 0]],
            [[0]]));
    }

    [Fact]
    public void NuloNaoEAceito()
    {
        Assert.Throws<ArgumentNullException>(() => new Preferencias(null!, [[0]]));
        Assert.Throws<ArgumentNullException>(() => new Preferencias([[0]], null!));
    }
}
