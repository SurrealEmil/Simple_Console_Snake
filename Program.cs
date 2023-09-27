using System;
using System.Threading;

namespace Simple_Console_Snake
{
    internal class Snake
    {
        int Height = 20;
        int Width = 30;

        int[] X = new int[50];
        int[] Y = new int[50];

        int fruitX;
        int fruitY;

        int parts = 3;
        int delay = 100;

        ConsoleKeyInfo keyInfo = new ConsoleKeyInfo();
        char key = '\0'; // Initial direction set to no movement.
        bool isPaused = false;
        bool canCollideWithSelf = true;

        char[,] screen = new char[30, 20];

        Random rnd = new Random();

        public Snake()
        {
            InitializeGame();
            Console.CursorVisible = false;
        }

        public void InitializeGame()
        {
            // Initialize the snake at a valid starting position.
            X[0] = Width / 2;
            Y[0] = Height / 2;
            // Initialize the fruit at a random position.
            fruitX = rnd.Next(1, Width - 1);
            fruitY = rnd.Next(1, Height - 1);
        }

        public void WhiteBoard()
        {
            // Draw the visible border.
            for (int i = 0; i < Width; i++)
            {
                Console.SetCursorPosition(i, 0);
                Console.Write("#");
                Console.SetCursorPosition(i, Height - 1);
                Console.Write("#");
            }

            for (int i = 0; i < Height; i++)
            {
                Console.SetCursorPosition(0, i);
                Console.Write("#");
                Console.SetCursorPosition(Width - 1, i);
                Console.Write("#");
            }
        }

        public void Input()
        {
            if (Console.KeyAvailable)
            {
                keyInfo = Console.ReadKey(true);
                key = char.ToLower(keyInfo.KeyChar);
                if (isPaused && (key == 'w' || key == 's' || key == 'a' || key == 'd'))
                {
                    isPaused = false;
                }
            }
        }

        public void WritePoint(int x, int y)
        {
            Console.SetCursorPosition(x, y);
            Console.Write("#");
        }

        public bool IsCollision(int x, int y)
        {
            // Check for collision with the border.
            if (x <= 0 || x >= Width - 1 || y <= 0 || y >= Height - 1)
                return true;

            for (int i = 1; i < parts; i++)
            {
                if (x == X[i] && y == Y[i])
                    return true;
            }
            return false;
        }

        public void Logic()
        {
            if (!canCollideWithSelf && isPaused)
            {
                // Allow the snake to move without collisions when the game is paused.
                canCollideWithSelf = true;
            }

            if (X[0] == fruitX && Y[0] == fruitY)
            {
                parts++;
                fruitX = rnd.Next(1, Width - 1);
                fruitY = rnd.Next(1, Height - 1);
                // Reduce delay as parts are added.
                delay = Math.Max(10, delay - 5);
            }

            Console.SetCursorPosition(X[parts - 1], Y[parts - 1]);
            Console.Write(" ");

            for (int i = parts - 1; i > 0; i--)
            {
                X[i] = X[i - 1];
                Y[i] = Y[i - 1];
            }
            if (!isPaused)
            {
                switch (key)
                {
                    case 'w':
                        Y[0]--;
                        break;
                    case 's':
                        Y[0]++;
                        break;
                    case 'd':
                        X[0]++;
                        break;
                    case 'a':
                        X[0]--;
                        break;
                }

                // Check for collision with the border.
                if (X[0] <= 0) X[0] = 1;
                if (X[0] >= Width - 1) X[0] = Width - 2;
                if (Y[0] <= 0) Y[0] = 1;
                if (Y[0] >= Height - 1) Y[0] = Height - 2;
            }

            if (canCollideWithSelf && IsCollision(X[0], Y[0]))
            {
                GameOver();
                return;
            }

            // Clear the previous snake position
            //Console.SetCursorPosition(X[parts], Y[parts]);
            //Console.Write(" ");

            WritePoint(X[0], Y[0]);
            WritePoint(fruitX, fruitY);

            Thread.Sleep(delay);
        }

        public void GameOver()
        {
            Console.Clear();
            Console.SetCursorPosition(Width / 2 - 5, Height / 2);
            Console.WriteLine("GameOver");
            Console.SetCursorPosition(Width / 2 - 11, Height / 2 + 1);
            Console.WriteLine($"\tYour Score: {parts - 3}");
            Console.ReadKey();
            Environment.Exit(0);
        }

        static void Main(string[] args)
        {
            Snake snake = new Snake();
            bool gameStarted = false;

            while (true)
            {
                snake.WhiteBoard();
                if (!gameStarted)
                {
                    Console.SetCursorPosition(snake.Width / 2 - 14, snake.Height / 2);
                    Console.Write("Press w, a, s, or d to start");
                    if (Console.KeyAvailable)
                    {
                        snake.keyInfo = Console.ReadKey(true);
                        snake.key = char.ToLower(snake.keyInfo.KeyChar);
                        if (snake.key == 'w' || snake.key == 's' || snake.key == 'a' || snake.key == 'd')
                        {
                            gameStarted = true;
                            Console.Clear();
                        }
                    }
                }
                else
                {
                    snake.Input();
                    snake.Logic();
                }
            }
        }
    }
}

