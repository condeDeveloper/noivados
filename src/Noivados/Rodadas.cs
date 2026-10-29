namespace Conde.Noivados;

/// <summary>
/// A enumeração de TODAS as rodadas possíveis de um tamanho: cada uma das
/// <c>n</c> pessoas de cada lado escolhendo uma das <c>n!</c> ordens.
///
/// Com dois de cada lado são 16 rodadas. Com três, 46.656. Com quatro seriam
/// 110 bilhões, e é por isso que o tamanho três é onde este repositório mora:
/// é o maior em que absolutamente tudo cabe.
/// </summary>
public static class Rodadas
{
    public static long Quantidade(int n)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(n);
        long fatorial = 1;
        for (var i = 2; i <= n; i++)
        {
            fatorial *= i;
        }

        long total = 1;
        for (var i = 0; i < 2 * n; i++)
        {
            total *= fatorial;
        }

        return total;
    }

    public static IEnumerable<Preferencias> Todas(int n)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(n);
        var ordens = Estabilidade.Permutacoes(n).ToList();
        var escolha = new int[2 * n];

        return Passo(0);

        IEnumerable<Preferencias> Passo(int i)
        {
            if (i == 2 * n)
            {
                var homens = new int[n][];
                var mulheres = new int[n][];
                for (var k = 0; k < n; k++)
                {
                    homens[k] = ordens[escolha[k]];
                    mulheres[k] = ordens[escolha[n + k]];
                }

                yield return new Preferencias(homens, mulheres);
                yield break;
            }

            for (var o = 0; o < ordens.Count; o++)
            {
                escolha[i] = o;
                foreach (var saida in Passo(i + 1))
                {
                    yield return saida;
                }
            }
        }
    }
}
