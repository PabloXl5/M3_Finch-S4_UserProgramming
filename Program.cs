using FinchAPI;
using System;
using System.IO;
using System.Collections.Generic;

namespace Project_FinchControl
{

    // **************************************************
    //
    // Title: Finch Control - Menu Starter
    // Description: Starter solution with the helper methods,
    //              opening and closing screens, and the menu
    // Application Type: Console
    // Author: Velis, John
    // Dated Created: 1/22/2020
    // Last Modified: 1/25/2020
    //
    // **************************************************

    /// <summary>
    /// User Commands
    /// </summary>

    public enum Command
    {
        NONE,
        MOVEFORWARD,
        MOVEBACKWARD,
        STOPMOTORS,
        WAIT,
        TURNRIGHT,
        TURNLEFT,
        LEDON,
        LEDOFF,
        GETTEMPERATURE,
        DONE
    }

    class Program
    {
        /// <summary>
        /// first method run when the app starts up
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            // SetTheme();
            DisplayLoginRegister();
            //DisplayWelcomeScreen();
            DisplayMenuScreen();
            DisplayClosingScreen();
        }


        #region  LOGIN/PASSWORD
        /// <summary>
        /// *****************************************************************
        /// *                 Login/Register Screen                         *
        /// *****************************************************************
        /// </summary>
        static void DisplayLoginRegister()
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.BackgroundColor = ConsoleColor.Black;

            DisplayScreenHeader("\tWelcome to Finch Control\n\n\n");

            Console.WriteLine("\t\t\tPlease Login/register\n\n");

