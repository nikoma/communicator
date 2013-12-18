using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using FirebirdSql.Data.FirebirdClient;
using FirebirdSql.Data.Isql;
using System.IO;
using System.Data;

namespace Remwave.Client
{

    public enum StorageItemDirection
    {
        In,
        Out,
        NA
    }
    public class StorageDDL
    {
        public String Version;
        public String DDL;

        public StorageDDL(String version, String ddl)
        {
            this.DDL = ddl;
            this.Version = version;
        }
    }

    public struct StorageMessageDate
    {
        public DateTime Date;
        public String Day;
        public Int32 DaysAway;
    }
    public struct StorageItemInstantMessage
    {
        public string GUID;
        public string JID;
        public string ContentText;
        public string ContentHTML;
        public StorageItemDirection Direction;
    }

    public class Storage
    {
        private String mStorageVersion;
        private List<StorageDDL> mStorageDDL;
        private readonly String mStorageUser = "SYSDBA";
        private readonly String mStoragePassword = "{DFCD1039-29D2-4e26-8D2C-6B93D4B45F2C}";
        private FbConnection mDatabaseConnection;
        private FbConnectionStringBuilder mDatabaseConnectionString = new FbConnectionStringBuilder();
        private Int32 mUserID;
        private String mUsername;

        public String Username
        {
            get { return mUsername; }
        }
        public struct StorageMessageArchiveUsers
        {
            public string JID;
            public StorageItemDirection Direction;
            public string Created;
        }
        public struct StorageMessage
        {
            public string JID;
            public string ContentHTML;
            public string ContentText;
            public StorageItemDirection Direction;
            public DateTime Created;
            public string GUID;
        }
        public string StorageGUID()
        {
            return System.Guid.NewGuid().ToString();
        }

        public Storage(List<StorageDDL> storageDDL, String version)
        {
            this.mStorageDDL = storageDDL;
            this.mStorageVersion = version;
        }

        public enum StorageType
        {
            FileSystem,
            InstantMessages
        }

        public bool Close()
        {
            try
            {
                if (this.mDatabaseConnection != null) 
                {
                    this.mDatabaseConnection.Close();
                    this.mDatabaseConnection.Dispose();
                  this.mDatabaseConnection  = null;
                }
                mDatabaseConnectionString = null;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Storage - Close : " + ex.Message);
                return false;
            }
            return true;
        }

        public bool Open(string storagePath, string storageName, string username, bool force)
        {
            try
            {
#if (DEBUG)
                String destinationFile = Path.GetTempPath() + Path.GetRandomFileName();
                File.Copy(storagePath + storageName, destinationFile);
#endif
                //if database broken and cant be accessed user can force deleting
                if (force) File.Delete(storagePath + storageName);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Storage - Delete : " + ex.Message);
            }


            try
            {
                mDatabaseConnectionString.Pooling = true;
                mDatabaseConnectionString.MinPoolSize = 2;
                mDatabaseConnectionString.MaxPoolSize = 20;
                mDatabaseConnectionString.UserID = mStorageUser;
                mDatabaseConnectionString.Password = mStoragePassword;
                mDatabaseConnectionString.Database = storagePath + storageName;
                mDatabaseConnectionString.ServerType = FbServerType.Embedded; // embedded Firebird
            }
            catch (Exception ex)
            {

                Console.WriteLine("Storage v." + this.mStorageVersion + " - Open : " + ex.Message);
                return false;
            }
            try
            {
                if (!File.Exists(storagePath + storageName))
                {
                    //Create new database 
                    FbConnection.CreateDatabase(mDatabaseConnectionString.ToString(), true);

                    // execute the SQL script
                    using (FbConnection db = new FbConnection(mDatabaseConnectionString.ToString()))
                    {
                        this.ExecuteDDL(db, mStorageDDL[0].DDL);
                    }
                }

                if (this.mDatabaseConnection == null)
                {
                    this.mDatabaseConnection = new FbConnection(mDatabaseConnectionString.ToString());
                }

                if (this.mDatabaseConnection.State != System.Data.ConnectionState.Open)
                {
                    this.mDatabaseConnection.Open();
                }

                FbCommand fbcmd = new FbCommand("OPENDB", mDatabaseConnection);
                fbcmd.CommandType = System.Data.CommandType.StoredProcedure;
                fbcmd.Parameters.Add("@iUSERNAME", username);
                fbcmd.Parameters.Add("@oUSER_ID", FbDbType.VarChar, 32).Direction = System.Data.ParameterDirection.ReturnValue;
                fbcmd.Parameters.Add("@oVERSION", FbDbType.VarChar, 32).Direction = System.Data.ParameterDirection.ReturnValue;

                fbcmd.ExecuteNonQuery();
                string resultUserID = fbcmd.Parameters["@oUSER_ID"].Value.ToString();
                string resultVersion = fbcmd.Parameters["@oVERSION"].Value.ToString();
                if (resultVersion == "") resultVersion = "1.0.0"; //first release bug fix

                int dummy;
                if (!Int32.TryParse(resultUserID, out dummy)) return false;
                this.mUsername = username;
                this.mUserID = dummy;

                //CHECK IF DATABASE UPDATE REQUIRED

                if (mStorageVersion != resultVersion)
                {
                    foreach (StorageDDL item in mStorageDDL)
                    {
                        if (resultVersion.CompareTo(item.Version) < 0)
                        {
                            this.ExecuteDDL(mDatabaseConnection, item.DDL);
                        }
                    }
                }

                if (this.mDatabaseConnection.State != System.Data.ConnectionState.Open)
                {
                    this.mDatabaseConnection.Open();
                }
            }

            catch (Exception ex)
            {
                Console.WriteLine("Storage v." + this.mStorageVersion + " - Open : " + ex.Message);
                if (this.mDatabaseConnection.State != ConnectionState.Closed) this.mDatabaseConnection.Close();
                this.mDatabaseConnection.Dispose();
                this.mDatabaseConnection = null;
                return false;
            }
            return this.mDatabaseConnection == null ? false : true;
        }


