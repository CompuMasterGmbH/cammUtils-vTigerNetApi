using System;
using System.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace VTigerApiUnitTests
{
    [TestClass]
    public class KeyValueDataGridViewTests
    {
        [TestMethod]
        public void ReadKeyValueTable()
        {
            DataTable OldTable = new DataTable();
            OldTable.Columns.Add("Test1", typeof(String)); //insert a column
            OldTable.Columns.Add("Test2", typeof(String)); //insert a column
            OldTable.Columns.Add("Test3", typeof(String)); //insert a column
            OldTable.Columns.Add("Test4", typeof(String)); //insert a column
            OldTable.Rows.Add(OldTable.NewRow()); //insert empty row with DBNull.Value
            DataTable InputTable = new DataTable();
            InputTable.Columns.Add("Test1", typeof(String)); //insert a column
            InputTable.Columns.Add("Test2", typeof(String)); //insert a column
            InputTable.Columns.Add("Test3", typeof(String)); //insert a column
            InputTable.Rows.Add(InputTable.NewRow()); //insert empty row with DBNull.Value
            InputTable.Rows.Add(InputTable.NewRow()); //insert empty row with DBNull.Value
            InputTable.Rows.Add(InputTable.NewRow()); //insert empty row with DBNull.Value
            InputTable.Rows.Add(InputTable.NewRow()); //insert empty row with DBNull.Value
            InputTable.Rows[0][0] = DBNull.Value;
            InputTable.Rows[1][0] = null;
            InputTable.Rows[2][0] = "";
            InputTable.Rows[3][0] = "Test";
            InputTable.Rows[0][1] = DBNull.Value;
            InputTable.Rows[1][1] = null;
            InputTable.Rows[2][1] = "";
            InputTable.Rows[3][1] = "Test";
            InputTable.Rows[0][2] = "DBNull.Value";
            InputTable.Rows[1][2] = "null";
            InputTable.Rows[2][2] = "empty";
            InputTable.Rows[3][2] = "Test";
            System.Console.WriteLine("Old table");
            System.Console.WriteLine(CompuMaster.Data.DataTables.ConvertToPlainTextTableFixedColumnWidths(OldTable));
            System.Console.WriteLine();
            System.Console.WriteLine("Input table");
            System.Console.WriteLine(CompuMaster.Data.DataTables.ConvertToPlainTextTableFixedColumnWidths(InputTable));
            DataTable Result = VTigerUserControls.Tools.ReadKeyValueTable(OldTable, InputTable);
            System.Console.WriteLine();
            System.Console.WriteLine("Result table");
            System.Console.WriteLine(CompuMaster.Data.DataTables.ConvertToPlainTextTableFixedColumnWidths(Result));
        }

        [TestMethod]
        public void GenerateKeyValueTable()
        {
            DataTable InputTable = new DataTable();
            InputTable.Columns.Add("Test1", typeof(String)); //insert a column
            InputTable.Columns.Add("Test2", typeof(String)); //insert a column
            InputTable.Columns.Add("Test3", typeof(String)); //insert a column
            InputTable.Rows.Add(InputTable.NewRow()); //insert empty row with DBNull.Value
            InputTable.Rows.Add(InputTable.NewRow()); //insert empty row with DBNull.Value
            InputTable.Rows.Add(InputTable.NewRow()); //insert empty row with DBNull.Value
            InputTable.Rows.Add(InputTable.NewRow()); //insert empty row with DBNull.Value
            InputTable.Rows.Add(InputTable.NewRow()); //insert empty row with DBNull.Value
            InputTable.Rows[0][0] = DBNull.Value;
            InputTable.Rows[1][0] = null;
            InputTable.Rows[2][0] = "";
            InputTable.Rows[3][0] = "Test";
            InputTable.Rows[0][1] = DBNull.Value;
            InputTable.Rows[1][1] = null;
            InputTable.Rows[2][1] = "";
            InputTable.Rows[3][1] = "Test";
            InputTable.Rows[0][2] = "DBNull.Value";
            InputTable.Rows[1][2] = "null";
            InputTable.Rows[2][2] = "empty";
            InputTable.Rows[3][2] = "Test";
            System.Console.WriteLine("Input table");
            System.Console.WriteLine(CompuMaster.Data.DataTables.ConvertToPlainTextTableFixedColumnWidths(InputTable));
            DataTable Result = VTigerUserControls.Tools.GenerateKeyValueTable(InputTable);
            System.Console.WriteLine();
            System.Console.WriteLine("Result table");
            System.Console.WriteLine(CompuMaster.Data.DataTables.ConvertToPlainTextTableFixedColumnWidths(Result));
        }
    }
}
