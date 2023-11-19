using Builder;
using DependencyInversion;
using InterfaceSegregation;
using LiskovSubstitution;
using OpenClosed;
using Practice;
using SingleResponsibility;

Console.WriteLine("SOLID Principles Start ----------->");

Console.WriteLine("Single Responsibility Demo ---->");
var singleResponsibilityExample = new SingleResponsibilityExample();
singleResponsibilityExample.ExecuteDemo();

Console.WriteLine("Open-closed Demo ---->");
var openClosedExample = new OpenClosedExample();
openClosedExample.ExecuteDemo();

Console.WriteLine("Liskov Substitution Demo ---->");
var liskovSubstitutionExample = new LiskovSubstitutionExample();
liskovSubstitutionExample.ExecuteDemo();

Console.WriteLine("Interface Segregation Demo ---->");
var interfaceSegregationExample = new InterfaceSegregationExample();
interfaceSegregationExample.ExecuteDemo();

Console.WriteLine("Dependency Inversion Demo ---->");
var dependencyInversionExample = new DependencyInversionExample();
dependencyInversionExample.ExecuteDemo();

Console.WriteLine("Builder Demo ---->");
var builderExample = new BuilderExample();
builderExample.ExecuteDemo();

Console.WriteLine("Practice Demo ---->");
var practiceExample = new PracticeExample();
practiceExample.ExecuteDemo();
