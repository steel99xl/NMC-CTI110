using FinchAPI;
using System;
using System.Collections.Generic;

namespace CommandArray
{
    // *************************************************************
    // add comment block here
    // *************************************************************

    /// <summary>
    /// control commands for the finch robot
    /// </summary>
    public enum FinchCommand
    {
        DONE,
        MOVEFORWARD,
        MOVEBACKWARD,
        STOPMOTORS,
        DELAY,
        TURNRIGHT,
        TURNLEFT,
        LEDON,
        LEDOFF
    }

    class Program
    {
        static void Main(string[] args)
        {
            Finch myFinch = new Finch();

            DisplayOpeningScreen();
            DisplayInitializeFinch(myFinch);
            DisplayMainMenu(myFinch);
            DisplayClosingScreen(myFinch);
        }

        /// <summary>
        /// display the main menu
        /// </summary>
        /// <param name="myFinch">Finch object</param>
        /// 
        static void DisplayMainMenu(Finch myFinch)
        {
            string menuChoice;
            bool exiting = false;

            int delayDuration = 0;
            int motorSpeed = 0;
            int LEDBrightness = 0;
            //FinchCommand[] commands = null;
            List<FinchCommand> commands = new List<FinchCommand>();

            while (!exiting)
            {
                //
                // display menu
                //
                Console.Clear();
                Console.WriteLine();
                Console.WriteLine("Main Menu");
                Console.WriteLine();

                Console.WriteLine("\t1) Get Command Parameters");
                Console.WriteLine("\t2) Get Finsh Robot Commands");
                Console.WriteLine("\t3) Display Finch Robot Commands");
                Console.WriteLine("\t4) Exicute");
                Console.WriteLine("\tE) Exit");
                Console.WriteLine();
                Console.Write("Enter Choice:");
                menuChoice = Console.ReadLine();

                //
                // process menu
                //
                switch (menuChoice)
                {
                    case "1":
                        delayDuration = DisplayGetDellayDuration();
                        motorSpeed = DisplayGetMotorSpeed();
                        LEDBrightness = DisplayGetLEDBrightness();

                        break;
                    case "2":

                        DisplayGetFinchCommands(commands);

                        break;
                    case "3":
                        DisplayFinchCommands(commands);

                        break;

                    case "4":
                        DisplayExicuteFinchCommands(myFinch, commands, delayDuration, motorSpeed, LEDBrightness);
                        break;

                    case "e":
                    case "E":
                        exiting = true;
                        break;
                    default:
                        break;
                }
            }
        }

        private static void DisplayExicuteFinchCommands(Finch myFinch, List<FinchCommand> commands, int delayDuration, int motorSpeed, int lEDBrightness)
        {
            DisplayHeader("Execute Finch Commands");

            Console.WriteLine("Press any key to execute commands");
            Console.ReadKey();

            for (int i = 0; i < commands.Count; i++)
            {
                Console.WriteLine("Command : "+commands[i]);

                switch (commands[i])
                {
                    case FinchCommand.DONE:
                        break;
                    case FinchCommand.MOVEFORWARD:
                        myFinch.setMotors(motorSpeed, motorSpeed);
                        break;
                    case FinchCommand.MOVEBACKWARD:
                        myFinch.setMotors(-motorSpeed, -motorSpeed);
                        break;
                    case FinchCommand.STOPMOTORS:
                        myFinch.setMotors(0, 0);
                        break;
                    case FinchCommand.DELAY:
                        myFinch.wait(delayDuration);
                        break;
                    case FinchCommand.TURNRIGHT:
                        myFinch.setMotors(motorSpeed, (motorSpeed / 2));
                        break;
                    case FinchCommand.TURNLEFT:
                        myFinch.setMotors((motorSpeed / 2), motorSpeed);
                        break;
                    case FinchCommand.LEDON:
                        myFinch.setLED(lEDBrightness, lEDBrightness, lEDBrightness);
                        break;
                    case FinchCommand.LEDOFF:
                        myFinch.setLED(0, 0, 0);
                        break;

                    default:
                        break;
                }
            }


            DisplayContinuePrompt();
        }

