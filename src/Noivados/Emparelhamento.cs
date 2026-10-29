namespace Conde.Noivados;

/// <summary>
/// Um emparelhamento: para cada homem, a mulher com quem ele ficou. É uma
/// bijeção, então guardar um lado já guarda o outro, e a volta é calculada uma
/// vez na construção.
/// </summary>
public sealed class Emparelhamento : IEquatable<Emparelhamento>
{
    private readonly int[] _mulherDo;
    private readonly int[] _homemDe;
    private readonly int _hash;

    public int N => _mulherDo.Length;

    public Emparelhamento(int[] mulherDoHomem)
    {
        ArgumentNullException.ThrowIfNull(mulherDoHomem);
        _mulherDo = (int[])mulherDoHomem.Clone();
        _homemDe = new int[_mulherDo.Length];
        Array.Fill(_homemDe, -1);

        foreach (var h in Enumerable.Range(0, _mulherDo.Length))
        {
            var m = _mulherDo[h];
            if (m < 0 || m >= _mulherDo.Length || _homemDe[m] != -1)
            {
                throw new ArgumentException("o emparelhamento precisa ser uma bijecao");
            }

            _homemDe[m] = h;
        }

        var acumulado = 17;
        foreach (var m in _mulherDo)
        {
            acumulado = (acumulado * 31) + m;
        }

        _hash = acumulado;
    }

    public int MulherDo(int homem) => _mulherDo[homem];

    public int HomemDe(int mulher) => _homemDe[mulher];

    public IReadOnlyList<int> PorHomem => _mulherDo;

    public bool Equals(Emparelhamento? outro)
    {
        if (outro is null || outro.N != N)
        {
            return false;
        }

        for (var h = 0; h < N; h++)
        {
            if (_mulherDo[h] != outro._mulherDo[h])
            {
                return false;
            }
        }

        return true;
    }

    public override bool Equals(object? obj) => Equals(obj as Emparelhamento);

    public override int GetHashCode() => _hash;

    public override string ToString() => string.Join(" ", _mulherDo);

    /// <summary>
    /// A soma dos postos de um lado. Quanto menor, mais feliz aquele lado está:
    /// é o número que mede quem levou vantagem.
    /// </summary>
    public int CustoDosHomens(Preferencias p)
    {
        var soma = 0;
        for (var h = 0; h < N; h++)
        {
            soma += p.PostoDoHomem(h, _mulherDo[h]);
        }

        return soma;
    }

    public int CustoDasMulheres(Preferencias p)
    {
        var soma = 0;
        for (var m = 0; m < N; m++)
        {
            soma += p.PostoDaMulher(m, _homemDe[m]);
        }

        return soma;
    }
}
