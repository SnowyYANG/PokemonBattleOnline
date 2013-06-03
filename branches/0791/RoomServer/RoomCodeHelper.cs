using System;
using System.Collections.Generic;
using System.Text;
using NetworkLib.Utilities;
using System.IO;
using System.Windows.Forms;
using System.Text.RegularExpressions;

namespace PokemonBattle.RoomServer
{
    public class RoomCodeHelper
    {
        private static object _roomCoder;
        private static string _assemblyName;

        private static Action<string> _broadcast;
        private static Action<string[]> _send;
        
        public static void LoadCode(string filePath)
        {
            string typeName = Properties.Settings.Default.CodeSetting.EntranceClass;
            if (string.IsNullOrEmpty(typeName)) return;

            try
            {
                _assemblyName = ReflectionHelper.LoadAssembly(filePath);
                if (!string.IsNullOrEmpty(_assemblyName))
                {
                    Type type = ReflectionHelper.GetTypeFormPool(_assemblyName, typeName);
                    if (type != null)
                    {
                        _roomCoder = ReflectionHelper.CreateInstance(type, new object[0], false);
                        Properties.Settings.Default.CodeSetting.RoomCodePath = filePath;
                        SetCallback();
                    }
                }
            }
            catch (Exception error)
            {
                Logger.LogException(error);
            }
        }

        public static void Unload()
        {
            _roomCoder = null;
            _assemblyName = string.Empty;
        }

        public static void SetCallback(Action<string> broadcast, Action<string[]> send)
        {
            _broadcast += broadcast;
            _send += send;
            SetCallback();
        }

        private static void SetCallback()
        {
            if (_roomCoder != null && _broadcast != null && _send != null)
            {
                object[] parameters = new object[] { _broadcast, _send };
                ReflectionHelper.InvokeMethod(_roomCoder, "SetCallback", parameters, false);
            }
        }

        public static bool VerifyUser(string name, string address, ref string message)
        {
            if (_roomCoder != null)
            {
                object[] parameters = new object[] { name, address, message };
                Type[] types = ReflectionHelper.GetTypes(parameters);
                types[2] = types[2].MakeByRefType();

                object result = ReflectionHelper.InvokeMethod(_roomCoder, "VerifyUser", parameters, types, false);
                if (result != null)
                {
                    message = (string)parameters[2];
                    return (bool)result;
                }
            }
            return true;
        }

        public static void ParseMessage(int identity, string message)
        {
            if (_roomCoder != null)
            {
                object[] parameters = new object[] { identity, message};
                Type[] types = ReflectionHelper.GetTypes(parameters);
                ReflectionHelper.InvokeMethod(_roomCoder, "ParseMessage", parameters, types, false);
            }
        }
    }
}
