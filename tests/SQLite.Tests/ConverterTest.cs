using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using NUnit.Framework;

namespace SQLite.Tests
{
	public class ConverterTest
	{
		public class ListStringConverter : IConverter<string, List<string>>
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
			[Column (Converter = typeof (ListStringConverter))]
			public List<string> Readers { get; set; }
			public int Age { get; set; }
		}

		[Test]
		public void Insert ()
		{
			var databasePath = Path.Combine (Environment.GetFolderPath (Environment.SpecialFolder.MyDocuments), "MyData.db");
			var db = new SQLiteConnection (databasePath);
			db.CreateTable<Stock> ();
			Stock stock = new Stock () {
				Readers = new List<string> { "1", "2", "3", "4" }
			};
			db.Insert (stock);
			List<Stock> stocks = db.Table<Stock> ().ToList ();
			db.DropTable<Stock> ();
			Assert.AreEqual (4, stocks[0].Readers.Count);
		}

		[Test]
		public void Update ()
		{
			var databasePath = Path.Combine (Environment.GetFolderPath (Environment.SpecialFolder.MyDocuments), "MyData.db");
			var db = new SQLiteConnection (databasePath);
			db.CreateTable<Stock> ();
			Stock stock = new Stock () {
				Readers = new List<string> { "1", "2", "3", "4" }
			};
			db.Insert (stock);
			stock = new Stock () {
				Id = 1,
				Readers = new List<string> { "5", "2", "3", "4" }
			};
			db.Update (stock);
			List<Stock> stocks = db.Table<Stock> ().ToList ();
			db.DropTable<Stock> ();
			Assert.AreEqual ("5", stocks[0].Readers[0]);
		}
	}
}
