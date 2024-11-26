using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VTigerUserControls
{
    public class Tools
    {
        static public DataTable GenerateKeyValueTable(DataTable inputTable)
        {
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

        static public DataTable ReadKeyValueTable(DataTable oldTable, DataTable inputTable)
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
                    if ((inRow[iCol] == null) || (inRow[iCol] == System.DBNull.Value) || ((string)inRow[iCol] == ""))
                        row[i] = DBNull.Value;
                    else
                        row[i] = inRow[iCol];
                    i++;
                }
                outputTable.Rows.Add(row);
            }

            return outputTable;
        }
    }
}
