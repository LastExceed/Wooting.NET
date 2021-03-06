﻿using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace Wooting
{
    public static class RGBControl
    {
        

#if LINUX
        private const string sdkDLL = "wooting-rgb-control.so";
#else
        private const string sdkDLL = "wooting-rgb-control.dll";
#endif
        //static RGBControl()
        //{
            //if (System.Runtime.InteropServices.RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            //    sdkDLL = "wooting-rgb-control.so";

        //}

        public const int MaxRGBRows = 6;
        public const int MaxRGBCols = 21;

        /// <summary>
        /// Check if keyboard connected.
        ///
        /// This function offers a quick check if the keyboard is connected.This doesn't open the keyboard or influences reading.
        /// It is recommended to poll this function at the start of your application and after a disconnect.
        /// </summary>
        /// <returns>This function returns true (1) if keyboard is found.</returns>
        [DllImport(sdkDLL, EntryPoint = "wooting_rgb_kbd_connected")]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern bool IsConnected();

        /// <summary>
        /// Set callback for when a keyboard disconnects.
        /// The callback will be called when a Wooting keyboard disconnects.This will trigger after a failed color change.
        /// </summary>
        /// <param name="cb">The function pointer of the callback</param>
        [DllImport(sdkDLL, EntryPoint = "wooting_rgb_set_disconnected_cb")]
        public static extern void SetDisconnectedCallback(DisconnectedCallback cb);

        /// <summary>
        /// Reset all colors on keyboard to the original colors. 
        /// This function will restore all the colours to the colours that were originally on the keyboard.This function
        /// should be called when you close the application.
        /// </summary>
        /// <returns>None</returns>
        [DllImport(sdkDLL, EntryPoint = "wooting_rgb_reset")]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern bool Reset();

        /// <summary>
        /// Directly reset 1 key on the keyboard to the original color.
        /// This function will directly reset the color of 1 key on the keyboard.This will not influence the keyboard color array.
        /// Use this function for simple applifications, like a notification.Use the array functions if you want to change the entire keyboard.
        /// </summary>
        /// <param name="row">The horizontal index of the key</param>
        /// <param name="column">The vertical index of the key</param>
        /// <returns>This functions return true (1) if the colour is reset.</returns>
        [DllImport(sdkDLL, EntryPoint = "wooting_rgb_direct_reset_key")]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern bool ResetKey(byte row, byte column);

        /// <summary>
        /// Directly reset 1 key on the keyboard to the original color.
        /// This function will directly reset the color of 1 key on the keyboard.This will not influence the keyboard color array.
        /// Use this function for simple applifications, like a notification.Use the array functions if you want to change the entire keyboard.
        /// </summary>
        /// <param name="key">The key to be reset</param>
        /// <returns>This functions return true (1) if the colour is reset.</returns>
        public static bool ResetKey(WootingKey.Keys key)
        {
            if (WootingKey.KeyMap.TryGetValue(key, out (byte row, byte column) index))
                return ResetKey(index.row, index.column);

            return false;
        }

        /// <summary>
        /// Send the colors from the color array to the keyboard.
        /// This function will send the changes made with the wooting_rgb_array_**_** functions to the keyboard.
        /// </summary>
        /// <returns>This functions return true (1) if the colours are updated.</returns>
        [DllImport(sdkDLL, EntryPoint = "wooting_rgb_array_update_keyboard")]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern bool UpdateKeyboard();

        /// <summary>
        /// Change the auto update flag for the wooting_rgb_array_**_** functions.
        /// This function can be used to set a auto update trigger after every change with a wooting_rgb_array_** _** function.
        /// Standard is set to false.
        /// </summary>
        /// <param name="auto_update">Change the auto update flag</param>
        [DllImport(sdkDLL, EntryPoint = "wooting_rgb_array_auto_update")]
        public static extern void SetAutoUpdate([MarshalAs(UnmanagedType.I1)] bool auto_update);

        /// <summary>
        /// Directly set and update 1 key on the keyboard.
        /// This function will directly change the color of 1 key on the keyboard.This will not influence the keyboard color array.
        /// Use this function for simple applifications, like a notification.Use the array functions if you want to change the entire keyboard.
        /// </summary>
        /// <param name="row">The horizontal index of the key</param>
        /// <param name="column">The vertical index of the key</param>
        /// <param name="red">A 0-255 value of the red color</param>
        /// <param name="green">A 0-255 value of the green color</param>
        /// <param name="blue">A 0-255 value of the blue color</param>
        /// <returns>This functions return true (1) if the colour is set.</returns>
        [DllImport(sdkDLL, EntryPoint = "wooting_rgb_direct_set_key")]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern bool _DirectSetKey(byte row, byte column, byte red, byte green, byte blue);

        /// <summary>
        /// Set a single color in the colour array.
        /// This function will set a single color in the colour array.This will not directly update the keyboard(unless the flag is set), so it can be called frequently.For example in a loop that updates the entire keyboard, if you don't want to send a C array from a different programming language.
        /// </summary>
        /// <param name="row">The horizontal index of the key</param>
        /// <param name="column">The vertical index of the key</param>
        /// <param name="red">A 0-255 value of the red color</param>
        /// <param name="green">A 0-255 value of the green color</param>
        /// <param name="blue">A 0-255 value of the blue color</param>
        /// <returns>This functions return true (1) if the colours are changed (optional: updated).</returns>
        [DllImport(sdkDLL, EntryPoint = "wooting_rgb_array_set_single")]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern bool _SetKey(byte row, byte column, byte red, byte green, byte blue);

        /// <summary>
        /// Set the colour of a key
        /// </summary>
        /// <param name="row">The horizontal index of the key</param>
        /// <param name="column">The vertical index of the key</param>
        /// <param name="red">A 0-255 value of the red color</param>
        /// <param name="green">A 0-255 value of the green color</param>
        /// <param name="blue">A 0-255 value of the blue color</param>
        /// <param name="direct">Determines if this is set directly to the keyboard or if it is stored in the keyboard color array</param>
        /// <returns>This functions return true (1) if the colours are changed (optional: updated)</returns>
        public static bool SetKey(byte row, byte column, byte red, byte green, byte blue, bool direct = false)
        {
            if (direct)
                return _DirectSetKey(row, column, red, green, blue);
            else
                return _SetKey(row, column, red, green, blue);
        }

        /// <summary>
        /// Set the colour of a key
        /// </summary>
        /// <param name="key">The key to be coloured</param>
        /// <param name="red">A 0-255 value of the red color</param>
        /// <param name="green">A 0-255 value of the green color</param>
        /// <param name="blue">A 0-255 value of the blue color</param>
        /// <param name="direct">Determines if this is set directly to the keyboard or if it is stored in the keyboard color array</param>
        /// <returns>This functions return true (1) if the colours are changed (optional: updated)</returns>
        public static bool SetKey(WootingKey.Keys key, byte red, byte green, byte blue, bool direct = false)
        {
            if (WootingKey.KeyMap.TryGetValue(key, out (byte row, byte column) index))
                return SetKey(index.row, index.column, red, green, blue, direct);

            return false;
        }

        /// <summary>
        /// Set the colour of a key
        /// </summary>
        /// <param name="key">The key to be coloured</param>
        /// <param name="colour">The colour for the key</param>
        /// <param name="direct">Determines if this is set directly to the keyboard or if it is stored in the keyboard color array</param>
        /// <returns>This functions return true (1) if the colours are changed (optional: updated)</returns>
        public static bool SetKey(WootingKey.Keys key, KeyColour colour, bool direct = false)
        {
            if (WootingKey.KeyMap.TryGetValue(key, out (byte row, byte column) index))
                return SetKey(index.row, index.column, colour.r, colour.g, colour.b, direct);

            return false;
        }

        /// <summary>
        /// Set a full colour array.
        /// This function will set a complete color array.This will not directly update the keyboard (unless the flag is set). 
        /// Use our online tool to generate a color array:
        /// If you use a non-C language it is recommended to use the wooting_rgb_array_set_single function to change the colors to avoid compatibility issues.
        /// </summary>
        /// <param name="colors_buffer">Pointer to a buffer of a full color array</param>
        /// <returns>This functions return true (1) if the colours are changed (optional: updated).</returns>
        [DllImport(sdkDLL, EntryPoint = "wooting_rgb_array_set_full")]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern bool SetFull([MarshalAs(UnmanagedType.LPArray, SizeConst = MaxRGBRows * MaxRGBCols)] KeyColour[,] colors_buffer);
    }
}

