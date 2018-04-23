using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using DevExpress.Xpf.Grid;

namespace DXGrid_MultiSelectColumn {
    public partial class MainPage : UserControl {
        List<TestData> list;
        Dictionary<Guid, bool> selectedValues = new Dictionary<Guid, bool>();
        public MainPage() {
            InitializeComponent();
            list = new List<TestData>();
            for (int i = 0; i < 20; i++) {
                list.Add(new TestData() { Id = Guid.NewGuid(), Number = i });
            }
            grid.ItemsSource = list;
        }

        private void grid_CustomUnboundColumnData(object sender, GridColumnDataEventArgs e) {
            if (e.Column.FieldName == "Selected") {
                Guid key = (Guid)e.GetListSourceFieldValue("Id");
                if (e.IsGetData) {
                    e.Value = GetIsSelected(key);
                }
                if (e.IsSetData) {
                    SetIsSelected(key, (bool)e.Value);
                }
            }
        }
        bool GetIsSelected(Guid key) {
            bool isSelected;
            if (selectedValues.TryGetValue(key, out isSelected))
                return isSelected;
            return false;
        }
        void SetIsSelected(Guid key, bool value) {
            if (value)
                selectedValues[key] = value;
            else
                selectedValues.Remove(key);
        }

        private void BtnInvert_Click(object sender, RoutedEventArgs e) {
            for (int i = 0; i < list.Count; i++) {
                bool newIsSelected = !GetIsSelected(list[i].Id);
                int rowHandle = grid.GetRowHandleByListIndex(i);
                grid.SetCellValue(rowHandle, "Selected", newIsSelected);
            }
        }
        private void BtnGetSelected_Click(object sender, RoutedEventArgs e) {
            string selectedIds = string.Empty;
            foreach (Guid key in selectedValues.Keys) {
                selectedIds += string.Format("{0}\n", key);
            }
            string caption = string.Format("Selected rows (Total: {0})", selectedValues.Count);
            MessageBox.Show(selectedIds, caption, MessageBoxButton.OK);
        }
    }

    public class TestData {
        public Guid Id { get; set; }
        public int Number { get; set; }
    }
}
