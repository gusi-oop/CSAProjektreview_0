using System;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using CSAProjectReview;
using Dapper;


try
{
    SqlConnectionStringBuilder sql_builder = new SqlConnectionStringBuilder();

    sql_builder.DataSource = "csa-app-server.database.windows.net";
    sql_builder.UserID = "csa-admin";
    sql_builder.Password = "MininGerzSchernus2022!";
    sql_builder.InitialCatalog = "csa-app-db";

    using (IDbConnection connection = new SqlConnection(sql_builder.ConnectionString))
    {
        Console.WriteLine("\nQuery data example:");
        Console.WriteLine("=========================================\n");

        connection.Open();

        ReadMaxTemperatur(connection);
        string stadt = "Berlin";
        ReadTemperaturUnter3(connection, stadt);
        ZeigeLieblingsstadt(connection);
        ZeigeAlleStaedte(connection);
      
        //String sql = "SELECT name, collation_name FROM sys.databases";

        //using (SqlCommand command = new SqlCommand(sql, connection))
        //{
        //    using (SqlDataReader reader = command.ExecuteReader())
        //    {
        //        while (reader.Read())
        //        {
        //            Console.WriteLine("{0} {1}", reader.GetString(0), reader.GetString(1));
        //        }
        //    }
        //}

    }
}
catch (SqlException e)
{
    Console.WriteLine(e.ToString());
}
Console.WriteLine("\nDone.");

var builder = WebApplication.CreateBuilder(args);

    // Add services to the container.

    builder.Services.AddControllers();
    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();

    var app = builder.Build();

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseHttpsRedirection();
    app.UseRouting();

    app.UseAuthorization();

    app.MapControllers();

    app.Run();


static void ReadMaxTemperatur(IDbConnection connection)
{
    List<WeatherForecast> temperatures = new List<WeatherForecast>();    
    string queryString = "SELECT * from WetterHistorie where Temperatur = (Select MAX(Temperatur) from WetterHistorie)";
    temperatures = connection.Query<WeatherForecast>(queryString).ToList();
    if (temperatures.Count != 0)
    {
        foreach (var item in temperatures)
        {
            if(item.Stadt != null)
            {
                Console.WriteLine("Die höchste Temperatur: ");
                Console.WriteLine(item.Stadt);
                Console.WriteLine(item.Datum);
                Console.WriteLine(item.Temperatur + " °C");
                Console.WriteLine("=========================================\n");
            }            
        }
    }
}

static void ReadTemperaturUnter3(IDbConnection connection, string stadt)
{
    string queryString = GetSQLPrepareStadt("WetterHistorie", stadt) + " and Temperatur < 3";
    List<WeatherForecast> temperatures = new List<WeatherForecast>();
    temperatures = connection.Query<WeatherForecast>(queryString).ToList();
    if (temperatures.Count != 0)
    {
        Console.WriteLine("Die Temperatur unter 3°C für " + stadt + ":");
        foreach (var item in temperatures)
        {
            Console.WriteLine(item.Datum.Day + "." + item.Datum.Month + "." + item.Datum.Year + ", " + item.Temperatur + " °C");
        }
        Console.WriteLine("=========================================\n");
    }
    CheckAufrufe(connection, stadt);
    ReadAufrufe(connection, stadt);
}

static void ReadAufrufe(IDbConnection connection, string stadt)
{
    string queryString = GetSQLPrepareStadt("AufrufStatistik", stadt);
    List<AufrufStatistik> staedte = new List<AufrufStatistik>();
    staedte = connection.Query<AufrufStatistik>(queryString).ToList();
    foreach (var item in staedte)
    {
        Console.WriteLine("Aufrufe von " + stadt + ": " + item.Aufrufe);
        Console.WriteLine("=========================================\n");
    }
}

static string GetSQLPrepareStadt(string table, string stadt)
{
    return "SELECT * FROM " + table + " WHERE Stadt = '" + stadt + "'";
}

static void ZeigeAlleStaedte(IDbConnection connection)
{
    String queryString = "SELECT Stadt FROM WetterHistorie GROUP BY Stadt";
    List<WeatherForecast> staedte = new List<WeatherForecast>();
    staedte = connection.Query<WeatherForecast>(queryString).ToList();
    if(staedte.Count > 0)
    {
        Console.WriteLine("Die Städte: ");
        foreach (var item in staedte)
        {
            if (item.Stadt != null)
            {
                Console.WriteLine(item.Stadt);
            }
        }
    }
    else Console.WriteLine("Es gibt keine Städte");
    Console.WriteLine("\n=========================================\n");
}

static void ZeigeLieblingsstadt(IDbConnection connection)
{
    string queryString = "Select Stadt from AufrufStatistik where Aufrufe = (SELECT MAX(Aufrufe) from AufrufStatistik)";
    List<AufrufStatistik> staedte = new List<AufrufStatistik>();
    staedte = connection.Query<AufrufStatistik>(queryString).ToList();
    foreach (var item in staedte)
    {
        if (item.Stadt != null)
        {
            Console.WriteLine("Die Lieblingsstadt ist " + item.Stadt);
            Console.WriteLine("=========================================\n");
        }
    }
}

static void CheckAufrufe(IDbConnection connection, string stadt)
{
    string queryString = GetSQLPrepareStadt("AufrufStatistik", stadt);
    List <AufrufStatistik> aufrufe = new List<AufrufStatistik>();
    aufrufe = connection.Query<AufrufStatistik>(queryString).ToList();
    foreach(var item in aufrufe)
    {
        if (item.Aufrufe == 0)
        {
            queryString = "INSERT INTO AufrufStatistik VALUES ('" + stadt + "', 1)";
        }
        else
        {
            queryString = "UPDATE Aufrufstatistik SET Aufrufe = Aufrufe + 1 WHERE Stadt = '" + stadt + "'";
        }
    }
    aufrufe = connection.Query<AufrufStatistik>(queryString).ToList();
}