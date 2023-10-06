// See https://aka.ms/new-console-template for more information
using ConsoleApp1.Connections;
using ConsoleApp1.Contexts;
using ConsoleApp1.Models;
using Microsoft.EntityFrameworkCore;

Console.WriteLine("Hello, World!");

string connectionString = "Server=localhost;Database=Test;Trusted_Connection=True;TrustServerCertificate=True";
var Efcon = new TSQLConnection(connectionString);

DateTime start = new DateTime(2023,1,1);
//Efcon.AddData();
await Efcon.GetCustomers(start, 1000);

