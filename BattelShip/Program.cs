using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BattelShipClassLibrary;
using BattelShipClassLibrary.Models;

namespace BattelShip
{
    class Program
    {
        public static void Main(string[] args)
        {
            Welcome();
            PlayerInfoModel player = CreatePlayer("Player1");
            PlayerInfoModel Opponent = CreatePlayer("Player2");
            PlayerInfoModel winner = null;
            do
            {
                DisplayGrid(player);
                FireTheSpot(player,Opponent);
                var isGameActive = GameLogic.IsGameStillActive(player);
                if (!isGameActive)
                {
                    winner = player;
                }
                else
                {
                    DisplayPlayerScore(player);
                    (Opponent,player)= (player,Opponent);
                }
                Console.ReadLine();
                Console.Clear();
            } while (winner == null);

            DisplayWinner(winner);
            Console.ReadLine();
        }

        private static void DisplayWinner(PlayerInfoModel winner)
        {
            Console.WriteLine($"Congratulation {winner.UserName}.");
            Console.WriteLine($"You took {GameLogic.GetShotcount(winner)}");
        }

        private static void DisplayPlayerScore(PlayerInfoModel player)
        {
            Console.WriteLine($"{ player.UserName} {GameLogic.GetLeftShotCount(player)} Ship Left to guess");
        }

        private static void FireTheSpot(PlayerInfoModel player, PlayerInfoModel opponent)
        {
            Console.WriteLine($"Please {player.UserName} fire the spot");
            bool isValid = false;
            string spotLetter;
            int spotNumber;
            do
            {
                var userFireInput = Console.ReadLine();
                (spotLetter, spotNumber) = GameLogic.SplitUserFireInput(userFireInput);
                isValid = GameLogic.IsShotValid(player, spotLetter, spotNumber);
                if(!isValid)
                {
                    Console.WriteLine("Invalid Location, Please try again");
                }
            } while (!isValid);

            bool isAHit = GameLogic.IdentifyShot(opponent, spotLetter, spotNumber);
            if (isAHit)
            {
                Console.WriteLine("Hit the spot");
            }
            else
            {
                Console.WriteLine("Missed");
            }
            GameLogic.SaveFireResult(player,spotLetter,spotNumber,isAHit);

        }

        private static void DisplayGrid(PlayerInfoModel player)
        {
            var currentLetter = player.ShotGrid[0].SpotLetter;
            foreach (var spot in player.ShotGrid)
            {
                if(currentLetter != spot.SpotLetter)
                {
                    Console.WriteLine();
                    currentLetter = spot.SpotLetter;
                }
                if (spot.Status == GridSpotStatus.Empty)
                {
                    Console.Write($" {spot.SpotLetter}{spot.SpotNumber} ");
                }
                else if (spot.Status == GridSpotStatus.Hit)
                {
                    Console.Write(" X ");
                }
                else if (spot.Status == GridSpotStatus.Miss)
                {
                    Console.Write(" O ");
                }
                else
                {
                    Console.Write(" ? ");
                }
                if (spot.SpotLetter == "E" && spot.SpotNumber == 5)
                {
                    Console.WriteLine();
                }
            }
        }

        private static void Welcome()
        {
            Console.WriteLine("Welcome to the BattelShip game");
            Console.WriteLine("Created by Nishchhal Prajapati");
            Console.WriteLine();
        }

        private static PlayerInfoModel CreatePlayer(string playerTitle)
        {
            PlayerInfoModel playerInfoModel = new PlayerInfoModel();
            Console.WriteLine($"{playerTitle} Information");
            //Ask player name
            playerInfoModel.UserName = AskPlayerName();
            // initialize shotGrid
            GameLogic.InitializeGrid(playerInfoModel);
            // Ask your for 5 ship placement
            PlaceShip(playerInfoModel);
            // clear screen
            Console.Clear();

            return playerInfoModel;
        }
        private static string AskPlayerName()
        {
            Console.WriteLine("Type your name");
            return Console.ReadLine();
        }

        private static void PlaceShip(PlayerInfoModel playerInfoModel) 
        {
            do
            {
                Console.WriteLine($"Where do you want to place ship number{playerInfoModel.ShipLocation.Count + 1}");
                var userFireInput = Console.ReadLine();
                bool isValidSpot = GameLogic.StoreShip(playerInfoModel,userFireInput);
                if (isValidSpot == false)
                {
                    Console.WriteLine("That was not valid location, Please try again");
                }
            } while (playerInfoModel.ShipLocation.Count < 5);
        }
    }
}
