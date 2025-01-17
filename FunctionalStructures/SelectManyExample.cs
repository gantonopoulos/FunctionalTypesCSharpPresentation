namespace FunctionalStructures;
using Pet = string;


public static class SelectManyExample
{
    private record Neighbour(string Name, Pet[] Pets);

    public static void Run()
    {
        Neighbour[] neighbours = new Neighbour[]
        {
            new("Lina", ["Bob", "Luna"]),
            new("Mike", []),
            new("Peter", ["Berny"])
        };

        IEnumerable<Pet[]> map = neighbours.Select(neighbour => neighbour.Pets);
        IEnumerable<Pet> bind = neighbours.SelectMany(neighbour => neighbour.Pets);
    }
}