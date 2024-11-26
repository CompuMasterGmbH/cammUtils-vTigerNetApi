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

        public DataTable DataTable
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
                if (showKeyValueTable != value && DataTable != null)
                {
                    if (value)
                        DataTable = GenerateKeyValueTable(DataTable);
                    else
                        DataTable = Tools.ReadKeyValueTable(originalTable, DataTable);
                }
                showKeyValueTable = value;
            }
        }

        new public object DataSource
        {
            get
            {
                if (showKeyValueTable && DataTable != null)
                    return Tools.ReadKeyValueTable(originalTable, DataTable);
                else
                    return base.DataSource;
            }
            set
            {
                base.DataSource = value;
                if (showKeyValueTable && DataTable != null)
                    DataTable = GenerateKeyValueTable(DataTable);
            }
        }

        internal DataTable GenerateKeyValueTable(DataTable inputTable)
        {
            originalTable = inputTable;
            return Tools.GenerateKeyValueTable(inputTable);
        }
    }
}