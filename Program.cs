using System;
using System.Data.SqlClient;
using System.Text;

try
{
    SqlConnectionStringBuilder sql_builder = new SqlConnectionStringBuilder();

        sql_builder.DataSource = "csa-app-server.database.windows.net"; 
        sql_builder.UserID = "csa-admin";            
        sql_builder.Password = "MininGerzSchernus2022!";     
        sql_builder.InitialCatalog = "csa-app-db";

        using (SqlConnection connection = new SqlConnection(sql_builder.ConnectionString))
        {
            Console.WriteLine("\nQuery data example:");
            Console.WriteLine("=========================================\n");
            
            connection.Open();
            ReadMaxTemperatur(connection);
            string stadt;
            stadt = "Berlin";
            ReadTemperaturUnter3(connection, stadt);
            ZeigeLieblingsstadt(connection);
            ZeigeAlleStaedte(connection);

        String sql = "SELECT name, collation_name FROM sys.databases";

            using (SqlCommand command = new SqlCommand(sql, connection))
            {
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Console.WriteLine("{0} {1}", reader.GetString(0), reader.GetString(1));
                    }
                }
            }  
                           
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

app.UseAuthorization();

app.MapControllers();

app.Run();

static void ReadMaxTemperatur(SqlConnection connection)
{
    string queryString = "SELECT * from WetterHistorie where Temperatur = (Select MAX(Temperatur) from WetterHistorie)";
    SqlCommand com = new SqlCommand(queryString, connection);
    using (SqlDataReader reader = com.ExecuteReader())
    {
        while (reader.Read())
        {
            Console.WriteLine("Die höchste Temperatur: ");
            Console.WriteLine(String.Format("{0},{1},{2}", reader[0], reader[1], reader[2]));
            Console.WriteLine("=========================================\n");
        }
    }
}

static void ReadTemperaturUnter3(SqlConnection connection, string stadt)
{
    string queryString = "Select * from WetterHistorie where Stadt = '" + stadt + "' and Temperatur < 3";
    SqlCommand com = new SqlCommand(queryString, connection);

    using (SqlDataReader reader = com.ExecuteReader())
    {
        while (reader.Read())
        {
            Console.WriteLine("Die Temperatur unter 3°C für Berlin:");
            Console.WriteLine(String.Format("{0}, {1} °C", reader[1], reader[2]));
            Console.WriteLine("=========================================\n");
        }
    }

    CheckAufrufe(connection, stadt);
    ReadAufrufe(connection, stadt);
}

static void TempMonat(SqlConnection connection, string stadt )
{

}

static void ZeigeAlleStaedte(SqlConnection connection)
{
    String queryString = "SELECT Stadt FROM WetterHistorie GROUP BY Stadt";
    SqlCommand com = new SqlCommand(queryString, connection);
    Console.WriteLine("Die Städte: ");
    using (SqlDataReader reader = com.ExecuteReader())
    {        
        while (reader.Read())
        {
            Console.WriteLine(reader[0]);
        }
    }
    Console.WriteLine("\n=========================================\n");
}

static void ZeigeLieblingsstadt(SqlConnection connection)
{
    string queryString = "Select Stadt from AufrufStatistik where Aufrufe = (SELECT MAX(Aufrufe) from AufrufStatistik)";
    SqlCommand com = new SqlCommand(queryString, connection);

    using (SqlDataReader reader = com.ExecuteReader())
    {
        while (reader.Read())
        {
            Console.WriteLine("Die Lieblingsstadt ist " + reader[0]);
            Console.WriteLine("=========================================\n");
        }
    }
}

static void CheckAufrufe(SqlConnection connection, string stadt)
{
    int count = 0;

    String queryString = "SELECT COUNT(*) FROM AufrufStatistik WHERE Stadt ='" + stadt + "'";
    SqlCommand com = new SqlCommand(queryString, connection);
    count = (int)com.ExecuteScalar();

    if (count == 0)
    {
        queryString = "INSERT INTO AufrufStatistik VALUES ('" + stadt + "', 1)";
    }
    else
    {
        queryString = "UPDATE Aufrufstatistik SET Aufrufe = Aufrufe + 1 WHERE Stadt = '" + stadt + "'";
    }
    com = new SqlCommand(queryString, connection);
    com.ExecuteNonQuery();

}

static void ReadAufrufe(SqlConnection connection, string stadt)
{
    string queryString = "SELECT * FROM AufrufStatistik";
    SqlCommand com = new SqlCommand(queryString, connection);

    using (SqlDataReader reader = com.ExecuteReader())
    {
        while (reader.Read())
        {
            Console.WriteLine("Aufrufe von " + stadt + ": " + reader[1]);
            Console.WriteLine("=========================================\n");
        }
    }
}





