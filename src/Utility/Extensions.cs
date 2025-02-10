﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace DiffClient.Utility
{
    internal static class Extensions
    {
        public static T SetDataContent<T>(this Control value, IViewModel viewmodel) where T : Control
        {
            T result = (T)value;
            value.DataContext = viewmodel;
            return result;
        }

        public static Image CreateIndexIco(string img = @"../resources/diffclient.ico")
        {
            Image myImage = new Image();
            BitmapImage myBitmapImage = new BitmapImage();
            myBitmapImage.BeginInit();
            myBitmapImage.UriSource = new Uri(img,UriKind.Relative);
            myBitmapImage.DecodePixelWidth = 200;
            myBitmapImage.EndInit();
            myImage.Source = myBitmapImage;
            return myImage;
        }

        public static string MapToShortName(this string origin, int maxlen)
        {
            if (origin.Length < maxlen)
            {
                return origin;
            }

            // todo
            return origin;
        }

        public static string GetPureName(this string functionName)
        {
            if(!functionName.Contains("("))
            {
                return functionName;
            }

            int off = functionName.IndexOf('(');
            return functionName.Substring(0,off);
        }

        public static int IndexOfLine(this string content,string search)
        {
            int off = content.IndexOf(search);
            int count = content.Substring(0,off).ToCharArray().Where(x => x == '\n').Count();
            return count;
        }

        public static void TabControlAddAndSelect(this TabControl tabControl,object obj)
        {
            if (tabControl == null || obj == null)
            {
                throw new ArgumentNullException(nameof(obj));
            }
            tabControl.Items.Add(obj);
            tabControl.SelectedItem = obj;
        }
    }
}
