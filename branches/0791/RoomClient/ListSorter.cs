using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Windows.Forms;
using System.Text.RegularExpressions;
namespace PokemonBattle.RoomClient
{
    internal class ListViewSorter : IComparer
    {
        #region Varibles
        private int _columnIndex;

        private SortOrder _order;
        #endregion

        public ListViewSorter(int columnIndex)
            : this(columnIndex, SortOrder.Ascending)
        {
        }
        public ListViewSorter(int columnIndex,SortOrder order)
        {
            _columnIndex = columnIndex;
            _order = order;
        }

        public int ColumnIndex
        {
            get { return _columnIndex; }
            set { _columnIndex = value; }
        }
        public SortOrder Order
        {
            get { return _order; }
            set { _order = value; }
        }

        #region IComparer 成员

        public int Compare(object x, object y)
        {
            string text1 = (x as ListViewItem).SubItems[_columnIndex].Text;
            string text2 = (y as ListViewItem).SubItems[_columnIndex].Text;
            int result;
            result = string.Compare(text1, text2);
            if (_order == SortOrder.Descending) result = -result;
            return result;
        }

        #endregion
    }
}
