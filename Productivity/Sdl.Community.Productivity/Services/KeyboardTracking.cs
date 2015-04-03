﻿using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using NLog;

namespace Sdl.Community.Productivity.Services
{
   
    public sealed class KeyboardTracking : IMessageFilter
    {
        [DllImport("user32.dll")]
        private static extern short GetAsyncKeyState(Keys key);

        private static readonly Lazy<KeyboardTracking> LazyInstance =
            new Lazy<KeyboardTracking>(() => new KeyboardTracking());

        public static KeyboardTracking Instance { get { return LazyInstance.Value; } }
        private const int WmKeydown = 0x100;
        private int _keyDownCounter;
        private int _studioKeyboardShortcuts;
        private Logger _logger;

        private KeyboardTracking()
        {
            _keyDownCounter = 0;
            _logger = LogManager.GetLogger("log");
        }

        public bool PreFilterMessage(ref Message m)
        {
            switch (m.Msg)
            {
                case WmKeydown:
                    
                    //if lparam is zero key down might not be generated by system
                    if (m.LParam != IntPtr.Zero)
                    {
                        if (!IsModifierPressed())
                        {
                            _keyDownCounter++;
                        }
                        else
                        {
                            _studioKeyboardShortcuts++;
                        }
                    }
                    break;

            }

            return false; // returning false allows messages to be processed normally
        }

        private bool IsModifierPressed()
        {
            return (GetAsyncKeyState(Keys.ControlKey) < 0) || (GetAsyncKeyState(Keys.ShiftKey) < 0) ||
                   (GetAsyncKeyState(Keys.Alt) < 0);
        }

        public void ClearShortcuts()
        {
            _keyDownCounter += _studioKeyboardShortcuts;
            _studioKeyboardShortcuts = 0;
        }

        public int GetCount()
        {
            var result = _keyDownCounter;
            _keyDownCounter = 0;
            _studioKeyboardShortcuts = 0;
            return result;
        }

    }
}
