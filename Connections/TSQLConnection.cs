using ConsoleApp1.Contexts;
using ConsoleApp1.Models;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1.Connections
{
    public class TSQLConnection
    {
        private string connectionString;

        public TSQLConnection(string connectionString)
        {
            this.connectionString = connectionString;
        }
        public void WriteList(List<CustomerViewModel> list)
        {
            foreach (var item in list)
            {
                Console.WriteLine($"{item.CustomerName} {item.ManagerName} {item.Amount}");
            }

        }
        public async Task<List<CustomerViewModel>> GetCustomers(DateTime beginDate, decimal sumAmount)
        {

            string sqlExpression = @"SELECT c.NAME AS Customer, m.NAME AS Manager, SUM(o.AMOUNT) AS Amount
                                    FROM CUSTOMERS c INNER JOIN MANAGERS m ON c.MANAGERID = m.ID
                                    INNER JOIN ORDERS o ON c.ID = o.CUSTOMERID
                                    WHERE o.DATE >= @beginDate AND o.DATE <= @currentDate
                                    GROUP BY c.NAME, m.NAME
                                    HAVING SUM(o.AMOUNT) > @sumAmount;";
            DateTime currentDate = DateTime.Now;
            var customers = new List<CustomerViewModel>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync();
                using (SqlCommand command = new SqlCommand(sqlExpression, connection))
                {
                    command.Parameters.Add("@beginDate", SqlDbType.DateTime).Value = beginDate;
                    command.Parameters.Add("@currentDate", SqlDbType.DateTime).Value = currentDate;
                    command.Parameters.Add("@sumAmount", SqlDbType.Decimal).Value = sumAmount;
                    SqlDataReader reader = await command.ExecuteReaderAsync();
                    if (reader.HasRows)
                    {                       
                        while (await reader.ReadAsync())
                        {
                            string CustomerName = reader["Customer"].ToString();
                            string ManagerName = reader["Manager"].ToString();
                            decimal Amount = (decimal)reader["Amount"];
                            customers.Add(new CustomerViewModel { CustomerName = CustomerName, ManagerName = ManagerName, Amount = Amount });
                        }
                    }
                }
            }
            return customers;
        }
    }
}

