# Aex_task
Запрос и метод для получения данных
<Br>Запрос:
<p><code>SELECT c.NAME AS Customer, m.NAME AS Manager, SUM(o.AMOUNT) AS Amount
        FROM CUSTOMERS c INNER JOIN MANAGERS m ON c.MANAGERID = m.ID
        INNER JOIN ORDERS o ON c.ID = o.CUSTOMERID
        WHERE o.DATE >= @beginDate AND o.DATE <= @currentDate
        GROUP BY c.ID, c.NAME, m.NAME
        HAVING SUM(o.AMOUNT) > @sumAmount;
</code></p>

<Br/> Метод GetCustomers (Service\CustomersService.cs)

<p><code>public  List<CustomerViewModel> GetCustomers(DateTime beginDate, decimal sumAmount)
        {
            DateTime currentDate = DateTime.Now;
            var customers = context.Customers
                .Where(c => c.Orders.Any(o => o.Date >= beginDate && o.Date <= currentDate))
                .Where(c => c.Orders.Sum(o => o.Amount) > sumAmount)
                .Select(c => new CustomerViewModel
                {
                    CustomerName = c.Name,
                    ManagerName = c.Manager.Name,
                    Amount = c.Orders.Sum(o => o.Amount)
                })
                .ToList();
            return customers;
        }</code></p>
