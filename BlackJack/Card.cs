using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlackJack
{
    public class Card
    {
        private int value;
        private char type;
        private char suite;
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
        public override string ToString()
        {
            return "Value:" + value+" Type:"+type+" Suite:"+suite;
        }
        public string GetTypeandValue()
        {
            return "V:" + value + " T:" + type;
        }
    }
}