        private void ExecuteDDL(FbConnection db, String fbDDL)
        {
            MemoryStream memStream = new MemoryStream();
            byte[] data = Encoding.ASCII.GetBytes(fbDDL);
            memStream.Write(data, 0, data.Length);
            memStream.Position = 0;
            TextReader txtReader = new StreamReader(memStream);

            // parse the SQL script
            FbScript script = new FbScript(txtReader);
            int i = script.Parse();

            FbBatchExecution fbe = new FbBatchExecution(db);
            foreach (string cmd in script.Results)
            {
                fbe.SqlStatements.Add(cmd);
            }
            if (fbe.SqlStatements.Count > 0) fbe.Execute();
        }



        public bool Add(StorageType storageType, object storageData)
        {
            if (this.mDatabaseConnection.State != System.Data.ConnectionState.Open)
            {
                this.mDatabaseConnection.Open();
            }
            try
            {
                switch (storageType)
                {
                    case StorageType.FileSystem:
                        break;
                    case StorageType.InstantMessages:

                        StorageItemInstantMessage item = (StorageItemInstantMessage)storageData;
                        FbCommand fbcmd = new FbCommand("MESSAGE_ADD", this.mDatabaseConnection);
                        fbcmd.CommandType = CommandType.StoredProcedure;
                        fbcmd.Parameters.Add("@iUSER_ID", mUserID.ToString());
                        fbcmd.Parameters.Add("@iGUID", item.GUID);
                        fbcmd.Parameters.Add("@iJID", item.JID);
                        fbcmd.Parameters.Add("@iCONTENT_TEXT", item.ContentText);
                        fbcmd.Parameters.Add("@iCONTENT_HTML", item.ContentHTML);
                        switch (item.Direction)
                        {
                            case StorageItemDirection.In:
                                fbcmd.Parameters.Add("@iDIRECTION", "IN");
                                break;
                            case StorageItemDirection.Out:
                                fbcmd.Parameters.Add("@iDIRECTION", "OUT");
                                break;
                            case StorageItemDirection.NA:
                                fbcmd.Parameters.Add("@iDIRECTION", "NA");
                                break;
                        }

                        fbcmd.ExecuteNonQuery();
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Storage - Add : " + ex.Message);
                return false;
            }
            return true;
        }
        public void AddMessageToArchive(string jid, string guid, string messageText, string messageHTML, bool outgoing)
        {
            StorageItemInstantMessage item = new StorageItemInstantMessage();
            item.ContentHTML = messageHTML;
            item.ContentText = messageText;
            item.Direction = outgoing ? StorageItemDirection.Out : StorageItemDirection.In;
            item.GUID = guid;
            item.JID = jid;
            this.Add(Storage.StorageType.InstantMessages, item);
        }

        public List<StorageMessage> GetMessageFromArchive(string jid, string search, int limit)
        {
            if (this.mDatabaseConnection.State != System.Data.ConnectionState.Open)
            {
                this.mDatabaseConnection.Open();
            }
            DataSet ds = new DataSet();
            List<StorageMessage> list = new List<StorageMessage>();
            try
            {
                FbDataAdapter fbda = new FbDataAdapter("MESSAGE_FIND", this.mDatabaseConnection);
                fbda.SelectCommand.CommandType = CommandType.StoredProcedure;
                fbda.SelectCommand.Parameters.Add("@iUSER_ID", this.mUserID);
                fbda.SelectCommand.Parameters.Add("@iJID", jid.Trim());
                fbda.SelectCommand.Parameters.Add("@iSEARCH", "%" + search.Trim('%') + "%");
                fbda.SelectCommand.Parameters.Add("@iLIMIT", limit);
                fbda.Fill(ds, "TABLE");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        StorageMessage item = new StorageMessage();
                        item.JID = row[0].ToString();
                        item.ContentText = row[1].ToString();
                        item.ContentHTML = row[2].ToString();
                        item.Direction = row[3].ToString() == "IN" ? StorageItemDirection.In : StorageItemDirection.Out;
                        item.Created = (DateTime)row[4];
                        list.Add(item);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Storage - GetMessageArchive : " + ex.Message);
            }

            return list;
        }


        public List<StorageMessage> GetMessageFromArchiveByDate(string jid, string search, int limit, string date)
        {
            if (this.mDatabaseConnection.State != System.Data.ConnectionState.Open)
            {
                this.mDatabaseConnection.Open();
            }
            DataSet ds = new DataSet();
            List<StorageMessage> list = new List<StorageMessage>();
            try
            {
                FbDataAdapter fbda = new FbDataAdapter("MESSAGE_FIND_BY_DATE", this.mDatabaseConnection);
                fbda.SelectCommand.CommandType = CommandType.StoredProcedure;
                fbda.SelectCommand.Parameters.Add("@iUSER_ID", this.mUserID);
                fbda.SelectCommand.Parameters.Add("@iJID", jid.Trim());
                fbda.SelectCommand.Parameters.Add("@iSEARCH", "%" + search.Trim('%') + "%");
                fbda.SelectCommand.Parameters.Add("@iLIMIT", limit);
                fbda.SelectCommand.Parameters.Add("@iDATE", date);
                fbda.Fill(ds, "TABLE");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        StorageMessage item = new StorageMessage();
                        item.JID = row[0].ToString();
                        item.ContentText = row[1].ToString();
                        item.ContentHTML = row[2].ToString();
                        item.Direction = row[3].ToString() == "IN" ? StorageItemDirection.In : StorageItemDirection.Out;
                        item.Created = (DateTime)row[4];
                        list.Add(item);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Storage - GetMessageFromArchiveByDate : " + ex.Message);
            }

            return list;
        }


        public List<StorageMessageArchiveUsers> GetMessageArchiveUsers()
        {
            if (this.mDatabaseConnection.State != System.Data.ConnectionState.Open)
            {
                this.mDatabaseConnection.Open();
            }

            DataSet ds = new DataSet();
            List<StorageMessageArchiveUsers> list = new List<StorageMessageArchiveUsers>();

            try
            {
                FbDataAdapter fbda = new FbDataAdapter("MESSAGE_ARCHIVE_USERS", this.mDatabaseConnection);
                fbda.SelectCommand.CommandType = CommandType.StoredProcedure;
                fbda.SelectCommand.Parameters.Add("@iUSER_ID", this.mUserID);
                fbda.Fill(ds, "TABLE");

                if (ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        StorageMessageArchiveUsers item = new StorageMessageArchiveUsers();
                        item.JID = row[0].ToString();
                        item.Direction = row[1].ToString() == "IN" ? StorageItemDirection.In : StorageItemDirection.Out;
                        item.Created = row[2].ToString();
                        list.Add(item);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Storage - GetMessageArchiveUsers : " + ex.Message);
            }

            return list;
        }


        public List<StorageMessageDate> GetMessageDatesFromArchive(string jid, string search, int limit)
        {
            if (this.mDatabaseConnection.State != System.Data.ConnectionState.Open)
            {
                this.mDatabaseConnection.Open();
            }
            DataSet ds = new DataSet();
            List<StorageMessageDate> list = new List<StorageMessageDate>();
            try
            {
                FbDataAdapter fbda = new FbDataAdapter("MESSAGE_DATES_FIND", this.mDatabaseConnection);
                fbda.SelectCommand.CommandType = CommandType.StoredProcedure;
                fbda.SelectCommand.Parameters.Add("@iUSER_ID", this.mUserID);
                fbda.SelectCommand.Parameters.Add("@iJID", jid.Trim());
                fbda.SelectCommand.Parameters.Add("@iSEARCH", "%" + search.Trim('%') + "%");
                fbda.SelectCommand.Parameters.Add("@iLIMIT", limit);
                fbda.Fill(ds, "TABLE");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        StorageMessageDate date = new StorageMessageDate();
                        date.Day = row[0].ToString();
                        date.Date = (DateTime)row[1];
                        date.DaysAway = 0;
                        Int32.TryParse(row[2].ToString(), out date.DaysAway);
                        list.Add(date);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Storage - GetMessageDatesFromArchive : " + ex.Message);
            }

            return list;
        }


        internal Boolean DeleteUserHistory(string jid)
        {
            if (this.mDatabaseConnection.State != System.Data.ConnectionState.Open)
            {
                this.mDatabaseConnection.Open();
            }
            try
            {
                FbCommand fbcmd = new FbCommand("MESSAGE_DELETE_JID", this.mDatabaseConnection);
                fbcmd.CommandType = CommandType.StoredProcedure;
                fbcmd.Parameters.Add("@iUSER_ID", mUserID.ToString());
                fbcmd.Parameters.Add("@iJID", jid);
                fbcmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Storage - Delete (JID) : " + ex.Message);
                return false;
            }
            return true;
        }
    }
}