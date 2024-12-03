using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BattelShipClassLibrary.Models;

namespace BattelShipClassLibrary
{
    public class GameLogic
    {
        public static object GetLeftShotCount(PlayerInfoModel player)
        {
            var totalHit = 0;
            foreach(var spot in player.ShotGrid.ToList())
            {
                if (spot.Status == GridSpotStatus.Hit)
                {
                    totalHit++;
                }
            }
            return 5-totalHit;
        }

        public static object GetShotcount(PlayerInfoModel player)
        {
            var totalShot = 0;
            foreach (var spot in player.ShotGrid.ToList())
            {
                if (spot.Status != GridSpotStatus.Empty)
                {
                    totalShot ++;
                }
            }
            return totalShot;
        }

        public static bool IdentifyShot(PlayerInfoModel player, string spotLetter, int spotNumber)
        {
            foreach (var ship in player.ShipLocation.ToList())
            {
                if (ship.SpotLetter == spotLetter && ship.SpotNumber == spotNumber)
                { 
                    return true;
                }
            }
            return false;
        }

        public static void InitializeGrid(PlayerInfoModel model)
        {
            List<string> letters = new List<string>()
            {
                "A",
                "B",
                "C",
                "D",
                "E"
            };
            List<int> numbers = new List<int>()
            {
                1,
                2,
                3,
                4,
                5
            };
            foreach (string letter in letters)
            {
                foreach (int number in numbers)
                {
                    AddSpot(model, letter,number);
                }
            }
        }

        public static bool IsGameStillActive(PlayerInfoModel player)
        {
            var totalHit = 0;
            foreach (var shot in player.ShotGrid.ToList())
            {
                if (shot.Status == GridSpotStatus.Hit)
                {
                    totalHit ++;
                }
            }
            return totalHit < 5;
        }

        public static bool IsShotValid(PlayerInfoModel playerInfo, string spotLetter, int spotNumber)
        {
            foreach(var shot in playerInfo.ShotGrid.ToList())
            {
                if (shot.SpotLetter == spotLetter && shot.SpotNumber == spotNumber && shot.Status == GridSpotStatus.Empty)
                {
                    return true;
                }
            }
            return false;
        }

        public static void SaveFireResult(PlayerInfoModel player, string spotLetter, int spotNumber, bool isAHit)
        {
            foreach (var spot in player.ShotGrid.ToList())
            {
                if (spot.SpotLetter == spotLetter && spot.SpotNumber == spotNumber && isAHit)
                {
                    spot.Status = GridSpotStatus.Hit;
                }
                else if (spot.SpotLetter == spotLetter && spot.SpotNumber == spotNumber && !isAHit)
                {
                    spot.Status = GridSpotStatus.Miss;
                }
            }
        }

        public static (string spotLetter, int spotNumber) SplitUserFireInput(string userFireInput)
        {
            string spotLetter = "";
            int spotNumber = 0;
            if (userFireInput == null || userFireInput.Length != 2)
            {
                return (spotLetter,spotNumber);
            }
            spotLetter = userFireInput[0].ToString();
            char number = userFireInput[1];
            spotNumber = int.Parse(number.ToString());
            return (spotLetter, spotNumber);
        }

        public static bool StoreShip(PlayerInfoModel playerInfoModel,string userFireInput)
        {
            // validation 
            (string spotLetter, int spotNumber) = GameLogic.SplitUserFireInput(userFireInput);
            if (spotLetter == "" || spotNumber == 0)
            {
                Console.WriteLine("Not valid a spot");
            }
            else if (!IsShipLocationValid(playerInfoModel,spotLetter,spotNumber))
            {
                Console.WriteLine("You already Used this spot");
            }
            else
            {
                GridSpotModel gridSpotModel = new GridSpotModel()
                {
                    SpotLetter = spotLetter,
                    SpotNumber = spotNumber,
                    Status = GridSpotStatus.Ship
                };
                playerInfoModel.ShipLocation.Add(gridSpotModel);
                return true;
            }
            return false;
        }

        private static bool IsShipLocationValid(PlayerInfoModel playerInfoModel, string spotLetter, int spotNumber)
        {
            foreach (var ship in playerInfoModel.ShipLocation.ToList())
            {
                if (ship.SpotLetter == spotLetter && ship.SpotNumber == spotNumber)
                {
                    return false;
                }
            }
            return true;
        }

        private static void AddSpot(PlayerInfoModel model, string letter, int number)
        {
            GridSpotModel gridSpot = new GridSpotModel()
            {
                SpotLetter = letter,
                SpotNumber = number,
                Status = GridSpotStatus.Empty
            };
            model.ShotGrid.Add(gridSpot);
        }
    }
}
