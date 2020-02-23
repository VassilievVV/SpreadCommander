using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SpreadCommander.Documents.Services
{
	public class DbSchemaViewerService: IDbSchemaViewerService
	{
		private readonly IWin32Window _Owner;

		public DbSchemaViewerService(IWin32Window owner)
		{
			_Owner = owner;
		}

		public void Show(DbConnection connection, string databaseName, string defaultSchemaName = null,
			List<string> collectionNames = null, Dictionary<string, string> restrictions = null)
		{
			//Duplicate connection
			var conn = (DbConnection)Activator.CreateInstance(connection.GetType());
			conn.ConnectionString = connection.ConnectionString;

#pragma warning disable IDE0067 // Dispose objects before losing scope
            var frm = new Dialogs.DbSchemaViewer(conn, databaseName, defaultSchemaName, collectionNames, restrictions);
#pragma warning restore IDE0067 // Dispose objects before losing scope
            frm.FormClosed += (s, e) =>
			{
				if (conn.State != ConnectionState.Closed)
					conn.Close();

                frm.Dispose();
			};

			frm.Show(_Owner);
		}
	}
}
