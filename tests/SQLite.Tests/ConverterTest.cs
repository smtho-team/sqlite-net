using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using NUnit.Framework;

namespace SQLite.Tests
{
	public class ConverterTest
	{
		public class ListStringConverter : Converter<string, List<string>>
		{
			public string Convert (List<string> t)
			{
				return string.Join (",", t.ToArray ());
			}

			public List<string> Unconvert (string s)
			{
				return new List<string> (s.Split (","));
			}
		}

		public class Stock
		{
			[PrimaryKey, AutoIncrement]
			public int Id { get; set; }
			[Column ("Readers", typeof (ListStringConverter))]
			public List<string> Readers { get; set; }
		}

		[Test]
		public void Test ()
		{
			var databasePath = Path.Combine (Environment.GetFolderPath (Environment.SpecialFolder.MyDocuments), "MyData.db");
			var db = new SQLiteConnection (databasePath);
			db.CreateTable<Stock> ();
			Stock stock = new Stock () {
				Readers = new List<string> { "1", "2", "3", "4" }
			};
			db.Insert (stock);
			List<Stock> stocks = db.Table<Stock> ().ToList ();
		}
	}
}
