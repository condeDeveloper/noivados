using Conde.Noivados;
using Xunit;

namespace Conde.Noivados.Testes;

public class GaleShapleyTestes
{
    [Fact]
    public void OResultadoESempreEstavel()
    {
        foreach (var p in Rodadas.Todas(3))
        {
            Assert.True(Estabilidade.Estavel(p, GaleShapley.HomensPedem(p).Emparelhamento));
            Assert.True(Estabilidade.Estavel(p, GaleShapley.MulheresPedem(p).Emparelhamento));
        }
    }

    [Fact]
    public void OResultadoEstaEntreOsQueAForcaBrutaAchou()
    {
        foreach (var p in Rodadas.Todas(3))
        {
            var estaveis = Estabilidade.Todos(p);
            Assert.Contains(GaleShapley.HomensPedem(p).Emparelhamento, estaveis);
            Assert.Contains(GaleShapley.MulheresPedem(p).Emparelhamento, estaveis);
        }
    }

    [Fact]
    public void QuemPedeNuncaFicaPiorDoQueQuemRecebe()
    {
        foreach (var p in Rodadas.Todas(3))
        {
            var deles = GaleShapley.HomensPedem(p).Emparelhamento;
            var delas = GaleShapley.MulheresPedem(p).Emparelhamento;
            Assert.True(deles.CustoDosHomens(p) <= delas.CustoDosHomens(p));
            Assert.True(delas.CustoDasMulheres(p) <= deles.CustoDasMulheres(p));
        }
    }

    [Fact]
    public void QuemPedeLevaOMelhorEstavelPossivel()
    {
        foreach (var p in Rodadas.Todas(3))
        {
            var estaveis = Estabilidade.Todos(p);
            var deles = GaleShapley.HomensPedem(p).Emparelhamento;

            // Nao e so que a soma e a menor: cada homem, um por um, fica com a
            // melhor mulher que ele consegue em QUALQUER emparelhamento estavel.
            for (var h = 0; h < p.N; h++)
            {
                var melhorPosto = estaveis.Min(e => p.PostoDoHomem(h, e.MulherDo(h)));
                Assert.Equal(melhorPosto, p.PostoDoHomem(h, deles.MulherDo(h)));
            }
        }
    }

    [Fact]
    public void QuemRecebeLevaOPiorEstavelPossivel()
    {
        foreach (var p in Rodadas.Todas(3))
        {
            var estaveis = Estabilidade.Todos(p);
            var deles = GaleShapley.HomensPedem(p).Emparelhamento;

            for (var m = 0; m < p.N; m++)
            {
                var piorPosto = estaveis.Max(e => p.PostoDaMulher(m, e.HomemDe(m)));
                Assert.Equal(piorPosto, p.PostoDaMulher(m, deles.HomemDe(m)));
            }
        }
    }

    [Fact]
    public void NenhumPedidoSeRepete()
    {
        foreach (var p in Rodadas.Todas(3))
        {
            var r = GaleShapley.HomensPedem(p);
            Assert.True(r.Pedidos <= p.N * p.N);
            Assert.Equal(r.Pedidos, r.Rodadas);
        }
    }

    [Theory]
    [InlineData(1, 1)]
    [InlineData(2, 3)]
    [InlineData(3, 7)]
    [InlineData(4, 13)]
    public void OLimiteDePedidos(int n, int esperado)
    {
        Assert.Equal(esperado, GaleShapley.LimiteDePedidos(n));
    }

    [Fact]
    public void OLimiteDePedidosEAlcancado()
    {
        var maximo = Rodadas.Todas(3).Max(p => GaleShapley.HomensPedem(p).Pedidos);
        Assert.Equal(GaleShapley.LimiteDePedidos(3), maximo);
    }

    [Fact]
    public void NuloNaoEAceito()
    {
        Assert.Throws<ArgumentNullException>(() => GaleShapley.HomensPedem(null!));
        Assert.Throws<ArgumentNullException>(() => GaleShapley.MulheresPedem(null!));
    }
}
