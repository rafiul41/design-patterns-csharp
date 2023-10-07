using System.Diagnostics;

namespace SingleResponsibility
{
  public class SingleResponsibilityExample
  {
    public void ExecuteDemo()
    {
      var journal = new Journal();
      journal.AddEntry("I am Rafi");
      journal.AddEntry("I am starting to learn design patterns");

      Console.WriteLine(journal.ToString());
      
      var fileName = @"G:\design-pattern-example-journal.txt"; // check this file to see if journal entries are written
      var persistance = new Persistance();
      persistance.Persist(journal, fileName, true);
    }
  }

  // We have a journal class where we can add entries and remove them at any index
  // If we want to persist the journal to a file
  // if we add functionality of persistance to the journal class then
  // it will have more than one reason to change - storing journal and persistance functionality
  public class Journal
  {
    private readonly List<string> entries = new List<string>();
    public static int count = 0;

    public void AddEntry(string entry)
    {
      entries.Add($"{++count}: {entry}");
    }

    public void RemoveEntryAt(int ind)
    {
      entries.RemoveAt(ind);
    }

    public override string ToString()
    {
      return string.Join(Environment.NewLine, entries);
    }
  }

  // persistance functionality is written in the Persistance class
  public class Persistance {
    public void Persist(Journal journal, string fileName, bool overwrite = false) {
      if(overwrite || !File.Exists(fileName)) {
        File.WriteAllText(fileName, journal.ToString());
      }
    }
  }
}

