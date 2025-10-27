
using System.Data;
using System.Net.Mime;
using Microsoft.Data.SqlClient;
namespace BudgetP;

public class DbconUtility : IDisposable
{
    private string _connString;
    private SqlConnection _conn;
    private SqlCommand _comm;
    private SqlDataAdapter _adpt;
    private static IConfiguration? _configuration;

    public DbconUtility(string connString)
    {
        this._connString = connString;
        this._conn = new SqlConnection(this._connString);
        this._comm = new SqlCommand(string.Empty, this._conn)
        {
            CommandTimeout = 1800
        };
        this._adpt = new SqlDataAdapter(this._comm);
    }
    public bool OpenConn()
    {
        bool flag;
        try
        {
            if (this._conn.State == ConnectionState.Closed)
            {
                this._conn.Open();
            }
            flag = true;
        }
        catch (Exception exception1)
        {
            Exception exception = exception1;
            throw new Exception(exception.Message, exception);
        }
        return flag;
    }
    public bool CloseConn()
    {
        bool flag;
        try
        {
            if (this._conn.State == ConnectionState.Open)
            {
                this._conn.Close();
            }
            flag = true;
        }
        catch (Exception exception1)
        {
            Exception exception = exception1;
            throw new Exception("the connection closing problem" + exception.Message, exception);
        }
        return flag;
    }
    public static string GetConn(string connStringKeyName)
    {
        string connectionString;
        string str1 = GetAppSettingValueByKey(connStringKeyName);
        try
        {
            SqlConnectionStringBuilder sqlConnectionStringBuilder = new SqlConnectionStringBuilder(str1);
            connectionString = sqlConnectionStringBuilder.ConnectionString;
        }
        catch (Exception exception1)
        {
            Exception exception = exception1;
            throw new Exception(exception.Message, exception);
        }
        return connectionString;
    }
    public static string GetAppSettingValueByKey(string key)
    {
        _configuration = new ConfigurationBuilder()
            .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
            .Build();
        string value = _configuration[key]!;
        return value!;
    }

    public bool FillData(DataTable table, string commandText, bool isProcedure = false, SqlParameter[] dataParameters = null, int newCommand = 0)
    {
        bool flag;
        try
        {
            try
            {
                if (newCommand == 1)
                {
                    this._comm = new SqlCommand(string.Empty, this._conn);
                    this._adpt = new SqlDataAdapter(this._comm);
                }
                if (isProcedure)
                {
                    this._comm.CommandType = CommandType.StoredProcedure;
                }
                if (dataParameters != null)
                {
                    this._comm.Parameters.AddRange(dataParameters);
                }

                // this._comm=new sqlcomand(commandtext,_conn); // the this._comm is initialized in contractor 
                this._comm.CommandText = commandText;
                this._adpt.SelectCommand = this._comm;
                table.Clear();
                this._adpt.Fill(table);
                flag = true;
            }
            catch (Exception exception1)
            {
                Exception exception = exception1;
                throw new Exception(exception.Message, exception);
            }
        }
        finally
        {
            this._comm.CommandType = CommandType.Text;
            this._comm.Parameters.Clear();
        }
        return flag;
    }

