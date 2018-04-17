using System.Windows.Input;

namespace SnapPeaApp.ViewModels
{
    /// <summary>
    /// Interaction logic for SaveDontSaveCancel dialog box
    /// </summary>
    partial class LayoutEditorViewModel
    {
        bool cancelClosing = false;

        #region Commands
        /// <summary>
        /// Command bound to Cancel button
        /// </summary>
        public ICommand CancelCommand
        {
            get
            {
                return new RelayCommand(Cancel);
            }
        }

        /// <summary>
        /// Command bound to Save button
        /// </summary>
        public ICommand SaveCommand
        {
            get
            {
                return new RelayCommand(Save);
            }
        }
        #endregion

        #region Methods
        /// <summary>
        /// Cancels window closing operation
        /// </summary>
        /// <param name="o"></param>
        void Cancel(object o)
        {
            cancelClosing = true;
            (o as System.Windows.Window).Close();
        }
        
        /// <summary>
        /// Saves layout to file
        /// </summary>
        /// <param name="o"></param>
        void Save(object o)
        {
            SaveLayout();
            (o as System.Windows.Window).Close();
        }
        #endregion
    }
}
