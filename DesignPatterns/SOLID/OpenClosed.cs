namespace OpenClosed
{

  // Product color enums
  public enum Color
  {
    Red, Green, Blue
  }

  // Product size enums
  public enum Size
  {
    Small, Medium, Large
  }
  public class OpenClosedExample
  {
    public void ExecuteDemo()
    {
      var products = new List<Product>() {
        new Product("Tree", Color.Green, Size.Medium),
        new Product("Pencil", Color.Red, Size.Small),
        new Product("House", Color.Blue, Size.Large)
      };

      var productFilter = new ProductFilter();
      var filteredProducts = productFilter.FilterByColor(products, Color.Red);

      Console.WriteLine("Filtered Products NAIVE:");
      foreach (var product in filteredProducts)
      {
        Console.WriteLine(product.Name);
      }

      var betterFilter = new BetterFilter();
      filteredProducts = betterFilter.Filter(products, new ColorSpecification(Color.Green));

      Console.WriteLine("Filtered Products AWESOME:");
      foreach (var product in filteredProducts)
      {
        Console.WriteLine(product.Name);
      }

      Console.WriteLine("AND SPECIFICATION FILTER");
      filteredProducts = betterFilter.Filter(products, new AndSpecification(new List<ISpecification<Product>>() 
      { 
        new ColorSpecification(Color.Blue), new SizeSpecification(Size.Large)
      }));
      foreach (var product in filteredProducts)
      {
        Console.WriteLine(product.Name);
      }
    }
  }

  public class Product
  {
    public string Name { get; set; }
    public Color Color { get; set; }
    public Size Size { get; set; }

    public Product(string name, Color color, Size size)
    {
      Name = name;
      Color = color;
      Size = size;
    }
  }

  // We want to filter the products by their size and color
  // Thus we are considering another class for filtering
  public class ProductFilter
  {
    // Suppose we are to filter the products by color
    // Then it will be our go to solution, to write the following function
    public IEnumerable<Product> FilterByColor(IEnumerable<Product> products, Color color)
    {
      foreach (var product in products)
      {
        if (product.Color == color) yield return product;
      }
    }

    // Again, if we wanted to filter by size
    public IEnumerable<Product> FilterBySize(IEnumerable<Product> products, Size size)
    {
      foreach (var product in products)
      {
        if (product.Size == size) yield return product;
      }
    }

    // Again, if we wanted to filter by size and Color
    // Then we have to add yet another function
    // *** PROBLEM: So for every specific combination of filter we have to add a function ***
    // In this way we are violating the open closed principle - classes should be open for extension but closed for modifications
    // But here, we are modifying the class ProductFilter every time there is a filter specification.
    public IEnumerable<Product> FilterByColorAndSize(IEnumerable<Product> products, Size size, Color color)
    {
      foreach (var product in products)
      {
        if (product.Size == size && product.Color == color) yield return product;
      }
    }
  }

  // To tackle the problem stated above we need to use an enterprise pattern, known as the specification pattern
  // ------------------------- Specification pattern starts here -----------------------------------------------
  // We need two interfaces two implement the specification pattern
  // One is a filter specification interface which have only one function
  public interface ISpecification<T>
  {
    bool IsSatisfied(T t);
  }

  // Second is the IFilter interface
  public interface IFilter<T>
  {
    IEnumerable<T> Filter(IEnumerable<T> items, ISpecification<T> spec);
  }

  // We can now create any filter specification as we want
  // Now for every specification we can create an ISpecification without touching the BetterFilter class
  public class ColorSpecification : ISpecification<Product>
  {
    private Color color;
    public ColorSpecification(Color color)
    {
      this.color = color;
    }

    public bool IsSatisfied(Product t)
    {
      return t.Color == color;
    }
  }

  public class SizeSpecification : ISpecification<Product>
  {
    private Size size;
    public SizeSpecification(Size size)
    {
      this.size = size;
    }

    public bool IsSatisfied(Product t)
    {
      return t.Size == size;
    }
  }

  // Now let's say that we need to check two specifications like an AND operator
  // We can create a generic specification which takes array of specifications
  public class AndSpecification : ISpecification<Product>
  {
    private List<ISpecification<Product>> specifications;

    public AndSpecification(List<ISpecification<Product>> specifications)
    {
      this.specifications = specifications;
    }
    public bool IsSatisfied(Product t)
    {
      foreach (var specification in specifications)
      {
        if(!specification.IsSatisfied(t)) {
          return false;
        }
      }

      return true;
    }
  }
  public class BetterFilter : IFilter<Product>
  {
    public IEnumerable<Product> Filter(IEnumerable<Product> items, ISpecification<Product> spec)
    {
      foreach (var item in items)
      {
        if (spec.IsSatisfied(item)) yield return item;
      }
    }
  }
}