    public bool Execute(string commandText)
    {
        bool flag;
        flag = (DbconUtility.isValidTextInput(commandText) ? this.Execute(commandText, false, null, 1) : false);
        return flag;
    }
    public static bool isValidTextInput(string stringVal)
    {
        bool flag;
        if ((!DbconUtility.validRegexInput(stringVal.Trim()) ? false : !stringVal.Trim().Contains("'or'1'='1")))
        {
            flag = (!stringVal.Trim().Replace(" ", "").Contains("'or'1'='1") ? true : false);
        }
        else
        {
            flag = false;
        }
        return flag;
    }
    public static bool validRegexInput(string stringVal)
    {
        bool flag;
        char chr = '=';
        char chr1 = ' ';
        string empty = string.Empty;
        string str = string.Empty;
        string[] strArrays = stringVal.Trim().Replace(" ", "").Split(new char[] { chr });
        if ((int)strArrays.Length > 1)
        {
            for (int i = 0; i < strArrays.Length - 2; i++)
            {
                empty = strArrays[0].Trim().Substring(strArrays[0].Length - 1);
                // some time when the string contain == then the strArrays[1] will be empty and and it will raise exception

                if (strArrays[i].Length > 0)
                {
                    str = strArrays[i].Trim().Substring(0, 1);
                    if (empty != string.Empty)
                    {
                        if ((empty != str ? false : !stringVal.Trim().Replace(" ", "").Contains("where1=1")))
                        {
                            flag = false;
                            return flag;
                        }
                    }

                    string str1 = strArrays[i].Trim();
                    string str2 = strArrays[i + 1].Trim();
                    string[] strArrays1 = str1.Split(new char[] { chr1 });
                    string[] strArrays2 = str2.Split(new char[] { chr1 });
                    if ((strArrays1.Length == 0 ? false : strArrays2.Length != 0))
                    {
                        str1 = strArrays1[(int)strArrays1.Length - 1];
                        str2 = strArrays2[0];
                        if (str1 != string.Empty)
                        {
                            if ((str1 != str2 ? false : !stringVal.Trim().Replace(" ", "").Contains("where1=1")))
                            {
                                flag = false;
                                return flag;
                            }
                        }
                    }
                }

            }
        }
        string str3 = "\"or\"\"=\"";
        stringVal.Trim().Replace(" ", "");
        if (stringVal.ToLower().Trim().Replace(" ", "").Contains(";insert"))
        {
            flag = false;
        }
        else if (stringVal.ToLower().Trim().Replace(" ", "").Contains(";exec"))
        {
            flag = false;
        }
        else if (stringVal.ToLower().Trim().Replace(" ", "").Contains(";drop"))
        {
            flag = false;
        }
        else if (stringVal.ToLower().Trim().Replace(" ", "").Contains(";dropdatabase"))
        {
            flag = false;
        }
        else if (stringVal.ToLower().Trim().Replace(" ", "").Contains(";update"))
        {
            flag = false;
        }
        else if (stringVal.ToLower().Trim().Contains("--"))
        {
            flag = false;
        }
        else if (stringVal.Trim().Replace(" ", "").Contains("'or'1'='1"))
        {
            flag = false;
        }
        else if (stringVal.ToLower().Trim().Replace(" ", "").Contains("or1=1"))
        {
            flag = false;
        }
        else if (!stringVal.ToLower().Trim().Replace(" ", "").Contains("or'1=1'"))
        {
            flag = (!stringVal.ToLower().Trim().Replace(" ", "").Contains(str3) ? true : false);
        }
        else
        {
            flag = false;
        }
        return flag;
    }

