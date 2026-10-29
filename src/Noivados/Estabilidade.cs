namespace Conde.Noivados;

/// <summary>
/// O juiz: diz se um emparelhamento é estável aplicando a DEFINIÇÃO, e nada
/// além dela.
///
/// Um par que não está casado entre si é um par bloqueador quando os dois
/// preferem um ao outro em vez de quem têm. Um emparelhamento é estável quando
/// não existe par bloqueador nenhum. É só isso, e é esta classe que decide
/// tudo o que o repositório afirma — o algoritmo de Gale e Shapley aparece
/// depois, e é conferido contra ela.
/// </summary>
public static class Estabilidade
{
    public static bool Estavel(Preferencias p, Emparelhamento e)
    {
        ArgumentNullException.ThrowIfNull(p);
        ArgumentNullException.ThrowIfNull(e);
        return Bloqueadores(p, e).Count == 0;
    }

    public static List<(int Homem, int Mulher)> Bloqueadores(Preferencias p, Emparelhamento e)
    {
        ArgumentNullException.ThrowIfNull(p);
        ArgumentNullException.ThrowIfNull(e);
        var pares = new List<(int, int)>();

        for (var h = 0; h < p.N; h++)
        {
            for (var m = 0; m < p.N; m++)
            {
                if (e.MulherDo(h) == m)
                {
                    continue;
                }

                if (p.HomemPrefere(h, m, e.MulherDo(h)) && p.MulherPrefere(m, h, e.HomemDe(m)))
                {
                    pares.Add((h, m));
                }
            }
        }

        return pares;
    }

    /// <summary>
    /// Todos os emparelhamentos estáveis de uma rodada, achados pela força
    /// bruta: gerar as n! bijeções possíveis e perguntar ao juiz uma a uma.
    ///
    /// É o método mais burro possível, e é por isso que ele serve de
    /// referência: ele não pode estar certo por acidente de algoritmo.
    /// </summary>
    public static List<Emparelhamento> Todos(Preferencias p)
    {
        ArgumentNullException.ThrowIfNull(p);
        var estaveis = new List<Emparelhamento>();
        foreach (var permutacao in Permutacoes(p.N))
        {
            var e = new Emparelhamento(permutacao);
            if (Estavel(p, e))
            {
                estaveis.Add(e);
            }
        }

        return estaveis;
    }

    public static IEnumerable<int[]> Permutacoes(int n)
    {
        var atual = new int[n];
        var usado = new bool[n];
        return Passo(0);

        IEnumerable<int[]> Passo(int i)
        {
            if (i == n)
            {
                yield return (int[])atual.Clone();
                yield break;
            }

            for (var v = 0; v < n; v++)
            {
                if (usado[v])
                {
                    continue;
                }

                usado[v] = true;
                atual[i] = v;
                foreach (var saida in Passo(i + 1))
                {
                    yield return saida;
                }

                usado[v] = false;
            }
        }
    }
}
