using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace IndependentDll
{
    public class Class1
    {
        public Class1()
        {
            text = "Hello from " + AppDomain.CurrentDomain.ToString();
        }

        public Class1(string text)
        {
            this.text = text;
        }

        public void SayHello()
        {
            MessageBox.Show(text);
        }

        string text;
    }
}
