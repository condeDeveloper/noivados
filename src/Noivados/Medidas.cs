namespace Conde.Noivados;

/// <summary>
/// As tabelas do README. Cada número sai de um método daqui e está preso num
/// teste; no arquivo de medidas só entra o que eu vi o programa imprimir.
/// </summary>
public static class Medidas
{
    public readonly record struct LinhaQuantos(int Estaveis, long Rodadas, double Fracao);

    public readonly record struct LinhaConfronto(
        long Rodadas,
        long HomensOtimoParaEles,
        long HomensPessimoParaElas,
        long UmSoEstavel,
        long GaleShapleyAcertou,
        long OsDoisLadosIguais);

    public readonly record struct LinhaPedidos(int Pedidos, long Quantas);

    public readonly record struct LinhaReticulado(long Rodadas, long ComEncontro, long ComJuncao, long Falhas);

    /// <summary>Quantas rodadas têm cada quantidade de emparelhamentos estáveis.</summary>
    public static List<LinhaQuantos> Quantos(int n)
    {
        var conta = new Dictionary<int, long>();
        long total = 0;

        foreach (var p in Rodadas.Todas(n))
        {
            var quantos = Estabilidade.Todos(p).Count;
            conta[quantos] = conta.GetValueOrDefault(quantos) + 1;
            total++;
        }

        var linhas = new List<LinhaQuantos>();
        foreach (var chave in conta.Keys.Order())
        {
            linhas.Add(new LinhaQuantos(chave, conta[chave], Math.Round(100.0 * conta[chave] / total, 2)));
        }

        return linhas;
    }

    /// <summary>
    /// A pergunta central: o resultado de quem pede é o melhor possível para
    /// quem pede, e o pior possível para quem recebe?
    /// </summary>
    public static LinhaConfronto Confronto(int n)
    {
        long rodadas = 0;
        long otimoParaEles = 0;
        long pessimoParaElas = 0;
        long umSo = 0;
        long acertou = 0;
        long iguais = 0;

        foreach (var p in Rodadas.Todas(n))
        {
            rodadas++;
            var estaveis = Estabilidade.Todos(p);
            var deles = GaleShapley.HomensPedem(p).Emparelhamento;
            var delas = GaleShapley.MulheresPedem(p).Emparelhamento;

            if (estaveis.Contains(deles) && estaveis.Contains(delas))
            {
                acertou++;
            }

            var melhorParaEles = estaveis.Min(e => e.CustoDosHomens(p));
            var piorParaElas = estaveis.Max(e => e.CustoDasMulheres(p));

            if (deles.CustoDosHomens(p) == melhorParaEles)
            {
                otimoParaEles++;
            }

            if (deles.CustoDasMulheres(p) == piorParaElas)
            {
                pessimoParaElas++;
            }

            if (estaveis.Count == 1)
            {
                umSo++;
            }

            if (deles.Equals(delas))
            {
                iguais++;
            }
        }

        return new LinhaConfronto(rodadas, otimoParaEles, pessimoParaElas, umSo, acertou, iguais);
    }

    /// <summary>Quantos pedidos o algoritmo gasta, contra o limite conhecido.</summary>
    public static List<LinhaPedidos> Pedidos(int n)
    {
        var conta = new Dictionary<int, long>();
        foreach (var p in Rodadas.Todas(n))
        {
            var pedidos = GaleShapley.HomensPedem(p).Pedidos;
            conta[pedidos] = conta.GetValueOrDefault(pedidos) + 1;
        }

        var linhas = new List<LinhaPedidos>();
        foreach (var chave in conta.Keys.Order())
        {
            linhas.Add(new LinhaPedidos(chave, conta[chave]));
        }

        return linhas;
    }

    /// <summary>
    /// O reticulado: pegando dois emparelhamentos estáveis e dando a cada homem
    /// a melhor das duas mulheres, o resultado é de novo estável. Idem com a
    /// pior. Aqui isso é conferido, não suposto.
    /// </summary>
    public static LinhaReticulado Reticulado(int n)
    {
        long rodadas = 0;
        long comEncontro = 0;
        long comJuncao = 0;
        long falhas = 0;

        foreach (var p in Rodadas.Todas(n))
        {
            rodadas++;
            var estaveis = Estabilidade.Todos(p);
            var conjunto = estaveis.ToHashSet();

            foreach (var a in estaveis)
            {
                foreach (var b in estaveis)
                {
                    var melhor = new int[n];
                    var pior = new int[n];
                    for (var h = 0; h < n; h++)
                    {
                        var ma = a.MulherDo(h);
                        var mb = b.MulherDo(h);
                        melhor[h] = p.PostoDoHomem(h, ma) <= p.PostoDoHomem(h, mb) ? ma : mb;
                        pior[h] = p.PostoDoHomem(h, ma) >= p.PostoDoHomem(h, mb) ? ma : mb;
                    }

                    if (EhBijecao(melhor) && conjunto.Contains(new Emparelhamento(melhor)))
                    {
                        comJuncao++;
                    }
                    else
                    {
                        falhas++;
                    }

                    if (EhBijecao(pior) && conjunto.Contains(new Emparelhamento(pior)))
                    {
                        comEncontro++;
                    }
                    else
                    {
                        falhas++;
                    }
                }
            }
        }

        return new LinhaReticulado(rodadas, comEncontro, comJuncao, falhas);
    }

    private static bool EhBijecao(int[] valores)
    {
        var visto = new bool[valores.Length];
        foreach (var v in valores)
        {
            if (visto[v])
            {
                return false;
            }

            visto[v] = true;
        }

        return true;
    }
}
