using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;

namespace ChurchRecordkeeping.DataAccess
{
    public class FundDataAccess
    {
        // chkAvailableFundName method is to check for FundName whether it is present or not  
        public static string chkAvailableFundName(string FundName)
        {
            int IsAvilabel = 0;
            //Represents a Transact-SQL transaction to be made in a SQL Server database. This class cannot be inherited
            SqlTransaction transation = null;
            try
            {    // Represents open conncetion to sql server to datatbase
                using (SqlConnection con = ConnectionManager.GetCRKSecurityConnection())
                {    //BeginTransaction commands start database transaction
                    transation = con.BeginTransaction();
                    //SqlCommand represents store procedure to execute against SQL server database
                    SqlCommand cmd = new SqlCommand();
                    //get or set the conncetion used by the instance of sql command
                    cmd.Connection = con;
                    //get or set the transaction used by the instance of sql command executes
                    cmd.Transaction = transation;
                    //Represents a parameter to a SqlCommand and optionally its mapping to DataSet columns.
                    cmd.CommandType = CommandType.StoredProcedure;

                    SqlParameter COUNT = new SqlParameter("COUNT", SqlDbType.Int);
                    //ParameterDirection specifies parameter within queries and Return value represents an return value from an operation such as stored procedure
                    COUNT.Direction = ParameterDirection.ReturnValue;
                    cmd.Parameters.Add(COUNT);
                    //get or set storeprocedure name to execute at datasource
                    cmd.CommandText = "[Fund_sp]";
                    //get the sql parameter conncetion using sqlcommand and initializes new instance of sql parametername and the value 
                    //of new system
                    cmd.Parameters.Add(new SqlParameter("@RESULT", "chkAvailableFundName"));
                    cmd.Parameters.Add(new SqlParameter("@FundName", FundName));
                    //Executes a Transact-SQL statement against the connection and returns the number of rows affected
                    cmd.ExecuteScalar();
                    //rollback transaction commit
                    cmd.Transaction.Commit();

                    IsAvilabel = Convert.ToInt32(COUNT.Value);
                }
            }
            catch (Exception ex)
            {
                ex.Message.ToString();

            }

            return IsAvilabel.ToString();
        }
        // InsertintoFund method is insert record of fund for member 
        public static int InsertintoFund(string FundName)
        {
            int noOfRowsAffected = 0;
            //Represents a Transact-SQL transaction to be made in a SQL Server database. This class cannot be inherited
            SqlTransaction transation = null;
            try
            {
                // Represents open conncetion to sql server to datatbase
                using (SqlConnection con = ConnectionManager.GetCRKSecurityConnection())
                {
                    //BeginTransaction commands start database transaction
                    transation = con.BeginTransaction();
                    //SqlCommand represents store procedure to execute against SQL server database
                    SqlCommand cmd = new SqlCommand();
                    //get or set the conncetion used by the instance of sql command
                    cmd.Connection = con;
                    cmd.Transaction = transation;

                    cmd.CommandType = CommandType.StoredProcedure;
                    //Represents a parameter to a SqlCommand and optionally its mapping to DataSet columns.
                    SqlParameter returnValue = new SqlParameter("returnVal", SqlDbType.Int);
                    //ParameterDirection specifies parameter within queries and Return value represents an return value from an operation such as stored procedure
                    returnValue.Direction = ParameterDirection.ReturnValue;
                    cmd.Parameters.Add(returnValue);
                    //get or set storeprocedure name to execute at datasource

                    cmd.CommandText = "[Fund_sp]";
                    //get the sql parameter conncetion using sqlcommand and initializes new instance of sql parametername and the value 
                    //of new system
                    cmd.Parameters.Add(new SqlParameter("@RESULT", "InsertintoFund"));
                    cmd.Parameters.Add(new SqlParameter("@FundName", FundName));
                    //Executes a Transact-SQL statement against the connection and returns the number of rows affected
                    cmd.ExecuteNonQuery();

                    noOfRowsAffected = Convert.ToInt32(returnValue.Value);
                    if (noOfRowsAffected != -1 && noOfRowsAffected != 0)
                    {
                        //rollback transaction from pending state
                        transation.Rollback();
                    }
                    else
                    {
                        //commit database transation
                        cmd.Transaction.Commit();

                    }
                }
            }
            catch (Exception ex)
            {
                ex.Message.ToString();

            }


            return noOfRowsAffected;
        }
        //Datatable represents one table memory data and Getfunddetails is method to retrive details from database in datatable
        public static DataTable getfunddetails()
        {
            //it represent table
            DataTable dt = new DataTable();
            try
            {
                // Represents open conncetion to sql server to datatbase
                using (SqlConnection con = ConnectionManager.GetCRKSecurityConnection())
                {
                    //It represents store procedure to execute against SQL server database
                    SqlCommand cmd = new SqlCommand();
                    //get or set the conncetion used by the instance of sql command
                    cmd.Connection = con;
                    cmd.CommandType = CommandType.StoredProcedure;
                    //get or set storeprocedure name to execute at datasource
                    cmd.CommandText = "[Fund_sp]";
                    //get the sql parameter conncetion using sqlcommand and initializes new instance of sql parametername and the value 
                    //of new system
                    cmd.Parameters.Add(new SqlParameter("@RESULT", "getfunddetails"));
                    //Provided a way of reading a forward stream of rows from sql server database
                    //sends command text to a conncetion and builds sql datareader using one of the behaviour rule
                    //CloseConnection using this associated datareader object is closed
                    SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                    // get the values whether SqlDataReader contain one or more rows
                    if (dr.HasRows)
                        //if the datatable contain the rows then incoming datasource is merged with existing rows.
                        dt.Load(dr);
                }
            }
            catch (Exception ex)
            {
            }
            return dt;
        }

