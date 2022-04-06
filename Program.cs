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



        
    
