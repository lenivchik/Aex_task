# Aex_task
Запрос и метод для получения данных
<Br>Запрос (вывод имен Customer и их Manager, которые сделали покупок на общую сумму больше 1000 с 01.01.2023):
<p><code>       SELECT c.NAME AS Customer, m.NAME AS Manager, SUM(o.AMOUNT) as Amount
        FROM CUSTOMERS c
        INNER JOIN MANAGERS m ON c.MANAGERID = m.ID
        INNER JOIN ORDERS o ON c.ID = o.CUSTOMERID
        WHERE o.DATE >= '2023-01-01'
        GROUP BY c.ID, c.NAME, m.NAME
        HAVING SUM(o.AMOUNT) > 1000;
</code></p>

<Br/> Метод GetCustomers (Service\CustomersService.cs) (возвращает List<CustomerViewModel>, где покупок на общую сумму больше sumAmount в промежуток с beginDate до текущего времени.) 

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
