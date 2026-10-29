# Casamentos estáveis

`n` pessoas de um lado, `n` do outro, e cada uma ordena todas as do outro lado da
primeira à última. Um emparelhamento é **estável** quando não existe um par que não está
junto e que preferiria estar: ninguém tem com quem fugir.

Gale e Shapley mostraram em 1962 que sempre existe pelo menos um, e deram um algoritmo
que acha um deles. O que o algoritmo **não** diz, e é o assunto daqui, é quanto ele
decide por você.

Aqui não se sorteia rodada: com três de cada lado existem `(3!)⁶ = 46.656` rodadas
possíveis, e **todas** são varridas. Para cada uma, as `3! = 6` maneiras de casar são
julgadas uma a uma pela definição.

```
dotnet test testes/Noivados.Testes/Noivados.Testes.csproj -c Release
dotnet run  --project ferramentas/Medidor/Medidor.csproj -c Release -- quantos
dotnet run  --project ferramentas/Medidor/Medidor.csproj -c Release -- confronto
dotnet run  --project ferramentas/Medidor/Medidor.csproj -c Release -- pedidos
dotnet run  --project ferramentas/Medidor/Medidor.csproj -c Release -- reticulado
```

## O juiz

A `Estabilidade` não sabe que Gale e Shapley existem. Ela aplica a definição:

> o par `(h, m)` bloqueia quando `h` prefere `m` a quem tem **e** `m` prefere `h` a quem
> tem. Sem par bloqueador, estável.

E a `Estabilidade.Todos` acha os estáveis do jeito mais burro possível: gera as `n!`
bijeções e pergunta ao juiz uma por uma. É por ser burra que ela serve de referência —
não dá para ela estar certa por acidente de algoritmo. O algoritmo de 1962 aparece
depois e é conferido contra ela nas 46.656 rodadas.

## Por que para no três

```
n     rodadas possiveis
2     16
3     46.656
4     110.075.314.176
```

Três é o maior tamanho em que **tudo** cabe. Quatro são cento e dez bilhões de rodadas,
e aí já seria amostra.

## Os achados

### 1. Quem pede leva o melhor possível e quem recebe leva o pior — nas 46.656, sem uma exceção

```
Com 3 de cada lado, 46656 rodadas varridas:
  o resultado de quem pede e o melhor possivel para quem pede: 46656
  e o pior possivel para quem recebe:                          46656
  Gale-Shapley devolveu um emparelhamento estavel nos dois lados: 46656
```

E não é a soma dos postos que bate: é **pessoa por pessoa**. Cada homem fica com a melhor
mulher que ele conseguiria em qualquer emparelhamento estável, e cada mulher fica com o
pior homem que ela teria em qualquer um deles. Não existe rodada em que um deles ganhe
alguma coisa no meio do caminho.

O algoritmo é apresentado como justo porque sempre devolve algo estável. Ele é estável e
é **completamente parcial**, e a parcialidade não é uma tendência: é uma identidade.

### 2. Em 73% das rodadas o algoritmo não decide nada

```
estaveis  rodadas   %
1         34080     73.05
2         11484     24.61
3         1092      2.34
```

Em quase três quartos das rodadas existe **um único** emparelhamento estável, e aí tanto
faz quem pede: o resultado é o mesmo. A contagem de rodadas em que pedir ou receber dá no
mesmo é `34.080` — o mesmo número, exatamente, porque são o mesmo conjunto.

Toda a discussão sobre quem leva vantagem vale para um quarto das rodadas. No resto, a
resposta já estava escrita nas listas.

### 3. O limite `n² − n + 1` de pedidos é atingido

```
pedidos  rodadas
3        10368
4        15552
5        14256
6        5832
7        648
```

Com três de cada lado o limite é `3² − 3 + 1 = 7`, e **648 rodadas gastam exatamente 7**.
É 1,4% delas: o pior caso é raro, mas existe, e o limite não tem folga nenhuma.

### 4. O reticulado fecha nas 89.844 combinações, sem uma falha

Pegue dois emparelhamentos estáveis da mesma rodada e dê a cada homem a **melhor** das
duas mulheres que ele tem neles. O resultado tem duas chances de dar errado: duas pessoas
podem acabar com a mesma parceira, e mesmo sendo bijeção pode não ser estável.

Nas 89.844 combinações possíveis (a soma dos quadrados das contagens de estáveis), não
deu errado **nenhuma vez** — nem com a melhor, nem com a pior. É um resultado que eu
conhecia como teorema e que aqui está medido: a estrutura dos emparelhamentos estáveis é
tão rígida que combinar dois nunca sai do conjunto.

## O que as medidas me corrigiram

- **Eu ia escrever que o algoritmo favorece quem pede "na média".** A palavra estava no
  rascunho, e ela é falsa por ser fraca demais. A vantagem não é média nenhuma: é pessoa
  por pessoa, em todas as 46.656 rodadas, sem uma única exceção. Escrever "na média"
  quando o que existe é "sempre" é errar para o lado que parece prudente.

- **E ia escrever que pedir é melhor que receber, e parar aí.** Em 73,05% das rodadas
  pedir e receber dão o **mesmo** emparelhamento, porque só existe um estável. A frase
  certa não é "quem pede ganha": é "quem pede ganha quando há o que ganhar, e na maioria
  das vezes não há". Essa foi a medida que mais mudou o texto.

- **Eu desconfiei do `34.080` repetido.** Os dois números apareceram iguais na primeira
  rodada do medidor e eu fui procurar o lugar onde eu tinha contado a mesma coisa duas
  vezes por engano. Não tinha: as rodadas com um único emparelhamento estável são
  exatamente as rodadas em que os dois lados do algoritmo coincidem, e isso sai direto da
  definição depois que se vê. Número repetido merece desconfiança, e às vezes a
  desconfiança termina em entender melhor.

- **Eu achava que quatro de cada lado caberia.** São 110.075.314.176 rodadas. A conta
  ficou no código e no README em vez de virar um "por limitação de tempo", porque o
  motivo de parar no três é um número, não uma preguiça.

- **Eu esperava alguma falha no reticulado.** Combinar dois emparelhamentos escolhendo a
  melhor parceira de cada homem me parecia a cara de uma operação que às vezes nem
  bijeção devolve — duas pessoas querendo a mesma. Zero falhas em 89.844.

## Estrutura

```
src/Noivados/
  Preferencias.cs    as listas, guardadas como posto para a comparacao ser subtracao
  Emparelhamento.cs  a bijecao e o custo de cada lado
  Estabilidade.cs    o juiz pela definicao, e a forca bruta sobre as n! bijecoes
  GaleShapley.cs     o algoritmo de 1962, nos dois sentidos
  Rodadas.cs         todas as rodadas de um tamanho
  Medidas.cs         as tabelas deste README
testes/Noivados.Testes/
ferramentas/Medidor/
```

59 testes.
