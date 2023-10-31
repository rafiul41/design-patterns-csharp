namespace DependencyInversion
{
  public class DependencyInversionExample
  {
    public void ExecuteDemo()
    {
      var relations = new RelationShips();
      relations.AddParentChildRelationShip(new Person("Liza"), new Person("Rafi"));
      relations.AddParentChildRelationShip(new Person("Liza"), new Person("Iqra"));
      relations.AddParentChildRelationShip(new Person("Liza"), new Person("Ifti"));

      // public property Relations is injected in the constructor for the research
      new Research(relations.Relations, "Liza");

      Console.WriteLine("Better relations ---");

      var betterRelations = new BetterRelationShips();
      betterRelations.AddParentChildRelationShip(new Person("Liza"), new Person("Rafi"));
      betterRelations.AddParentChildRelationShip(new Person("Liza"), new Person("Iqra"));
      betterRelations.AddParentChildRelationShip(new Person("Liza"), new Person("Ifti"));

      new BetterResearch(betterRelations, "Liza");
    }
  }

  public enum RelationShipType
  {
    Parent,
    Child,
    Sibling
  }

  public class Person
  {
    public string Name { get; set; }

    public Person(string name)
    {
      Name = name;
    }
  }

  // Low-level module
  public class RelationShips
  {
    private List<(Person, RelationShipType, Person)> relations = new List<(Person, RelationShipType, Person)>();
    public void AddParentChildRelationShip(Person parent, Person child)
    {
      relations.Add((parent, RelationShipType.Parent, child));
      relations.Add((child, RelationShipType.Child, parent));
    }

    // We are exposing the private relations as a public property so that we can use it in the research
    public List<(Person, RelationShipType, Person)> Relations => relations;
  }

  // high-level module
  // The primary function of this research class is to find the children of a parent which is implemented in the constructor
  public class Research
  {
    // Find children function of research is dependent on a low-level module property
    // For this reason the relations is made public for the following method to work
    public Research(List<(Person, RelationShipType, Person)> relations, string parentName)
    {
      foreach (var relation in relations.Where(x => x.Item1.Name == parentName && x.Item2 == RelationShipType.Parent))
      {
        Console.WriteLine($"{relation.Item1.Name} is a parent of {relation.Item3.Name}");
      }
    }
  }

  // Let's say for the above example, we want to change the implementation of storing the relations (like using a dictionary instead of a list) in RelationShips class
  // Then we also need to change the FindChildren function of the Research Class
  // Thus they are tightly coupled
  // To make it loosely coupled we need the Research Class to depend on an abstraction of the RelationShips class
  // We can achieve this using interface like below
  public interface IFindChildren
  {
    void FindChildren(string parentName);
  }

  public class BetterRelationShips : IFindChildren
  {
    private List<(Person, RelationShipType, Person)> relations = new List<(Person, RelationShipType, Person)>();

    public void AddParentChildRelationShip(Person parent, Person child)
    {
      relations.Add((parent, RelationShipType.Parent, child));
      relations.Add((child, RelationShipType.Child, parent));
    }

    public void FindChildren(string parentName)
    {
      foreach (var relation in relations.Where(x => x.Item1.Name == parentName && x.Item2 == RelationShipType.Parent))
      {
        Console.WriteLine($"{relation.Item1.Name} is a parent of {relation.Item3.Name}");
      }
    }
  }

  public class BetterResearch
  {
    // Here the research is not dependent on a low-level property but on an abstraction which is the IFindChildren
    public BetterResearch(IFindChildren relations, string parentName)
    {
      relations.FindChildren(parentName);
    }
  }
}

