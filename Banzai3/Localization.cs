using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.SqlServer.Server;

namespace Banzai3
{
    /// <summary>
    /// TODO not released yet
    /// </summary>
    public static class Localization
    {
        private static readonly Dictionary<string, string> Cultures =
            new Dictionary<string, string>
            {
                {"en-US", "en"},
                {"ru-ru", "ru"},
            };

        private static readonly Dictionary<string, string> Original;
        private static readonly Dictionary<string, string> Transtation;

        static Localization()
        {
            var culture = CultureInfo.CurrentCulture;

            var name = culture.Name.ToLower();
            if (Cultures.ContainsKey(name))
                name = Cultures[name].ToLower();
            Original = new Dictionary<string, string>();
            Transtation = new Dictionary<string, string>();
            //TODO just do it :)
        }

        public static string GetLocalName(string text)
        {
            if (Transtation.ContainsKey(text))
                return Transtation[text];
            if (Original.ContainsKey(text))
            {
                //TODO log warning no translation here
                return Transtation[text];
            }
            else
            {
                //TODO log error here
                return text;
            }
        }
        public static void SetLocalName(Control control)
        {
            control.Text = GetLocalName(control.Text);
        }

        public static void SetLocalName(ToolStripItem control)
        {
            control.Text = GetLocalName(control.Text);
        }
    }
}
