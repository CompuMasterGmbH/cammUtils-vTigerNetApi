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
            VTigerUserControls.KeyValueDataGridView TestGrid = new VTigerUserControls.KeyValueDataGridView();
            DataTable OldTable = new DataTable();
            OldTable.Columns.Add("Test", typeof(String)); //insert a column
            OldTable.Rows.Add(OldTable.NewRow()); //insert empty row with DBNull.Value
            DataTable InputTable = new DataTable();
            InputTable.Columns.Add("Test", typeof(String)); //insert a column
            InputTable.Rows.Add(InputTable.NewRow()); //insert empty row with DBNull.Value
            InputTable.Rows.Add(InputTable.NewRow()); //insert empty row with DBNull.Value
            InputTable.Rows[0][0] = DBNull.Value;
            InputTable.Rows[1][0] = null;
            TestGrid.ReadKeyValueTable(OldTable, InputTable);
        }

        [TestMethod]
        public void GenerateKeyValueTable()
        {
            VTigerUserControls.KeyValueDataGridView TestGrid = new VTigerUserControls.KeyValueDataGridView();
            DataTable InputTable = new DataTable();
            InputTable.Columns.Add("Test", typeof(String)); //insert a column
            InputTable.Rows.Add(InputTable.NewRow()); //insert empty row with DBNull.Value
            InputTable.Rows.Add(InputTable.NewRow()); //insert empty row with DBNull.Value
            InputTable.Rows[0][0] = DBNull.Value;
            InputTable.Rows[1][0] = null;
            TestGrid.GenerateKeyValueTable(InputTable);
        }
    }
}
