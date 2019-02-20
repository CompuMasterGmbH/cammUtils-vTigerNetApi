using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace VTigerUserControls
{
    /// <summary>
    /// Klasse zum anzeigen von DataTables als normale Tabelle oder als umgeformte Key-Value Tabelle
    /// </summary>
    public partial class KeyValueDataGridView : DataGridView
    {
        bool showKeyValueTable;

        DataTable dataTable
        {
            get
            {
                if (base.DataSource is DataTable)
                    return base.DataSource as DataTable;
                return null;
            }
            set
            {
                base.DataSource = value;
            }
        }

        DataTable originalTable;

        /// <summary>
        /// Gibt an, ob die Tabelle normal oder als Key-Value Tabelle angezeigt wird.
        /// </summary>
        public bool ShowKeyValueTable
        {
            get
            {
                return showKeyValueTable;
            }
            set
            {
                if (showKeyValueTable != value && dataTable != null)
                {
                    if (value)
                        dataTable = GenerateKeyValueTable(dataTable);
                    else
                        dataTable = ReadKeyValueTable(originalTable, dataTable);
                }
                showKeyValueTable = value;
            }
        }

        new public object DataSource
        {
            get
            {
                if (showKeyValueTable && dataTable != null)
                    return ReadKeyValueTable(originalTable, dataTable);
                else
                    return base.DataSource;
            }
            set
            {
                base.DataSource = value;
                if (showKeyValueTable && dataTable != null)
                    dataTable = GenerateKeyValueTable(dataTable);
            }
        }

        private DataTable GenerateKeyValueTable(DataTable inputTable)
        {
            originalTable = inputTable;
            if (inputTable == null)
                return null;
            DataTable outputTable = new DataTable();

            outputTable.Columns.Add("Key");
            outputTable.Columns[0].ReadOnly = true;
            for (int i = 0; i < inputTable.Rows.Count; i++)
                outputTable.Columns.Add(i.ToString());

            // Add rows by looping columns        
            for (int rCount = 0; rCount <= inputTable.Columns.Count - 1; rCount++)
            {
                DataRow newRow = outputTable.NewRow();

                // First column is inputTable's Header row's second column
                newRow[0] = inputTable.Columns[rCount].ColumnName.ToString();
                for (int cCount = 0; cCount < inputTable.Rows.Count; cCount++)
                {
                    string colValue = inputTable.Rows[cCount][rCount].ToString();
                    newRow[cCount + 1] = colValue;
                }
                outputTable.Rows.Add(newRow);
            }
            return outputTable;
        }

        private DataTable ReadKeyValueTable(DataTable oldTable, DataTable inputTable)
        {
            if (inputTable == null)
                return null;
            DataTable outputTable;
            if (oldTable == null)
            {
                outputTable = new DataTable();
                // Add columns by looping rows
                foreach (DataRow inRow in inputTable.Rows)
                {
                    string newColName = inRow[0].ToString();
                    outputTable.Columns.Add(newColName);
                }
            }
            else
            {
                //outputTable = oldTable.Clone();
                outputTable = oldTable;
                outputTable.Rows.Clear();
            }

            for (int iCol = 1; iCol < inputTable.Columns.Count; iCol++)
            {
                DataRow row = outputTable.NewRow();

                // Add output-row by looping input-cols
                int i = 0;
                foreach (DataRow inRow in inputTable.Rows)
                {
                    //string colValue = inRow[iCol].ToString();
                    try
                    {
                        
                        if (string.IsNullOrEmpty(inRow[iCol].ToString()))
                            row[i] = DBNull.Value;
                        else
                            row[i] = inRow[iCol];
                    }
                    catch (Exception e)
                    {
                        
                        
                    }
                   
                    i++;
                }
                outputTable.Rows.Add(row);
            }

            return outputTable;
        }

    }
}
