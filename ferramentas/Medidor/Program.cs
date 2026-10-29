using Conde.Noivados;

if (args.Length == 0)
{
    Console.WriteLine("uso: medidor <quantos|confronto|pedidos|reticulado>");
    return 1;
}

switch (args[0])
{
    case "quantos":
        Quantos();
        break;
    case "confronto":
        Confronto();
        break;
    case "pedidos":
        Pedidos();
        break;
    case "reticulado":
        Reticulado();
        break;
    default:
        Console.WriteLine($"comando desconhecido: {args[0]}");
        return 1;
}

return 0;

static void Quantos()
{
    foreach (var n in new[] { 2, 3 })
    {
        Console.WriteLine($"Com {n} de cada lado: {Rodadas.Quantidade(n)} rodadas possiveis");
        Console.WriteLine("estaveis  rodadas   %");
        foreach (var l in Medidas.Quantos(n))
        {
            Console.WriteLine($"{l.Estaveis,-10}{l.Rodadas,-10}{l.Fracao}");
        }

        Console.WriteLine();
    }
}

static void Confronto()
{
    foreach (var n in new[] { 2, 3 })
    {
        var c = Medidas.Confronto(n);
        Console.WriteLine($"Com {n} de cada lado, {c.Rodadas} rodadas varridas:");
        Console.WriteLine($"  o resultado de quem pede e o melhor possivel para quem pede: {c.HomensOtimoParaEles}");
        Console.WriteLine($"  e o pior possivel para quem recebe:                          {c.HomensPessimoParaElas}");
        Console.WriteLine($"  Gale-Shapley devolveu um emparelhamento estavel nos dois lados: {c.GaleShapleyAcertou}");
        Console.WriteLine($"  rodadas com um unico emparelhamento estavel:                 {c.UmSoEstavel}");
        Console.WriteLine($"  rodadas em que pedir ou receber da no mesmo:                 {c.OsDoisLadosIguais}");
        Console.WriteLine();
    }
}

static void Pedidos()
{
    foreach (var n in new[] { 2, 3 })
    {
        Console.WriteLine($"Pedidos gastos com {n} de cada lado (limite n2-n+1 = {GaleShapley.LimiteDePedidos(n)})");
        Console.WriteLine("pedidos  rodadas");
        foreach (var l in Medidas.Pedidos(n))
        {
            Console.WriteLine($"{l.Pedidos,-9}{l.Quantas}");
        }

        Console.WriteLine();
    }
}

static void Reticulado()
{
    foreach (var n in new[] { 2, 3 })
    {
        var r = Medidas.Reticulado(n);
        Console.WriteLine($"Com {n} de cada lado, {r.Rodadas} rodadas:");
        Console.WriteLine($"  pares em que a melhor escolha de cada homem deu estavel: {r.ComJuncao}");
        Console.WriteLine($"  pares em que a pior escolha de cada homem deu estavel:   {r.ComEncontro}");
        Console.WriteLine($"  falhas:                                                  {r.Falhas}");
        Console.WriteLine();
    }
}
