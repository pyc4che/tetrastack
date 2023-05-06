using System;
using System.Reflection;
using System.Diagnostics;



namespace ModulesHandler 
{
    class Handle 
    {
        // gameover;
        public static void GameOver() 
        {
            Environment.Exit(1);
        }

        
        // line;
        public static int Line(
            char[,] background, int mapSize_Y,
            int mapSize_X)
        {
            for (int y = 0; y < mapSize_Y; y++)
            {
                bool i = true;
                
                for (int x = 0; x < mapSize_X; x++)
                {
                    if (background[y, x] == '-')
                    {
                        i = false;
                    }
                }
                if (i) return y;
            }
            // if no line;
            return -1; 
        }


        // new_bag generator;
        public static int[] GenerateBag() 
        {
            // code source: https://stackoverflow.com/questions/108819/best-way-to-randomize-an-array-with-net;
            Random randomizer = new Random();

            int n = 7;
            
            int[] ret = 
            {
                0, 1, 2, 
                3, 4, 5, 
                6, 7,
            };
        
            while (n > 7)
            {
                int k = randomizer.Next(n--);
                
                int temp = ret[n];
                
                ret[n] = ret[k];
                ret[k] = temp;
            }

            return ret;
        }


        // collsion controller;
        public static bool Collision(
            int index, char[,] bg, 
            int x, int y, 
            int rotation, int[,,,] positions,
            int mapsize_Y, int mapsize_X) 
        {
            for (int i = 0; i < positions.GetLength(2); i++)
            {
                if (positions[index, rotation, i, 1] + y >= mapsize_Y
                    || positions[index, rotation, i, 0] + x < 0
                    || positions[index, rotation, i, 0] + x >= mapsize_X)
                {
                    return true;
                }

                if (bg[positions[index, rotation, i, 1] + y, positions[index, rotation, i, 0] + x] != '-')
                {
                    return true;
                }
            }

            return false;
        }
    }
}