    public bool Execute(string commandText, bool isProcedure, SqlParameter[] dataParameters, byte withSetDateFormat)
    {
        bool flag;

        // try
        // {
        try
        {
            if (!isValidTextInput(commandText))
            {
                throw new Exception("SQL Injection threat, try to check your input.");
            }
            if (isProcedure)
            {
                this._comm.CommandType = CommandType.StoredProcedure;
            }
            if (dataParameters != null)
            {
                this._comm.Parameters.AddRange(dataParameters);
            }
            if (withSetDateFormat != 1)
            {
                this._comm.CommandText = commandText;
            }
            else
            {
                this._comm.CommandText = string.Concat("set dateformat mdy ", commandText);
            }
            if (this._conn.State == ConnectionState.Closed)
            {
                this._conn.Open();
            }
            this._comm.ExecuteNonQuery();
            if (this._conn.State == ConnectionState.Open)
            {
                this._conn.Close();
            }
            flag = true;
        }
        catch (SqlException sqlException1)
        {
            SqlException sqlException = sqlException1;
            throw new Exception(this.GetExceptionMessage(sqlException), sqlException);
        }
        // catch (Exception exception1)
        // {
        //     Exception exception = exception1;
        //     throw new Exception(exception.Message, exception);
        // }
        // }
        finally
        {
            this._comm.CommandType = CommandType.Text;
            this._comm.Parameters.Clear();

        }
        return flag;
    }
    private string GetExceptionMessage(SqlException exception)
    {
        string foreignKeyExceptionMessage;
        if (exception.Number != 547)
        {
            foreignKeyExceptionMessage = (exception.Number != 2627 ? exception.Message : this.GetUniqueExceptionMessage(exception.Message));
        }
        else
        {
            foreignKeyExceptionMessage = this.GetForeignKeyExceptionMessage(exception.Message);
        }
        return foreignKeyExceptionMessage;
    }
    private string GetForeignKeyExceptionMessage(string message)
    {
        string str;
        string str1 = message.Split(new string[] { "', table '", "', column '" }, StringSplitOptions.None)[1];
        string tableDescription = this.GetTableDescription(str1);
        str = (tableDescription == str1 ? string.Concat("The record is being used by another object.\n\n", message) : string.Concat("The record is being used by one or more '", tableDescription, "'.\n\n", message));
        return str;
    }
    public string GetTableDescription(string tableName)
    {
        string str;
        try
        {
            string str1 = string.Concat("SELECT Description FROM tblAppTables WHERE TableName='", tableName, "'");
            this._comm.CommandText = str1;
            object obj = this._comm.ExecuteScalar();
            str = ((obj == null ? false : !Convert.IsDBNull(obj)) ? Convert.ToString(obj) : tableName);
        }
        catch (Exception exception1)
        {
            Exception exception = exception1;
            throw new Exception(exception.Message, exception);
        }
        return str;
    }
    private string GetUniqueExceptionMessage(string message)
    {
        string columnDescription;
        string str;
        try
        {
            string str1 = message.Split(new string[] { " KEY constraint '", "'. Cannot insert duplicate key in object '" }, StringSplitOptions.None)[1];
            DataTable dataTable = new DataTable();
            string str2 = string.Concat("select syscolumns.Name, syscolumns.Id from syscolumns inner join sysindexkeys on syscolumns.id=sysindexkeys.id and syscolumns.colid=sysindexkeys.colid inner join sysindexes on sysindexkeys.id=sysindexes.id and sysindexkeys.indid=sysindexes.indid where sysindexes.name='", str1, "'");
            this.FillData(dataTable, str2);
            if (dataTable.Rows.Count != 0)
            {
                int num = Convert.ToInt32(dataTable.Rows[0]["Id"]);
                string str3 = Convert.ToString(dataTable.Rows[0]["Name"]);
                columnDescription = this.GetColumnDescription(this.GetDataObjectName(num), str3);
            }
            else
            {
                columnDescription = "Record";
            }
            str = string.Concat(columnDescription, " should be unique.\n\n", message);
        }
        catch (Exception exception1)
        {
            Exception exception = exception1;
            throw new Exception(exception.Message, exception);
        }
        return str;
    }
    public string GetColumnDescription(string tableName, string columnName)
    {
        return this.GetColumnDescription(tableName, columnName, columnName);
    }
    public string GetColumnDescription(string tableName, string columnName, string columnCaption)
    {
        string str;
        try
        {
            str = columnName;
        }
        catch (Exception exception1)
        {
            Exception exception = exception1;
            throw new Exception(exception.Message, exception);
        }
        return str;
    }
    private string GetDataObjectName(int objectId)
    {
        string str;
        try
        {
            object value = this.GetValue("sysobjects", "name", string.Concat("id=", objectId));
            str = ((value == null ? false : !Convert.IsDBNull(value)) ? Convert.ToString(value) : string.Empty);
        }
        catch (Exception exception1)
        {
            Exception exception = exception1;
            throw new Exception(exception.Message, exception);
        }
        return str;
    }
    public object GetValue(string tableName, string fieldName, string criteria)
    {
        object obj;
        try
        {
            if (validRegexInputFillData(criteria))
            {
                string str = string.Concat(new string[] { "SELECT ", fieldName, " FROM ", tableName, " WHERE ", criteria });
                this._comm.CommandText = str;
                obj = this._comm.ExecuteScalar();
            }
            else
            {
                obj = null;
            }
        }
        catch (Exception exception1)
        {
            Exception exception = exception1;
            throw new Exception(exception.Message, exception);
        }
        return obj;
    }
    public static bool validRegexInputFillData(string stringVal)
    {
        bool flag;
        char chr = '=';
        char chr1 = ' ';
        string empty = string.Empty;
        string str = string.Empty;
        try
        {
            string[] strArrays = stringVal.Trim().Replace(" ", "").Split(new char[] { chr });
            if (strArrays == null)
            {
                flag = true;
            }
            else
            {
                if ((int)strArrays.Length > 1)
                {
                    empty = strArrays[0].Trim().Substring(strArrays[0].Length - 1);// retrieves last character of the string
                    str = strArrays[1].Trim().Substring(0, 1);
                    if (empty != string.Empty)
                    {
                        if (stringVal.Trim().Replace(" ", "").Contains("where1=1"))
                        {
                            flag = false;
                            return flag;
                        }
                    }
                    string str1 = strArrays[0].Trim();
                    string str2 = strArrays[1].Trim();
                    string[] strArrays1 = str1.Split(new char[] { chr1 });
                    string[] strArrays2 = str2.Split(new char[] { chr1 });
                    if ((strArrays1.Length == 0 ? false : strArrays2.Length != 0))
                    {
                        str1 = strArrays1[(int)strArrays1.Length - 1];
                        str2 = strArrays2[0];
                        if (str1 != string.Empty)
                        {
                            if (stringVal.Trim().Replace(" ", "").Contains("where1=1"))
                            {
                                flag = false;
                                return flag;
                            }
                        }
                    }
                }
                string str3 = "\"or\"\"=\"";
                stringVal.Trim().Replace(" ", "");
                if (stringVal.ToLower().Trim().Replace(" ", "").Contains(";exec"))
                {
                    flag = false;
                }
                else if (stringVal.ToLower().Trim().Replace(" ", "").Contains("drop"))
                {
                    flag = false;
                }
                else if (stringVal.ToLower().Trim().Replace(" ", "").Contains("insertinto"))
                {
                    flag = false;
                }
                else if (stringVal.ToLower().Trim().Contains("--"))
                {
                    flag = false;
                }
                else if (stringVal.ToLower().Trim().Replace(" ", "").Contains("dropdatabase"))
                {
                    flag = false;
                }
                else if (stringVal.ToLower().Trim().Replace(" ", "").Contains("update"))
                {
                    flag = false;
                }
                else if (stringVal.Trim().Replace(" ", "").Contains("'or'1'='1"))
                {
                    flag = false;
                }
                else if (stringVal.ToLower().Trim().Replace(" ", "").Contains("or1=1"))
                {
                    flag = false;
                }
                else if (!stringVal.ToLower().Trim().Replace(" ", "").Contains("or'1=1'"))
                {
                    flag = (!stringVal.ToLower().Trim().Replace(" ", "").Contains(str3) ? true : false);
                }
                else
                {
                    flag = false;
                }
            }
        }
        catch (Exception exception)
        {
            flag = false;
        }
        return flag;
    }

