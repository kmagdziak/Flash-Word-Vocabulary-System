using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Data;


namespace wordLib
{
    /***********************************************************
	    *  CLASS: Word
	    *  
	    *  NOTES: Contains all of the properties of a Word class 
	    *         object and their accessors.
    ***********************************************************/ 
    public class Word
    {
	    private string spelling;    //The correctly spelled word
	    private string meaning;     //The word's definition
	    private string sample;      //The word used in a sample sentence

	    public string Spelling
	    {
		    get { return spelling; }
		    set { spelling = value; }
	    }

	    public string Meaning
	    {
		    get { return meaning; }
		    set { meaning = value; }
	    }

	    public string Sample
	    {
		    get { return sample; }
		    set { sample = value; }
	    }
    }

    public class Database
    {
//        public const string connstr = "server=10.158.56.48;uid=net10;pwd=dtbz10;database=notebase10;";
// new string must be declared. no longer have access to db
        public const string connstr = "declare new string";
        SqlConnection conn = null;
	    SqlCommand cmd = new SqlCommand();
	    SqlDataAdapter query = new SqlDataAdapter();

        public Database()
        {
            if (!this.checkTable("Allword"))     //check if the table exists
            {
                this.CreateTable("Allword");    //call create table if it does not

                Word newWord = new Word();
                // insert 10 entries
                for (int i = 0; i < 11; i++)
                {
                    newWord.Spelling = "Phone" + i;
                    newWord.Meaning = "Telephone";
                    newWord.Sample = "what is phone";
                }
            }

            if (!this.checkTable("Flashword"))
            {
                this.CreateTable("Flashword");
            }
        }

        /****************************************************************************
        * CheckTable
        * 
        * args: string
        * 
        * returns: boolean value for true or false
        * 
        * function: See if the table exsits
        * ************************************************************************/
        private bool checkTable(string tableName)
        {
            using (conn = new SqlConnection(connstr))
            {
                conn.Open();

                String[] restrictions = new string[4];
                restrictions[2] = tableName; // the 3rd position indicates table name
                DataTable res = conn.GetSchema("Tables", restrictions);     //check the database schema

                if (res.Rows.Count == 0)
                {
                    // safe to make table
                    return false;
                }

                return true;        //true if it exsits
            }
        }

        /************************************************************************
       * CreateTable
       * 
       * args: string
       * 
       * returns: nothing
       * 
       * function: make a table in the database
       * ************************************************************************/
        private void CreateTable(string tablename)
        {
            using (conn = new SqlConnection(connstr))
            {
                conn.Open();
                cmd.Connection = conn;                                                      //call createtable command 
                cmd.CommandText = "CREATE TABLE " + tablename + " (ID int NOT NULL identity(1,1) PRIMARY KEY, Spelling VARCHAR(50), Meaning VARCHAR(250), Example VARCHAR(250), TimeCreated datetime, TimeUpdated datetime);";

                cmd.ExecuteNonQuery();          //execute the non query
            }
        }

        /**************************************************************************
	    *SearchAllWord
	    * 
	    * args: string
	    * 
	    * returns: word
	    * 
	    * function: return a word from a query into a data from the interface
	    * ************************************************************************/
        public Word SearchAllWord(string spelling)
        {
            using (conn)        //open the connection
            {
                conn = new SqlConnection(connstr);
                cmd = new SqlCommand("SELECT spelling, meaning, example FROM Allword where Spelling = '" + spelling + "';", conn);   //query the database
                conn.Open();

                SqlDataReader reader = cmd.ExecuteReader();     //execute the query
                Word word = new Word();                         //set new value for word

                if (reader.Read())
                {
                    word.Spelling = reader.GetString(0);        //get value to string
                    word.Meaning = reader.GetString(1);
                    word.Sample = reader.GetString(2);
                }
                else
                {
                    return null;                                //return null if empty
                }

                return word;                                    //word if not
            }
        }

        /**************************************************************************
        * InsertAllWord 
        * 
        * args: 3 strings
        * 
        * returns: nothing
        * 
        * function: insert all words into the interface
        * ************************************************************************/
        public void InsertAllword(string spelling, string meaning, string example)
        {
            try
            {
                /*if (string.spelling = " ")
                {
                    throw null;
                }*/

                using (conn)   //open the connection
                {
                    conn = new SqlConnection(connstr);
                    cmd = new SqlCommand("SELECT spelling FROM Allword where Spelling = '" + spelling + "';", conn);   //query the database
                    conn.Open();

                    SqlDataReader reader = cmd.ExecuteReader();     //execute query

                    if (reader.HasRows == false)        //check for rows
                    {
                        reader.Close();
                        cmd = new SqlCommand();
                        cmd.Connection = conn;
                        cmd.CommandText = "INSERT INTO Allword VALUES (@Spelling, @Meaning, @Example, @TimeCreated, @TimeUpdated)";     //insert into database
                        cmd.Parameters.AddWithValue("@Spelling", spelling);         //meaningful values
                        cmd.Parameters.AddWithValue("@Meaning", meaning);
                        cmd.Parameters.AddWithValue("@Example", example);
                        cmd.Parameters.AddWithValue("@TimeCreated", DateTime.Now);
                        cmd.Parameters.AddWithValue("@TimeUpdated", DateTime.Now);
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.ToString());
            }
        }

        /**************************************************************************
        * UpdateAllWord
        * 
        * args: string
        * 
        * returns: nothing
        * 
        * function: update a flash word from the database with a call from the 
        *          interface
        * ************************************************************************/
        public void UpdateAllWord(string spelling, string meaning, string sample)
        {
            using (conn = new SqlConnection(connstr))
            {
                conn.Open();

                cmd = new SqlCommand();
                cmd.Connection = conn;
                cmd.CommandText = "UPDATE Allword SET Meaning = @Meaning, Example = @Example WHERE Spelling = @Spelling;";      //update command from the database
                cmd.Parameters.AddWithValue("@Spelling", spelling);                                                             //with these values
                cmd.Parameters.AddWithValue("@Meaning", meaning);
                cmd.Parameters.AddWithValue("@Example", sample);
                cmd.ExecuteNonQuery();
            }
        }

        /**************************************************************************
         * DeleteAllWord
         * 
         * args: 
         * 
         * returns:
         * 
         * function: 
         *************************************************************************/
        public void DeleteAllWord(string spelling)
        {
            using (conn)        //open the connection
            {
                conn = new SqlConnection(connstr);
                conn.Open();

                cmd = new SqlCommand();
                cmd.Connection = conn;
                cmd.CommandText = "DELETE FROM Allword WHERE Spelling = @Spelling";     //delete command for the database
                cmd.Parameters.AddWithValue("@Spelling", spelling);

                cmd.ExecuteNonQuery();          //execute it
            }
        }
    }
}