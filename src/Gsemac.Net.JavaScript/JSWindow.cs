﻿using System;
using System.Text;

namespace Gsemac.Net.JavaScript {

    public class JSWindow :
        IJSWindow {

        // Public members

        public IJSConsole Console { get; } = new JSConsole();
        public IJSStorage LocalStorage { get; } = new JSStorage();
        public IJSStorage SessionStorage { get; } = new JSStorage();

        public string Atob(string encodedData) {

            byte[] bytes = Convert.FromBase64String(encodedData);

            return Encoding.GetEncoding(28591).GetString(bytes);


        }
        public string Btoa(string stringToEncode) {

            byte[] bytes = Encoding.GetEncoding(28591).GetBytes(stringToEncode);

            return Convert.ToBase64String(bytes);

        }

    }

}