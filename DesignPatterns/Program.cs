﻿// See https://aka.ms/new-console-template for more information
using OpenClosed;
using SingleResponsibility;

Console.WriteLine("SOLID Principles Start ----------->");

Console.WriteLine("Single Responsibility Demo ---->");
var singleResponsibilityExample = new SingleResponsibilityExample();
singleResponsibilityExample.ExecuteDemo();

Console.WriteLine("Open-closed Responsibility Demo ---->");
var openClosedExample = new OpenClosedExample();
openClosedExample.ExecuteDemo();
