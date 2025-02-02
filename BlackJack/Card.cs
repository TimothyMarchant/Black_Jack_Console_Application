using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlackJack
{
    //card class
    public class Card
    {
        private int value;
        private char type;
        private char suite;
        //default constructor.  Shouldn't really be called
        public Card()
        {
            value = 1;
            type = 'a';
            suite = 'S';
        }
        public Card(int value,char type,char suite)
        {
            this.value = value;
            this.type = type;
            this.suite = suite;
        }
        public int GetValue()
        {
            return value;
        }
        public char GetType()
        {
            return type;
        }
        public char GetSuite()
        {
            return suite;
        }
        //default print
        public override string ToString()
        {
            return "Value:" + value+" Type:"+type+" Suite:"+suite;
        }
        //print this in general
        public string GetTypeandValue()
        {
            return "V:" + value + " T:" + type;
        }
    }
}
