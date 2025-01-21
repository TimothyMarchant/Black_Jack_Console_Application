using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace BlackJack
{
    internal class Program
    {
        static int GetWinner(int dealervalue,int playervalue)
        {
            const int win = 1;
            const int lose = 0;
            const int tie = 2;
            if (dealervalue > playervalue)
            {
                return lose;
            }
            else if (dealervalue == playervalue)
            {
                return tie;
            }
            return win;
        }
        static void printhands(bool secondphase,Player Dealer,Player player)
        {
            const bool playerhand = false;
            Console.WriteLine("Current hands:");
            Console.Write("Dealer:");
            Dealer.showhand(secondphase);
            Console.Write("Player:");
            player.showhand(playerhand);
        }
        static bool DealerTurn(Deck deck,Player Dealer)
        {
            if (Dealer.GetValue() >= 17)
            {
                return true;
            }
            else
            {
                Card temp = deck.getTopCard();
                Dealer.Addcard(temp);
            }
            return false;
        }
        static bool PlayerTurn(Deck deck,Player player)
        {
            const bool finished = false;
            const bool playing = true;
            Console.WriteLine("Type x for hit or y for stand");
            string choice = Console.ReadLine();
            if (choice.ToLower().Equals("x"))
            {
                Card temp = deck.getTopCard();
                player.Addcard(temp);
                if (player.GetValue() > 21)
                {
                    return finished;
                }
            }
            else if (choice.ToLower().Equals("y"))
            {
                return finished;
            }
            else
            {
                Console.WriteLine("Pick valid choice");
            }
            
            return playing;
        }
        static bool CheckforBlackJack(Player player )
        {
            if (player.getHandSize() == 2)
            {
                return true;
            }
            return false;
        }
        static int BlackjackTieBreaker(Player player,Player Dealer)
        {
            const int tie = 0;
            const int playerwins = 1;
            const int dealerwins = 2;
            if (CheckforBlackJack(player) && CheckforBlackJack(Dealer))
            {
                return tie;
            }
            else if (CheckforBlackJack(player) && !CheckforBlackJack(Dealer))
            {
                return playerwins;
            }
            else 
            { 
                return dealerwins; 
            }
        }
        static int PlayRound(Deck deck,Player player,Player Dealer)
        {
            const int win = 1;
            const int lose = 0;
            const int blackjackwin = 2;
            const int tie = 3;
            const bool AtSecondPhase = true;
            bool PlayerisPlaying = true;
            bool DealerAt17 = false;
            while (PlayerisPlaying)
            {
                printhands(!AtSecondPhase, Dealer, player);
                PlayerisPlaying =PlayerTurn(deck, player);
                
            }
            printhands(AtSecondPhase, Dealer, player);
            if (player.GetValue() > 21)
            {
                return lose;
            }
            while (!DealerAt17)
            {
                printhands(AtSecondPhase, Dealer, player);
                DealerAt17 = DealerTurn(deck, Dealer);
                
            }
            int score = GetWinner(Dealer.GetValue(), player.GetValue());
            if (Dealer.GetValue() > 21)
            {
                Console.WriteLine("Player wins, dealer busted");
                return win;
            }
            
            else if (score==win)
            {
                Console.WriteLine("Winner: Player");
                //black jack is defined when the person has an ace and a 10 value card.  (only 2 cards; according to wikipedia)
                if (player.GetValue() == 21&&CheckforBlackJack(player))
                {
                    Console.WriteLine("BlackJack");
                    return blackjackwin;
                }
                return win;
            }
            else if (score==lose)
            {
                Console.WriteLine("Winner: Dealer");
                return lose;
            }
            else
            {
                //having a blackjack trumps just having 21.
                if (player.GetValue() == 21)
                {
                    const int dealerwontiebreaker = 2;
                    int result=BlackjackTieBreaker(player, Dealer);
                    if (result == win)
                    {
                        Console.WriteLine("Winner: Player");
                        Console.WriteLine("BlackJack");
                        return blackjackwin;
                    }
                    else if (result == dealerwontiebreaker)
                    {
                        Console.WriteLine("Winner: Dealer");
                        Console.WriteLine("Dealer had blackjack");
                        return lose;
                    }
                }
                return tie;
            }
        }
        static int StartRound(int bet)
        {
            const int win= 1;
            const int lose = 0;
            const int blackjackwin= 2;
            Player player = new Player(false);
            Player dealer = new Player(true);
            Deck deck = new Deck();
            deck.ShuffleDeck();
            for (int i = 0; i < 2; i++)
            {
                player.Addcard(deck.getTopCard());
                dealer.Addcard(deck.getTopCard());
            }
            int result=PlayRound(deck,player,dealer);
            //take the dealers money!
            if (result==win)
            {
                return 2 * bet;
            }
            //have you considered counting cards before?
            else if (result == lose)
            {
                return -1 * bet;
            }
            //
            else if (result == blackjackwin)
            {
                return 3 * bet;
            }
            //push
            else
            {
                return bet;
            }
        }
        static void playBlackJack()
        {
            
            int UserMoney = 200;
            while (UserMoney>0)
            {
                Console.WriteLine("Current cash: "+UserMoney);
                Console.Write("Enter bet:");
                int bet=int.Parse(Console.ReadLine());
                if (bet < 0)
                {

                }
                else if (bet > UserMoney)
                {

                }
                //play game
                else
                {
                    bet=StartRound(bet);
                    UserMoney += bet;
                }
            }
        }
        static void UnitTestValuecounting()
        {
            Player player = new Player(false);
            Deck deck = new Deck();
            //natural 21
            player.Addcard(deck.getCard(0));
            player.Addcard(deck.getCard(10));
            Console.WriteLine(player.GetValue());
            player.emptyhand();
            //generic hand ace+6=17
            player.Addcard(deck.getCard(0));
            player.Addcard(deck.getCard(5));
            Console.WriteLine(player.GetValue());
            player.emptyhand();
            //overload 10+7+ace=18
            player.Addcard(deck.getCard(0));
            player.Addcard(deck.getCard(6));
            player.Addcard(deck.getCard(10));
            Console.WriteLine(player.GetValue());
            player.emptyhand();
            //two aces should be 12 (ace+ace=11+1=12)
            player.Addcard(deck.getCard(0));
            player.Addcard(deck.getCard(13));
            Console.WriteLine(player.GetValue());
            player.emptyhand();
            //out of order hand (ace in the middle) 5+ace=16;
            player.Addcard(deck.getCard(4));
            player.Addcard(deck.getCard(0));
            Console.WriteLine(player.GetValue());
            player.emptyhand();
            //5+ace+ace=17
            player.Addcard(deck.getCard(0));
            player.Addcard(deck.getCard(4));
            player.Addcard(deck.getCard(13));
            Console.WriteLine(player.GetValue());
            player.emptyhand();

        }
        static void Main(string[] args)
        {
            playBlackJack();
            //UnitTestValuecounting();
        }
    }
}
