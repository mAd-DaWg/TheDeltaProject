/* Originally coded and documented by Joshua Gatley-Dewing */

/*            Does not work under linux. database should be run with mysql and not msAccess
using System;
using System.Collections.Generic;
using System.Data.Odbc;
using System.Data;
using System.Text;

namespace Delta.Brain
{
    class Database
    {
        private string m_ConnectionString;
        private OdbcDataAdapter m_OdbcDataAdapter;
        private OdbcConnection m_OdbcConnection;
        private OdbcCommand m_OdbcCommand;
        
        public Database()
        {
            this.m_ConnectionString = @"Driver={Microsoft Access Driver (*.mdb)};DBQ=D:\TestDB.mdb";
            this.m_OdbcConnection = new OdbcConnection(this.m_ConnectionString);
            m_OdbcDataAdapter = new OdbcDataAdapter();
            m_OdbcCommand = new OdbcCommand();
        }
        
        public string[,] executeQuery(string query)
        {
            DataTable dataTable = null;
            try
            {
                this.m_OdbcConnection.Open();
                this.m_OdbcCommand.Connection = this.m_OdbcConnection;
                this.m_OdbcCommand.CommandText = query;
                this.m_OdbcDataAdapter.SelectCommand = this.m_OdbcCommand;
                DataSet dataSet = new DataSet();
                this.m_OdbcDataAdapter.Fill(dataSet);
                dataTable = dataSet.Tables[0];
            }

            catch (Exception exp)
            {
                Console.WriteLine(exp.Message);
            }
            
            finally
            {
                if (this.m_OdbcConnection.State != ConnectionState.Closed)
                {
                    this.m_OdbcConnection.Close();
                }
            }

            //convert the results into a 2D string array(output[row][coloumn])
            DataRow[] result = dataTable.Select();
                //count how many rows there are
            int rowCount = result.Length;
                //count how many coloumns there are
            string holder = result[0].ToString();
            string[] delim = {", "};
            string[] holder2 = holder.Split(delim, System.StringSplitOptions.RemoveEmptyEntries);
            int coloumnCount = holder2.Length;
                //create a 2D array using the values we just counted
            string[,] output = new string[rowCount, coloumnCount];
                //put all the results into the 2D array string array(output[row][coloumn])
            for (int i = 0; i < rowCount; i++)
            {
                holder = result[i].ToString();
                holder2 = holder.Split(delim, System.StringSplitOptions.RemoveEmptyEntries);
                for (int j = 0; j < coloumnCount; j++)
                {
                    output[i, j] = holder2[j];
                }
            }

            return output;
        }
        
        public int excecuteNonQuery(string query)
        {
            int noOfAffectedRows = -1;
            try
            {
                this.m_OdbcConnection.Open();
                this.m_OdbcCommand.Connection = this.m_OdbcConnection;
                this.m_OdbcCommand.CommandText = query;
                noOfAffectedRows = this.m_OdbcCommand.ExecuteNonQuery();
            }
            
            catch (Exception exp)
            {
                Console.WriteLine(exp.Message);
            }
            
            finally
            {
                if (this.m_OdbcConnection.State != ConnectionState.Closed)
                {
                    this.m_OdbcConnection.Close();
                }
            }

            return noOfAffectedRows;
        }
    }
}

//Example usage:

//dm.excecuteNonQuery("insert into person(name, address) values ('Ashis Saha', 'Mohammadpur, Dhaka.');");

*/