        private static void DisplayGetFinchCommands(List<FinchCommand> commands)
        {
            FinchCommand command;
            bool getCommands = true;
            int x = 0;

            DisplayHeader("Get Finch Commands");
            Console.WriteLine("Enter DONE to finish entering commands");

            while (getCommands) 
            {
                x += 1;
                Console.Write("Command " + (x) + ":");
                Enum.TryParse(Console.ReadLine().ToUpper(), out command);
                commands.Add(command);
                if (command.ToString() == "DONE")
                {
                    getCommands = false;
                }
            }
            Console.WriteLine("The commands");

            foreach (FinchCommand finchcommand  in commands)
            {
                Console.WriteLine(finchcommand);
            }

            DisplayContinuePrompt();
        }

        private static int DisplayGetDellayDuration()
        {
            string userResponce;
            DisplayHeader("Length of delay");

            Console.Write("Enter legth of delay (milliseconds) : ");
            userResponce = Console.ReadLine();
            int.TryParse(userResponce, out int delayDuration);

            DisplayContinuePrompt();
            return delayDuration;
        }

        static void DisplayFinchCommands(List<FinchCommand> commands)
        {
            DisplayHeader("Finch commands");

           Console.WriteLine("The commands");

            Console.WriteLine("The commands");

            foreach (FinchCommand finchcommand in commands)
            {
                Console.WriteLine(finchcommand);
            }

            DisplayContinuePrompt();
        }

        /// <summary>
        /// initialize and confirm the finch connects
        /// </summary>
        /// <param name="myFinch"></param>
        static void DisplayInitializeFinch(Finch myFinch)
        {
            DisplayHeader("Initialize the Finch");

            Console.WriteLine("Please plug your Finch Robot into the computer.");
            Console.WriteLine();
            DisplayContinuePrompt();

            while (!myFinch.connect())
            {
                Console.WriteLine("Please confirm the Finch Robot is connect");
                DisplayContinuePrompt();
            }

            FinchConnectedAlert(myFinch);
            Console.WriteLine("Your Finch Robot is now connected");

            DisplayContinuePrompt();
        }

        /// <summary>
        /// audio notification that the finch is connected
        /// </summary>
        /// <param name="myFinch">Finch object</param>
        static void FinchConnectedAlert(Finch myFinch)
        {
            myFinch.setLED(0, 255, 0);

            for (int frequency = 17000; frequency > 100; frequency -= 100)
            {
                myFinch.noteOn(frequency);
                myFinch.wait(10);
            }

            myFinch.noteOff();
        }

        /// <summary>
        /// display opening screen
        /// </summary>
        static void DisplayOpeningScreen()
        {
            Console.WriteLine();
            Console.WriteLine("\tProgram Your Finch");
            Console.WriteLine();

            DisplayContinuePrompt();
        }

        /// <summary>
        /// display closing screen and disconnect finch robot
        /// </summary>
        /// <param name="myFinch">Finch object</param>
        static void DisplayClosingScreen(Finch myFinch)
        {
            Console.Clear();
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("\t\tThank You!");
            Console.WriteLine();

            myFinch.disConnect();

            DisplayContinuePrompt();
        }

        #region HELPER  METHODS

        /// <summary>
        /// display header
        /// </summary>
        /// <param name="header"></param>
        static void DisplayHeader(string header)
        {
            Console.Clear();
            Console.WriteLine();
            Console.WriteLine("\t\t" + header);
            Console.WriteLine();
        }

        /// <summary>
        /// display the continue prompt
        /// </summary>
        static void DisplayContinuePrompt()
        {
            Console.WriteLine();
            Console.WriteLine("Press any key to continue.");
            Console.ReadKey();
        }

        static int DisplayGetMotorSpeed()
        {
            //motor speed

            int motorSpeed;
            string userResponse;

            DisplayHeader("Motor Speed");

            Console.Write("Enter Motor Speed (-255 ~ 255):");
            userResponse = Console.ReadLine();

            motorSpeed = int.Parse(userResponse);

            return motorSpeed;
        }
        static int DisplayGetLEDBrightness()
        {
            //motor speed

            int LEDBrightness;
            string userResponse;

            DisplayHeader("LED Brightness");

            Console.Write("Enter LED Brightness (0 ~ 255):");
            userResponse = Console.ReadLine();

            LEDBrightness = int.Parse(userResponse);

            return LEDBrightness;
        }

        #endregion
    }
}
