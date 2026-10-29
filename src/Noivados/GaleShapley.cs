namespace Conde.Noivados;

/// <summary>
/// O algoritmo de 1962: enquanto houver homem sozinho, ele pede a mulher que
/// mais quer e que ainda não o recusou; ela fica com o melhor dos dois e
/// dispensa o outro.
///
/// A versão com os papéis trocados é o mesmo código com os lados invertidos, e
/// está aqui porque a diferença entre os dois resultados é o assunto principal
/// deste repositório.
/// </summary>
public static class GaleShapley
{
    public readonly record struct Resultado(Emparelhamento Emparelhamento, int Pedidos, int Rodadas);

    public static Resultado HomensPedem(Preferencias p)
    {
        ArgumentNullException.ThrowIfNull(p);
        return Rodar(p, true);
    }

    public static Resultado MulheresPedem(Preferencias p)
    {
        ArgumentNullException.ThrowIfNull(p);
        return Rodar(p, false);
    }

    private static Resultado Rodar(Preferencias p, bool homensPedem)
    {
        var n = p.N;
        var parDoQuePede = new int[n];
        var parDoQueRecebe = new int[n];
        Array.Fill(parDoQuePede, -1);
        Array.Fill(parDoQueRecebe, -1);

        var proximo = new int[n];
        var sozinhos = new Stack<int>(Enumerable.Range(0, n).Reverse());
        var pedidos = 0;
        var rodadas = 0;

        while (sozinhos.Count > 0)
        {
            rodadas++;
            var quemPede = sozinhos.Pop();
            var alvo = homensPedem
                ? p.ListaDosHomens[quemPede][proximo[quemPede]]
                : p.ListaDasMulheres[quemPede][proximo[quemPede]];
            proximo[quemPede]++;
            pedidos++;

            var atual = parDoQueRecebe[alvo];
            if (atual == -1)
            {
                parDoQueRecebe[alvo] = quemPede;
                parDoQuePede[quemPede] = alvo;
                continue;
            }

            var ficaComONovo = homensPedem
                ? p.MulherPrefere(alvo, quemPede, atual)
                : p.HomemPrefere(alvo, quemPede, atual);

            if (ficaComONovo)
            {
                parDoQueRecebe[alvo] = quemPede;
                parDoQuePede[quemPede] = alvo;
                parDoQuePede[atual] = -1;
                sozinhos.Push(atual);
            }
            else
            {
                sozinhos.Push(quemPede);
            }
        }

        var porHomem = homensPedem ? parDoQuePede : parDoQueRecebe;
        return new Resultado(new Emparelhamento(porHomem), pedidos, rodadas);
    }

    /// <summary>
    /// O pior caso conhecido da quantidade de pedidos: <c>n² − n + 1</c>.
    /// Está aqui para ser comparado com a varredura, não para ser citado.
    /// </summary>
    public static int LimiteDePedidos(int n) => (n * n) - n + 1;
}
