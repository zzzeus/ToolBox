using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using ToolBox.Helpers;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Globalization;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“空白页”项模板

namespace ToolBox.pages
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class RubyPage : Page
    {
        public RubyPage()
        {
            this.InitializeComponent();
        }
        
        public string turnRuby(string input)
        {
            StringBuilder output = new StringBuilder();
            // Analyze the Japanese text according to the specified algorithm.
            IReadOnlyList<JapanesePhoneme> words = JapanesePhoneticAnalyzer.GetWords(input, false);
            foreach (JapanesePhoneme word in words)
            {
                //// Put each phrase on its own line.
                //if (output.Length != 0 && word.IsPhraseStart)
                //{
                //    output.AppendLine();
                //}

                

                // DisplayText is the display text of the word, which has same characters as the input of GetWords().
                // YomiText is the reading text of the word, as known as Yomi, which typically consists of Hiragana characters.
                // However, please note that the reading can contains some non-Hiragana characters for some display texts such as emoticons or symbols.
                if (StringHelper.isContainKanji(word.DisplayText))
                    output.AppendFormat("<ruby>{0}<rt>{1}</rt></ruby>", word.DisplayText, word.YomiText);
                else
                    output.Append(word.DisplayText);
            }

            // Display the result.
            string outputString = output.ToString();
            return outputString;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //Debug.WriteLine(textBox.Text.Length);
            //var array = System.Text.Encoding.ASCII.GetBytes(textBox.Text); //string转换的字母
            //int asciicode = (short)(array[1]); /* 何问起 hovertree.com */
            //var a = Convert.ToString(asciicode); //将转换一的ASCII码转换成string型
            //tb.Text = a;
            byte[] array = new byte[1];
            array[0] = (byte)(Convert.ToInt32(13)); //ASCII码强制转换二进制
            var Newlinecharacter = Convert.ToString(System.Text.Encoding.ASCII.GetString(array));
            string text = textBox.Text;
            StringBuilder sb = new StringBuilder();
            char[] charSeparators = new char[] { '。' };
            string[] paragraphs = text.Split(new string[] { Newlinecharacter }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var paragraph in paragraphs)
            {
                Debug.WriteLine(paragraph);
                Debug.WriteLine(".................");
                string[] sentences = paragraph.Split(charSeparators, StringSplitOptions.RemoveEmptyEntries);

                foreach (var item in sentences)
                {
                    var t = turnRuby(item);
                    if (t.Length < 1)
                        t = turnLong(item);
                    sb.AppendFormat("{0}。", t);
                }
                sb.Append("<br><br>");
            }
            //sb.Replace(" ", "<br><br>");
            webview.NavigateToString(sb.ToString());
        }
        public string turnLong(string text)
        {
            StringBuilder sb = new StringBuilder();
            char[] charSeparators = new char[] { '、' };
            List<String> list = new List<string>();
            string[] sentences = text.Split(charSeparators, StringSplitOptions.RemoveEmptyEntries);
            foreach (var item in sentences)
            {
                var t = turnRuby(item);
                list.Add(t);
            }
            var s=String.Join("、", list);
            return s;
        }
    }
}
