using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
namespace SnapPeaApp
{
    /// <summary>
    /// Encapsulates hotkey codes including modifiers.
    /// </summary>
    /// <remarks>
    /// Uses System.Windows.Input key code enums
    /// </remarks>
    public class Hotkey
    {
        public Hotkey()
        {
            Modifiers = 0;
            Key = 0;
        }

        public Hotkey(int modifiers, int key)
        {
            Modifiers = modifiers;
            Key = key;
        }

        /// <summary>
        /// int representation of System.Windows.Input.ModifierKeys enum
        /// </summary>
        public int Modifiers { get; set; }

        /// <summary>
        /// int representation of System.Windows.Input.Key enum
        /// </summary>
        public int Key { get; set; }

        public override string ToString()
        {
            string result;

            if ((ModifierKeys)Modifiers == ModifierKeys.None)
            {
                result = String.Empty;
            }
            else
            {
                result = ((ModifierKeys)Modifiers).ToString().Replace(", ", "+");
            }

            var key = KeyInterop.KeyFromVirtualKey(Key);
            switch (key)
            {
                case System.Windows.Input.Key.LWin:
                case System.Windows.Input.Key.RWin:
                case System.Windows.Input.Key.LeftShift:
                case System.Windows.Input.Key.RightShift:
                case System.Windows.Input.Key.LeftCtrl:
                case System.Windows.Input.Key.RightCtrl:
                case System.Windows.Input.Key.LeftAlt:
                case System.Windows.Input.Key.RightAlt:
                case System.Windows.Input.Key.None:
                    break;
                default:
                    result += String.IsNullOrEmpty(result) ? key.ToString() : "+" + key.ToString();
                    break;
            }

            return result;
        }
    }
}
