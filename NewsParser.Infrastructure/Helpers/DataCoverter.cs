using System.Data;

namespace NewsParser.Infrastructure.Helpers
{
    public static class DataCoverter
    {
        public static DataTable ConvertToDataTable<T>(IEnumerable<T> data)
        {
            var properties = typeof(T).GetProperties();
            var dataTable = new DataTable();
            foreach (var property in properties)
            {
                dataTable.Columns.Add(property.Name, Nullable.GetUnderlyingType(property.PropertyType) ?? property.PropertyType);
            }
            foreach (var item in data)
            {
                var values = new object[properties.Length];
                for (var i = 0; i < properties.Length; i++)
                {
                    values[i] = properties[i].GetValue(item) ?? DBNull.Value;
                }
                dataTable.Rows.Add(values);
            }
            return dataTable;
        }
    }
}