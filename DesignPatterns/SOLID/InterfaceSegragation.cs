namespace InterfaceSegregation
{
  public class InterfaceSegregationExample
  {
    public void ExecuteDemo()
    {
      var newPrinter = new NewPrinter();
      newPrinter.Scan();
    }
  }

  // this interface has both print, scan and photocopy functionality
  // But if we have an object that can only print and scan then after implementing this interface
  // we would have a functionality Photocopy which we will not use.
  // Thus interfaces should be segregated into it's atomic functionalities
  interface IMultifunctionalDevice
  {
    void Print();
    void Scan();
    void Photocopy();
  }

  interface IPrinter
  {
    void Print();
  }

  interface IScanner
  {
    void Scan();
  }

  interface IPhotocopier
  {
    void Photocopy();
  }

  // After segregating the interfaces we can now combine functionalities to make parent interfaces like below:
  // If we have object that can Print and Scan

  // If we would have implemented the IMultifunctionalDevice interface then we would get an exception if we called the photocopy method
  // But following is more feasible since not dependant on any methods that are not used
  interface IPrinterAndScanner : IPrinter, IScanner
  {
  }

  class NewPrinter : IPrinterAndScanner
  {
    public void Print()
    {
      Console.WriteLine("Printing Documents");
    }

    public void Scan()
    {
      Console.WriteLine("Scanning Documents");
    }
  }
}

