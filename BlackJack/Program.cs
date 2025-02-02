using System;
using System.CodeDom;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace BlackJack
{
    internal class Program
    {
        //get the winner of the round (or a draw).
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
        //print the hands of the dealer and player.
        static void printhands(bool secondphase,Player Dealer,Player player)
        {
            const bool playerhand = false;
            Console.WriteLine("Current hands:");
            Console.Write("Dealer:");
            Dealer.showhand(secondphase);
            Console.Write("Player:");
            player.showhand(playerhand);
        }
        //Plays the dealer.
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
        //Player turn, gets user input for whether or not to take a hit or stand.
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
        //for tie breaking a 21 (blackjack trumps a score of 21 if it's not a blackjack).
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
        //This function actually plays the round (that is taking hits/stand).
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
        //Starts the round and sets up the deck.
        //Ignore the sarcastic remarks
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
                //The user never lost money in the calling method (or actually put forth money).
                return bet;
            }
            //have you considered counting cards before?
            else if (result == lose)
            {
                return -1 * bet;
            }
            //Black jack win
            else if (result == blackjackwin)
            {
                return 2 * bet;
            }
            //push
            else
            {
                //gain nothing
                return 0;
            }
        }
        //main controlling function.
        //This function holds all user info (or really just their money).  And has other helper methods for actually playing the game.
        static void playBlackJack()
        {
            int UserMoney = 200;
            bool wanttoquit = false;
            const string progresspresent = "present";
            const string progressnotpresent = "notpresent";
            const string savefile = "blackjacksave.txt";
            const string startingmoney = "200";
            StreamWriter writer;

            StreamReader reader;
            //for saving progress
            try
            {
                reader = new StreamReader(savefile);
                if (reader.ReadLine().Equals(progresspresent))
                {
                    UserMoney = int.Parse(reader.ReadLine());
                    if (UserMoney < 200)
                    {
                        UserMoney = 200;
                    }
                }
                reader.Close();
            }
            //Some sort of file problem.
            catch (Exception e)
            {
                Console.WriteLine("Error reading save data");
                //simply create a new file
                writer = new StreamWriter(savefile);
                writer.WriteLine(progressnotpresent);
                writer.WriteLine(startingmoney);
                writer.Close();
            }
            
            Console.WriteLine("Welcome to Blackjack");
            while (!wanttoquit)
            {
                Console.WriteLine("Type quit to exit the program when finished, otherwise type a bet");
                Console.WriteLine("Current cash: "+UserMoney);
                Console.Write("Enter bet:");
                try
                {
                    string Bet_To_Be_Parsed = Console.ReadLine();
                    Bet_To_Be_Parsed = Bet_To_Be_Parsed.ToLower();
                    if (Bet_To_Be_Parsed.Equals("quit"))
                    {
                        wanttoquit = true;
                        
                        break;
                    }
                    int bet=int.Parse(Bet_To_Be_Parsed);
                    if (bet < 0||bet>UserMoney)
                    {
                        throw new Exception();
                    }
                    //play game
                    else
                    {
                        bet = StartRound(bet);
                        UserMoney += bet;
                    }
                    if (UserMoney <= 0)
                    {
                        Console.WriteLine("You're out of money, type loan (or anything honestly) to get a loan of 200 dollars, or type quit to quit right before your big win");
                        string finalrequest = Console.ReadLine();
                        finalrequest = finalrequest.ToLower();
                        if (finalrequest.Equals("quit"))
                        {
                            break;
                        }
                        else
                        {
                            UserMoney = int.Parse(startingmoney);
                        }
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine("Type a valid option");
                }
            }
            try
            {
                //save progress
                writer = new StreamWriter(savefile);
                writer.WriteLine(progresspresent);
                writer.WriteLine(UserMoney.ToString());
                writer.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine("Error writing save data");
            }
            Console.WriteLine("Have a nice day");
        }
        //Look for a save file.  If one does not exist create a new file
        static void SearchForSave()
        {
            const string progressnotpresent = "notpresent";
            const string save = "blackjacksave.txt";
            try
            {
                string currentdirectory = Directory.GetCurrentDirectory();
                string[] file=Directory.GetFiles(currentdirectory, save);
                if (file.Length == 0)
                {
                    //create file
                    StreamWriter writer=new StreamWriter(save);
                    writer.WriteLine(progressnotpresent);
                    writer.WriteLine("200");
                    writer.Close();
                }
                else
                {
                    return;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
        static void Main(string[] args)
        {
            //look for a file
            SearchForSave();
            //Play blackjack
            playBlackJack();
        }
    }
}
