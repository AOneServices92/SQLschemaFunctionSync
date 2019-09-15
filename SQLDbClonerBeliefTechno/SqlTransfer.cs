using Microsoft.SqlServer.Management.Common;
using Microsoft.SqlServer.Management.Smo;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace SQLDbClonerBeliefTechno.Core.Schema
{
    public class SqlTransfer
    {
        public string _sourceConnectionString;
        public string _destinationConnectionString;
        public Database sourceDatabase;
        public Database destinationDatabase;
        public Server sourceServer;
        public Server destinationServer;
        public Transfer transfer;
        public ServerConnection sourceConnection;
        public ServerConnection destinationConnection;
        public List<SqlObject> SourceObjects;
        public List<SqlObject> DestinationObjects;

        public SqlTransfer(string src, string dst)
        {
            _sourceConnectionString = src;
            _destinationConnectionString = dst;

            sourceConnection = new ServerConnection(new SqlConnection(_sourceConnectionString));
            sourceServer = new Server(sourceConnection);

            destinationConnection = new ServerConnection(new SqlConnection(_destinationConnectionString));
            destinationServer = new Server(destinationConnection);

            InitServer(sourceServer);
            InitServer(destinationServer);

            sourceDatabase = sourceServer.Databases[sourceServer.ConnectionContext.DatabaseName];
            destinationDatabase = destinationServer.Databases[destinationServer.ConnectionContext.DatabaseName];

            transfer = new Transfer(sourceDatabase);

            transfer.DestinationServer = destinationConnection.ServerInstance;
            transfer.DestinationDatabase = destinationConnection.DatabaseName;
            transfer.DestinationLogin = destinationConnection.Login;
            transfer.DestinationPassword = destinationConnection.Password;

            transfer.Options.ContinueScriptingOnError = true;
            transfer.Options.NoFileGroup = true;
            transfer.Options.NoExecuteAs = true;
            transfer.Options.WithDependencies = false;
            transfer.Options.DriDefaults = true;
            transfer.CopySchema = true;
            transfer.CopyData = false;
            transfer.DropDestinationObjectsFirst = false;

            SourceObjects = GetSqlObjects(sourceDatabase);
            DestinationObjects = GetSqlObjects(destinationDatabase);
        }

        private void InitServer(Server serv)
        {
            // set the default properties we want upon partial instantiation -
            // smo is *really* slow if you don't do this
            serv.SetDefaultInitFields(typeof(Table), "IsSystemObject", "Name");
            serv.SetDefaultInitFields(typeof(StoredProcedure), "IsSystemObject", "Name");
            serv.SetDefaultInitFields(typeof(UserDefinedFunction), "IsSystemObject", "Name");
            serv.SetDefaultInitFields(typeof(Microsoft.SqlServer.Management.Smo.View), "IsSystemObject", "Name");
            serv.SetDefaultInitFields(typeof(Column), "Identity");
            serv.SetDefaultInitFields(typeof(Index), "IndexKeyType");
        }

        private void ResetTransfer()
        {
            transfer.CopyAllDatabaseTriggers = false;
            transfer.CopyAllDefaults = false;
            transfer.CopyAllLogins = false;
            transfer.CopyAllObjects = false;
            transfer.CopyAllPartitionFunctions = false;
            transfer.CopyAllPartitionSchemes = false;
            transfer.CopyAllRoles = false;
            transfer.CopyAllRules = false;
            transfer.CopyAllSchemas = false;
            transfer.CopyAllSqlAssemblies = false;
            transfer.CopyAllStoredProcedures = false;
            transfer.CopyAllSynonyms = false;
            transfer.CopyAllTables = false;
            transfer.CopyAllUserDefinedAggregates = false;
            transfer.CopyAllUserDefinedDataTypes = false;
            transfer.CopyAllUserDefinedFunctions = false;
            transfer.CopyAllUserDefinedTypes = false;
            transfer.CopyAllUsers = false;
            transfer.CopyAllViews = false;
            transfer.CopyAllXmlSchemaCollections = false;
            transfer.CreateTargetDatabase = false;
            //transfer.DropDestinationObjectsFirst = false;
            transfer.PrefetchObjects = false;
            transfer.SourceTranslateChar = false;
        }

        public void DropAndCreateObject(NamedSmoObject obj)
        {
            ResetTransfer();
            transfer.ObjectList.Clear();
            transfer.ObjectList.Add(obj);
            if (DestinationObjects.Any(d => d.Name == obj.Name))
            {
                transfer.Options.ScriptDrops = true;
                foreach (var script in transfer.ScriptTransfer())
                    (new SqlCommand(script, destinationConnection.SqlConnectionObject)).ExecuteNonQuery();
            }
            transfer.Options.ScriptDrops = false;
            foreach (var script in transfer.ScriptTransfer())
                (new SqlCommand(script, destinationConnection.SqlConnectionObject)).ExecuteNonQuery();
        }

        public void CreateObject(NamedSmoObject obj)
        {
            ResetTransfer();
            transfer.ObjectList.Clear();
            transfer.ObjectList.Add(obj);
            var st = transfer.ScriptTransfer();
            foreach (var script in st)
            {
                (new SqlCommand(script, destinationConnection.SqlConnectionObject)).ExecuteNonQuery();
            }
        }

        private List<SqlObject> GetSqlObjects(Database db)
        {
            List<SqlObject> items = new List<SqlObject>();

            foreach (SqlAssembly item in db.Assemblies)
            {
                if (!item.IsSystemObject)
                    items.Add(new SqlObject { Name = item.Name, Object = item, Type = item.GetType().Name });
            }

            foreach (UserDefinedDataType item in db.UserDefinedDataTypes)
            {
                items.Add(new SqlObject { Name = item.Name, Object = item, Type = item.GetType().Name });
            }

            foreach (UserDefinedTableType item in db.UserDefinedTableTypes)
            {
                items.Add(new SqlObject { Name = item.Name, Object = item, Type = item.GetType().Name });
            }

            foreach (Table item in db.Tables)
            {
                if (!item.IsSystemObject)
                {
                    List<NamedSmoObject> Cols = new List<NamedSmoObject>();
                    foreach (Column col in item.Columns)
                    {
                        Cols.Add(col);
                    }
                    items.Add(new SqlObject { Name = item.Name, Object = item, SubObject = Cols, Type = item.GetType().Name });
                }
            }

            foreach (Microsoft.SqlServer.Management.Smo.View item in db.Views)
            {
                if (!item.IsSystemObject)
                    items.Add(new SqlObject { Name = item.Name, Object = item, Type = item.GetType().Name });
            }

            foreach (UserDefinedFunction item in db.UserDefinedFunctions)
            {
                if (!item.IsSystemObject)
                    items.Add(new SqlObject { Name = item.Name, Object = item, Type = item.GetType().Name });
            }

            foreach (StoredProcedure item in db.StoredProcedures)
            {
                if (!item.IsSystemObject)
                    items.Add(new SqlObject { Name = item.Name, Object = item, Type = item.GetType().Name });
            }

            foreach (Microsoft.SqlServer.Management.Smo.DatabaseDdlTrigger item in db.Triggers)
            {
                if (!item.IsSystemObject)
                    items.Add(new SqlObject { Name = item.Name, Object = item, Type = item.GetType().Name });
            }

            return items;
        }

        internal void ApplyIndexes(NamedSmoObject sTable)
        {
            destinationDatabase = destinationServer.Databases[destinationServer.ConnectionContext.DatabaseName];
            Refresh();
            var dTable = destinationDatabase.Tables[sTable.Name];
            foreach (Index srcind in (sTable as Table).Indexes)
            {
                try
                {
                    string name = srcind.Name;
                    Index index = new Index(dTable, name);

                    index.IndexKeyType = srcind.IndexKeyType;
                    index.IsClustered = srcind.IsClustered;
                    index.IsUnique = srcind.IsUnique;
                    index.CompactLargeObjects = srcind.CompactLargeObjects;
                    index.IgnoreDuplicateKeys = srcind.IgnoreDuplicateKeys;
                    index.IsFullTextKey = srcind.IsFullTextKey;
                    index.PadIndex = srcind.PadIndex;
                    index.FileGroup = srcind.FileGroup;

                    foreach (IndexedColumn srccol in srcind.IndexedColumns)
                    {
                        IndexedColumn column =
                         new IndexedColumn(index, srccol.Name, srccol.Descending);
                        column.IsIncluded = srccol.IsIncluded;
                        index.IndexedColumns.Add(column);
                    }

                    index.FileGroup = dTable.FileGroup ?? index.FileGroup;
                    index.Create();
                }
                catch (Exception exc)
                {
                    // Not yet handled
                    throw exc;
                }
            }
        }

        internal void ApplyForeignKeys(NamedSmoObject sTable)
        {
            destinationDatabase = destinationServer.Databases[destinationServer.ConnectionContext.DatabaseName];
            Refresh();
            var dTable = destinationDatabase.Tables[sTable.Name];
            foreach (ForeignKey sourcefk in (sTable as Table).ForeignKeys)
            {
                try
                {
                    string name = sourcefk.Name;
                    ForeignKey foreignkey = new ForeignKey(dTable, name);
                    foreignkey.DeleteAction = sourcefk.DeleteAction;
                    foreignkey.IsChecked = sourcefk.IsChecked;
                    foreignkey.IsEnabled = sourcefk.IsEnabled;
                    foreignkey.ReferencedTable = sourcefk.ReferencedTable;
                    foreignkey.ReferencedTableSchema = sourcefk.ReferencedTableSchema;
                    foreignkey.UpdateAction = sourcefk.UpdateAction;

                    foreach (ForeignKeyColumn scol in sourcefk.Columns)
                    {
                        string refcol = scol.ReferencedColumn;
                        ForeignKeyColumn column =
                         new ForeignKeyColumn(foreignkey, scol.Name, refcol);
                        foreignkey.Columns.Add(column);
                    }

                    foreignkey.Create();
                }
                catch (Exception exc)
                {
                    // Not yet handled
                    throw exc;
                }
            }
        }

        internal void ApplyChecks(NamedSmoObject sTable)
        {
            destinationDatabase = destinationServer.Databases[destinationServer.ConnectionContext.DatabaseName];
            Refresh();
            var dTable = destinationDatabase.Tables[sTable.Name];
            foreach (Check chkConstr in (sTable as Table).Checks)
            {
                try
                {
                    Check check = new Check(dTable, chkConstr.Name);
                    check.IsChecked = chkConstr.IsChecked;
                    check.IsEnabled = chkConstr.IsEnabled;
                    check.Text = chkConstr.Text;
                    check.Create();
                }
                catch (Exception exc)
                {
                    // Not yet handled
                    throw exc;
                }
            }
        }

        internal void CreateColumns(NamedSmoObject sTable, NamedSmoObject _sColumn)
        {
            Table dTable = destinationDatabase.Tables[sTable.Name];
            Column sColumn = (_sColumn as Column);
            string s = string.Empty;
            try
            {
                Column coln = new Column(dTable, sColumn.Name, sColumn.DataType);
                coln.Nullable = true;
                coln.Create();
            }
            catch (Exception ex)
            {
                s = ex.Message;
            }
        }

        internal void Refresh()
        {
            SourceObjects = GetSqlObjects(sourceDatabase);
            DestinationObjects = GetSqlObjects(destinationDatabase);
        }
    }
}