public DataTable GetCustomerInfo(string id,DateTime? startdate = null, DateTime? endDate=null)
{
    var dt = new DataTable();
    using (var conn = new SqlConnection("...")) // Connection string is hardcoded
    {
        conn.Open();
        var sql = (startDate.HasValue, endDate.HasValue) switch
            {
                (true, true) => 
                    "SELECT * FROM Customers WHERE Id = '" + Id + "' AND CreatedDate >= '" + startDate.Value + "' AND CreatedDate <= '" + endDate.Value + "'",
                
                (true, false) => 
                    "SELECT * FROM Customers WHERE Id = '" + Id + "' AND CreatedDate >= '" + startDate.Value + "'",
                
                (false, true) => 
                    "SELECT * FROM Customers WHERE Id = '" + Id + "' AND CreatedDate <= '" + endDate.Value + "'",
                
                (false, false) => 
                    "SELECT * FROM Customers WHERE Id = '" + Id + "'"
            };
        using (var da = new SqlDataAdapter(sql, conn))
        {
            da.Fill(dt);
        }
            

    }
    return dt;
}