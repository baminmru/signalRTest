using Microsoft.EntityFrameworkCore;
using System.Data;
using Npgsql;
using System.Collections.Generic;
using System;
using Newtonsoft.Json;

namespace Server.models
{


    public class ComboInfo
    {
        public string id;
        public string name;
    }

    public class myContext : DbContext
    {

        /// <summary>
        /// Defines the _serializerSettings
        /// </summary>
        private JsonSerializerSettings _serializerSettings;

        /// <summary>
        /// The serializerSettings
        /// </summary>
        /// <returns>The <see cref="JsonSerializerSettings"/></returns>
        public JsonSerializerSettings serializerSettings()
        {

            return _serializerSettings;
        }

        public myContext(DbContextOptions<myContext> options) : base(options)
        {
            
            _serializerSettings = new JsonSerializerSettings()
            {
                TypeNameHandling = TypeNameHandling.None
            };
        }



        public List<Dictionary<string, string>> GetRaw(string SQL)
        {
            List<Dictionary<string, string>> rows = new List<Dictionary<string, string>>();
            Dictionary<string, string> row;
            DataTable dt;
            NpgsqlDataAdapter da;
            NpgsqlCommand cmd = new NpgsqlCommand(SQL, (NpgsqlConnection)Database.GetDbConnection());
            cmd.CommandType = CommandType.Text;
            dt = new DataTable();
            da = new NpgsqlDataAdapter(cmd);
            try
            {

                // retrive data from db
                da.Fill(dt);

                foreach (DataRow dr in dt.Rows)
                {
                    row = new Dictionary<string, string>();
                    foreach (DataColumn col in dt.Columns)
                    {
                        if (dr[col] != DBNull.Value)
                            row.Add(col.ColumnName, dr[col].ToString());
                        else
                            row.Add(col.ColumnName,"");
                    }
                    rows.Add(row);
                }


            }
            catch (System.Exception ex)
            {
                System.Diagnostics.Debug.Print(ex.Message);
            }
            finally
            {
                dt.Dispose();
                da.Dispose();
                cmd.Dispose();
            }

            return rows;
        }

        public DataTable DoQuery(string SQL)
        {
            DataTable dt;
            NpgsqlDataAdapter da;
            NpgsqlCommand cmd = new NpgsqlCommand(SQL, (NpgsqlConnection)Database.GetDbConnection());
            cmd.CommandType = CommandType.Text;
            dt = new DataTable();
            da = new NpgsqlDataAdapter(cmd);
            Boolean my_open = false;
            try
            {
                if (cmd.Connection.State != ConnectionState.Open) { cmd.Connection.Open(); my_open = true; }
                // retrive data from db
                da.Fill(dt);

            }
            catch (System.Exception ex)
            {
                System.Diagnostics.Debug.Print(ex.Message);
            }
            finally
            {
                if (my_open && cmd.Connection.State == ConnectionState.Open) cmd.Connection.Close();
                da.Dispose();
                cmd.Dispose();
            }

            return dt;
        }


        public string DoExec(string SQL)
        {

            NpgsqlCommand cmd = new NpgsqlCommand(SQL, (NpgsqlConnection)Database.GetDbConnection());
            cmd.CommandType = CommandType.Text;
            System.Diagnostics.Debug.Print("Exec: " + SQL);
            Boolean my_open = false;
            try
            {
                if (cmd.Connection.State != ConnectionState.Open) { cmd.Connection.Open(); my_open = true; }
                cmd.ExecuteNonQuery();

            }
            catch (System.Exception ex)
            {
                System.Diagnostics.Debug.Print(ex.Message);
                return ex.Message;
            }
            finally
            {
                if (my_open && cmd.Connection.State == ConnectionState.Open) cmd.Connection.Close();
                cmd.Dispose();
            }
            return "";
        }












        public DbSet<Server.models.perfdata> perfdata { get; set; }
        public DbSet<Server.models.perfdriverinfo> perfdriverinfo { get; set; }
        public DbSet<Server.models.agentsettings> agentsettings { get; set; }
        


    }
}

