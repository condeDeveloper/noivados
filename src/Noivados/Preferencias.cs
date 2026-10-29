namespace Conde.Noivados;

/// <summary>
/// As listas de preferência de uma rodada: cada pessoa de um lado ordena todas
/// as pessoas do outro, da primeira à última, sem empate e sem exclusão.
///
/// Guardar a lista é confortável para ler, mas toda pergunta do problema é da
/// forma "ele prefere esta àquela?", então o que fica guardado de verdade é a
/// POSIÇÃO de cada pessoa na lista de cada pessoa. A comparação vira uma
/// subtração em vez de duas buscas.
/// </summary>
public sealed class Preferencias
{
    private readonly int[][] _postoDosHomens;
    private readonly int[][] _postoDasMulheres;

    public int N { get; }

    public IReadOnlyList<int[]> ListaDosHomens { get; }

    public IReadOnlyList<int[]> ListaDasMulheres { get; }

    public Preferencias(int[][] homens, int[][] mulheres)
    {
        ArgumentNullException.ThrowIfNull(homens);
        ArgumentNullException.ThrowIfNull(mulheres);
        if (homens.Length != mulheres.Length)
        {
            throw new ArgumentException("os dois lados precisam ter o mesmo tamanho");
        }

        N = homens.Length;
        Conferir(homens);
        Conferir(mulheres);

        ListaDosHomens = homens;
        ListaDasMulheres = mulheres;
        _postoDosHomens = Postos(homens);
        _postoDasMulheres = Postos(mulheres);
    }

    private void Conferir(int[][] listas)
    {
        foreach (var lista in listas)
        {
            if (lista.Length != N)
            {
                throw new ArgumentException("toda lista precisa citar todo mundo do outro lado");
            }

            var vistos = new bool[N];
            foreach (var quem in lista)
            {
                if (quem < 0 || quem >= N || vistos[quem])
                {
                    throw new ArgumentException("lista com repetido ou fora da faixa");
                }

                vistos[quem] = true;
            }
        }
    }

    private int[][] Postos(int[][] listas)
    {
        var postos = new int[N][];
        for (var i = 0; i < N; i++)
        {
            postos[i] = new int[N];
            for (var p = 0; p < N; p++)
            {
                postos[i][listas[i][p]] = p;
            }
        }

        return postos;
    }

    /// <summary>Quanto o homem gosta da mulher: zero é a favorita.</summary>
    public int PostoDoHomem(int homem, int mulher) => _postoDosHomens[homem][mulher];

    /// <summary>Quanto a mulher gosta do homem: zero é o favorito.</summary>
    public int PostoDaMulher(int mulher, int homem) => _postoDasMulheres[mulher][homem];

    public bool HomemPrefere(int homem, int candidata, int atual)
        => _postoDosHomens[homem][candidata] < _postoDosHomens[homem][atual];

    public bool MulherPrefere(int mulher, int candidato, int atual)
        => _postoDasMulheres[mulher][candidato] < _postoDasMulheres[mulher][atual];
}
