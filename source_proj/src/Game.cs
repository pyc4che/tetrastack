using ModulesHandler;


namespace Tetris
{
    class Game 
    {
        // init map and bg;
        const int mapSize_X = 10;
        const int mapSize_Y = 20;
        static char[,] background = new char[mapSize_Y, mapSize_X];

        // game;
        static int score = 0;
    
        // holding info;
        const int holdSize_X = 6; 
        const int holdSize_Y = mapSize_Y;
        
        static char hold_char;
        static int hold_index = -1;

        const int upNextSize = 6;

        static ConsoleKeyInfo onpressed;

        // moving info;
        static int current_X = 0;
        static int current_Y = 0;

        static char current_char = 'O';
        static int current_rotation = 0;

        // blocks and bags;
        static int[] bag = {

        };
        static int[] next_bag = {
            
        };

        static int bag_index;
        static int current_index;

        // timer;
        static int timer = 0;
        static int amount = 0;

        static int maximum_time = 20;
    
        // block region;
        #region block_assets

        readonly static string characters = "OILJSZT";
        readonly static int[,,,] pos = 
        {
            {
                {{0,0},{1,0},{0,1},{1,1}},
                {{0,0},{1,0},{0,1},{1,1}},
                {{0,0},{1,0},{0,1},{1,1}},
                {{0,0},{1,0},{0,1},{1,1}},
            },

            {
                {{2,0},{2,1},{2,2},{2,3}},
                {{0,2},{1,2},{2,2},{3,2}},
                {{1,0},{1,1},{1,2},{1,3}},
                {{0,1},{1,1},{2,1},{3,1}},
            },

            {
                {{1,0},{1,1},{1,2},{2,2}},
                {{1,2},{1,1},{2,1},{3,1}},
                {{1,1},{2,1},{2,2},{2,3}},
                {{2,1},{2,2},{1,2},{0,2}},
            },

            {
                {{2,0},{2,1},{2,2},{1,2}},
                {{1,1},{1,2},{2,2},{3,2}},
                {{2,1},{1,1},{1,2},{1,3}},
                {{0,1},{1,1},{2,1},{2,2}},
            },

            {
                {{2,1},{1,1},{1,2},{0,2}},
                {{1,0},{1,1},{2,1},{2,2}},
                {{2,1},{1,1},{1,2},{0,2}},
                {{1,0},{1,1},{2,1},{2,2}},
            },

            {
                {{0,1},{1,1},{1,2},{2,2}},
                {{1,0},{1,1},{0,1},{0,2}},
                {{0,1},{1,1},{1,2},{2,2}},
                {{1,0},{1,1},{0,1},{0,2}},
            },

            {
                {{0,1},{1,1},{1,0},{2,1}},
                {{1,0},{1,1},{2,1},{1,2}},
                {{0,1},{1,1},{1,2},{2,1}},
                {{1,0},{1,1},{0,1},{1,2}},
            },
        };

        #endregion


        // generate new block;
        static void NewBlock() 
        {
            if (bag_index >= 7)
            {
                bag_index = 0;
                bag = next_bag;
                next_bag = Handle.GenerateBag();
            }

            current_Y = 0;
            current_X = 4;

            current_char = characters[bag[bag_index]];
            current_index = bag[bag_index];

            if (Handle.Collision(
                current_index, background, 
                current_X, current_Y,
                current_rotation, pos,
                mapSize_Y, mapSize_X) && amount > 0)
            {
                Console.Clear();


                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine(
                    "\n ░█▀▀▀░█▀▀▄░█▀▄▀█░█▀▀░░░▄▀▀▄░▄░░░▄░█▀▀░█▀▀▄\n ░█░▀▄░█▄▄█░█░▀░█░█▀▀░░░█░░█░░█▄█░░█▀▀░█▄▄▀\n ░▀▀▀▀░▀░░▀░▀░░▒▀░▀▀▀░░░░▀▀░░░░▀░░░▀▀▀░▀░▀▀");
                Console.WriteLine();
                
                Thread.Sleep(3000);

                Console.ResetColor();
                Handle.GameOver();
            }

            bag_index++;
            amount++;
        }


        // block collision
        static void BlockDownCollision()
        {
            // add blocks
            for (int i = 0; i < pos.GetLength(2); i++)
            {
                background[pos[current_index, current_rotation, i, 1] + current_Y, pos[current_index, current_rotation, i, 0] + current_X] = current_char;
            }

            while (true)
            {
                // check line
                int line_Y = Handle.Line(
                    background, mapSize_Y, 
                    mapSize_X);

                // line detected
                if (line_Y != -1)
                {
                    ClearLine(line_Y);
                    continue;
                }

                break;
            }

            // new block
            NewBlock();
        }


