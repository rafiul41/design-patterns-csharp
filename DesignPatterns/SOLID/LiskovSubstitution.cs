namespace LiskovSubstitution
{
  class LiskovSubstitutionExample
  {
    public void ExecuteDemo()
    {
      Rectangle rc = new Rectangle();
      rc.Width = 3;
      rc.Height = 2;

      Console.WriteLine($"Rectangle has area {getAreaOfRectangle(rc)}");

      // Square sq = new Square();
      // It is perfectly legal to replace the Square on the left hand side in the above statement with Rectangle
      // Since Square is a rectangle
      // But this gives the area as 0, breaking the liskov substitution principle
      // which states if subtype is replaced with a base type then we will get the same result, but we didn't
      // For getting the same result we need to override the properties of the parent
      Rectangle sq = new Square();
      sq.Width = 4; // If parent properties not overridden then we are setting only the width of a rectangle if sq is treated as rectangle on the left hand side
      // Since square is also a rectangle we can use square as the argument of the getAreaOfRectangle function
      Console.WriteLine($"Square has area {getAreaOfRectangle(sq)}");
    }

    public int getAreaOfRectangle(Rectangle rc)
    {
      return rc.Height * rc.Width;
    }
  }

  class Rectangle
  {
    // To override the parent class we need to use virtual keyword
    public virtual int Width { get; set; }
    public virtual int Height { get; set; }
  }

  class Square : Rectangle
  {
    // We are using override keyword instead of new 
    public override int Width
    {
      set { base.Height = base.Width = value; }
    }

    public override int Height
    {
      set { base.Height = base.Width = value; }
    }
  }
}