        // DELETEDonation method is delete record of donation for Envelopenumber and Fundname
        public static int DELETEFund(int FundNumber)
        {
            int noofRowsAffected = 0;
            // Represents open conncetion to sql server to datatbase
            using (SqlConnection connection = ConnectionManager.GetCRKSecurityConnection())
            {
                try
                {   //It represents store procedure to execute against SQL server database
                    SqlCommand command = new SqlCommand("fund_sp", connection);
                    //get or set storeprocedure name to execute at datasource
                    command.CommandType = CommandType.StoredProcedure;
                    //Removes all SqlCLientParameter
                    command.Parameters.Clear();
                    //get the sql parameter conncetion using sqlcommand and initializes new instance of sql parametername and the value 
                    //of new system
                    command.Parameters.Add(new SqlParameter("@RESULT", "DELETEFund"));
                    command.Parameters.Add(new SqlParameter("@FundID", FundNumber));
                    //Executes a Transact-SQL statement against the connection and returns the number of rows affected
                    noofRowsAffected = Convert.ToInt32(command.ExecuteNonQuery());
                    noofRowsAffected = 1;
                }
                catch (Exception ex)
                {
                    if (connection != null)
                        connection.Close();
                }
            }
            return noofRowsAffected;
        }

        // getEDITFUNDDETAILS method is to edit record of fund for FundNumber
        public static SqlDataReader getEDITFUNDDETAILS(int FundNumber)
        {
            SqlDataReader dr = null;
            try
            {
                //It opens the connection.
                SqlConnection con = ConnectionManager.GetCRKSecurityConnection();
                //To execute the sql server database.
                SqlCommand cmd = new SqlCommand();
                //Get or set the connection used by the instance of the sql command.
                cmd.Connection = con;
                //Get or set the store procedure name to execute at data source
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "[Fund_sp]";
                cmd.Parameters.Add(new SqlParameter("@Result", "EDITFUNDDETAILS"));
                cmd.Parameters.Add(new SqlParameter("@FundID", FundNumber));
                dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);

            }
            catch (Exception ex)
            {

            }
            return dr;

        }

        // UpdateFundDetails method is to update record of fund for FundNumber using FundNumber,FundName
        public static int UpdateFundDetails(int FundNumber, string FundName)
        {
            int noOfRowsAffected = 0;
            SqlTransaction transation = null;
            try
            {
                using (SqlConnection con = ConnectionManager.GetCRKSecurityConnection())
                {
                    transation = con.BeginTransaction();
                    //It represents store procedure to execute against SQL server database
                    SqlCommand cmd = new SqlCommand();
                    //Get or set the connection used by the instance of the sql command.
                    cmd.Connection = con;
                    cmd.Transaction = transation;
                    //Get or set the store procedure name to execute at data source
                    cmd.CommandType = CommandType.StoredProcedure;
                    //Return the values from the database.
                    SqlParameter returnValue = new SqlParameter("returnVal", SqlDbType.Int);
                    returnValue.Direction = ParameterDirection.ReturnValue;
                    cmd.Parameters.Add(returnValue);


                    //To get or set the storeprocedure at the data base.
                    cmd.CommandText = "[Fund_sp]";
                    cmd.Parameters.Add(new SqlParameter("@RESULT", "UpdateFundDetails"));
                    cmd.Parameters.Add(new SqlParameter("@FundName", FundName));
                    cmd.Parameters.Add(new SqlParameter("@FundID", FundNumber));


                    cmd.ExecuteNonQuery();
                    //To update the rows effected from the transaction. 
                    noOfRowsAffected = Convert.ToInt32(returnValue.Value);
                    if (noOfRowsAffected != -1 && noOfRowsAffected != 0)
                    {
                        transation.Rollback();
                    }
                    else
                    {
                        //To commit the transaction.
                        cmd.Transaction.Commit();

                    }
                }
            }
            catch (Exception ex)
            {
                ex.Message.ToString();

            }


            return noOfRowsAffected;
        }

    }
}