        // clear line;
        static void ClearLine(int line_Y)
        {
            score += 50;

            // clear line;
            for (int x = 0; x < mapSize_X; x++)
            {
                background[line_Y, x] = '-';
            }

            // all block above line 
            for (int y = line_Y - 1; y > 0; y--)
            {
                for (int x = 0; x < mapSize_X; x++)
                {
                    // move
                    char character = background[y, x];
                    
                    if (character != '-')
                    {
                        background[y, x] = '-';
                        background[y + 1, x] = character;
                    }
                }
            }
        }


        // render view; 
        static char[,] RenderView()
        {
            char[,] view = new char[mapSize_Y, mapSize_X];

            // view == background
            for (int y = 0; y < mapSize_Y; y++)
            {
                for (int x = 0; x < mapSize_X; x++)
                {
                    view[y, x] = background[y, x];
                }
            }

            // overlay
            for (int i = 0; i < pos.GetLength(2); i++)
            {
                view[pos[current_index, current_rotation, i, 1] + current_Y, pos[current_index, current_rotation, i, 0] + current_X] = current_char;
            }

            return view;
        }


        // render hold;
        static char[,] RenderHold()
        {
            char[,] hold = new char[holdSize_Y, holdSize_X];

            // hold = ' ' array;
            for (int y = 0; y < holdSize_Y; y++)
            {
                for (int x = 0; x < holdSize_X; x++)
                {
                    hold [y, x] = ' ';
                }
            }

            // check for held block;
            if (hold_index != -1)
            {
                // overlay;
                for (int i = 0; i < pos.GetLength(2); i++)
                {
                    hold[pos[hold_index, 0, i, 1] + 1, pos[hold_index, 0, i, 0] + 1] = hold_char;
                }
            }

            return hold;
        }


        // render up-next;
        static char[,] RenderUpNext()
        {
            // next = ' ' array;
            char[,] next = new char[mapSize_Y, upNextSize];

            for (int y = 0; y < mapSize_Y; y++)
            {
                for (int x = 0; x < upNextSize; x++)
                {
                    next[y, x] = ' ';
                }
            }

            int next_bag_index = 0;

            for (int i = 0; i < 3; i++) // next 3 blocks; 
            {
                for (int l = 0; l < pos.GetLength(2); l++)
                {
                    if (i + bag_index >= 7) // need to access next bag;
                    {
                        next[pos[next_bag[next_bag_index], 0, l, 1] + 5 * i, pos[next_bag[next_bag_index], 0, l, 0] + 1] = characters[next_bag[next_bag_index]];
                    }

                    else
                    {
                        next[pos[bag[bag_index + i], 0, l, 1] + 5 * i, pos[bag[bag_index + i], 0, l, 0] + 1] = characters[bag[bag_index + i]];
                    }
                }
                
                if (i + bag_index >= 7) next_bag_index++;
            }

            return next;
        }


        // print block;
        static void Print(
            char[,] view, char[,] hold, 
            char[,] next)
        {
            for (int y = 0; y < mapSize_Y; y++)
            {
                for (int x = 0; x < holdSize_X + mapSize_X + upNextSize; x++)
                {
                    char i = ' ';

                    if (x < holdSize_X) i = hold[y, x]; 
                    
                    else if (x >= holdSize_X + mapSize_X) i = next[y, x - mapSize_X - upNextSize];

                    else i = view[y, x - holdSize_X];
                
                    // colors;
                    switch (i)
                    {
                        case 'O':
                            Console.ForegroundColor = ConsoleColor.Red;
                        Console.Write(i);
                            break;
                        
                        case 'I':
                            Console.ForegroundColor = ConsoleColor.Blue;
                            Console.Write(i);
                            break;

                        case 'T':
                            Console.ForegroundColor = ConsoleColor.Cyan;
                            Console.Write(i);
                            break;

                        case 'S':
                            Console.ForegroundColor = ConsoleColor.DarkMagenta;
                            Console.Write(i);
                            break;
                        
                        case 'Z':
                            Console.ForegroundColor = ConsoleColor.DarkCyan;
                            Console.Write(i);
                            break;
                        
                        case 'L':
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.Write(i);
                            break;

                        
                        case 'J':
                            Console.ForegroundColor = ConsoleColor.DarkCyan;
                            Console.Write(i);
                            break;
                        
                        default:
                            Console.ForegroundColor = ConsoleColor.White;
                            Console.Write(i);
                            break;
                    }
                }

                if (y == 1)
                {
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.Write($"   {score}");
                }
                Console.WriteLine();
            }

            // reset cursor
            Console.SetCursorPosition(0, Console.CursorTop - mapSize_Y);
        }


