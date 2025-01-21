﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace BlackJack
{
    internal class Player
    {
        protected List<Card> Hand;
        private bool IsDealer;
        protected int value;
        public Player(bool IsDealer)
        {
            Hand = new List<Card>();
            value = 0;
            this.IsDealer = IsDealer;
        }
        public void Addcard(Card card)
        {
            
            Hand.Add(card);
            calculatevalue();
        }
        public void emptyhand()
        {
            //hand is empty so value must be zero.
            value = 0;
            Hand.Clear();
        }
        public void showhand(bool secondphase)
        {
            if (IsDealer&& !secondphase)
            {
                //only print the first card.  The other one is hidden.
                Console.WriteLine(Hand[0]);
                Console.WriteLine("Value:" + Hand[0].GetValue());
            }
            else
            {
                foreach (Card card in Hand)
                {
                    Console.Write(card.GetTypeandValue() + ",");
                }
                Console.WriteLine();
                Console.WriteLine("Value:" + value);
            }
        }
        private void calculatevalue()
        {
            //reset value
            value = 0;
            bool AcePresent = false;
            int numaces = 0;
            foreach (Card card in Hand)
            {
                if (card.GetType() == 'a')
                {
                    AcePresent = true;
                    numaces++;
                    if (value + 11 <= 21)
                    {
                        value += 11;
                    }
                    else
                    {
                        value += 1;
                    }
                }
                else
                {
                    value += card.GetValue();
                }
            }
            if (AcePresent && value > 21)
            {
                int tempvalue = 0;
                foreach (Card card in Hand)
                {
                    if (card.GetType() != 'a')
                    {
                        tempvalue += card.GetValue();
                    }
                }
                for (int i = 0; i < numaces; i++)
                {
                    if (tempvalue + 11 > 21)
                    {
                        tempvalue++;
                    }
                    else
                    {
                        tempvalue += 11;
                    }
                }
                value = tempvalue;
            }
        }
        public int getHandSize()
        {
            return Hand.Count;
        }
        public int GetValue()
        {
            calculatevalue();
            return value;
        }
    }
}
