﻿using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpreadCommander.Documents.Services
{
	public interface IDbSchemaViewerService
	{
		void Show(DbConnection connection, string databaseName = null, string defaultSchemaName = null,
			List<string> collectionNames = null, Dictionary<string, string> restrictions = null);
	}
}