        // OnPressedKey_Handler;
        static void InputHandler() 
        {
            // check input;
            switch (onpressed.Key)
            {
                // left arrow, 'A' key (if no Handle.Collision expected);
                case ConsoleKey.A:
                case ConsoleKey.LeftArrow:
                    if(!Handle.Collision(
                        current_index, background, 
                        current_X - 1, current_Y,
                        current_rotation, pos,
                        mapSize_Y, mapSize_X)) { current_X -= 1; }
                    break;

                // right arrow, 'D' key (if no Handle.Collision expected);
                case ConsoleKey.D:
                case ConsoleKey.RightArrow:
                    if(!Handle.Collision(
                        current_index, background, 
                        current_X + 1, current_Y,
                        current_rotation, pos,
                        mapSize_Y, mapSize_X)) { current_X += 1; }
                    break;

                // up arrow, 'W' key (rotate block, if no Handle.Collision expected);
                case ConsoleKey.W:
                case ConsoleKey.UpArrow:
                    int new_rotation = current_rotation + 1;
                    
                    if (new_rotation >= 4) { new_rotation = 0; }
                    
                    if(!Handle.Collision(
                        current_index, background, 
                        current_X, current_Y,
                        new_rotation, pos,
                        mapSize_Y, mapSize_X)) { current_rotation = new_rotation; }
                    break;

                // down arrow, 'S' key (move down faster);
                case ConsoleKey.S:
                case ConsoleKey.DownArrow:
                    timer = maximum_time;
                    break;              

                // instnatly drop block down (hard fall);
                case ConsoleKey.Spacebar:
                    int i = 0;

                    while (true) 
                    {
                        i++;
                        if (Handle.Collision(
                        current_index, background, 
                        current_X, current_Y + i,
                        current_rotation, pos,
                        mapSize_Y, mapSize_X)) 
                        {
                            current_Y += i - 1;
                            break;
                        }
                    }
                    
                    score += i + 1;
                    break;

                // close session;
                case ConsoleKey.Escape:
                    Console.Clear();
                    Environment.Exit(1);
                    break;

                // freeze block;
                case ConsoleKey.Enter:
                    
                    // if no current block;
                    if (hold_index == -1)
                    {
                        hold_char = current_char;
                        hold_index = current_index;
                        NewBlock();
                    }

                    // if there is current block;
                    else 
                    {
                        if (!Handle.Collision(
                            hold_index, background, 
                            current_X, current_Y,
                            0, pos,
                            mapSize_Y, mapSize_X)) // Check for collision;
                        {

                            // Switch current and hold;
                            int cindex = current_index;
                            char cchar = current_char;
                            current_index = hold_index;
                            current_char = hold_char;
                            hold_index = cindex;
                            hold_char = cchar;
                        }
                    }
                    break;

                default:
                    break;
            }
        }
        

        // get input;
        static void Input()
        {
            while (true)
            {
                onpressed = Console.ReadKey(true);
            }
        }
    

        // launch processes;
        public static void Launch()
        {
            // threading
            Thread threader = new Thread(Input);
            threader.Start();

            // generate bag/block
            bag = Handle.GenerateBag();
            next_bag = Handle.GenerateBag();
            NewBlock();

            // generate empty bg
            for (int y = 0; y < mapSize_Y; y++)
            {
                for (int x = 0; x < mapSize_X; x++)
                {
                    background[y, x] = '-';
                }  
            }

            while (true)
            {
                // force down;
                if (timer >= maximum_time)
                {
                    // if no collision move down
                    if (!Handle.Collision(
                        current_index, background, 
                        current_X, current_Y + 1,
                        current_rotation, pos,
                        mapSize_Y, mapSize_X))
                    {
                        current_Y++;
                    }

                    else
                    {
                        BlockDownCollision();
                    }

                    timer = 0;
                }

                timer++;
    
                // input;
                InputHandler();
                onpressed = new ConsoleKeyInfo();

                // render current;
                char[,] view = RenderView();

                // render hold; 
                char[,] hold = RenderHold();

                // render up-next;
                char[,] next = RenderUpNext();

                // print view;
                Print(
                    view, hold,
                    next);

                Thread.Sleep(20);
            } 
        }
    }
}