namespace FactoryMethod
{
  public class FactoryMethodExample
  {
    public void ExecuteDemo()
    {
      // If no factory method was used we would have to use enum - cartesian, polar
      // Now, no enum is necessary since point types are embedded in the method names
      var cartesianPoint = Point.Factory.NewCartesianPoint(1, 2);
      var polarPoint = Point.Factory.NewPolarPoint(2, 2);

      Console.WriteLine(cartesianPoint);
      Console.WriteLine(polarPoint);
    }
  }

  public class Point
  {
    private double A { get; set; }
    private double B { get; set; }

    public override string ToString()
    {
      return $"A = {A}, B = {B}";
    }

    // We are making the constructor private as will be using factory
    private Point(double x, double y)
    {
      A = x;
      B = y;
    }

    // The best approach is to keep the factory inside
    // otherwise if it is kept outside then the constructor would have to be public
    // thus anyone could create a point
    public static class Factory
    {
      // Factory method helps us in the following
      // Since both type of point use 2 double parameters we cannot overload the constructor
      // No switch statements necessary for point types --> no optional parameters in constructor
      // Parameters well defined for each point types --> if single constructor was used then if we use x in parameter this will be rad in polar
      public static Point NewCartesianPoint(double x, double y)
      {
        return new Point(x, y);
      }
      public static Point NewPolarPoint(double rad, double theta)
      {
        return new Point(rad * Math.Cos(theta), rad * Math.Sin(theta));
      }
    }
  }
}