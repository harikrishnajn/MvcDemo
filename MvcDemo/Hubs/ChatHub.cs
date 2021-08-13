using System;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNet.SignalR;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Data.SqlClient;
using System.Data;
using System.Text;
using System.Configuration;
using MvcDemo.Models;

namespace MvcDemo.Hubs
{
    public class ChatHub : Hub
    {

        string sConnectionString = ConfigurationManager.ConnectionStrings["Databaseconnection"].ConnectionString;

        static ConcurrentDictionary<string, string> dic = new ConcurrentDictionary<string, string>();

        public void Send(string name, string message)
        {
            Clients.All.broadcastMessage(name, message);
        }

        public void SendToSpecific(string name, string message, string to)
        {
            // Call the broadcastMessage method to update clients.
            var date = DateTime.Now.ToString("ddd, dd MMM yyy HH’:’mm’:’ss ‘GMT’");
               // DateTime.Now.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss")
            Clients.Caller.broadcastMessage(name, message);
            Clients.Client(dic[to]).broadcastMessage(name, message);
            string create = "INSERT INTO ChatHistory (FromAddress,ToAddress,Messages,CreatedDate)  VALUES('" + name + "','" + to + "','" + message + "','"+date+"')";
            try
            {
                using (SqlConnection conn = CreateConnection(sConnectionString))
                {
                    SqlCommand com = new SqlCommand(create, conn);
                    conn.Open();
                    com.ExecuteNonQuery();
                }
            }
            catch(Exception e)
            {
                Console.WriteLine(e + " " + e.StackTrace + " " + e.Message);
            }
        }

        public   DataSet getDataTables (string TableName)
        {
            DataSet ds = new DataSet();
            List<string> list = new List<string>();
            SqlConnection conn = new SqlConnection(sConnectionString);
            string command = "SELECT TOP 0 * FROM " + TableName + "";
            conn.Open();
            SqlCommand cmd = new SqlCommand(command, conn);
            SqlDataAdapter adapter;
            //Adapter bind to query and connection object
            adapter = new SqlDataAdapter(command, conn);
            //fill the dataset
            adapter.Fill(ds);          
            conn.Close();
            return ds;
        }




        public static SqlConnection CreateConnection(string sConnectionString)
        {
            return new SqlConnection(sConnectionString);
        }
        public static void CloseConnection(SqlConnection sConnection)
        {
            if (sConnection.State != ConnectionState.Closed)
            {
                sConnection.Close();
            }
        }

        public void Notify(string name, string id)
        {
            if (dic.ContainsKey(name))
            {
                Clients.Caller.differentName();
            }
            else
            {
                dic.TryAdd(name, id);
                foreach (KeyValuePair<String, String> entry in dic)
                {
                    Clients.Caller.online(entry.Key);
                }
                Clients.Others.enters(name);
            }
        }
        public override Task OnDisconnected()
        {
            var name = dic.FirstOrDefault(x => x.Value == Context.ConnectionId.ToString());
            string s;
            dic.TryRemove(name.Key, out s);
            return Clients.All.disconnected(name.Key);
        }
    }
}