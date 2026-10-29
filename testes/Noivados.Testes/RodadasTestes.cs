using Conde.Noivados;
using Xunit;

namespace Conde.Noivados.Testes;

public class RodadasTestes
{
    [Theory]
    [InlineData(1, 1)]
    [InlineData(2, 16)]
    [InlineData(3, 46656)]
    public void AQuantidadeDeRodadasEFatorialElevadoADoisN(int n, long esperado)
    {
        Assert.Equal(esperado, Rodadas.Quantidade(n));
        Assert.Equal(esperado, Rodadas.Todas(n).Count());
    }

    [Fact]
    public void OTamanhoQuatroNaoCabeEEstaAnotado()
    {
        // 24^8 = 110 bilhoes de rodadas. A conta existe para dizer por que o
        // repositorio para no tres, em vez de o leitor ter que supor.
        Assert.Equal(110075314176L, Rodadas.Quantidade(4));
    }

    [Fact]
    public void NenhumaRodadaSeRepete()
    {
        var vistas = new HashSet<string>();
        foreach (var p in Rodadas.Todas(2))
        {
            var chave = string.Join("|", p.ListaDosHomens.Select(l => string.Join(",", l)))
                + "//" + string.Join("|", p.ListaDasMulheres.Select(l => string.Join(",", l)));
            Assert.True(vistas.Add(chave));
        }

        Assert.Equal(16, vistas.Count);
    }

    [Fact]
    public void TodaRodadaTemListasValidas()
    {
        foreach (var p in Rodadas.Todas(3))
        {
            Assert.Equal(3, p.N);
            for (var i = 0; i < 3; i++)
            {
                Assert.Equal([0, 1, 2], p.ListaDosHomens[i].Order().ToArray());
                Assert.Equal([0, 1, 2], p.ListaDasMulheres[i].Order().ToArray());
            }
        }
    }

    [Fact]
    public void TamanhoNegativoNaoEAceito()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => Rodadas.Quantidade(-1));
        Assert.Throws<ArgumentOutOfRangeException>(() => Rodadas.Todas(-1).ToList());
    }
}
