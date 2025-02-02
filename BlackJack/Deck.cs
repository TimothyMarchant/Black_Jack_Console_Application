using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
namespace BlackJack
{
    public class Deck
    {
        private const int decklength=52;
        //Treat the deck like a stack.
        private Card[] deck;
        private int stackpointer;
        public Deck()
        {
            deck = new Card[decklength];
            for (int i = 0; i < deck.Length; i++)
            {
                char suite = GetSuite(i / 13);
                int value = i % 13;
                char type=GetType(value);
                if (value > 9)
                {
                    value = 9;
                }
                deck[i] = new Card(value + 1, type, suite);
            }
            stackpointer = decklength-1;
        }
        //get the suite of the card
        private char GetSuite(int suitenum)
        {
            char temp = 'a';
            switch (suitenum)
            {
                case 0:
                    //spades
                    temp = 'S';
                    break;
                case 1:
                    //hearts
                    temp = 'H';
                    break;
                case 2:
                    //clubs
                    temp = 'C';
                    break;
                case 3:
                    //diamonds
                    temp = 'D';
                    break;
                    //should never happen
                default:
                    temp = '!';
                    break;
            }
            return temp;
        }
        private char GetType(int index)
        {
            char[] typetable = new char[] { 'a', '2', '3', '4', '5', '6', '7', '8', '9', '0', 'J', 'Q', 'K' };
            return typetable[index];
        }
        //shuffle the deck using the Fisher-Yates shuffle algorithm
        public void ShuffleDeck()
        {
            int index = 0;
            for (int i = decklength - 1; i >= 0; i--)
            {
                Random random = new Random();
                index =random.Next(0,i+1);
                Card temp = new Card();
                temp = deck[i];
                deck[i] = deck[index];
                deck[index] = temp;
            }
        }
        //print the deck
        public void printdeck()
        {
            for (int i = 0; i < decklength; i++)
            {
                Console.Write(i + ": ");
                Console.WriteLine(deck[i]);
            }
        }
        //return the top card and change the stackpointer.  (Pop() operation)
        public Card getTopCard()
        {
            Card card = deck[stackpointer];
            RemoveCard();
            return card;
        }
        //we're working with an array, so we don't actually have to change anything other than decrement the stack pointer.
        public void RemoveCard()
        {
            stackpointer--;
        }
        //get a card at a specific index.  Shouldn't really be called except for testing.
        public Card getCard(int index)
        {
            return deck[index];
        }
    }
}