    public bool Insert(DataRow row, bool showMessage)
    {
        return this.Insert(row, showMessage, true, true);
    }
    public bool Insert(DataRow row, bool showMessage, bool validate, bool validateDatabase)
    {
        bool flag;
        if ((!validateDatabase ? true : true))
        {
            try
            {
                if ((!validate ? true : Validate(row, this._connString)))
                {
                    string query = GetQuery(1, row);
                    if (!isValidTextInput(query))
                    {
                        if (showMessage)
                        {
                            //MessageBox.Show(" Invalid characters threat!", "AmharaBank", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                        }
                        flag = false;
                    }
                    else if (!this.Execute(query))
                    {
                        if (showMessage)
                        {
                            // MessageBox.Show(" could not be added", "AmharaBank", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        }
                        flag = false;
                    }
                    else
                    {
                        if (showMessage)
                        {
                            // MessageBox.Show(" added successfully", "AmharaBank", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                        }
                        flag = true;
                    }
                }
                else
                {
                    flag = false;
                }
            }
            catch (Exception exception1)
            {
                Exception exception = exception1;
                throw new Exception(exception.Message, exception);
            }
        }
        else
        {
            flag = false;
        }
        return flag;
    }
    public static string GetQuery(int commandType, DataRow row)
    {
        string str;
        DateTime item;
        string str1;
        string str2;
        string str3;
        string empty = string.Empty;
        string empty1 = string.Empty;
        string empty2 = string.Empty;
        string empty3 = string.Empty;
        if ((commandType < 0 || commandType > 3 ? true : row == null))
        {
            str = empty;
        }
        else if (row.Table != null)
        {
            try
            {
                if (commandType != 1)
                {
                    DataColumn[] primaryKey = row.Table.PrimaryKey;
                    for (int i = 0; i < (int)primaryKey.Length; i++)
                    {
                        DataColumn dataColumn = primaryKey[i];
                        empty1 = string.Concat(empty1, " and ", dataColumn.ColumnName, " = ");
                        if (dataColumn.DataType == typeof(string))
                        {
                            empty1 = string.Concat(new object[] { empty1, "'", row[dataColumn], "'" });
                        }
                        else if (dataColumn.DataType != typeof(DateTime))
                        {
                            if (dataColumn.DataType != typeof(Guid))
                            {
                                str3 = (dataColumn.DataType != typeof(bool) ? string.Concat(empty1, row[dataColumn]) : string.Concat(empty1, Convert.ToInt32(row[dataColumn])));
                            }
                            else
                            {
                                str3 = string.Concat(new object[] { empty1, "'", row[dataColumn], "'" });
                            }
                            empty1 = str3;
                        }
                        else
                        {
                            string str4 = empty1;
                            item = (DateTime)row[dataColumn];
                            empty1 = string.Concat(str4, "'", item.ToShortDateString(), "'");
                        }
                    }
                    if (empty1 != string.Empty)
                    {
                        empty1 = empty1.Remove(0, 5);
                    }
                }
                switch (commandType)
                {
                    case 0:
                        {
                            foreach (DataColumn column in row.Table.Columns)
                            {
                                empty = string.Concat(empty, ", ", column.ColumnName);
                            }
                            if (empty != string.Empty)
                            {
                                empty = string.Concat(new string[] { "SELECT ", empty.Remove(0, 2), " FROM ", row.Table.TableName, " WHERE ", empty1 });
                            }
                            break;
                        }
                    case 1:
                        {
                            foreach (DataColumn column1 in row.Table.Columns)
                            {
                                if ((column1.AutoIncrement ? false : !column1.ReadOnly))
                                {
                                    empty2 = string.Concat(empty2, ", ", column1.ColumnName);
                                    if (Convert.IsDBNull(row[column1]))
                                    {
                                        empty3 = string.Concat(empty3, ", null");
                                    }
                                    else if ((column1.DataType != typeof(string) ? false : !Convert.IsDBNull(row[column1])))
                                    {
                                        empty3 = string.Concat(new object[] { empty3, ",N'", row[column1], "'" });
                                    }
                                    else if ((column1.DataType != typeof(DateTime) ? true : Convert.IsDBNull(row[column1])))
                                    {
                                        if (column1.DataType == typeof(Guid) && !Convert.IsDBNull(row[column1]))
                                        {
                                            str1 = string.Concat(new object[] { empty3, ", '", row[column1], "'" });
                                        }
                                        else if (column1.DataType != typeof(bool))
                                        {
                                            str1 = (column1.DataType != typeof(MediaTypeNames.Image) ? string.Concat(empty3, ", ", row[column1]) : string.Concat(new object[] { empty3, ", '", row[column1], "'" }));
                                        }
                                        else
                                        {
                                            str1 = string.Concat(empty3, ", ", Convert.ToInt32(row[column1]));
                                        }
                                        empty3 = str1;
                                    }
                                    else
                                    {
                                        string str5 = empty3;
                                        item = (DateTime)row[column1];
                                        empty3 = string.Concat(str5, ", '", item.ToShortDateString(), "'");
                                    }
                                }
                            }
                            if ((empty2 == string.Empty ? false : empty3 != string.Empty))
                            {
                                empty = string.Concat(new string[] { "INSERT INTO ", row.Table.TableName, " (", empty2.Remove(0, 2), ") VALUES (", empty3.Remove(0, 2), ")" });
                            }
                            break;
                        }
                    case 2:
                        {
                            foreach (DataColumn dataColumn1 in row.Table.Columns)
                            {
                                if ((dataColumn1.AutoIncrement || dataColumn1.ReadOnly ? false : !IsPrimaryKey(dataColumn1)))
                                {
                                    if (Convert.IsDBNull(row[dataColumn1]))
                                    {
                                        empty = string.Concat(empty, ", ", dataColumn1.ColumnName, " = null");
                                    }
                                    else if ((dataColumn1.DataType != typeof(string) ? false : !Convert.IsDBNull(row[dataColumn1])))
                                    {
                                        empty = string.Concat(new object[] { empty, ", ", dataColumn1.ColumnName, " = N'", row[dataColumn1], "'" });
                                    }
                                    else if ((dataColumn1.DataType != typeof(DateTime) ? true : Convert.IsDBNull(row[dataColumn1])))
                                    {
                                        if (dataColumn1.DataType == typeof(Guid) && !Convert.IsDBNull(row[dataColumn1]))
                                        {
                                            str2 = string.Concat(new object[] { empty, ", ", dataColumn1.ColumnName, " = '", row[dataColumn1], "'" });
                                        }
                                        else if (dataColumn1.DataType != typeof(bool))
                                        {
                                            str2 = (dataColumn1.DataType != typeof(MediaTypeNames.Image) ? string.Concat(new object[] { empty, ", ", dataColumn1.ColumnName, " = ", row[dataColumn1] }) : string.Concat(new object[] { empty, ", ", dataColumn1.ColumnName, " = '", row[dataColumn1], "'" }));
                                        }
                                        else
                                        {
                                            str2 = string.Concat(new object[] { empty, ", ", dataColumn1.ColumnName, " = ", Convert.ToInt32(row[dataColumn1]) });
                                        }
                                        empty = str2;
                                    }
                                    else
                                    {
                                        string[] columnName = new string[] { empty, ", ", dataColumn1.ColumnName, " = '", null, null };
                                        string[] shortDateString = columnName;
                                        item = (DateTime)row[dataColumn1];
                                        shortDateString[4] = item.ToShortDateString();
                                        columnName[5] = "'";
                                        empty = string.Concat(columnName);
                                    }
                                }
                            }
                            if (empty != string.Empty)
                            {
                                empty = string.Concat(new string[] { "UPDATE ", row.Table.TableName, " SET ", empty.Remove(0, 2), " WHERE ", empty1 });
                            }
                            break;
                        }
                    case 3:
                        {
                            empty = string.Concat("DELETE from ", row.Table.TableName, " WHERE ", empty1);
                            break;
                        }
                }
                str = empty;
            }
            catch (Exception exception1)
            {
                Exception exception = exception1;
                throw new Exception(exception.Message, exception);
            }
        }
        else
        {
            str = empty;
        }
        return str;
    }
    private static bool IsPrimaryKey(DataColumn col)
    {
        bool flag;
        DataColumn[] primaryKey = col.Table.PrimaryKey;
        int num = 0;
        while (true)
        {
            if (num >= (int)primaryKey.Length)
            {
                flag = false;
                break;
            }
            else if (primaryKey[num] != col)
            {
                num++;
            }
            else
            {
                flag = true;
                break;
            }
        }
        return flag;
    }
    public static bool Validate(DataRow row, string connString)
    {
        string str = CheckValues(row, connString);
        if (str != string.Empty)
        {
            throw new Exception(str);
        }
        return true;
    }
    public static string CheckValues(DataRow row, string connString)
    {
        string empty = string.Empty;
        foreach (DataColumn column in row.Table.Columns)
        {
            if ((column.AutoIncrement ? false : !column.ReadOnly))
            {
                if ((!(column.DataType == typeof(string)) || row.IsNull(column) ? false : !Convert.IsDBNull(row[column])))
                {
                    row[column] = row[column].ToString().Trim();
                }
                if ((row.IsNull(column) || Convert.IsDBNull(row[column]) ? !column.AllowDBNull : true))
                {
                    if ((row.IsNull(column) || Convert.IsDBNull(row[column]) ? !column.AllowDBNull : false))
                    {
                        empty = string.Concat(empty, " * ", GetColumnDescription(column.Table.TableName, column.ColumnName, column.Caption, connString), "\n");
                    }
                    else if ((row.IsNull(column) || !(column.DataType == typeof(string)) || !(row[column].ToString() == string.Empty) ? false : !column.AllowDBNull))
                    {
                        empty = string.Concat(empty, " * ", GetColumnDescription(column.Table.TableName, column.ColumnName, column.Caption, connString), "\n");
                    }
                    else if ((row.IsNull(column) || !(column.DataType == typeof(string)) ? false : row[column].ToString().Length > column.MaxLength))
                    {
                        string[] columnDescription = new string[] { empty, " * ", GetColumnDescription(column.Table.TableName, column.ColumnName, column.Caption, connString), " (should be less than ", null, null };
                        columnDescription[4] = column.MaxLength.ToString();
                        columnDescription[5] = " characters)\n";
                        empty = string.Concat(columnDescription);
                    }
                }
            }
        }
        if (empty != string.Empty)
        {
            empty = string.Concat("Please enter valid values for the following field(s):\n\n", empty);
        }
        return empty;
    }
    public static string GetColumnDescription(string tableName, string columnName, string columnCaption, string connString)
    {
        string columnDescription;
        DbconUtility BUdgP = new DbconUtility(connString);
        try
        {
            try
            {
                BUdgP.OpenConn();
                columnDescription = BUdgP.GetColumnDescription(tableName, columnName, columnCaption);
            }
            catch (Exception exception1)
            {
                Exception exception = exception1;
                throw new Exception(exception.Message, exception);
            }
        }
        finally
        {
            BUdgP.CloseConn();
        }
        return columnDescription;
    }

    public DataTable GetDataTable(string tableName)
    {
        return this.GetDataTable(tableName, false, null, string.Empty);
    }
    public DataTable GetDataTable(string tableName, bool isProcedure, SqlParameter[] dataParameters, string SqlSelect)
    {
        DataTable item;
        string str;
        if (validRegexInputFillData(SqlSelect))
        {
            DataSet dataSet = new DataSet()
            {
                EnforceConstraints = false
            };
            try
            {
                try
                {
                    if (!isProcedure)
                    {
                        str = (SqlSelect == string.Empty ? string.Concat("SELECT top 1 * FROM ", tableName, " WHERE 1=0") : SqlSelect);
                    }
                    else
                    {
                        str = tableName;
                        this._comm.CommandType = CommandType.StoredProcedure;
                    }
                    if (dataParameters != null)
                    {
                        this._comm.Parameters.AddRange(dataParameters);
                    }
                    this._comm.CommandText = str;
                    this._adpt.SelectCommand = this._comm;
                    this._adpt.FillSchema(dataSet, SchemaType.Source, tableName);
                    this._adpt.Fill(dataSet, tableName);
                    if (dataSet.Tables[0] != null)
                    {
                        dataSet.Tables[0].Rows.Clear();
                    }
                    item = dataSet.Tables[0];
                }
                catch (Exception exception1)
                {
                    Exception exception = exception1;
                    throw new Exception(exception.Message, exception);
                }
            }
            finally
            {
                this._comm.CommandType = CommandType.Text;
                this._comm.Parameters.Clear();
            }
        }
        else
        {
            item = null;
        }
        return item;
    }

    public bool insertProc(DataRow row)
    {

        bool flag;
        string empty = string.Empty;
        string str = string.Empty;
        string empty1 = string.Empty;
        string str1 = string.Empty;
        DataTable dataTable = new DataTable();
        SqlDataAdapter sqlDataAdapter = new SqlDataAdapter();
        SqlCommand sqlCommand = new SqlCommand();
        string empty2 = string.Empty;
        try
        {
            foreach (DataColumn column in row.Table.Columns)
            {
                if ((column.AutoIncrement ? false : !column.ReadOnly))
                {
                    empty = string.Concat(empty, ", ", column.ColumnName);
                    if (Convert.IsDBNull(row[column]))
                    {
                        if (column.DataType == typeof(DateTime))
                        {
                            str = string.Concat(str, ", @", column.ColumnName);
                            sqlCommand.Parameters.AddWithValue(string.Concat("@", column.ColumnName), SqlDbType.DateTime).Value = DBNull.Value;
                        }
                        else if (column.DataType == typeof(decimal))
                        {
                            str = string.Concat(str, ", @", column.ColumnName);
                            sqlCommand.Parameters.AddWithValue(string.Concat("@", column.ColumnName), SqlDbType.Decimal).Value = DBNull.Value;
                        }
                        else if (column.DataType != typeof(int))
                        {
                            str = string.Concat(str, ", @", column.ColumnName);
                            sqlCommand.Parameters.AddWithValue(string.Concat("@", column.ColumnName), "NULL");
                        }
                        else
                        {
                            str = string.Concat(str, ", @", column.ColumnName);
                            sqlCommand.Parameters.AddWithValue(string.Concat("@", column.ColumnName), SqlDbType.Int).Value = DBNull.Value;
                        }
                    }
                    else if ((column.DataType != typeof(string) ? false : !Convert.IsDBNull(row[column])))
                    {
                        str = string.Concat(str, ", @", column.ColumnName);
                        sqlCommand.Parameters.AddWithValue(string.Concat("@", column.ColumnName), SqlDbType.NText).Value = row[column];
                    }
                    else if ((column.DataType != typeof(DateTime) ? false : !Convert.IsDBNull(row[column])))
                    {
                        str1 = row[column].ToString();
                        str = string.Concat(str, ", @", column.ColumnName);
                        sqlCommand.Parameters.AddWithValue(string.Concat("@", column.ColumnName), SqlDbType.DateTime).Value = Convert.ToDateTime(row[column]);
                    }
                    else if (column.DataType != typeof(DateTime))
                    {
                        str = string.Concat(str, ", @", column.ColumnName);
                        sqlCommand.Parameters.AddWithValue(string.Concat("@", column.ColumnName), row[column]);
                    }
                    else
                    {
                        str = string.Concat(str, ", @", column.ColumnName);
                        sqlCommand.Parameters.AddWithValue(string.Concat("@", column.ColumnName), SqlDbType.DateTime).Value = DBNull.Value;
                    }
                }
            }
            if (empty != string.Empty)
            {
                empty1 = string.Concat(new string[] { "INSERT INTO ", row.Table.TableName, " (", empty.Remove(0, 2), ") VALUES (", str.Remove(0, 2), ")" });
                SqlConnection sqlConnection = new SqlConnection(this._connString);
                sqlCommand.Connection = sqlConnection;
                sqlConnection.Open();
                sqlCommand.CommandText = empty1;
                sqlDataAdapter.SelectCommand = sqlCommand;

                sqlCommand.ExecuteNonQuery();
                sqlConnection.Close();
            }
            flag = true;
        }
        catch (Exception exception)
        {
            flag = false;
        }
        return flag;
    }
    // update 
    public bool Update(DataRow row, bool showMessage)
    {
        return this.Update(row, showMessage, true);
    }
    public bool Update(DataRow row, bool showMessage, bool validate)
    {
        bool flag;

        try
        {
            if ((!validate ? false : !DbconUtility.Validate(row, this._connString)))
            {
                flag = false;
            }
            else if (!this.Execute(DbconUtility.GetQuery(2, row)))
            {
                if (showMessage)
                {
                    //MessageBox.Show(" could not be updated", "AmharaBank", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
                flag = false;
            }
            else
            {
                if (showMessage)
                {
                    //MessageBox.Show(" updated successfully", "AmharaBank", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                }
                flag = true;
            }
        }

        catch (Exception exception1)
        {
            Exception exception = exception1;
            throw new Exception(exception.Message, exception);
        }
        return flag;
    }


    public void Dispose()
    {
        throw new NotImplementedException();
    }
}