            Console.Write("\t\tAre you a registered user [ yes | no ]?");
            if (Console.ReadLine().ToLower() == "yes")
            {
                DisplayLogin();
            }
            else
            {
                DisplayRegisterUser();
                DisplayLogin();
            }
        }



        /// <summary>
        /// *****************************************************************
        /// *                          Login Screen                         *
        /// *****************************************************************
        /// </summary>
        static void DisplayLogin()
        {
            string userName;
            string password;
            string userResponse;
            bool validLogin;

            do
            {
                DisplayScreenHeader("Login");

                Console.WriteLine();
                Console.Write("\tEnter your user name:");
                userName = Console.ReadLine();
                Console.Write("\tEnter your password:");
                password = Console.ReadLine();

                validLogin = IsValidLoginInfo(userName, password);

                Console.WriteLine();
                if (validLogin)
                {
                    Console.WriteLine("\tYou are now logged in.");
                }
                else
                {
                    Console.WriteLine("\tIt appears either the user name or password is incorrect.");
                    Console.WriteLine("\tPlease try again.");
                    //userResponse = Console.ReadLine().ToLower();

                    //breakloop();
                }

                DisplayContinuePrompt();
            } while (!validLogin);
        }

        //static void breakloop()
        //{
        //    if (userResponse == "exit") ;
        //    {
        //        DisplayContinuePrompt();
        //    }
        //}

        /// <summary>
        /// check user login
        /// </summary>
        /// <param name="userName">user name entered</param>
        /// <param name="password">password entered</param>
        /// <returns>true if valid user</returns>
        static bool IsValidLoginInfo(string userName, string password)
        {
            List<(string userName, string password)> registeredUserLoginInfo = new List<(string userName, string password)>();
            bool validUser = false;

            registeredUserLoginInfo = ReadLoginInfoData();

            //
            // loop through the list of registered user login tuples and check each one against the login info
            //
            foreach ((string userName, string password) userLoginInfo in registeredUserLoginInfo)
            {
                if ((userLoginInfo.userName == userName) && (userLoginInfo.password == password))
                {
                    validUser = true;
                    break;
                }
            }

            return validUser;
        }

        /// <summary>
        /// *****************************************************************
        /// *                       Register Screen                         *
        /// *****************************************************************
        /// write login info to data file
        /// </summary>
        static void DisplayRegisterUser()
        {
            string userName;
            string password;

            DisplayScreenHeader("Register");

            Console.Write("\tEnter your user name:");
            userName = Console.ReadLine();
            Console.Write("\tEnter your password:");
            password = Console.ReadLine();

            WriteLoginInfoData(userName, password);

            Console.WriteLine();
            Console.WriteLine("\tYou entered the following information and it has be saved.");
            Console.WriteLine($"\tUser name: {userName}");
            Console.WriteLine($"\tPassword: {password}");

            DisplayContinuePrompt();
        }

        /// <summary>
        /// read login info from data file
        /// Note: no error or validation checking
        /// </summary>
        /// <returns>list of tuple of user name and password</returns>
        static List<(string userName, string password)> ReadLoginInfoData()
        {
            string dataPath = @"Data/LoginData.txt";

            string[] loginInfoArray;
            (string userName, string password) loginInfoTuple;

            List<(string userName, string password)> registeredUserLoginInfo = new List<(string userName, string password)>();

            loginInfoArray = File.ReadAllLines(dataPath);

            //
            // loop through the array
            // split the user name and password into a tuple
            // add the tuple to the list
            //
            foreach (string loginInfoText in loginInfoArray)
            {
                //
                // use the Split method to separate the user name and password into an array
                //
                loginInfoArray = loginInfoText.Split(',');

                loginInfoTuple.userName = loginInfoArray[0];
                loginInfoTuple.password = loginInfoArray[1];

                registeredUserLoginInfo.Add(loginInfoTuple);

            }

            return registeredUserLoginInfo;
        }

        /// <summary>
        /// write login info to data file
        /// Note: no error or validation checking
        /// </summary>
        static void WriteLoginInfoData(string userName, string password)
        {
            string dataPath = @"Data/LoginData.txt";
            string loginInfoText;

            loginInfoText = userName + "," + password + "\n";
            //
            // use the AppendAllText method to not overwrite the existing logins
            //
            File.AppendAllText(dataPath, loginInfoText);
        }
        #endregion

        /// <summary>
        ///  Main Menu          
        /// </summary>
        static void DisplayMenuScreen()
        {
            (ConsoleColor foregroundColor, ConsoleColor backgroundColor) themeColors;

            themeColors = ReadThemeData();

            Console.ForegroundColor = themeColors.foregroundColor;
            Console.BackgroundColor = themeColors.backgroundColor;
            Console.CursorVisible = true;

            bool quitApplication = false;
            string menuChoice;

            Finch finchRobot = new Finch();

            do
            {
                DisplayScreenHeader("\t\tMain Menu\n\n");

                //
                // get user menu choice
                //
                Console.WriteLine("\ta) Connect Finch Robot");
                Console.WriteLine("\tb) Set Theme");
                Console.WriteLine("\tc) Talent Show");
                Console.WriteLine("\td) Data Recorder");
                Console.WriteLine("\te) Alarm System");
                Console.WriteLine("\tf) User Programming");
                Console.WriteLine("\tg) Disconnect Finch Robot");
                Console.WriteLine("\tq) Quit");
                Console.WriteLine();
                Console.Write("\t\tEnter Choice:");
                menuChoice = Console.ReadLine().ToLower();

                //
                // process user menu choice
                //
                switch (menuChoice)
                {
                    case "a":
                        DisplayConnectFinchRobot(finchRobot);
                        break;

                    case "b":
                        DisplaySetThemeMenuScreen(finchRobot);
                        break;

                    case "c":
                        DisplayTalentShowMenuScreen(finchRobot);
                        break;

                    case "d":
                        DataRecorderDisplayMenuScreen(finchRobot);
                        break;

                    case "e":
                        LightAlarmDisplayMenuScreen(finchRobot);
                        break;

                    case "f":
                        UserProgrammingDisplayMenuScreen(finchRobot);
                        break;

                    case "g":
                        DisplayDisconnectFinchRobot(finchRobot);
                        break;

                    case "q":
                        DisplayQuit(finchRobot);
                        quitApplication = true;
                        break;

                    default:
                        Console.WriteLine();
                        Console.WriteLine("\tPlease enter a letter for the menu choice.");
                        DisplayContinuePrompt();
                        break;
                }

            } while (!quitApplication);
        }

        #region SET THEME

        /// <summary>
        /// setup the console theme
        /// </summary>
        /// 
        static void DisplaySetThemeMenuScreen(Finch finchRobot)
        {
            (ConsoleColor foregroundColor, ConsoleColor backgroundColor) themeColors;
            bool themeChosen = false;

            //
            // set current theme from data
            //
            themeColors = ReadThemeData();
            Console.ForegroundColor = themeColors.foregroundColor;
            Console.BackgroundColor = themeColors.backgroundColor;
            Console.Clear();
            DisplayScreenHeader("Set Application Theme");

            Console.WriteLine($"\tCurrent text color: {Console.ForegroundColor}");
            Console.WriteLine($"\tCurrent background color: {Console.BackgroundColor}");
            Console.WriteLine();

            Console.Write("\tWould you like to change the current theme [ yes | no ]?");
            if (Console.ReadLine().ToLower() == "yes")
            {
                do
                {
                    themeColors.foregroundColor = GetConsoleColorFromUser("text");
                    themeColors.backgroundColor = GetConsoleColorFromUser("background");

                    //
                    // set new theme
                    //
                    Console.ForegroundColor = themeColors.foregroundColor;
                    Console.BackgroundColor = themeColors.backgroundColor;
                    Console.Clear();
       
                   
                    Console.WriteLine($"\tNew text color: {Console.ForegroundColor}");
                    Console.WriteLine($"\tNew background color: {Console.BackgroundColor}");

                    Console.WriteLine();
                    Console.Write("\tIs this the theme you would like?");
                    if (Console.ReadLine().ToLower() == "yes")
                    {
                        themeChosen = true;
                        WriteThemeData(themeColors.foregroundColor, themeColors.backgroundColor);
                    }

                } while (!themeChosen);
            }
            DisplayContinuePrompt();
        }

        /// <summary>
        /// get a console color from the user
        /// </summary>
        /// <param name="property">foreground or background</param>
        /// <returns>user's console color</returns>
        static ConsoleColor GetConsoleColorFromUser(string property)
        {
            ConsoleColor consoleColor;
            bool validConsoleColor;

            do
            {

                DisplayScreenHeader("\tSet Application Theme\n\n");

                Console.WriteLine("\tRed       Green       Yellow       Cyan       Blue       Magenta");
                Console.WriteLine("\tDarkRed   DarkGreen   DarkYellow   DarkCyan   DarkBlue   DarkMagenta");
                Console.WriteLine("\tGray      DrakGray");
                Console.WriteLine("\tBlack");
                Console.WriteLine("\tWhite\n\n");

                Console.Write($"\t\tEnter a value for the {property}:");
                validConsoleColor = Enum.TryParse<ConsoleColor>(Console.ReadLine(), true, out consoleColor);

                if (!validConsoleColor)
                {
                    Console.WriteLine("\n\t***** It appears you did not provide a valid console color. Please try again. *****\n");
                }
                else
                {
                    validConsoleColor = true;
                }

            } while (!validConsoleColor);

            return consoleColor;
        }

        /// <summary>
        /// read theme info from data file
        /// Note: no error or validation checking
        /// </summary>
        /// <returns>tuple of foreground and background</returns>
        static (ConsoleColor foregroundColor, ConsoleColor backgroundColor) ReadThemeData()
        {
            string dataPath = @"Data/SetTheme.txt";
            string[] themeColors;

            ConsoleColor foregroundColor;
            ConsoleColor backgroundColor;

            themeColors = File.ReadAllLines(dataPath);

            Enum.TryParse(themeColors[0], true, out foregroundColor);
            Enum.TryParse(themeColors[1], true, out backgroundColor);

            return (foregroundColor, backgroundColor);
        }

        /// <summary>
        /// write theme info to data file
        /// Note: no error or validation checking
        /// </summary>
        /// <returns>tuple of foreground and background</returns>
        static void WriteThemeData(ConsoleColor text, ConsoleColor background)
        {
            string dataPath = @"Data/SetTheme.txt";

            File.WriteAllText(dataPath, text.ToString() + "\n");
            File.AppendAllText(dataPath, background.ToString());
        }



        #endregion

        #region TALENT SHOW

        /// <summary>
        /// *****************************************************************
        /// *                     Talent Show Menu                          *
        /// *****************************************************************
        /// </summary>
        static void DisplayTalentShowMenuScreen(Finch myFinch)
        {
            Console.CursorVisible = true;

            bool quitTalentShowMenu = false;
            string menuChoice;

            do
            {
                DisplayScreenHeader("Talent Show Menu");

                //
                // get user menu choice
                //
                Console.WriteLine("\ta) Lights and Sounds");
                Console.WriteLine("\tb) On the Move");
                Console.WriteLine("\tc) Patterns of Geometry");
                Console.WriteLine("\td) ");
                Console.WriteLine("\tq) Main Menu");
                Console.Write("\t\tEnter Choice:");
                menuChoice = Console.ReadLine().ToLower();

                //
                // process user menu choice
                //
                switch (menuChoice)
                {
                    case "a":
                        LightsAndSounds(myFinch);
                        break;

                    case "b":
                        OnTheMove(myFinch);
                        break;

                    case "c":

                        break;

                    case "d":

                        break;

                    case "q":
                        quitTalentShowMenu = true;
                        break;

                    default:
                        Console.WriteLine();
                        Console.WriteLine("\tPlease enter a letter for the menu choice.");
                        DisplayContinuePrompt();
                        break;
                }

            } while (!quitTalentShowMenu);
        }

        /// <summary>
        /// *****************************************************************
        /// *               Talent Show > Light and Sound                   *
        /// *****************************************************************
        /// </summary>
        /// <param name="finchRobot">finch robot object</param>
        //static void DisplayLightAndSound(Finch myFinch)
        //{
        //    Console.CursorVisible = false;

        //    DisplayScreenHeader("Light and Sound");

        //    Console.WriteLine("\tThe Finch robot will now show off its glowing talent!");
        //    DisplayContinuePrompt();

        //    for (int lightSoundLevel = 200; lightSoundLevel < 255; lightSoundLevel++)
        //    {
        //        myFinch.setLED(lightSoundLevel, lightSoundLevel, lightSoundLevel);
        //        myFinch.noteOn(lightSoundLevel * 10);
        //    }

        //    DisplayMenuPrompt("Talent Show Menu");
        //}

        static void LightsAndSounds(Finch myFinch)
        {
            Console.CursorVisible = false;

            bool quitLightsAndSoundsMenu = false;
            string menuChoice;

            do
            {
                DisplayScreenHeader("Lights and Sounds Menu");

                //
                // get user menu choice
                //
                Console.WriteLine("\ta) Mario Theme");
                Console.WriteLine("\tb) Mario 3 Theme");
                Console.WriteLine("\tc) ");
                Console.WriteLine("\td) ");
                Console.WriteLine("\tq) Talent Show Menu");
                Console.Write("\t\tEnter Choice:");
                menuChoice = Console.ReadLine().ToLower();

                //
                // process user menu choice
                //
                switch (menuChoice)
                {
                    case "a":
                        MarioOne(myFinch);
                        break;

                    case "b":
                        MarioThree(myFinch);
                        break;

                    case "c":
                        ColorScale(myFinch);
                        break;

                    case "d":

                        break;

                    case "q":
                        quitLightsAndSoundsMenu = true;
                        break;

                    default:
                        Console.WriteLine();
                        Console.WriteLine("\tPlease enter a letter for the menu choice.");
                        DisplayContinuePrompt();
                        break;
                }

            } while (!quitLightsAndSoundsMenu);
        }

        static void ColorScale(Finch myFinch)
        {

        }

        static void MarioOne(Finch myFinch)
        {
            int userColorOne;
            int userColorTwo;
            int userColorThree;
            string userResponse;

            Console.CursorVisible = true;

            Console.WriteLine("Yo Mario, Choose a color!");
            Console.WriteLine("Type a number between 0 - 255 for each section.");
            Console.WriteLine("For example: Red is 255-0-0, Green is 0-255-0, Blue is 0-0-255, Yellow is 255, 255, 0.");

            Console.Write("First Number:");

            userResponse = Console.ReadLine();
            userColorOne = int.Parse(userResponse);

            Console.Write($"Second Number: ({userColorOne},    ,    )");

            userResponse = Console.ReadLine();
            userColorTwo = int.Parse(userResponse);

            Console.Write($"Second Number: ({userColorOne}, {userColorTwo},    )");

            userResponse = Console.ReadLine();
            userColorThree = int.Parse(userResponse);

            Console.WriteLine($" Your Color is ({userColorOne}, {userColorTwo}, {userColorThree})");
            Console.WriteLine("Press any key to continue");
            Console.ReadKey();


            myFinch.setLED(userColorOne, userColorTwo, userColorThree);
            myFinch.noteOn(659);
            myFinch.wait(175);
            myFinch.noteOff();
            myFinch.setLED(0, 0, 0);

            myFinch.setLED(userColorOne, userColorTwo, userColorThree);
            myFinch.noteOn(659);
            myFinch.wait(175);
            myFinch.noteOff();
            myFinch.setLED(0, 0, 0);

            myFinch.wait(125);

            myFinch.setLED(userColorOne, userColorTwo, userColorThree);
            myFinch.noteOn(659);
            myFinch.wait(300);
            myFinch.noteOff();
            myFinch.setLED(0, 0, 0);

            myFinch.wait(167);

            myFinch.setLED(userColorOne, userColorTwo, userColorThree);
            myFinch.noteOn(523);
            myFinch.wait(175);
            myFinch.noteOff();
            myFinch.setLED(0, 0, 0);

            myFinch.setLED(userColorOne, userColorTwo, userColorThree);
            myFinch.noteOn(659);
            myFinch.wait(175);
            myFinch.noteOff();
            myFinch.setLED(0, 0, 0);

            myFinch.wait(125);

            myFinch.setLED(userColorOne, userColorTwo, userColorThree);
            myFinch.noteOn(784);
            myFinch.wait(175);
            myFinch.noteOff();
            myFinch.setLED(0, 0, 0);

            myFinch.wait(375);

            myFinch.setLED(userColorOne, userColorTwo, userColorThree);
            myFinch.noteOn(392);
            myFinch.wait(125);
            myFinch.noteOff();
            myFinch.setLED(0, 0, 0);

            myFinch.wait(375);

            myFinch.setLED(userColorOne, userColorTwo, userColorThree);
            myFinch.noteOn(523);
            myFinch.wait(125);
            myFinch.noteOff();
            myFinch.setLED(0, 0, 0);

            myFinch.wait(25);

            myFinch.setLED(userColorOne, userColorTwo, userColorThree);
            myFinch.noteOn(392);
            myFinch.wait(125);
            myFinch.noteOff();
            myFinch.setLED(0, 0, 0);

            myFinch.wait(25);

            myFinch.setLED(userColorOne, userColorTwo, userColorThree);
            myFinch.noteOn(330);
            myFinch.wait(125);
            myFinch.noteOff();
            myFinch.setLED(0, 0, 0);

            myFinch.wait(250);
            // .250

            myFinch.setLED(userColorOne, userColorTwo, userColorThree);
            myFinch.noteOn(440);
            myFinch.wait(125);
            myFinch.noteOff();
            myFinch.setLED(0, 0, 0);

            myFinch.wait(125);
            // .125

            myFinch.setLED(userColorOne, userColorTwo, userColorThree);
            myFinch.noteOn(494);
            myFinch.wait(125);
            myFinch.noteOff();
            myFinch.setLED(0, 0, 0);

            myFinch.wait(125);
            // .125

            myFinch.setLED(userColorOne, userColorTwo, userColorThree);
            myFinch.noteOn(466);
            myFinch.wait(125);
            myFinch.noteOff();
            myFinch.setLED(0, 0, 0);

            myFinch.wait(42);
            // .42

            myFinch.setLED(userColorOne, userColorTwo, userColorThree);
            myFinch.noteOn(440);
            myFinch.wait(125);
            myFinch.noteOff();
            myFinch.setLED(0, 0, 0);

            myFinch.wait(125);
            // .125

            myFinch.setLED(userColorOne, userColorTwo, userColorThree);
            myFinch.noteOn(392);
            myFinch.wait(125);
            myFinch.noteOff();
            myFinch.setLED(0, 0, 0);

            myFinch.wait(125);
            // .125

            myFinch.setLED(userColorOne, userColorTwo, userColorThree);
            myFinch.noteOn(659);
            myFinch.wait(125);
            myFinch.noteOff();
            myFinch.setLED(0, 0, 0);

            myFinch.wait(125);
            // .125

            myFinch.setLED(userColorOne, userColorTwo, userColorThree);
            myFinch.noteOn(784);
            myFinch.wait(125);
            myFinch.noteOff();
            myFinch.setLED(0, 0, 0);

            myFinch.wait(125);
            // .125

            myFinch.setLED(userColorOne, userColorTwo, userColorThree);
            myFinch.noteOn(880);
            myFinch.wait(125);
            myFinch.noteOff();
            myFinch.setLED(0, 0, 0);

            myFinch.wait(125);
            // .125

            myFinch.setLED(userColorOne, userColorTwo, userColorThree);
            myFinch.noteOn(698);
            myFinch.wait(125);
            myFinch.noteOff();
            myFinch.setLED(0, 0, 0);

            myFinch.setLED(userColorOne, userColorTwo, userColorThree);
            myFinch.noteOn(784);
            myFinch.wait(125);
            myFinch.noteOff();
            myFinch.setLED(0, 0, 0);

            myFinch.wait(125);
            // .125

            myFinch.setLED(userColorOne, userColorTwo, userColorThree);
            myFinch.noteOn(659);
            myFinch.wait(125);
            myFinch.noteOff();
            myFinch.setLED(0, 0, 0);

            myFinch.wait(125);
            // .125

            myFinch.setLED(userColorOne, userColorTwo, userColorThree);
            myFinch.noteOn(523);
            myFinch.wait(125);
            myFinch.noteOff();
            myFinch.setLED(0, 0, 0);

            myFinch.wait(125);
            // .125

            myFinch.setLED(userColorOne, userColorTwo, userColorThree);
            myFinch.noteOn(587);
            myFinch.wait(125);
            myFinch.noteOff();
            myFinch.setLED(0, 0, 0);

            myFinch.setLED(userColorOne, userColorTwo, userColorThree);
            myFinch.noteOn(494);
            myFinch.wait(125);
            myFinch.noteOff();
            myFinch.setLED(0, 0, 0);

            myFinch.wait(125);
            // .125

            myFinch.setLED(userColorOne, userColorTwo, userColorThree);
            myFinch.noteOn(523);
            myFinch.wait(125);
            myFinch.noteOff();
            myFinch.setLED(0, 0, 0);

            myFinch.wait(125);
            // .125

            myFinch.setLED(userColorOne, userColorTwo, userColorThree);
            myFinch.noteOn(392);
            myFinch.wait(125);
            myFinch.noteOff();
            myFinch.setLED(0, 0, 0);

            myFinch.wait(250);
            // .250

            myFinch.setLED(userColorOne, userColorTwo, userColorThree);
            myFinch.noteOn(330);
            myFinch.wait(125);
            myFinch.noteOff();
            myFinch.setLED(0, 0, 0);

            myFinch.wait(250);
            // .250

            myFinch.setLED(userColorOne, userColorTwo, userColorThree);
            myFinch.noteOn(440);
            myFinch.wait(125);
            myFinch.noteOff();
            myFinch.setLED(0, 0, 0);

            myFinch.wait(125);
            // .125

            myFinch.setLED(userColorOne, userColorTwo, userColorThree);
            myFinch.noteOn(494);
            myFinch.wait(125);
            myFinch.noteOff();
            myFinch.setLED(0, 0, 0);

            myFinch.wait(125);
            // .125

            myFinch.setLED(userColorOne, userColorTwo, userColorThree);
            myFinch.noteOn(466);
            myFinch.wait(125);
            myFinch.noteOff();
            myFinch.setLED(0, 0, 0);

            myFinch.wait(42);
            // .42

            myFinch.setLED(userColorOne, userColorTwo, userColorThree);
            myFinch.noteOn(440);
            myFinch.wait(125);
            myFinch.noteOff();
            myFinch.setLED(0, 0, 0);

            myFinch.wait(125);
            // .125

            myFinch.setLED(userColorOne, userColorTwo, userColorThree);
            myFinch.noteOn(392);
            myFinch.wait(125);
            myFinch.noteOff();
            myFinch.setLED(0, 0, 0);

            myFinch.wait(125);
            // .125

            myFinch.setLED(userColorOne, userColorTwo, userColorThree);
            myFinch.noteOn(659);
            myFinch.wait(125);
            myFinch.noteOff();
            myFinch.setLED(0, 0, 0);

            myFinch.wait(125);
            // .125

            myFinch.setLED(userColorOne, userColorTwo, userColorThree);
            myFinch.noteOn(784);
            myFinch.wait(125);
            myFinch.noteOff();
            myFinch.setLED(0, 0, 0);

            myFinch.wait(125);
            // .125

            myFinch.setLED(userColorOne, userColorTwo, userColorThree);
            myFinch.noteOn(880);
            myFinch.wait(125);
            myFinch.noteOff();
            myFinch.setLED(0, 0, 0);

            myFinch.wait(125);
            // .125

            myFinch.setLED(userColorOne, userColorTwo, userColorThree);
            myFinch.noteOn(698);
            myFinch.wait(125);
            myFinch.noteOff();
            myFinch.setLED(0, 0, 0);

            myFinch.setLED(userColorOne, userColorTwo, userColorThree);
            myFinch.noteOn(784);
            myFinch.wait(125);
            myFinch.noteOff();
            myFinch.setLED(0, 0, 0);

            myFinch.wait(125);
            // .125

            myFinch.setLED(userColorOne, userColorTwo, userColorThree);
            myFinch.noteOn(659);
            myFinch.wait(125);
            myFinch.noteOff();
            myFinch.setLED(0, 0, 0);

            myFinch.wait(125);
            // .125

            myFinch.setLED(userColorOne, userColorTwo, userColorThree);
            myFinch.noteOn(523);
            myFinch.wait(125);
            myFinch.noteOff();
            myFinch.setLED(0, 0, 0);

            myFinch.wait(125);
            // .125

            myFinch.setLED(userColorOne, userColorTwo, userColorThree);
            myFinch.noteOn(587);
            myFinch.wait(125);
            myFinch.noteOff();
            myFinch.setLED(0, 0, 0);

            myFinch.setLED(userColorOne, userColorTwo, userColorThree);
            myFinch.noteOn(494);
            myFinch.wait(125);
            myFinch.noteOff();
            myFinch.setLED(0, 0, 0);

            myFinch.wait(375);
            // .375

            myFinch.setLED(userColorOne, userColorTwo, userColorThree);
            myFinch.noteOn(784);
            myFinch.wait(125);
            myFinch.noteOff();
            myFinch.setLED(0, 0, 0);

            myFinch.setLED(userColorOne, userColorTwo, userColorThree);
            myFinch.noteOn(740);
            myFinch.wait(125);
            myFinch.noteOff();
            myFinch.setLED(0, 0, 0);

            myFinch.setLED(userColorOne, userColorTwo, userColorThree);
            myFinch.noteOn(698);
            myFinch.wait(125);
            myFinch.noteOff();
            myFinch.setLED(0, 0, 0);

            myFinch.wait(42);

            myFinch.setLED(userColorOne, userColorTwo, userColorThree);
            myFinch.noteOn(622);
            myFinch.wait(125);
            myFinch.noteOff();
            myFinch.setLED(0, 0, 0);

            myFinch.wait(125);

            myFinch.setLED(userColorOne, userColorTwo, userColorThree);
            myFinch.noteOn(659);
            myFinch.wait(125);
            myFinch.noteOff();
            myFinch.setLED(0, 0, 0);

            myFinch.wait(167);

            myFinch.setLED(userColorOne, userColorTwo, userColorThree);
            myFinch.noteOn(415);
            myFinch.wait(125);
            myFinch.noteOff();
            myFinch.setLED(0, 0, 0);

            myFinch.setLED(userColorOne, userColorTwo, userColorThree);
            myFinch.noteOn(440);
            myFinch.wait(125);
            myFinch.noteOff();
            myFinch.setLED(0, 0, 0);

            myFinch.setLED(userColorOne, userColorTwo, userColorThree);
            myFinch.noteOn(523);
            myFinch.wait(125);
            myFinch.noteOff();
            myFinch.setLED(0, 0, 0);

            myFinch.wait(125);

            myFinch.setLED(userColorOne, userColorTwo, userColorThree);
            myFinch.noteOn(440);
            myFinch.wait(125);
            myFinch.noteOff();
            myFinch.setLED(0, 0, 0);

            myFinch.setLED(userColorOne, userColorTwo, userColorThree);
            myFinch.noteOn(523);
            myFinch.wait(125);
            myFinch.noteOff();
            myFinch.setLED(0, 0, 0);

            myFinch.setLED(userColorOne, userColorTwo, userColorThree);
            myFinch.noteOn(587);
            myFinch.wait(125);
            myFinch.noteOff();
            myFinch.setLED(0, 0, 0);

            myFinch.wait(250);
            //wait .250

            myFinch.setLED(userColorOne, userColorTwo, userColorThree);
            myFinch.noteOn(784);
            myFinch.wait(125);
            myFinch.noteOff();
            myFinch.setLED(0, 0, 0);

            myFinch.setLED(userColorOne, userColorTwo, userColorThree);
            myFinch.noteOn(740);
            myFinch.wait(125);
            myFinch.noteOff();
            myFinch.setLED(0, 0, 0);

            myFinch.setLED(userColorOne, userColorTwo, userColorThree);
            myFinch.noteOn(698);
            myFinch.wait(125);
            myFinch.noteOff();
            myFinch.setLED(0, 0, 0);

            myFinch.wait(42);
            //wait .42

            myFinch.setLED(userColorOne, userColorTwo, userColorThree);
            myFinch.noteOn(622);
            myFinch.wait(125);
            myFinch.noteOff();
            myFinch.setLED(0, 0, 0);

            myFinch.wait(125);
            // .125

            myFinch.setLED(userColorOne, userColorTwo, userColorThree);
            myFinch.noteOn(659);
            myFinch.wait(125);
            myFinch.noteOff();
            myFinch.setLED(0, 0, 0);

            myFinch.wait(167);
            // .167

            myFinch.setLED(userColorOne, userColorTwo, userColorThree);
            myFinch.noteOn(698);
            myFinch.wait(125);
            myFinch.noteOff();
            myFinch.setLED(0, 0, 0);

            myFinch.wait(125);
            //.125

            myFinch.setLED(userColorOne, userColorTwo, userColorThree);
            myFinch.noteOn(698);
            myFinch.wait(125);
            myFinch.noteOff();
            myFinch.setLED(0, 0, 0);

            myFinch.setLED(userColorOne, userColorTwo, userColorThree);
            myFinch.noteOn(698);
            myFinch.wait(125);
            myFinch.noteOff();
            myFinch.setLED(0, 0, 0);

            myFinch.wait(625);
            // .625

            myFinch.setLED(userColorOne, userColorTwo, userColorThree);
            myFinch.noteOn(784);
            myFinch.wait(125);
            myFinch.noteOff();
            myFinch.setLED(0, 0, 0);

            myFinch.setLED(userColorOne, userColorTwo, userColorThree);
            myFinch.noteOn(740);
            myFinch.wait(125);
            myFinch.noteOff();
            myFinch.setLED(0, 0, 0);

            myFinch.setLED(userColorOne, userColorTwo, userColorThree);
            myFinch.noteOn(698);
            myFinch.wait(125);
            myFinch.noteOff();
            myFinch.setLED(0, 0, 0);

            myFinch.wait(42);
            // .42

            myFinch.setLED(userColorOne, userColorTwo, userColorThree);
            myFinch.noteOn(622);
            myFinch.wait(125);
            myFinch.noteOff();
            myFinch.setLED(0, 0, 0);

            myFinch.wait(125);
            // .125

            myFinch.setLED(userColorOne, userColorTwo, userColorThree);
            myFinch.noteOn(659);
            myFinch.wait(125);
            myFinch.noteOff();
            myFinch.setLED(0, 0, 0);

            myFinch.wait(167);
            // .167

            myFinch.setLED(userColorOne, userColorTwo, userColorThree);
            myFinch.noteOn(415);
            myFinch.wait(125);
            myFinch.noteOff();
            myFinch.setLED(0, 0, 0);

            myFinch.setLED(userColorOne, userColorTwo, userColorThree);
            myFinch.noteOn(440);
            myFinch.wait(125);
            myFinch.noteOff();
            myFinch.setLED(0, 0, 0);

            myFinch.setLED(userColorOne, userColorTwo, userColorThree);
            myFinch.noteOn(523);
            myFinch.wait(125);
            myFinch.noteOff();
            myFinch.setLED(0, 0, 0);

            myFinch.wait(125);
            // .125

            myFinch.setLED(userColorOne, userColorTwo, userColorThree);
            myFinch.noteOn(440);
            myFinch.wait(125);
            myFinch.noteOff();
            myFinch.setLED(0, 0, 0);

            myFinch.setLED(userColorOne, userColorTwo, userColorThree);
            myFinch.noteOn(523);
            myFinch.wait(125);
            myFinch.noteOff();
            myFinch.setLED(0, 0, 0);

            myFinch.setLED(userColorOne, userColorTwo, userColorThree);
            myFinch.noteOn(587);
            myFinch.wait(125);
            myFinch.noteOff();
            myFinch.setLED(0, 0, 0);

            myFinch.wait(250);
            // .250

            myFinch.setLED(userColorOne, userColorTwo, userColorThree);
            myFinch.noteOn(622);
            myFinch.wait(125);
            myFinch.noteOff();
            myFinch.setLED(0, 0, 0);

            myFinch.wait(250);
            // .250

            myFinch.setLED(userColorOne, userColorTwo, userColorThree);
            myFinch.noteOn(587);
            myFinch.wait(125);
            myFinch.noteOff();
            myFinch.setLED(0, 0, 0);

            myFinch.wait(250);
            // .250

            myFinch.setLED(userColorOne, userColorTwo, userColorThree);
            myFinch.noteOn(523);
            myFinch.wait(125);
            myFinch.noteOff();
            myFinch.setLED(0, 0, 0);

            myFinch.wait(113);
            // .1125

            myFinch.setLED(userColorOne, userColorTwo, userColorThree);
            myFinch.noteOn(784);
            myFinch.wait(125);
            myFinch.noteOff();
            myFinch.setLED(0, 0, 0);

            myFinch.setLED(userColorOne, userColorTwo, userColorThree);
            myFinch.noteOn(740);
            myFinch.wait(125);
            myFinch.noteOff();
            myFinch.setLED(0, 0, 0);

            myFinch.setLED(userColorOne, userColorTwo, userColorThree);
            myFinch.noteOn(698);
            myFinch.wait(125);
            myFinch.noteOff();
            myFinch.setLED(0, 0, 0);

            myFinch.wait(42);
            // .42

            myFinch.setLED(userColorOne, userColorTwo, userColorThree);
            myFinch.noteOn(622);
            myFinch.wait(125);
            myFinch.noteOff();
            myFinch.setLED(0, 0, 0);

            myFinch.wait(125);
            // .125

            myFinch.setLED(userColorOne, userColorTwo, userColorThree);
            myFinch.noteOn(659);
            myFinch.wait(125);
            myFinch.noteOff();
            myFinch.setLED(0, 0, 0);

            myFinch.wait(167);
            // .167

            myFinch.setLED(userColorOne, userColorTwo, userColorThree);
            myFinch.noteOn(415);
            myFinch.wait(125);
            myFinch.noteOff();
            myFinch.setLED(0, 0, 0);

            myFinch.setLED(userColorOne, userColorTwo, userColorThree);
            myFinch.noteOn(440);
            myFinch.wait(125);
            myFinch.noteOff();
            myFinch.setLED(0, 0, 0);

            myFinch.setLED(userColorOne, userColorTwo, userColorThree);
            myFinch.noteOn(523);
            myFinch.wait(125);
            myFinch.noteOff();
            myFinch.setLED(0, 0, 0);

            myFinch.wait(125);
            // .125

            myFinch.setLED(userColorOne, userColorTwo, userColorThree);
            myFinch.noteOn(440);
            myFinch.wait(125);
            myFinch.noteOff();
            myFinch.setLED(0, 0, 0);

            myFinch.setLED(userColorOne, userColorTwo, userColorThree);
            myFinch.noteOn(523);
            myFinch.wait(125);
            myFinch.noteOff();
            myFinch.setLED(0, 0, 0);

            myFinch.setLED(userColorOne, userColorTwo, userColorThree);
            myFinch.noteOn(587);
            myFinch.wait(125);
            myFinch.noteOff();
            myFinch.setLED(0, 0, 0);

            myFinch.wait(250);
            // .250

            myFinch.setLED(userColorOne, userColorTwo, userColorThree);
            myFinch.noteOn(784);
            myFinch.wait(125);
            myFinch.noteOff();
            myFinch.setLED(0, 0, 0);

            myFinch.setLED(userColorOne, userColorTwo, userColorThree);
            myFinch.noteOn(740);
            myFinch.wait(125);
            myFinch.noteOff();
            myFinch.setLED(0, 0, 0);

            myFinch.setLED(userColorOne, userColorTwo, userColorThree);
            myFinch.noteOn(698);
            myFinch.wait(125);
            myFinch.noteOff();
            myFinch.setLED(0, 0, 0);

            myFinch.wait(42);
            // .42

            myFinch.setLED(userColorOne, userColorTwo, userColorThree);
            myFinch.noteOn(622);
            myFinch.wait(125);
            myFinch.noteOff();
            myFinch.setLED(0, 0, 0);

            myFinch.wait(125);
            // .125

            myFinch.setLED(userColorOne, userColorTwo, userColorThree);
            myFinch.noteOn(659);
            myFinch.wait(125);
            myFinch.noteOff();
            myFinch.setLED(0, 0, 0);

            myFinch.wait(167);
            // .167

            myFinch.setLED(userColorOne, userColorTwo, userColorThree);
            myFinch.noteOn(698);
            myFinch.wait(125);
            myFinch.noteOff();
            myFinch.setLED(0, 0, 0);

            myFinch.wait(125);
            // .125

            myFinch.setLED(userColorOne, userColorTwo, userColorThree);
            myFinch.noteOn(698);
            myFinch.wait(125);
            myFinch.noteOff();
            myFinch.setLED(0, 0, 0);

            myFinch.setLED(userColorOne, userColorTwo, userColorThree);
            myFinch.noteOn(698);
            myFinch.wait(125);
            myFinch.noteOff();
            myFinch.setLED(0, 0, 0);

            myFinch.wait(625);
            // .625

            myFinch.setLED(userColorOne, userColorTwo, userColorThree);
            myFinch.noteOn(784);
            myFinch.wait(125);
            myFinch.noteOff();
            myFinch.setLED(0, 0, 0);

            myFinch.setLED(userColorOne, userColorTwo, userColorThree);
            myFinch.noteOn(740);
            myFinch.wait(125);
            myFinch.noteOff();
            myFinch.setLED(0, 0, 0);

            myFinch.setLED(userColorOne, userColorTwo, userColorThree);
            myFinch.noteOn(698);
            myFinch.wait(125);
            myFinch.noteOff();
            myFinch.setLED(0, 0, 0);

            myFinch.wait(42);
            // .42

            myFinch.setLED(userColorOne, userColorTwo, userColorThree);
            myFinch.noteOn(622);
            myFinch.wait(125);
            myFinch.noteOff();
            myFinch.setLED(0, 0, 0);

            myFinch.wait(125);
            // .125

            myFinch.setLED(userColorOne, userColorTwo, userColorThree);
            myFinch.noteOn(659);
            myFinch.wait(125);
            myFinch.noteOff();
            myFinch.setLED(0, 0, 0);

            myFinch.wait(167);
            // .167

            myFinch.setLED(userColorOne, userColorTwo, userColorThree);
            myFinch.noteOn(415);
            myFinch.wait(125);
            myFinch.noteOff();
            myFinch.setLED(0, 0, 0);

            myFinch.setLED(userColorOne, userColorTwo, userColorThree);
            myFinch.noteOn(440);
            myFinch.wait(125);
            myFinch.noteOff();
            myFinch.setLED(0, 0, 0);

            myFinch.setLED(userColorOne, userColorTwo, userColorThree);
            myFinch.noteOn(523);
            myFinch.wait(125);
            myFinch.noteOff();
            myFinch.setLED(0, 0, 0);

            myFinch.wait(125);
            // .125

            myFinch.setLED(userColorOne, userColorTwo, userColorThree);
            myFinch.noteOn(440);
            myFinch.wait(125);
            myFinch.noteOff();
            myFinch.setLED(0, 0, 0);

            myFinch.setLED(userColorOne, userColorTwo, userColorThree);
            myFinch.noteOn(523);
            myFinch.wait(125);
            myFinch.noteOff();
            myFinch.setLED(0, 0, 0);

            myFinch.setLED(userColorOne, userColorTwo, userColorThree);
            myFinch.noteOn(587);
            myFinch.wait(125);
            myFinch.noteOff();
            myFinch.setLED(0, 0, 0);

            myFinch.wait(250);
            // .250

            myFinch.setLED(userColorOne, userColorTwo, userColorThree);
            myFinch.noteOn(622);
            myFinch.wait(125);
            myFinch.noteOff();
            myFinch.setLED(0, 0, 0);

            myFinch.wait(250);
            // .250

            myFinch.setLED(userColorOne, userColorTwo, userColorThree);
            myFinch.noteOn(587);
            myFinch.wait(125);
            myFinch.noteOff();
            myFinch.setLED(0, 0, 0);

            myFinch.wait(250);
            // .250

            myFinch.setLED(userColorOne, userColorTwo, userColorThree);
            myFinch.noteOn(523);
            myFinch.wait(125);
            myFinch.noteOff();
            myFinch.setLED(0, 0, 0);

        }

        static void MarioThree(Finch myFinch)
        {
            int userColorOne;
            int userColorTwo;
            int userColorThree;
            string userResponse;

            Console.CursorVisible = true;

            Console.WriteLine("Yo Mario, Choose a color!");
            Console.WriteLine("Type a number between 0 - 255 for each section.");
            Console.WriteLine("For example: Red is 255-0-0, Green is 0-255-0, Blue is 0-0-255.");

            Console.Write("First Number:");

            userResponse = Console.ReadLine();
            userColorOne = int.Parse(userResponse);

            Console.Write($"Second Number: ({userColorOne},    ,    )");

            userResponse = Console.ReadLine();
            userColorTwo = int.Parse(userResponse);

            Console.Write($"Second Number: ({userColorOne}, {userColorTwo},    )");

            userResponse = Console.ReadLine();
            userColorThree = int.Parse(userResponse);

            Console.WriteLine($" Your Color is ({userColorOne}, {userColorTwo}, {userColorThree})");
            Console.WriteLine("Press any key to continue");
            Console.ReadKey();

            myFinch.setLED(userColorOne, userColorTwo, userColorThree);
            myFinch.noteOn(480);
            myFinch.wait(200);
            myFinch.noteOff();
            myFinch.setLED(0, 0, 0);

            myFinch.setLED(userColorOne, userColorTwo, userColorThree);
            myFinch.noteOn(1568);
            myFinch.wait(200);
            myFinch.noteOff();
            myFinch.setLED(0, 0, 0);

            myFinch.setLED(userColorOne, userColorTwo, userColorThree);
            myFinch.noteOn(1568);
            myFinch.wait(200);
            myFinch.noteOff();
            myFinch.setLED(0, 0, 0);

            myFinch.setLED(userColorOne, userColorTwo, userColorThree);
            myFinch.noteOn(1568);
            myFinch.wait(200);
            myFinch.noteOff();
            myFinch.setLED(0, 0, 0);

            myFinch.setLED(userColorOne, userColorTwo, userColorThree);
            myFinch.noteOn(740);
            myFinch.wait(200);
            myFinch.noteOff();
            myFinch.setLED(0, 0, 0);

            myFinch.setLED(userColorOne, userColorTwo, userColorThree);
            myFinch.noteOn(784);
            myFinch.wait(200);
            myFinch.noteOff();
            myFinch.setLED(0, 0, 0);

            myFinch.setLED(userColorOne, userColorTwo, userColorThree);
            myFinch.noteOn(784);
            myFinch.wait(200);
            myFinch.noteOff();
            myFinch.setLED(0, 0, 0);

            myFinch.setLED(userColorOne, userColorTwo, userColorThree);
            myFinch.noteOn(784);
            myFinch.wait(200);
            myFinch.noteOff();
            myFinch.setLED(0, 0, 0);

            myFinch.setLED(userColorOne, userColorTwo, userColorThree);
            myFinch.noteOn(784);
            myFinch.wait(200);
            myFinch.noteOff();
            myFinch.setLED(0, 0, 0);

            myFinch.setLED(userColorOne, userColorTwo, userColorThree);
            myFinch.noteOn(370);
            myFinch.wait(200);
            myFinch.noteOff();
            myFinch.setLED(0, 0, 0);

            myFinch.setLED(userColorOne, userColorTwo, userColorThree);
            myFinch.noteOn(392);
            myFinch.wait(200);
            myFinch.noteOff();
            myFinch.setLED(0, 0, 0);

            myFinch.setLED(userColorOne, userColorTwo, userColorThree);
            myFinch.noteOn(370);
            myFinch.wait(200);
            myFinch.noteOff();
            myFinch.setLED(0, 0, 0);

            myFinch.setLED(userColorOne, userColorTwo, userColorThree);
            myFinch.noteOn(392);
            myFinch.wait(200);
            myFinch.noteOff();
            myFinch.setLED(0, 0, 0);

            myFinch.setLED(userColorOne, userColorTwo, userColorThree);
            myFinch.noteOn(392);
            myFinch.wait(400);
            myFinch.noteOff();
            myFinch.setLED(0, 0, 0);

            myFinch.setLED(userColorOne, userColorTwo, userColorThree);
            myFinch.noteOn(196);
            myFinch.wait(400);
            myFinch.noteOff();
            myFinch.setLED(0, 0, 0);

            myFinch.setLED(userColorOne, userColorTwo, userColorThree);
            myFinch.noteOn(740);
            myFinch.wait(200);
            myFinch.noteOff();
            myFinch.setLED(0, 0, 0);

            myFinch.setLED(userColorOne, userColorTwo, userColorThree);
            myFinch.noteOn(784);
            myFinch.wait(200);
            myFinch.noteOff();
            myFinch.setLED(0, 0, 0);

            myFinch.setLED(userColorOne, userColorTwo, userColorThree);
            myFinch.noteOn(784);
            myFinch.wait(200);
            myFinch.noteOff();
            myFinch.setLED(0, 0, 0);

            myFinch.setLED(userColorOne, userColorTwo, userColorThree);
            myFinch.noteOn(740);
            myFinch.wait(200);
            myFinch.noteOff();
            myFinch.setLED(0, 0, 0);

            myFinch.setLED(userColorOne, userColorTwo, userColorThree);
            myFinch.noteOn(784);
            myFinch.wait(200);
            myFinch.noteOff();
            myFinch.setLED(0, 0, 0);

            myFinch.setLED(userColorOne, userColorTwo, userColorThree);
            myFinch.noteOn(784);
            myFinch.wait(200);
            myFinch.noteOff();
            myFinch.setLED(0, 0, 0);

            myFinch.setLED(userColorOne, userColorTwo, userColorThree);
            myFinch.noteOn(740);
            myFinch.wait(200);
            myFinch.noteOff();
            myFinch.setLED(0, 0, 0);

            myFinch.setLED(userColorOne, userColorTwo, userColorThree);
            myFinch.noteOn(84);
            myFinch.wait(200);
            myFinch.noteOff();
            myFinch.setLED(0, 0, 0);

            myFinch.setLED(userColorOne, userColorTwo, userColorThree);
            myFinch.noteOn(880);
            myFinch.wait(200);
            myFinch.noteOff();
            myFinch.setLED(0, 0, 0);

            myFinch.setLED(userColorOne, userColorTwo, userColorThree);
            myFinch.noteOn(831);
            myFinch.wait(200);
            myFinch.noteOff();
            myFinch.setLED(0, 0, 0);

            myFinch.setLED(userColorOne, userColorTwo, userColorThree);
            myFinch.noteOn(880);
            myFinch.wait(200);
            myFinch.noteOff();
            myFinch.setLED(0, 0, 0);

            myFinch.setLED(userColorOne, userColorTwo, userColorThree);
            myFinch.noteOn(980);
            myFinch.wait(400);
            myFinch.noteOff();
            myFinch.setLED(0, 0, 0);

            myFinch.setLED(userColorOne, userColorTwo, userColorThree);
            myFinch.noteOn(880);
            myFinch.wait(200);
            myFinch.noteOff();
            myFinch.setLED(0, 0, 0);

            myFinch.setLED(userColorOne, userColorTwo, userColorThree);
            myFinch.noteOn(784);
            myFinch.wait(200);
            myFinch.noteOff();
            myFinch.setLED(0, 0, 0);

            myFinch.setLED(userColorOne, userColorTwo, userColorThree);
            myFinch.noteOn(699);
            myFinch.wait(200);
            myFinch.noteOff();
            myFinch.setLED(0, 0, 0);

            myFinch.setLED(userColorOne, userColorTwo, userColorThree);
            myFinch.noteOn(740);
            myFinch.wait(200);
            myFinch.noteOff();
            myFinch.setLED(0, 0, 0);

            myFinch.setLED(userColorOne, userColorTwo, userColorThree);
            myFinch.noteOn(784);
            myFinch.wait(200);
            myFinch.noteOff();
            myFinch.setLED(0, 0, 0);

            myFinch.setLED(userColorOne, userColorTwo, userColorThree);
            myFinch.noteOn(784);
            myFinch.wait(200);
            myFinch.noteOff();
            myFinch.setLED(0, 0, 0);

            myFinch.setLED(userColorOne, userColorTwo, userColorThree);
            myFinch.noteOn(740);
            myFinch.wait(200);
            myFinch.noteOff();
            myFinch.setLED(0, 0, 0);

            myFinch.setLED(userColorOne, userColorTwo, userColorThree);
            myFinch.noteOn(784);
            myFinch.wait(200);
            myFinch.noteOff();
            myFinch.setLED(0, 0, 0);

            myFinch.setLED(userColorOne, userColorTwo, userColorThree);
            myFinch.noteOn(784);
            myFinch.wait(200);
            myFinch.noteOff();
            myFinch.setLED(0, 0, 0);

            myFinch.setLED(userColorOne, userColorTwo, userColorThree);
            myFinch.noteOn(740);
            myFinch.wait(200);
            myFinch.noteOff();
            myFinch.setLED(0, 0, 0);

            myFinch.setLED(userColorOne, userColorTwo, userColorThree);
            myFinch.noteOn(784);
            myFinch.wait(200);
            myFinch.noteOff();
            myFinch.setLED(0, 0, 0);

            myFinch.setLED(userColorOne, userColorTwo, userColorThree);
            myFinch.noteOn(880);
            myFinch.wait(200);
            myFinch.noteOff();
            myFinch.setLED(0, 0, 0);

            myFinch.setLED(userColorOne, userColorTwo, userColorThree);
            myFinch.noteOn(988);
            myFinch.wait(400);
            myFinch.noteOff();
            myFinch.setLED(0, 0, 0);
            //time sleep?

            myFinch.setLED(userColorOne, userColorTwo, userColorThree);
            myFinch.noteOn(1180);
            myFinch.wait(10);
            myFinch.noteOff();
            myFinch.setLED(0, 0, 0);

            myFinch.setLED(userColorOne, userColorTwo, userColorThree);
            myFinch.noteOn(1174);
            myFinch.wait(200);
            myFinch.noteOff();
            myFinch.setLED(0, 0, 0);

            myFinch.setLED(userColorOne, userColorTwo, userColorThree);
            myFinch.noteOn(1480);
            myFinch.wait(10);
            myFinch.noteOff();
            myFinch.setLED(0, 0, 0);

            myFinch.setLED(userColorOne, userColorTwo, userColorThree);
            myFinch.noteOn(1568);
            myFinch.wait(200);
            myFinch.noteOff();
            myFinch.setLED(0, 0, 0);
            //time sleep?

            myFinch.setLED(userColorOne, userColorTwo, userColorThree);
            myFinch.noteOn(740);
            myFinch.wait(200);
            myFinch.noteOff();
            myFinch.setLED(0, 0, 0);

            myFinch.setLED(userColorOne, userColorTwo, userColorThree);
            myFinch.noteOn(784);
            myFinch.wait(200);
            myFinch.noteOff();
            myFinch.setLED(0, 0, 0);

            myFinch.setLED(userColorOne, userColorTwo, userColorThree);
            myFinch.noteOn(784);
            myFinch.wait(200);
            myFinch.noteOff();
            myFinch.setLED(0, 0, 0);

            myFinch.setLED(userColorOne, userColorTwo, userColorThree);
            myFinch.noteOn(740);
            myFinch.wait(200);
            myFinch.noteOff();
            myFinch.setLED(0, 0, 0);

            myFinch.setLED(userColorOne, userColorTwo, userColorThree);
            myFinch.noteOn(784);
            myFinch.wait(200);
            myFinch.noteOff();
            myFinch.setLED(0, 0, 0);

            myFinch.setLED(userColorOne, userColorTwo, userColorThree);
            myFinch.noteOn(784);
            myFinch.wait(200);
            myFinch.noteOff();
            myFinch.setLED(0, 0, 0);

            myFinch.setLED(userColorOne, userColorTwo, userColorThree);
            myFinch.noteOn(740);
            myFinch.wait(200);
            myFinch.noteOff();
            myFinch.setLED(0, 0, 0);

            myFinch.setLED(userColorOne, userColorTwo, userColorThree);
            myFinch.noteOn(784);
            myFinch.wait(200);
            myFinch.noteOff();
            myFinch.setLED(0, 0, 0);

            myFinch.setLED(userColorOne, userColorTwo, userColorThree);
            myFinch.noteOn(880);
            myFinch.wait(200);
            myFinch.noteOff();
            myFinch.setLED(0, 0, 0);

            myFinch.setLED(userColorOne, userColorTwo, userColorThree);
            myFinch.noteOn(988);
            myFinch.wait(400);
            myFinch.noteOff();
            myFinch.setLED(0, 0, 0);

            myFinch.setLED(userColorOne, userColorTwo, userColorThree);
            myFinch.noteOn(880);
            myFinch.wait(200);
            myFinch.noteOff();
            myFinch.setLED(0, 0, 0);

            myFinch.setLED(userColorOne, userColorTwo, userColorThree);
            myFinch.noteOn(784);
            myFinch.wait(200);
            myFinch.noteOff();
            myFinch.setLED(0, 0, 0);

            myFinch.setLED(userColorOne, userColorTwo, userColorThree);
            myFinch.noteOn(699);
            myFinch.wait(200);
            myFinch.noteOff();
            myFinch.setLED(0, 0, 0);

            myFinch.setLED(userColorOne, userColorTwo, userColorThree);
            myFinch.noteOn(659);
            myFinch.wait(200);
            myFinch.noteOff();
            myFinch.setLED(0, 0, 0);

            myFinch.setLED(userColorOne, userColorTwo, userColorThree);
            myFinch.noteOn(699);
            myFinch.wait(200);
            myFinch.noteOff();
            myFinch.setLED(0, 0, 0);

            myFinch.setLED(userColorOne, userColorTwo, userColorThree);
            myFinch.noteOn(784);
            myFinch.wait(200);
            myFinch.noteOff();
            myFinch.setLED(0, 0, 0);

            myFinch.setLED(userColorOne, userColorTwo, userColorThree);
            myFinch.noteOn(880);
            myFinch.wait(400);
            myFinch.noteOff();
            myFinch.setLED(0, 0, 0);

            myFinch.setLED(userColorOne, userColorTwo, userColorThree);
            myFinch.noteOn(784);
            myFinch.wait(200);
            myFinch.noteOff();
            myFinch.setLED(0, 0, 0);

            myFinch.setLED(userColorOne, userColorTwo, userColorThree);
            myFinch.noteOn(699);
            myFinch.wait(200);
            myFinch.noteOff();
            myFinch.setLED(0, 0, 0);

            myFinch.setLED(userColorOne, userColorTwo, userColorThree);
            myFinch.noteOn(659);
            myFinch.wait(200);
            myFinch.noteOff();
            myFinch.setLED(0, 0, 0);

            myFinch.setLED(userColorOne, userColorTwo, userColorThree);
            myFinch.noteOn(587);
            myFinch.wait(200);
            myFinch.noteOff();
            myFinch.setLED(0, 0, 0);

            myFinch.setLED(userColorOne, userColorTwo, userColorThree);
            myFinch.noteOn(659);
            myFinch.wait(200);
            myFinch.noteOff();
            myFinch.setLED(0, 0, 0);

            myFinch.setLED(userColorOne, userColorTwo, userColorThree);
            myFinch.noteOn(699);
            myFinch.wait(200);
            myFinch.noteOff();
            myFinch.setLED(0, 0, 0);

            myFinch.setLED(userColorOne, userColorTwo, userColorThree);
            myFinch.noteOn(784);
            myFinch.wait(400);
            myFinch.noteOff();
            myFinch.setLED(0, 0, 0);

            myFinch.setLED(userColorOne, userColorTwo, userColorThree);
            myFinch.noteOn(699);
            myFinch.wait(200);
            myFinch.noteOff();
            myFinch.setLED(0, 0, 0);

            myFinch.setLED(userColorOne, userColorTwo, userColorThree);
            myFinch.noteOn(659);
            myFinch.wait(200);
            myFinch.noteOff();
            myFinch.setLED(0, 0, 0);

            myFinch.setLED(userColorOne, userColorTwo, userColorThree);
            myFinch.noteOn(587);
            myFinch.wait(200);
            myFinch.noteOff();
            myFinch.setLED(0, 0, 0);

            myFinch.setLED(userColorOne, userColorTwo, userColorThree);
            myFinch.noteOn(523);
            myFinch.wait(200);
            myFinch.noteOff();
            myFinch.setLED(0, 0, 0);

            myFinch.setLED(userColorOne, userColorTwo, userColorThree);
            myFinch.noteOn(587);
            myFinch.wait(200);
            myFinch.noteOff();
            myFinch.setLED(0, 0, 0);

            myFinch.setLED(userColorOne, userColorTwo, userColorThree);
            myFinch.noteOn(659);
            myFinch.wait(200);
            myFinch.noteOff();
            myFinch.setLED(0, 0, 0);

            myFinch.setLED(userColorOne, userColorTwo, userColorThree);
            myFinch.noteOn(699);
            myFinch.wait(400);
            myFinch.noteOff();
            myFinch.setLED(0, 0, 0);

            myFinch.setLED(userColorOne, userColorTwo, userColorThree);
            myFinch.noteOn(659);
            myFinch.wait(200);
            myFinch.noteOff();
            myFinch.setLED(0, 0, 0);

            myFinch.setLED(userColorOne, userColorTwo, userColorThree);
            myFinch.noteOn(587);
            myFinch.wait(200);
            myFinch.noteOff();
            myFinch.setLED(0, 0, 0);

            myFinch.setLED(userColorOne, userColorTwo, userColorThree);
            myFinch.noteOn(494);
            myFinch.wait(200);
            myFinch.noteOff();
            myFinch.setLED(0, 0, 0);

            myFinch.setLED(userColorOne, userColorTwo, userColorThree);
            myFinch.noteOn(523);
            myFinch.wait(200);
            myFinch.noteOff();
            myFinch.setLED(0, 0, 0);

            myFinch.setLED(userColorOne, userColorTwo, userColorThree);
            myFinch.noteOn(349);
            myFinch.wait(400);
            myFinch.noteOff();
            myFinch.setLED(0, 0, 0);

            myFinch.setLED(userColorOne, userColorTwo, userColorThree);
            myFinch.noteOn(392);
            myFinch.wait(200);
            myFinch.noteOff();
            myFinch.setLED(0, 0, 0);

            myFinch.setLED(userColorOne, userColorTwo, userColorThree);
            myFinch.noteOn(330);
            myFinch.wait(200);
            myFinch.noteOff();
            myFinch.setLED(0, 0, 0);

            myFinch.setLED(userColorOne, userColorTwo, userColorThree);
            myFinch.noteOn(532);
            myFinch.wait(200);
            myFinch.noteOff();
            myFinch.setLED(0, 0, 0);

            myFinch.setLED(userColorOne, userColorTwo, userColorThree);
            myFinch.noteOn(494);
            myFinch.wait(200);
            myFinch.noteOff();
            myFinch.setLED(0, 0, 0);

            myFinch.setLED(userColorOne, userColorTwo, userColorThree);
            myFinch.noteOn(466);
            myFinch.wait(200);
            myFinch.noteOff();
            myFinch.setLED(0, 0, 0);

            myFinch.setLED(userColorOne, userColorTwo, userColorThree);
            myFinch.noteOn(440);
            myFinch.wait(200);
            myFinch.noteOff();
            myFinch.setLED(0, 0, 0);

            myFinch.setLED(userColorOne, userColorTwo, userColorThree);
            myFinch.noteOn(494);
            myFinch.wait(200);
            myFinch.noteOff();
            myFinch.setLED(0, 0, 0);

            myFinch.setLED(userColorOne, userColorTwo, userColorThree);
            myFinch.noteOn(523);
            myFinch.wait(200);
            myFinch.noteOff();
            myFinch.setLED(0, 0, 0);

            myFinch.setLED(userColorOne, userColorTwo, userColorThree);
            myFinch.noteOn(880);
            myFinch.wait(200);
            myFinch.noteOff();
            myFinch.setLED(0, 0, 0);

            myFinch.setLED(userColorOne, userColorTwo, userColorThree);
            myFinch.noteOn(494);
            myFinch.wait(200);
            myFinch.noteOff();
            myFinch.setLED(0, 0, 0);

            myFinch.setLED(userColorOne, userColorTwo, userColorThree);
            myFinch.noteOn(880);
            myFinch.wait(200);
            myFinch.noteOff();
            myFinch.setLED(0, 0, 0);

            myFinch.setLED(userColorOne, userColorTwo, userColorThree);
            myFinch.noteOn(1760);
            myFinch.wait(200);
            myFinch.noteOff();
            myFinch.setLED(0, 0, 0);

            myFinch.setLED(userColorOne, userColorTwo, userColorThree);
            myFinch.noteOn(440);
            myFinch.wait(200);
            myFinch.noteOff();
            myFinch.setLED(0, 0, 0);

            myFinch.setLED(userColorOne, userColorTwo, userColorThree);
            myFinch.noteOn(392);
            myFinch.wait(200);
            myFinch.noteOff();
            myFinch.setLED(0, 0, 0);

            myFinch.setLED(userColorOne, userColorTwo, userColorThree);
            myFinch.noteOn(440);
            myFinch.wait(200);
            myFinch.noteOff();
            myFinch.setLED(0, 0, 0);

            myFinch.setLED(userColorOne, userColorTwo, userColorThree);
            myFinch.noteOn(494);
            myFinch.wait(200);
            myFinch.noteOff();
            myFinch.setLED(0, 0, 0);

            myFinch.setLED(userColorOne, userColorTwo, userColorThree);
            myFinch.noteOn(784);
            myFinch.wait(200);
            myFinch.noteOff();
            myFinch.setLED(0, 0, 0);

            myFinch.setLED(userColorOne, userColorTwo, userColorThree);
            myFinch.noteOn(440);
            myFinch.wait(200);
            myFinch.noteOff();
            myFinch.setLED(0, 0, 0);

            myFinch.setLED(userColorOne, userColorTwo, userColorThree);
            myFinch.noteOn(784);
            myFinch.wait(200);
            myFinch.noteOff();
            myFinch.setLED(0, 0, 0);

            myFinch.setLED(userColorOne, userColorTwo, userColorThree);
            myFinch.noteOn(1568);
            myFinch.wait(200);
            myFinch.noteOff();
            myFinch.setLED(0, 0, 0);

            myFinch.setLED(userColorOne, userColorTwo, userColorThree);
            myFinch.noteOn(392);
            myFinch.wait(200);
            myFinch.noteOff();
            myFinch.setLED(0, 0, 0);

            myFinch.setLED(userColorOne, userColorTwo, userColorThree);
            myFinch.noteOn(349);
            myFinch.wait(200);
            myFinch.noteOff();
            myFinch.setLED(0, 0, 0);

            myFinch.setLED(userColorOne, userColorTwo, userColorThree);
            myFinch.noteOn(392);
            myFinch.wait(200);
            myFinch.noteOff();
            myFinch.setLED(0, 0, 0);

            myFinch.setLED(userColorOne, userColorTwo, userColorThree);
            myFinch.noteOn(440);
            myFinch.wait(200);
            myFinch.noteOff();
            myFinch.setLED(0, 0, 0);

            myFinch.setLED(userColorOne, userColorTwo, userColorThree);
            myFinch.noteOn(699);
            myFinch.wait(200);
            myFinch.noteOff();
            myFinch.setLED(0, 0, 0);

            myFinch.setLED(userColorOne, userColorTwo, userColorThree);
            myFinch.noteOn(415);
            myFinch.wait(200);
            myFinch.noteOff();
            myFinch.setLED(0, 0, 0);

            myFinch.setLED(userColorOne, userColorTwo, userColorThree);
            myFinch.noteOn(699);
            myFinch.wait(200);
            myFinch.noteOff();
            myFinch.setLED(0, 0, 0);

            myFinch.setLED(userColorOne, userColorTwo, userColorThree);
            myFinch.noteOn(1397);
            myFinch.wait(200);
            myFinch.noteOff();
            myFinch.setLED(0, 0, 0);

            myFinch.setLED(userColorOne, userColorTwo, userColorThree);
            myFinch.noteOn(349);
            myFinch.wait(200);
            myFinch.noteOff();
            myFinch.setLED(0, 0, 0);

            myFinch.setLED(userColorOne, userColorTwo, userColorThree);
            myFinch.noteOn(330);
            myFinch.wait(200);
            myFinch.noteOff();
            myFinch.setLED(0, 0, 0);

            myFinch.setLED(userColorOne, userColorTwo, userColorThree);
            myFinch.noteOn(311);
            myFinch.wait(200);
            myFinch.noteOff();
            myFinch.setLED(0, 0, 0);

            myFinch.setLED(userColorOne, userColorTwo, userColorThree);
            myFinch.noteOn(330);
            myFinch.wait(200);
            myFinch.noteOff();
            myFinch.setLED(0, 0, 0);

            myFinch.setLED(userColorOne, userColorTwo, userColorThree);
            myFinch.noteOn(659);
            myFinch.wait(200);
            myFinch.noteOff();
            myFinch.setLED(0, 0, 0);

            myFinch.setLED(userColorOne, userColorTwo, userColorThree);
            myFinch.noteOn(699);
            myFinch.wait(400);
            myFinch.noteOff();
            myFinch.setLED(0, 0, 0);

            myFinch.setLED(userColorOne, userColorTwo, userColorThree);
            myFinch.noteOn(784);
            myFinch.wait(400);
            myFinch.noteOff();
            myFinch.setLED(0, 0, 0);

            myFinch.setLED(userColorOne, userColorTwo, userColorThree);
            myFinch.noteOn(440);
            myFinch.wait(200);
            myFinch.noteOff();
            myFinch.setLED(0, 0, 0);

            myFinch.setLED(userColorOne, userColorTwo, userColorThree);
            myFinch.noteOn(494);
            myFinch.wait(200);
            myFinch.noteOff();
            myFinch.setLED(0, 0, 0);

            myFinch.setLED(userColorOne, userColorTwo, userColorThree);
            myFinch.noteOn(523);
            myFinch.wait(200);
            myFinch.noteOff();
            myFinch.setLED(0, 0, 0);

            myFinch.setLED(userColorOne, userColorTwo, userColorThree);
            myFinch.noteOn(880);
            myFinch.wait(200);
            myFinch.noteOff();
            myFinch.setLED(0, 0, 0);

            myFinch.setLED(userColorOne, userColorTwo, userColorThree);
            myFinch.noteOn(494);
            myFinch.wait(200);
            myFinch.noteOff();
            myFinch.setLED(0, 0, 0);

            myFinch.setLED(userColorOne, userColorTwo, userColorThree);
            myFinch.noteOn(880);
            myFinch.wait(200);
            myFinch.noteOff();
            myFinch.setLED(0, 0, 0);

            myFinch.setLED(userColorOne, userColorTwo, userColorThree);
            myFinch.noteOn(1760);
            myFinch.wait(200);
            myFinch.noteOff();
            myFinch.setLED(0, 0, 0);

            myFinch.setLED(userColorOne, userColorTwo, userColorThree);
            myFinch.noteOn(440);
            myFinch.wait(200);
            myFinch.noteOff();
            myFinch.setLED(0, 0, 0);

            myFinch.setLED(userColorOne, userColorTwo, userColorThree);
            myFinch.noteOn(392);
            myFinch.wait(200);
            myFinch.noteOff();
            myFinch.setLED(0, 0, 0);

            myFinch.setLED(userColorOne, userColorTwo, userColorThree);
            myFinch.noteOn(440);
            myFinch.wait(200);
            myFinch.noteOff();
            myFinch.setLED(0, 0, 0);

            myFinch.setLED(userColorOne, userColorTwo, userColorThree);
            myFinch.noteOn(494);
            myFinch.wait(200);
            myFinch.noteOff();
            myFinch.setLED(0, 0, 0);

            myFinch.setLED(userColorOne, userColorTwo, userColorThree);
            myFinch.noteOn(784);
            myFinch.wait(200);
            myFinch.noteOff();
            myFinch.setLED(0, 0, 0);

            myFinch.setLED(userColorOne, userColorTwo, userColorThree);
            myFinch.noteOn(440);
            myFinch.wait(200);
            myFinch.noteOff();
            myFinch.setLED(0, 0, 0);

            myFinch.setLED(userColorOne, userColorTwo, userColorThree);
            myFinch.noteOn(784);
            myFinch.wait(200);
            myFinch.noteOff();


            myFinch.setLED(userColorOne, userColorTwo, userColorThree);
            myFinch.noteOn(1568);
            myFinch.wait(200);
            myFinch.noteOff();
            myFinch.setLED(0, 0, 0);

            myFinch.setLED(userColorOne, userColorTwo, userColorThree);
            myFinch.noteOn(392);
            myFinch.wait(200);
            myFinch.noteOff();
            myFinch.setLED(0, 0, 0);

            myFinch.setLED(userColorOne, userColorTwo, userColorThree);
            myFinch.noteOn(349);
            myFinch.wait(200);
            myFinch.noteOff();
            myFinch.setLED(0, 0, 0);

            myFinch.setLED(userColorOne, userColorTwo, userColorThree);
            myFinch.noteOn(392);
            myFinch.wait(200);
            myFinch.noteOff();
            myFinch.setLED(0, 0, 0);

            myFinch.setLED(userColorOne, userColorTwo, userColorThree);
            myFinch.noteOn(440);
            myFinch.wait(200);
            myFinch.noteOff();
            myFinch.setLED(0, 0, 0);

            myFinch.setLED(userColorOne, userColorTwo, userColorThree);
            myFinch.noteOn(699);
            myFinch.wait(200);
            myFinch.noteOff();
            myFinch.setLED(0, 0, 0);

            myFinch.setLED(userColorOne, userColorTwo, userColorThree);
            myFinch.noteOn(659);
            myFinch.wait(200);
            myFinch.noteOff();
            myFinch.setLED(0, 0, 0);

            myFinch.setLED(userColorOne, userColorTwo, userColorThree);
            myFinch.noteOn(699);
            myFinch.wait(200);
            myFinch.noteOff();
            myFinch.setLED(0, 0, 0);

            myFinch.setLED(userColorOne, userColorTwo, userColorThree);
            myFinch.noteOn(740);
            myFinch.wait(200);
            myFinch.noteOff();
            myFinch.setLED(0, 0, 0);

            myFinch.setLED(userColorOne, userColorTwo, userColorThree);
            myFinch.noteOn(784);
            myFinch.wait(200);
            myFinch.noteOff();
            myFinch.setLED(0, 0, 0);

            myFinch.setLED(userColorOne, userColorTwo, userColorThree);
            myFinch.noteOn(392);
            myFinch.wait(200);
            myFinch.noteOff();
            myFinch.setLED(0, 0, 0);

            myFinch.setLED(userColorOne, userColorTwo, userColorThree);
            myFinch.noteOn(392);
            myFinch.wait(200);
            myFinch.noteOff();
            myFinch.setLED(0, 0, 0);

            myFinch.setLED(userColorOne, userColorTwo, userColorThree);
            myFinch.noteOn(392);
            myFinch.wait(200);
            myFinch.noteOff();
            myFinch.setLED(0, 0, 0);

            myFinch.setLED(userColorOne, userColorTwo, userColorThree);
            myFinch.noteOn(392);
            myFinch.wait(200);
            myFinch.noteOff();
            myFinch.setLED(0, 0, 0);

            myFinch.setLED(userColorOne, userColorTwo, userColorThree);
            myFinch.noteOn(196);
            myFinch.wait(200);
            myFinch.noteOff();
            myFinch.setLED(0, 0, 0);

            myFinch.setLED(userColorOne, userColorTwo, userColorThree);
            myFinch.noteOn(196);
            myFinch.wait(200);
            myFinch.noteOff();
            myFinch.setLED(0, 0, 0);

            myFinch.setLED(userColorOne, userColorTwo, userColorThree);
            myFinch.noteOn(196);
            myFinch.wait(200);
            myFinch.noteOff();
            myFinch.setLED(0, 0, 0);

            myFinch.setLED(userColorOne, userColorTwo, userColorThree);
            myFinch.noteOn(185);
            myFinch.wait(200);
            myFinch.noteOff();
            myFinch.setLED(0, 0, 0);

            myFinch.setLED(userColorOne, userColorTwo, userColorThree);
            myFinch.noteOn(196);
            myFinch.wait(200);
            myFinch.noteOff();
            myFinch.setLED(0, 0, 0);

            myFinch.setLED(userColorOne, userColorTwo, userColorThree);
            myFinch.noteOn(185);
            myFinch.wait(200);
            myFinch.noteOff();
            myFinch.setLED(0, 0, 0);

            myFinch.setLED(userColorOne, userColorTwo, userColorThree);
            myFinch.noteOn(196);
            myFinch.wait(200);
            myFinch.noteOff();
            myFinch.setLED(0, 0, 0);

            myFinch.setLED(userColorOne, userColorTwo, userColorThree);
            myFinch.noteOn(208);
            myFinch.wait(200);
            myFinch.noteOff();
            myFinch.setLED(0, 0, 0);

            myFinch.setLED(userColorOne, userColorTwo, userColorThree);
            myFinch.noteOn(220);
            myFinch.wait(200);
            myFinch.noteOff();
            myFinch.setLED(0, 0, 0);

            myFinch.setLED(userColorOne, userColorTwo, userColorThree);
            myFinch.noteOn(233);
            myFinch.wait(200);
            myFinch.noteOff();
            myFinch.setLED(0, 0, 0);

            myFinch.setLED(userColorOne, userColorTwo, userColorThree);
            myFinch.noteOn(247);
            myFinch.wait(200);
            myFinch.noteOff();
            myFinch.setLED(0, 0, 0);

            //for (int frequency = 0; frequency < 20000; frequency = frequency + 100)
            //{
            //    myFinch.noteOn(frequency);
            //    myFinch.wait(2);
            //    myFinch.noteOff();
            //}

            //for (int count = 0; count < 10; count++)
            //{
            //    myFinch.noteOn(261);
            //    myFinch.wait(200);
            //    myFinch.noteOff();
            //    myFinch.wait(200);
            //}
        }

        static void OnTheMove(Finch myFinch)
        {
            Console.CursorVisible = true;

            bool quitOnTheMoveMenu = false;
            string menuChoice;

            do
            {
                DisplayScreenHeader("On The Move Menu");

                //
                // get user menu choice
                //
                Console.WriteLine("\ta) Back and Forth");
                Console.WriteLine("\tb) Circles");
                Console.WriteLine("\tc) Waddle");
                Console.WriteLine("\td) ");
                Console.WriteLine("\tq) Talent Show Menu");
                Console.Write("\t\tEnter Choice:");
                menuChoice = Console.ReadLine().ToLower();

                //
                // process user menu choice
                //
                switch (menuChoice)
                {
                    case "a":
                        BackAndForth(myFinch);
                        break;

                    case "b":
                        Circles(myFinch);
                        break;

                    case "c":
                        Waddle(myFinch);
                        break;

                    case "d":

                        break;

                    case "q":
                        quitOnTheMoveMenu = true;
                        break;

                    default:
                        Console.WriteLine();
                        Console.WriteLine("\tPlease enter a letter for the menu choice.");
                        DisplayContinuePrompt();
                        break;
                }

            } while (!quitOnTheMoveMenu);
        }

        static void BackAndForth(Finch myFinch)
        {
            Console.CursorVisible = false;

            DisplayScreenHeader("Forwards then Backwards");

            Console.WriteLine("\tThe Finch robot will now move Back and Forth!");
            DisplayContinuePrompt();

            myFinch.setMotors(255, 255);
            myFinch.wait(2000);
            myFinch.setMotors(-255, -255);
            myFinch.wait(2000);
            myFinch.setMotors(0, 0);
            myFinch.wait(500);

            DisplayMenuPrompt("On the Move Menu");
        }

        static void Circles(Finch myFinch)
        {
            Console.CursorVisible = false;

            DisplayScreenHeader("Back and Forth");

            Console.WriteLine("\tThe Finch robot will now move Back and Forth!");
            DisplayContinuePrompt();

            myFinch.setMotors(-255, 255);
            myFinch.wait(2000);
            myFinch.setMotors(255, -255);
            myFinch.wait(2000);
            myFinch.setMotors(0, 0);

            DisplayMenuPrompt("On the Move Menu");
        }

        static void Waddle(Finch myFinch)
        {
            Console.CursorVisible = false;

            DisplayScreenHeader("Back and Forth");

            Console.WriteLine("\tThe Finch robot will now move Back and Forth!");
            DisplayContinuePrompt();

            myFinch.setMotors(-255, 200);
            myFinch.wait(500);
            myFinch.setMotors(200, -255);
            myFinch.wait(500);
            myFinch.setMotors(-255, 200);
            myFinch.wait(500);
            myFinch.setMotors(200, -255);
            myFinch.wait(500);
            myFinch.setMotors(-255, 200);
            myFinch.wait(500);
            myFinch.setMotors(200, -255);
            myFinch.wait(500);
            myFinch.setMotors(0, 0);
            myFinch.wait(500);

            DisplayMenuPrompt("On the Move Menu");
        }
        #endregion

        #region DATA RECORDER

        /// <summary>
        /// Data Recorder Menu 
        /// </summary>
        /// <returns>Data Recorder Menu</returns>
        static void DataRecorderDisplayMenuScreen(Finch finchRobot)
        {
            int numdberOfDataPoints = 0;
            double dataPointFrequency = 0;
            double[] temperatures = null;

            Console.CursorVisible = true;

            bool quitMenu = false;
            string menuChoice;

            do
            {
                DisplayScreenHeader(" Menu");

                //
                // get user menu choice
                //
                Console.WriteLine("\ta) Number of Data Points ");
                Console.WriteLine("\tb) Frequency of Data Point");
                Console.WriteLine("\tc) Get Data");
                Console.WriteLine("\td) Show Data");
                Console.WriteLine("\tq) Main Menu");
                Console.Write("\t\tEnter Choice:");
                menuChoice = Console.ReadLine().ToLower();

                //
                // process user menu choice
                //
                switch (menuChoice)
                {
                    case "a":
                        numdberOfDataPoints = DataRecorderDisplayGetNumberOfDataPoints();
                        break;

                    case "b":
                        dataPointFrequency = DataRecorderDisplayGetDataPointFrequency();
                        break;

                    case "c":
                        temperatures = DataRecorderDisplayGetData(numdberOfDataPoints, dataPointFrequency, finchRobot);
                        break;

                    case "d":
                        DataRecorderDisplayGetData(temperatures);
                        break;

                    case "q":
                        quitMenu = true;
                        break;

                    default:
                        Console.WriteLine();
                        Console.WriteLine("\tPlease enter a letter for the menu choice.");
                        DisplayContinuePrompt();
                        break;
                }

            } while (!quitMenu);
        }

        static void DataRecorderDisplayGetData(double[] temperatures)
        {
            DisplayScreenHeader("Show Data");

            DataRecorderDisplayTable(temperatures);

            DisplayContinuePrompt();
        }

        static void DataRecorderDisplayTable(double[] temperatures)
        {
            //
            // display table headers
            //
            Console.WriteLine(
                "Recording #".PadLeft(15) +
                "Temp".PadLeft(15)
                );
            Console.WriteLine(
                "-----------".PadLeft(15) +
                "-----------".PadLeft(15)
                );

            //
            // display table data
            //

            for (int index = 0; index < temperatures.Length; index++)
            {
                Console.WriteLine(
                    (index + 1).ToString().PadLeft(15) +
                    temperatures[index].ToString("n2").PadLeft(15)
                    );
            }
        }

        static double[] DataRecorderDisplayGetData(int numdberOfDataPoints, double dataPointFrequency, Finch finchRobot)
        {
            double[] temperatures = new double[numdberOfDataPoints];

            DisplayScreenHeader("Get Data");

            Console.WriteLine($"\tNumber of data points: {numdberOfDataPoints}");
            Console.WriteLine($"\tData point frequency: {dataPointFrequency}");
            Console.WriteLine();
            Console.WriteLine("\tThe Finch robot is ready to begin recording the temperature data.");

            DisplayContinuePrompt();

            for (int index = 0; index < numdberOfDataPoints; index++)
            {
                temperatures[index] = finchRobot.getTemperature();
                Console.WriteLine($"\tReading {index + 1}: {temperatures[index].ToString("n2")}");
                int waitInSeconds = (int)(dataPointFrequency * 1000);
                finchRobot.wait(waitInSeconds);
            }

            DisplayContinuePrompt();
            DisplayScreenHeader("Get Data");

            Console.WriteLine();
            Console.WriteLine("\tTable of Temperatures");
            Console.WriteLine();
            DataRecorderDisplayTable(temperatures);

            DisplayContinuePrompt();

            return temperatures;
        }


        /// <summary>
        /// get the frequency of data points from the user 
        /// </summary>
        /// <returns>frequency of data points</returns>
        static double DataRecorderDisplayGetDataPointFrequency()
        {
            double dataPointFrequency;



            DisplayScreenHeader("\tData Point Frequency");

            Console.Write("Frequency of data points: ");

            // validate user input
            double.TryParse(Console.ReadLine(), out dataPointFrequency);


            DisplayContinuePrompt();

            return dataPointFrequency;
        }

        /// <summary>
        /// get the number of data points from the user
        /// </summary>
        /// <returns>number of data points</returns>
        static int DataRecorderDisplayGetNumberOfDataPoints()
        {
            int numberOfDataPoints;
            string userResponse;


            DisplayScreenHeader("\tNumber of Data Points");

            Console.Write("Number of data points: ");
            userResponse = Console.ReadLine();

            // validate user input
            int.TryParse(userResponse, out numberOfDataPoints);


            DisplayContinuePrompt();

            return numberOfDataPoints;
        }
        #endregion

        #region ALARM SYSTEM

        /// <summary>
        /// Light Alarm Menu 
        /// </summary>

        static void LightAlarmDisplayMenuScreen(Finch finchRobot)
        {
            Console.CursorVisible = true;

            bool quitMenu = false;
            string menuChoice;

            string sensorsToMonitor = null;
            string rangeType = null;
            int minMaxThresholdValue = 0;
            int timeToMonitor = 0;

            do
            {
                DisplayScreenHeader("Light Alarm Menu");

                //
                // get user menu choice
                //
                Console.WriteLine("\ta) Set Sensors to Monitor");
                Console.WriteLine("\tb) Set Range Type");
                Console.WriteLine("\tc) Set Minimum/Maximum Threshold Value");
                Console.WriteLine("\td) Set Time to Monitor");
                Console.WriteLine("\te) Set Alarm");
                Console.WriteLine("\tq) Return to Main Menu");
                Console.Write("\t\tEnter Choice:");
                menuChoice = Console.ReadLine().ToLower();

                //
                // process user menu choice
                //
                switch (menuChoice)
                {
                    case "a":
                        sensorsToMonitor = LightAlarmDisplaySetSensorsToMonitor();
                        break;

                    case "b":
                        rangeType = LightAlarmDisplaySetRangeType();
                        break;

                    case "c":
                        minMaxThresholdValue = LightAlarmSetMinMaxThresholdValue(rangeType, finchRobot);
                        break;

                    case "d":
                        timeToMonitor = LightAlarmSetTimeToMonitor();
                        break;

                    case "e":
                        LightAlarmSetAlarm(finchRobot, sensorsToMonitor, rangeType, minMaxThresholdValue, timeToMonitor);
                        break;

                    case "q":
                        quitMenu = true;
                        break;

                    default:
                        Console.WriteLine();
                        Console.WriteLine("\tPlease enter a letter for the menu choice.");
                        DisplayContinuePrompt();
                        break;
                }

            } while (!quitMenu);
        }

        static void LightAlarmSetAlarm(Finch finchRobot,
            string sensorsToMonitor,
            string rangeType,
            int minMaxThresholdValue,
            int timeToMonitor)
        {
            int secondsElapsed = 0;
            bool thresholdExceeded = false;
            int currentLightSensorValue = 0;

            DisplayScreenHeader("Set Alarm");

            Console.WriteLine($"\tSensors to Monitor {sensorsToMonitor}");
            Console.WriteLine($"\tRange Type: {rangeType}");
            Console.WriteLine("\tMin/Max threshold value: " + minMaxThresholdValue);
            Console.WriteLine($"\tTime to monitor: {timeToMonitor}");
            Console.WriteLine();

            Console.WriteLine("Press any key to begin monitoring.");
            Console.ReadKey();
            Console.WriteLine();

            while ((secondsElapsed < timeToMonitor) && !thresholdExceeded)
            {
                switch (sensorsToMonitor)
                {
                    case "left":
                        currentLightSensorValue = finchRobot.getLeftLightSensor();
                        break;

                    case "right":
                        currentLightSensorValue = finchRobot.getRightLightSensor();
                        break;

                    case "both":
                        currentLightSensorValue = (finchRobot.getRightLightSensor() + finchRobot.getLeftLightSensor()) / 2;
                        break;
                }

                switch (rangeType)
                {
                    case "minimum":
                        if (currentLightSensorValue < minMaxThresholdValue)
                        {
                            thresholdExceeded = true;

                        }
                        break;

                    case "maximum":
                        if (currentLightSensorValue > minMaxThresholdValue)
                        {
                            thresholdExceeded = true;
                        }
                        break;
                }

                finchRobot.wait(1000);
                secondsElapsed++;
            }

            if (thresholdExceeded)
            {
                Console.WriteLine($"\tThe {rangeType} threshold value of {minMaxThresholdValue} was exceeded by the current light sensor value of {currentLightSensorValue}.");
            }

            else
            {
                Console.WriteLine($"\tThe {rangeType} threshold value of {minMaxThresholdValue} was not exceeded.");
            }

            DisplayMenuPrompt("Light Alarm");
        }

        static int LightAlarmSetTimeToMonitor()
        {
            int timeToMoniter;

            DisplayScreenHeader("Time to Monitor");


            // validate value

            Console.Write($"\tTime to Monitor:");
            int.TryParse(Console.ReadLine(), out timeToMoniter);

            // echo value

            DisplayMenuPrompt("Light Alarm");

            return timeToMoniter;
        }

        static int LightAlarmSetMinMaxThresholdValue(string rangeType, Finch finchRobot)
        {
            int minMaxThresholdValue;

            DisplayScreenHeader("Minimum/Maximum Thrshold Value");

            Console.WriteLine($"\tLeft light sesnor ambient value {finchRobot.getLeftLightSensor()}");
            Console.WriteLine($"\tRight light sesnor ambient value {finchRobot.getRightLightSensor()}");
            Console.WriteLine();

            // validate value

            Console.Write($"\tEnter the {rangeType} light sensor value:");
            int.TryParse(Console.ReadLine(), out minMaxThresholdValue);

            // echo value

            DisplayMenuPrompt("Light Alarm");

            return minMaxThresholdValue;
        }

        static string LightAlarmDisplaySetSensorsToMonitor()
        {
            string sensorsToMonitor;

            DisplayScreenHeader("Sensors to Monitor");

            Console.Write("\tSensors to monitor [left, right, both]: ");
            sensorsToMonitor = Console.ReadLine();

            DisplayMenuPrompt("Light Alarm");

            return sensorsToMonitor;

        }

        static string LightAlarmDisplaySetRangeType()
        {
            string rangeType;

            DisplayScreenHeader("Range Type");

            Console.Write("\tRange Type [minimum, maximum]: ");
            rangeType = Console.ReadLine();

            DisplayMenuPrompt("Light Alarm");

            return rangeType;

        }

        #endregion

        #region USER PROGRAMMING

        /// <summary>
        /// ************************************************************
        /// *                 User Programming Menu                    *
        /// ************************************************************
        /// </summary>
        /// <param name="finchRodot"></param>

        static void UserProgrammingDisplayMenuScreen(Finch finchRodot)
        {
            string menuChoice;
            bool quitMenu = false;

            //
            //  tuple to store all three command parameters
            //

            (int motorSpeed, int ledBrightness, double waitSeconds) commandParameters;
            commandParameters.motorSpeed = 0;
            commandParameters.ledBrightness = 0;
            commandParameters.waitSeconds = 0;

            List<Command> commands = new List<Command>();

            do
            {
                DisplayScreenHeader("User Programming Menu");

                //
                //  get user menu choice
                //
                Console.WriteLine("\ta) Set Command Paremeters");
                Console.WriteLine("\tb) Add Commands");
                Console.WriteLine("\tc) View Commands");
                Console.WriteLine("\td) Execute Commands");
                Console.WriteLine("\tq) Quit");
                Console.Write("\t\tEnter Choice");
                menuChoice = Console.ReadLine().ToLower();

                //
                //  process user menu choice
                //

                switch (menuChoice)
                {
                    case "a":
                        commandParameters = UserProgrammingDisplayGetCommandParameters();
                        break;

                    case "b":
                        UserProgrammingDisplayGetFinchCommands(commands);
                        break;

                    case "c":
                        UserProgrammingDisplayFinchCommands(commands);
                        break;

                    case "d":
                        UserProgrammingDisplayExecuteFinchCommands(finchRodot, commands, commandParameters);
                        break;

                    case "q":
                        quitMenu = true;
                        break;

                    default:
                        Console.WriteLine();
                        Console.WriteLine("\tPlease enter a letter for the menu choice.");
                        DisplayContinuePrompt();
                        break;
                }

            } while (!quitMenu);
        }

        static void UserProgrammingDisplayExecuteFinchCommands(Finch finchRobot, List<Command> commands, (int motorSpeed, int ledBrightness, double waitSeconds) commandParameters)
        {
            int motorSpeed = commandParameters.motorSpeed;
            int ledBrightness = commandParameters.ledBrightness;
            int waitMilliSeconds = (int)(commandParameters.waitSeconds * 1000);
            string commandFeedback = "";
            const int TURNING_MOTOR_SPEED = 100;

            DisplayScreenHeader("Execute Finch Commands");

            Console.WriteLine("\tThe Finch robot is ready to execute the list of commands");
            DisplayContinuePrompt();

            foreach (Command command in commands)
            {
                switch (command)
                {
                    case Command.NONE:
                        break;

                    case Command.MOVEFORWARD:
                        finchRobot.setMotors(motorSpeed, motorSpeed);
                        commandFeedback = Command.MOVEFORWARD.ToString();
                        break;

                    case Command.MOVEBACKWARD:
                        finchRobot.setMotors(-motorSpeed, -motorSpeed);
                        commandFeedback = Command.MOVEFORWARD.ToString();
                        break;

                    case Command.STOPMOTORS:
                        finchRobot.setMotors(0, 0);
                        commandFeedback = Command.STOPMOTORS.ToString();
                        break;

                    case Command.WAIT:
                        finchRobot.wait(waitMilliSeconds);
                        commandFeedback = Command.WAIT.ToString();
                        break;

                    case Command.TURNRIGHT:
                        finchRobot.setMotors(TURNING_MOTOR_SPEED, -TURNING_MOTOR_SPEED);
                        commandFeedback = Command.TURNRIGHT.ToString();
                        break;

                    case Command.TURNLEFT:
                        finchRobot.setMotors(-TURNING_MOTOR_SPEED, TURNING_MOTOR_SPEED);
                        commandFeedback = Command.TURNLEFT.ToString();
                        break;

                    case Command.LEDON:
                        finchRobot.setLED(ledBrightness, ledBrightness, ledBrightness);
                        commandFeedback = Command.LEDON.ToString();
                        break;

                    case Command.LEDOFF:
                        finchRobot.setLED(ledBrightness, ledBrightness, ledBrightness);
                        commandFeedback = Command.LEDOFF.ToString();
                        break;

                    case Command.GETTEMPERATURE:
                        commandFeedback = $"Temperature: {finchRobot.getTemperature().ToString("n2")}\n";
                        break;

                    case Command.DONE:
                        commandFeedback = Command.DONE.ToString();
                        break;

                    default:

                        break;
                }

                Console.WriteLine($"\t{commandFeedback}");
            }

            DisplayMenuPrompt("User Programming");
        }

        static void UserProgrammingDisplayFinchCommands(List<Command> commands)
        {
            DisplayScreenHeader("Finch Robot Commands");

            foreach (Command command in commands)
            {
                Console.WriteLine($"\t{command}");
            }

            DisplayMenuPrompt("User Programming");
        }

        static void UserProgrammingDisplayGetFinchCommands(List<Command> commands)
        {
            Command command = Command.NONE;

            DisplayScreenHeader("Finch Robot Commands");

            //
            // list commands
            //

            int commandCount = 1;
            Console.WriteLine("\tList of Available Commands");
            Console.WriteLine();
            Console.WriteLine("\t");

            foreach (string commandName in Enum.GetNames(typeof(Command)))
            {
                Console.WriteLine($"- {commandName.ToLower()}  -");
                if (commandCount % 5 == 0) Console.Write("-\n\t-");
                commandCount++;
            }
            Console.WriteLine();

            while (command != Command.DONE)
            {
                Console.Write("\tEnter Command:");

                if(Enum.TryParse(Console.ReadLine().ToUpper(), out command))
                {
                    commands.Add(command);
                }
                else
                {
                    Console.WriteLine("\t\t********************************************");
                    Console.WriteLine("\t\tPlease enter a command from the list above.");
                    Console.WriteLine("\t\t********************************************");
                }
            }

            // echo commands
            DisplayMenuPrompt("User Programming");
        }

        /// <summary>
        /// **************************************************************
        /// *             Get Command Parameters from User               *     
        /// **************************************************************
        /// </summary>
        /// <returns>tuple of command parameters</returns>

        static (int motorSpeed, int ledBrightness, double waitSeconds) UserProgrammingDisplayGetCommandParameters()
        {
            DisplayScreenHeader("Command Parameters");

            (int motorSpeed, int ledBrightness, double waitSeconds) commandParameters;
            commandParameters.motorSpeed = 0;
            commandParameters.ledBrightness = 0;
            commandParameters.waitSeconds = 0;

            GetValidInteger("\tEnter Motor Speed [1 - 255]:", 1, 255, out commandParameters.motorSpeed);
            GetValidInteger("\tEnter LED Brightness [1 - 255]:", 1, 255, out commandParameters.motorSpeed);
            GetValidDouble("\tEnter Wait in Seconds:", 0, 10, out commandParameters.waitSeconds);

            Console.WriteLine();
            Console.WriteLine($"\tMotor speed: {commandParameters.motorSpeed}");
            Console.WriteLine($"\tLED brightness: {commandParameters.ledBrightness}");
            Console.WriteLine($"\tWait command duration: {commandParameters.waitSeconds}");

            DisplayMenuPrompt("Command Parameters");

            return commandParameters;
        }

        private static void GetValidDouble(string v1, int v2, int v3, out double waitSeconds)
        {
            throw new NotImplementedException();
        }

        private static void GetValidInteger(string v1, int v2, int v3, out int motorSpeed)
        {
            throw new NotImplementedException();
        }


        #endregion

        #region FINCH ROBOT MANAGEMENT

        /// <summary>
        /// *****************************************************************
        /// *              Quit/Disconnect the Finch Robot                      *
        /// *****************************************************************
        /// </summary>
        /// <param name="finchRobot">finch robot object</param>
        static void DisplayDisconnectFinchRobot(Finch finchRobot)
        {
            Console.CursorVisible = false;

            DisplayScreenHeader("\tDisconnect Finch Robot\n");

            Console.WriteLine("\t\tAbout to disconnect from the Finch robot.\n\n\n");
            DisplayContinuePrompt();

            finchRobot.disConnect();

            Console.Clear();

            Console.WriteLine("\t\tThe Finch robot is now disconnected.\n\n");
            DisplayMenuPrompt("MainMenu");
        }

        static void DisplayQuit(Finch finchRobot)
        {
            Console.CursorVisible = false;

            //Console.ForegroundColor = ConsoleColor.White;
            //Console.BackgroundColor = ConsoleColor.Black;

            DisplayScreenHeader("\tDisconnect Finch Robot\n");

            Console.WriteLine("\t\tAbout to disconnect from the Finch robot.\n\n\n");
            DisplayContinuePrompt();

            finchRobot.disConnect();

            Console.Clear();

            Console.WriteLine("\n\n\t\t\tThe Finch robot is now disconnected.\n");
            DisplayExitPrompt();
        }

        /// <summary>
        /// *****************************************************************
        /// *                  Connect the Finch Robot                      *
        /// *****************************************************************
        /// </summary>
        /// <param name="finchRobot">finch robot object</param>
        /// <returns>notify if the robot is connected</returns>
        static bool DisplayConnectFinchRobot(Finch finchRobot)
        {
            Console.CursorVisible = false;

            bool robotConnected;

            DisplayScreenHeader("Connect Finch Robot");

            Console.WriteLine("\tAbout to connect to Finch robot. Please be sure the USB cable is connected to the robot and computer now.");
            DisplayContinuePrompt();

            robotConnected = finchRobot.connect();

            // TODO test connection and provide user feedback - text, lights, sounds

            DisplayMenuPrompt("Main Menu");

            //
            // reset finch robot
            //
            finchRobot.setLED(0, 0, 0);
            finchRobot.noteOff();

            return robotConnected;
        }

        #endregion

        #region USER INTERFACE

        /// <summary>
        /// *****************************************************************
        /// *                     Welcome Screen                            *
        /// *****************************************************************
        /// </summary>
        static void DisplayWelcomeScreen()
        {
            Console.CursorVisible = false;

            Console.ForegroundColor = ConsoleColor.White;
            Console.BackgroundColor = ConsoleColor.Black;

            Console.Clear();
            Console.WriteLine();
            Console.WriteLine("\t\t\t\tFinch Control\n\n");
            Console.WriteLine();

            DisplayContinuePrompt();
        }

        /// <summary>
        /// *****************************************************************
        /// *                     Closing Screen                            *
        /// *****************************************************************
        /// </summary>
        static void DisplayClosingScreen()
        {
            Console.CursorVisible = false;

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.BackgroundColor = ConsoleColor.DarkGreen;

            Console.Clear();

            Console.WriteLine("\n\n\n\n\n\n\n\n\n\t\t\tThank you for using Finch Control!");
            Console.WriteLine();

            DisplayQuitPrompt();
        }

        /// <summary>
        /// display continue prompt
        /// </summary>
        static void DisplayContinuePrompt()
        {
            Console.WriteLine();
            Console.WriteLine("\t\t\tPress any key to continue.");
            Console.ReadKey();
        }

        static void DisplayExitPrompt()
        {
            Console.WriteLine();
            Console.WriteLine("\t\t\tPress any key to Exit Application.");
            Console.ReadKey();
        }

        static void DisplayQuitPrompt()
        {
            Console.WriteLine();
            Console.WriteLine("\t\t\t\tPress any key to Quit.");
            Console.ReadKey();
        }

        /// <summary>
        /// display menu prompt
        /// </summary>
        static void DisplayMenuPrompt(string menuName)
        {
            Console.WriteLine();
            Console.WriteLine($"\tPress any key to return to the {menuName} Menu.");
            Console.ReadKey();
        }

        /// <summary>
        /// display screen header
        /// </summary>
        static void DisplayScreenHeader(string headerText)
        {
            Console.Clear();
            Console.WriteLine();
            Console.WriteLine("\t\t" + headerText);
            Console.WriteLine();
        }

        #endregion
    }
}