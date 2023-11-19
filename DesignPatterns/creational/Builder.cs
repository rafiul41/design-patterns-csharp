using System.Text;

namespace Builder
{
  public class BuilderExample
  {
    public void ExecuteDemo()
    {
      // Simplest builder pattern already implemented in c# is the stringBuilder
      // Constructor does not take multiple parameters instead construction can be done using Append
      var str = new StringBuilder();
      str.Append("Hello");
      str.Append(" World");
      Console.WriteLine(str);

      // What if we want to build an html string
      // For that we need to have a tree like structure
      // checkout the HtmlElement and HtmlBuilder class
      var htmlBuilder = new HtmlBuilder("root");
      htmlBuilder.AddChild("div", "div text 1");
      // We can use fluent interface like below if we return the builder object in the Add child function
      htmlBuilder.AddChild("div", "div text 2").AddChild("div", "div text 3");
      Console.WriteLine("After populating the html builder");
      Console.WriteLine(htmlBuilder.ToString());

      htmlBuilder.Clear();
      Console.WriteLine("After clearing");
      Console.WriteLine(htmlBuilder.ToString());

      var naivePersonBuilder = new NaivePersonJobBuilder();
      // below we cannot chain SetJob as SetName returns NaivePersonInfoBuilder which doesn't have SetJob method
      // naivePersonBuilder.SetName("Rafi").SetJob("SDE");
      // For this we need to return the same type for all of the builders, including the inherited ones

      // Awesome fluent builder starts here
      // Builder should be instantiated from the lowest level of inheritance
      // in this case it is the job builder
      // But we need a generic type T to instantiate which we do not have
      // Thus an API is exposed in the Person class itself
      var person = Person.New.SetName("Rafi").SetJob("SDE").Build();
      Console.WriteLine(person);

      // I can define the Type as Employee as EmployeeBuilder has an implicit conversion operator
      Employee employee = new EmployeeBuilder() // returns employeeBuilder
        .Address 
          .SetStreetName("Nakhalpara") // returns employeeAddressBuilder
          .SetPostCode("1215")
        .Job // Job is present in employeeAddressBuilder since it is also a employee builder
          .SetSalary(123)
          .SetTitle("SDE");
      Console.WriteLine(employee.ToString());
    }
  }

  #region HTML string builder
  // Following is the html element that we need to build the html string
  public class HtmlElement
  {
    public string? ElementName { get; set; }
    public string? Text { get; set; }
    private const int IndentSize = 2;

    public List<HtmlElement>? Children { get; set; } = new List<HtmlElement>();
    public HtmlElement()
    {

    }
    public HtmlElement(string elementName, string text)
    {
      ElementName = elementName ?? throw new ArgumentNullException("Must have elementName");
      Text = text ?? throw new ArgumentNullException("Must have text");
    }

    private string getSpaces(int spaceCount)
    {
      return new string(' ', spaceCount);
    }

    public string ToHtmlString(int indentCount)
    {
      var sb = new StringBuilder();
      sb.AppendLine($"{getSpaces(indentCount * IndentSize)}<{ElementName}>");
      if (!string.IsNullOrEmpty(Text))
      {
        sb.AppendLine($"{getSpaces((indentCount + 1) * IndentSize)}{Text}");
      }
      foreach (var element in Children)
      {
        sb.Append(element.ToHtmlString(indentCount + 1));
      }
      sb.AppendLine($"{getSpaces(indentCount * IndentSize)}</{ElementName}>");
      return sb.ToString();
    }
  }

  // We need a builder to build the whole html string
  public class HtmlBuilder
  {
    // Initially the builder needs a root element
    HtmlElement rootElement = new HtmlElement();

    public HtmlBuilder(string rootName)
    {
      rootElement.ElementName = rootName;
    }
    public HtmlBuilder AddChild(string childName, string childText)
    {
      rootElement.Children?.Add(new HtmlElement(childName, childText));
      return this;
    }

    public override string ToString()
    {
      return rootElement.ToHtmlString(0);
    }
    public void Clear()
    {
      rootElement = new HtmlElement { ElementName = rootElement.ElementName };
    }
  }
  #endregion

  #region Naive Fluent Builder
  public class NaivePerson
  {
    public string Name { get; set; }
    public string Job { get; set; }
  }

  public class NaivePersonInfoBuilder
  {
    protected NaivePerson person { get; set; }
    public NaivePersonInfoBuilder()
    {
      person = new NaivePerson();
    }
    public NaivePersonInfoBuilder SetName(string name)
    {
      person.Name = name;
      return this;
    }
  }

  public class NaivePersonJobBuilder : NaivePersonInfoBuilder
  {
    public NaivePersonJobBuilder SetJob(string job)
    {
      person.Job = job;
      return this;
    }
  }
  #endregion

  #region Awesome Fluent Builder
  public class Person
  {
    public string Name { get; set; }
    public string Job { get; set; }
    // API START
    // Notice here also recursive generics is used
    // Thus following builder is the lowest level of inheritance
    // ***This is one way to expose the builder
    public class Builder : PersonJobBuilder<Builder>
    {
    }
    public static Builder New => new Builder();
    // API END
    public override string ToString()
    {
      return $"Name: {Name}, Job: {Job}";
    }
  }

  // we are making the Person builder abstract as we don't want an instance of this as it has no functionality of building
  public abstract class PersonBuilder
  {
    protected Person person = new Person();
    public Person Build()
    {
      return person;
    }
  }
  public class PersonInfoBuilder<T> : PersonBuilder where T : PersonInfoBuilder<T>
  {
    public T SetName(string name)
    {
      person.Name = name;
      return (T)this; // ***lowest level of inheritance type is called. HOW?????? try to think about this then everything will be easy.***
    }
  }

  // Following is the most important class and you can get the gist how type is propagated from child to parent
  // here PersonJobBuilder is propagated to PersonInfoBuilder as T, and we are returning this as T thus job builder is returned in SetName
  // which should be the case
  public class PersonJobBuilder<T> : PersonInfoBuilder<PersonJobBuilder<T>> where T : PersonJobBuilder<T>
  {
    public T SetJob(string job)
    {
      person.Job = job;
      return (T)this;
    }
  }
  #endregion

  #region Faceted Builder
  public class Employee
  {
    // address
    public string StreetName { get; set; }
    public string PostCode { get; set; }

    // Job
    public int Salary { get; set; }
    public string Title { get; set; }

    public override string ToString()
    {
      return $"StreetName: {StreetName}, PostCode: {PostCode}, Salary: {Salary}, Title: {Title}";
    }
  }

  // Following works as a Facade for the address and job builder
  public class EmployeeBuilder
  {
    protected Employee employee = new Employee();

    public EmployeeAddressBuilder Address => new EmployeeAddressBuilder();
    public EmployeeJobBuilder Job => new EmployeeJobBuilder();

    // Implicit conversion operator
    public static implicit operator Employee(EmployeeBuilder builder)
    {
      return builder.employee;
    }
  }
  public class EmployeeAddressBuilder : EmployeeBuilder
  {

    public EmployeeAddressBuilder SetStreetName(string streetName)
    {
      employee.StreetName = streetName;
      return this;
    }

    public EmployeeAddressBuilder SetPostCode(string postCode)
    {
      employee.PostCode = postCode;
      return this;
    }
  }

  public class EmployeeJobBuilder : EmployeeBuilder
  {
    public EmployeeJobBuilder SetSalary(int salary)
    {
      employee.Salary = salary;
      return this;
    }

    public EmployeeJobBuilder SetTitle(string title) {
      employee.Title = title;
      return this;
    }
  }
  #endregion
}

