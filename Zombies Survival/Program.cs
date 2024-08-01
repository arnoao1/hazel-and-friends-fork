using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing;
using System.Runtime.Intrinsics.X86;
using System.Security.Principal;
using System.Threading;
using System.Xml.Serialization;
using static System.Net.Mime.MediaTypeNames;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Skeleton_Program
{
    internal class Program
    {
        static string[] inventory = new string[10];
        static SortedList<int, bool> zombies = new SortedList<int, bool>();         //Holds key pairs of zombies
        static SortedList<string, int> ammo = new SortedList<string, int>();        //Holds key pairs different ammo types
        static SortedList<int, int> zombieHealth = new SortedList<int, int>();      //Zombies health compares keys with 'zombies'(tKEY,tVALUE Pair)
        static SortedList<string, bool> checks = new SortedList<string, bool>();    //WIP
        static Random rand = new Random();

        static bool zombieAwake,                //Checks if zombie is awake
        alive = true,                           //Bool(true) converted to zombie 'Alive' state for easier reading
        dead = false,                           //Bool(false) converted to zombie 'Dead' state for easier reading
        medkitCheck = false,                    //Checks if medkit was picked up
        ammoPickupCheck = false,                //Checks if ammo was picked up
        enteredCheck = false,
        courtyardCheck = false,
        showerCheck = false;

        static int health = 100,                //Player health
        cellBlockZombies = 0,                   //Checks which cell block zombie is selected   
        fear = 5;                               //Player fear based on zombie attacks done. Cause's gun inaccuracy. Medkits etc, entering new rooms and killing zombies decreases it
        static void Main()
        {


            int tasks;
            do

            {
                for (int i = 0; i < inventory.Length; i++)      //Removes all inventory, ammo, zombies, and zombieHealth objects from game if any exist
                {
                    if (inventory[i] != null)
                    {
                        inventory[i] = null;
                    }
                }
                zombies.Clear();
                ammo.Clear();
                zombieHealth.Clear();

                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\r\n █     █░ ▄▄▄       ██▓     ██ ▄█▀ ██▓ ███▄    █   ▄████     ▄▄▄██▀▀▀▄▄▄       ██▓ ██▓    \r\n▓█░ █ ░█░▒████▄    ▓██▒     ██▄█▒ ▓██▒ ██ ▀█   █  ██▒ ▀█▒      ▒██  ▒████▄    ▓██▒▓██▒    \r\n▒█░ █ ░█ ▒██  ▀█▄  ▒██░    ▓███▄░ ▒██▒▓██  ▀█ ██▒▒██░▄▄▄░      ░██  ▒██  ▀█▄  ▒██▒▒██░    \r\n░█░ █ ░█ ░██▄▄▄▄██ ▒██░    ▓██ █▄ ░██░▓██▒  ▐▌██▒░▓█  ██▓   ▓██▄██▓ ░██▄▄▄▄██ ░██░▒██░    \r\n░░██▒██▓  ▓█   ▓██▒░██████▒▒██▒ █▄░██░▒██░   ▓██░░▒▓███▀▒    ▓███▒   ▓█   ▓██▒░██░░██████▒\r\n░ ▓░▒ ▒   ▒▒   ▓▒█░░ ▒░▓  ░▒ ▒▒ ▓▒░▓  ░ ▒░   ▒ ▒  ░▒   ▒     ▒▓▒▒░   ▒▒   ▓▒█░░▓  ░ ▒░▓  ░\r\n  ▒ ░ ░    ▒   ▒▒ ░░ ░ ▒  ░░ ░▒ ▒░ ▒ ░░ ░░   ░ ▒░  ░   ░     ▒ ░▒░    ▒   ▒▒ ░ ▒ ░░ ░ ▒  ░\r\n  ░   ░    ░   ▒     ░ ░   ░ ░░ ░  ▒ ░   ░   ░ ░ ░ ░   ░     ░ ░ ░    ░   ▒    ▒ ░  ░ ░   \r\n    ░          ░  ░    ░  ░░  ░    ░           ░       ░     ░   ░        ░  ░ ░      ░  ░\r\n                                                                                          \r\n");
                Console.ResetColor();
                Console.WriteLine("1. Play game \n\n2. Instructions \n\n3. Options \n\n4. Credits \n\n0. Exit game");
                tasks = Convert.ToInt32(Console.ReadLine());
                Console.Clear();

                switch (tasks)
                {

                    case 1:

                        introduction();
                        break;

                    case 2:

                        task2();
                        break;

                    case 3:

                        task3();
                        break;
                    case 4:

                        task4();
                        break;

                    case 0:
                        Console.WriteLine("Exit");
                        break;

                }

            } while (tasks != 0);
            
           
            static void task2()
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(" ██▓ ███▄    █   ██████ ▄▄▄█████▓ ██▀███   █    ██  ▄████▄  ▄▄▄█████▓ ██▓ ▒█████   ███▄    █   ██████ \r\n▓██▒ ██ ▀█   █ ▒██    ▒ ▓  ██▒ ▓▒▓██ ▒ ██▒ ██  ▓██▒▒██▀ ▀█  ▓  ██▒ ▓▒▓██▒▒██▒  ██▒ ██ ▀█   █ ▒██    ▒ \r\n▒██▒▓██  ▀█ ██▒░ ▓██▄   ▒ ▓██░ ▒░▓██ ░▄█ ▒▓██  ▒██░▒▓█    ▄ ▒ ▓██░ ▒░▒██▒▒██░  ██▒▓██  ▀█ ██▒░ ▓██▄   \r\n░██░▓██▒  ▐▌██▒  ▒   ██▒░ ▓██▓ ░ ▒██▀▀█▄  ▓▓█  ░██░▒▓▓▄ ▄██▒░ ▓██▓ ░ ░██░▒██   ██░▓██▒  ▐▌██▒  ▒   ██▒\r\n░██░▒██░   ▓██░▒██████▒▒  ▒██▒ ░ ░██▓ ▒██▒▒▒█████▓ ▒ ▓███▀ ░  ▒██▒ ░ ░██░░ ████▓▒░▒██░   ▓██░▒██████▒▒\r\n░▓  ░ ▒░   ▒ ▒ ▒ ▒▓▒ ▒ ░  ▒ ░░   ░ ▒▓ ░▒▓░░▒▓▒ ▒ ▒ ░ ░▒ ▒  ░  ▒ ░░   ░▓  ░ ▒░▒░▒░ ░ ▒░   ▒ ▒ ▒ ▒▓▒ ▒ ░\r\n ▒ ░░ ░░   ░ ▒░░ ░▒  ░ ░    ░      ░▒ ░ ▒░░░▒░ ░ ░   ░  ▒       ░     ▒ ░  ░ ▒ ▒░ ░ ░░   ░ ▒░░ ░▒  ░ ░\r\n ▒ ░   ░   ░ ░ ░  ░  ░    ░        ░░   ░  ░░░ ░ ░ ░          ░       ▒ ░░ ░ ░ ▒     ░   ░ ░ ░  ░  ░  \r\n ░           ░       ░              ░        ░     ░ ░                ░      ░ ░           ░       ░  \r\n                                                   ░                                                  \n\n");
                Console.ResetColor();
                Console.WriteLine("1. You will be presented with choices throughout the game.\n\n");
                Console.WriteLine("2. Type the letter or word corresponding to your choice and press Enter.\n\n");
                Console.WriteLine("3. Your goal is to survive and escape the WALKING JAIL.\n\n");
                Console.WriteLine("4. Pay attention to the details in the story to make informed decisions.\n\n");
                Console.WriteLine("");
                Console.WriteLine("");
                Console.WriteLine("                            Press ENTER to return");
                Console.ReadLine();
            }
            static void task3()
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(" ▒█████   ██▓███  ▄▄▄█████▓ ██▓ ▒█████   ███▄    █   ██████ \r\n▒██▒  ██▒▓██░  ██▒▓  ██▒ ▓▒▓██▒▒██▒  ██▒ ██ ▀█   █ ▒██    ▒ \r\n▒██░  ██▒▓██░ ██▓▒▒ ▓██░ ▒░▒██▒▒██░  ██▒▓██  ▀█ ██▒░ ▓██▄   \r\n▒██   ██░▒██▄█▓▒ ▒░ ▓██▓ ░ ░██░▒██   ██░▓██▒  ▐▌██▒  ▒   ██▒\r\n░ ████▓▒░▒██▒ ░  ░  ▒██▒ ░ ░██░░ ████▓▒░▒██░   ▓██░▒██████▒▒\r\n░ ▒░▒░▒░ ▒▓▒░ ░  ░  ▒ ░░   ░▓  ░ ▒░▒░▒░ ░ ▒░   ▒ ▒ ▒ ▒▓▒ ▒ ░\r\n  ░ ▒ ▒░ ░▒ ░         ░     ▒ ░  ░ ▒ ▒░ ░ ░░   ░ ▒░░ ░▒  ░ ░\r\n░ ░ ░ ▒  ░░         ░       ▒ ░░ ░ ░ ▒     ░   ░ ░ ░  ░  ░  \r\n    ░ ░                     ░      ░ ░           ░       ░  \r\n                                                            ");
                Console.WriteLine("");
                Console.WriteLine("");
                Console.WriteLine("           YOU HAVE NO OPTION BUT TO PLAY THIS GAME HEHE \n\n");
                Console.ResetColor();
                Console.WriteLine("                    Press ENTER to return");
                Console.ReadLine();
            }
            static void task4()
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("▄████▄   ██▀███  ▓█████ ▓█████▄  ██▓▄▄▄█████▓  ██████ \r\n▒██▀ ▀█  ▓██ ▒ ██▒▓█   ▀ ▒██▀ ██▌▓██▒▓  ██▒ ▓▒▒██    ▒ \r\n▒▓█    ▄ ▓██ ░▄█ ▒▒███   ░██   █▌▒██▒▒ ▓██░ ▒░░ ▓██▄   \r\n▒▓▓▄ ▄██▒▒██▀▀█▄  ▒▓█  ▄ ░▓█▄   ▌░██░░ ▓██▓ ░   ▒   ██▒\r\n▒ ▓███▀ ░░██▓ ▒██▒░▒████▒░▒████▓ ░██░  ▒██▒ ░ ▒██████▒▒\r\n░ ░▒ ▒  ░░ ▒▓ ░▒▓░░░ ▒░ ░ ▒▒▓  ▒ ░▓    ▒ ░░   ▒ ▒▓▒ ▒ ░\r\n  ░  ▒     ░▒ ░ ▒░ ░ ░  ░ ░ ▒  ▒  ▒ ░    ░    ░ ░▒  ░ ░\r\n░          ░░   ░    ░    ░ ░  ░  ▒ ░  ░      ░  ░  ░  \r\n░ ░         ░        ░  ░   ░     ░                 ░  \r\n░                         ░                           ");
                Console.ResetColor();
                Console.WriteLine("WALKING JAIL");
                Console.WriteLine();
                Console.WriteLine("Developed by:");
                Console.WriteLine("HAZEL AND FRIENDS");
                Console.WriteLine();
                Console.WriteLine("Credits:");
                Console.WriteLine("- [ASTON]");
                Console.WriteLine("- [SAMUEL]");
                Console.WriteLine("- [EDWARD]");
                Console.WriteLine("- [HAZEL]");
                Console.WriteLine("- [Sound Effects & Music Designer]");
                Console.WriteLine("- [QA Testers]");
                Console.WriteLine("- [Special Thanks to Contributors or Supporters]");

                Console.WriteLine();
                Console.WriteLine("© [2024] [WALKING JAIL.LTD]");
                Console.WriteLine("Press ENTER to return");
                Console.ReadLine();


            }

        }

        static void Text(string text)
        {
            for (int i = 0; i < text.Length; i++)
            {
                Console.Write(text[i]);
                Thread.Sleep(2);
            }
            Console.WriteLine();
        }

        public static void introduction()
        {

            Console.WriteLine("INTRODUCTION");
            Console.WriteLine("------------");
            Text("You wake up to the sound of knocking at your door, it's your landlord.");
            Text("Do you want to skip the rest?\n\ny/n");
            char temp = Convert.ToChar(Console.ReadLine().ToLower());
            if(temp == 'y')
            {
                task1();
            }



            Text("She says you are behind on the rent again, you explain how you got fired and it's not your fault but she doesn't care.");
            Text("She demands you pay your rent by tonight or else you'll be kicked out onto the street.");
            Text("To calm down and come up with ideas, you decide to go for a walk.");
            Text("");
            Text("PRESS ENTER TO CONTINUE");
            Console.ReadLine();
            Console.WriteLine("------------");
            Text("After walking through town thinking about all the bad choices you've made, you notice a vacant mansion across the street.");
            Text("All the curtains are open, no one looks like they're inside and there are no cars in the driveway.");
            Text("You peek into the curtains and notice various expensive-looking objects you could sell.");
            Text("You also remember you need the money for your rent. By tonight.");
            Text("");
            Text("PRESS ENTER TO CONTINUE");
            Console.ReadLine();
            Console.WriteLine("------------");
            Text("You picklock the door with the picklocks you keep in your pocket, and start exploring and pocketing items in the house.");
            Console.Beep((int)592.2, 1000);
            Console.Beep((int)370.2, 1000);
            Console.Beep((int)592.2, 1000);
            Console.Beep((int)370.2, 1000);
            Text("After filling your pockets, you suddenly hear a police siren getting closer and closer.");
            Text("You try to make a run for it onto the streets, but the cops are already there, waiting for you.");
            Text("");
            Text("PRESS ENTER TO CONTINUE");
            Console.ReadLine();
            Console.WriteLine("------------");
            Text("After 3 years, you're very tired of being in prison.");
            Console.Beep((int)294.2, 800);
            Console.Beep((int)277.2, 800);
            Console.Beep((int)262.2, 800);
            Console.Beep((int)247.2, 1100);
            Text("you and you're cellmate are in your cell when suddenly you hear screaming.");
            Console.WriteLine("PRESS ENTER TO CONTINUE");
            Console.ReadLine();
            Console.WriteLine("");
            Text("The riot alarm has been sounded and all the cell doors suddenly fly open.");
            Console.Beep((int)772.2, 100);
            Console.Beep((int)772.2, 100);
            Console.Beep((int)772.2, 100);
            Console.Beep((int)772.2, 100);
            Console.Beep((int)772.2, 100);
            Console.Beep((int)772.2, 100);
            Console.Beep((int)772.2, 100);
            Console.Beep((int)772.2, 100);
            Console.Beep((int)772.2, 100);
            Console.Beep((int)772.2, 100);
            Text("The halls are filled with prisoners shouting and running.");
            Text("You try to step out of your cell, but your cellmate knocks you out cold before you can step through the doorway.");
            Text("");
            Console.WriteLine("------------");
            Text("You wake up, feeling nauseous and uncomfortable, probably from sleeping on the cold prison floor for two days straight.");
            Text("The prison is much quieter than usual, and the cell doors are all still open.");
            Console.WriteLine("");
            Console.ReadLine();
            task1();

        }
        public static void mapmethod()
        {
            string map = @"

--------------------------------------------------------------------------------------------------------------------------------------------------
- ~~	* ~	* ~	~`	*	~	`	`*	`	*`			~		*	`  ~      `		 -
- _______________________________________________________________________________	*   ~		  *	 *		*	 	 -
-||___________________________________________________________||____         ____|  `		`*	 `		`*	    *		 -
-||				*			      ||____	     ____|    _________			*	    ~ `	 	 *	 -
-||		*	*	~	*	*	*     ||____         ____|   | _______ |    *  _____________________________		 -
-||          ~              	*		*	      ||____	     ____|   | [Guard] |      |	   |    |      []   |       |	    * 	 -
-||	*		*				      ||____ Showers ____|   | [Tower] |      |	   |    |[]	    | Armory|	* 	 -
-|| 		*	      Courtyard		*	      ||____	     ____|   | [__3__] |      |	   |    |           |       | 		 -
-||	~					~	*     ||____	     ____|   |_________|      |Bath|Bath|[]	    |_____  |     *	 - 
-||		~		~	*		      ||____	     ____| 		  *   |Room|Room|  Offices	    |	 	 -
-||	*              *          *      ~     	*	      ||____	     ____|	* *           |	   |    |         []        | 	    *	 -
-|\   ________________________________________________________||                 | 	~	*     |	   |    |		    |	 	 -
-|   |_________________________________________________________|_______    ______|____________________|	|__|__| |		  []| *		 -
-| 														| []     []         |	  *	 -
-| 																  []|		 -
-|______________________________     _________________________________________________________________    ______|___________________|	*	 -
-                           |           | 		*		*	*		*     |	  _____	  	            |	   *	 -
-   `     	~	    |           | 	*	 _________	   * `   `	~	      |	 |_____|~~~~~~~~~~~~~~~~~~| |    	 -
-         *                 | CellBlock | *	~	| _______ |    * ~       ~	 *	      |	 |	Visitor	    	    |	  *   *	 - 
-     ~    	*	    |     2	|  *		| [Guard] |			    *	 ~    |	 |	Room	  	    |	 	 -
-                     ~     |	        | ~	 *	| [Tower] |              *                    |  |  []   []         []  []  |    *       -
-~ `			    |           |		| [__1__] |	*	   *    `       *     |	 |_______________  _________|____________-______	  
-       *  		    |           |     *		|_________|		`		      |	 			    |[]                 \
-        	    `	    |           | 					*	  *	      |	 	  Reception		       EXIT     
-       ~   `               |___     ___| *	~  * ~     * `~		*	`		      |		  		    
-         			|   | 	   *			*   ~		~		~     |____________________________|____________ ______/		 
-        *		*	|   | 	~	* 	~    *		*			~					         -
-            `    ~             |   |          _________________________________ 	`		  *		  *	~     *   *      -
-    ~    			|   |	      | 	  |	   |            |              *       		 				 -
-         	 ~         	|   |	      | 	    Library             |   *     ~            ~ 		*	     ~           -
-        *	        ________|   |_________|       ____|________|______      |                     *			~			 -
-        	       | []  []	      []  []  |       |	     ~            |     |      *               	  ~			 *		 -
-      ~  	       | []  []	      []  []  |       |	  *        *      |     |            *        		  *		~		 -
-        	  ~    | []  []	      []  []  |       |	      *    ~      |     |    *			*   ~			   *	 	 -
- 	    *          | []  []	      []  []  |       |	 *  `       *     |     |              *         	     ~	 _________	         -
-   *     	       | []  []	      []  []  |       |           `       |     |_________________________	~	| _______ | *	         -
-         ~	       |      Cafeteria	              |  `     *     `    |                   |      	  |	*	| [Guard] |		 -
-     *    	 *     |         	              |    `         *    |                         V     |		| [Tower] |		 -
-   ___________        | []  []	      []  []  |       |      *            |      _____________| CellBlock |*		| [__2__] |		 -
-   |  _____  |        | []  []	      []  []  |       |   *	`    `    |     |             |     1	  |		|_________|	 *	 -
-   | [Guard] |	   ~   | []  []	      []  []  |       | `	~     *   |     |             |     	  |*				         -
-   | [Tower] |	*      | []  []	      []  []  |       |    `	  *       |     |             |   	  |			*		 -
-   | [__4__] |        | []  []	      []  []  |       |  *	          |     | 	~     |*	~ |	*	~			 -
-   |_________|        |______________________|       |    `     *   *    |     | 	      |___________|				*	 - 
-        	                 	      |       |___________________|     |   *		~			    			 -
-        	*    ~       *		      |	      |	                  |     | 	*		~	~		*		 -
-     *      ~     		  ~	      |	              Rec  	        | 			*		  *			 - 
-                	*		      |	    	     Room               | 							*	 -
-      *     ~                      ~         |_______|___________________|_____| 	~	~						 -
- 		*					*							*				 -
- 				*				*	~		*				*			 - 
--------------------------------------------------------------------------------------------------------------------------------------------------";
            Console.WriteLine(map);
        }
        public static void task1()
        {


            string temp;


            Console.WriteLine("Which way do you want to go right (r) or left (l)? \n\n");

            char user = char.Parse(Console.ReadLine());

            if (user == 'l' || user == 'L')

            {
                //Died();
                Console.WriteLine("You leave your cell and turned to the left and you saw a fellow inmate standing with his back facing you");
                Console.WriteLine("You go up to him and asked him what happened and where everyone is, you get close to him and as you are about to tap his shoulder");
                Console.WriteLine("He turns around, his face is covered in blood. You stumble backwards, you see him lunge at you, you swiftly dodge around him");
                Console.WriteLine("You gaze into the distance and see flashing lights and run towards them ");
                Console.WriteLine("PRESS ENTER TO CONTINUE");
                Console.ReadLine();
                Console.WriteLine("------------");
                Corridor();
            }
            else if (user == 'r' || user == 'R')
            {

                Console.WriteLine("You decide to go right, all you see is a series of lined  metal doors \nIn your mind you think to yourself this could be leading to cells or storage rooms.\n\n");
                Thread.Sleep(1000);
                Console.WriteLine("Your footsteps echo ominously as you make your way forward. ");
                Thread.Sleep(1000);
                Console.WriteLine("Suddenly, a faint sound catches your attention.");
                Thread.Sleep(1000);
                Console.WriteLine("");
                Console.WriteLine("------------");
                Console.WriteLine("All you hear is a muffled groan, coming from just around the corner.");
                Thread.Sleep(1000);
                Console.WriteLine("You pause, your heart pounding, and slowly look ahead");
                Thread.Sleep(1000);
                Console.WriteLine("There, in a pool of blood, lies a figure, unmoving.");
                Thread.Sleep(1000);
                Console.WriteLine("------------");
                Console.WriteLine("As your heart races even faster, you approach cautiously.");
                Console.Beep((int)60, 100);
                Thread.Sleep(1000);
                Console.WriteLine("You try and focus your eyes in the darkness and you slowly start to see the form of a battered man.");
                Console.WriteLine("");
                Console.WriteLine("------------\n\n");
                Thread.Sleep(1000);
                Console.WriteLine("As you get closer, you notice he's a fellow cellmate and his uniform is all tattered and covered in blood.");
                Thread.Sleep(1000);
                Console.WriteLine("He turns his head weakly, his eyes widening as he sees you.");
                Thread.Sleep(1000);
                Console.WriteLine("Please...he rasps, his voice barely above a whisper. Help me. \n\n");
                Thread.Sleep(1000);
                Console.WriteLine("Would you like to help him (H) or are you going to ignore him(I)");
                string choice = Console.ReadLine().ToUpper();

                if (choice == "H")
                {
                    Console.WriteLine("You tear a piece of your shirt and pass it to him \n He presses the piece of cloth onto his neck where it looked like he was badly scratched and attacked");
                    Thread.Sleep(1000);
                    Console.WriteLine("He manages to say, coughing up a mouthful of blood.");
                    Thread.Sleep(1000);
                    Console.WriteLine("They took... my keys.");
                    Thread.Sleep(1000);
                    Console.WriteLine("You glance down and notice him holding a set of keys.");
                    Thread.Sleep(1000);
                    Console.WriteLine("Hold on, you say, reaching for the keys. I may be able to use these to help us escape.");
                    Thread.Sleep(1000);
                    Console.WriteLine("The man nods weakly, a faint smile crossing his lips. Take them... use them well and make sure that....");
                    inventory[1] = "Key";
                    Console.WriteLine("INVENTORY UPDATED");
                    for (int i = 0; i < inventory.Length; i++)
                    {
                        Console.WriteLine(inventory[i]);
                    }
                    Console.WriteLine("Just before your fellow cellmate was about to finish what he was saying, he falls onto the ground and starts getting zombie possessed");
                    Thread.Sleep(1000);
                    Console.WriteLine("RUN!!!!");

                }


                if (choice == "I")
                {
                    Console.WriteLine("You look down at the wounded prisoner, his pleading eyes desperate for help.\n  But the risks of assisting him seem too great, and the chance of your own escape feels more important.");
                    Thread.Sleep(1000);
                    Console.WriteLine("With a heavy heart, you turn your back and continue down the hallway. \n The prisoner's faint cries echo behind you.");
                    Thread.Sleep(1000);
                    Console.WriteLine("The keys he mentioned could be a valuable resource, but at what cost?\n You push the guilt aside, knowing that your own survival  is a priority ");
                    Thread.Sleep(1000);
                    Console.WriteLine("You continue to make your way through the prison");
                }



                Console.WriteLine("You reach the bottom of the stairs, all the cell doors are open as well. You see a sign with the words 'corridor'. Would you like to enter the 'corridor'? Yes (y) or No (n)");
                char answer = char.Parse(Console.ReadLine());
                if (answer == 'y')
                {
                    Corridor();
                }
                else
                {
                    Console.WriteLine("You stayed in cell blocks");
                    Console.WriteLine("You stayed in cell blocks, you feel a sharp pain on your neck");
                    Console.WriteLine("You fall to the ground, your vision starts blur and you embrace death.");
                    Console.WriteLine("PRESS ENTER");
                    Console.ReadLine();
                    Died();

                }
            }
        }

        static void Corridor()
        {

            Console.WriteLine("You enter a long corridor \nyou begin to walk down it. \n you see 2 bodies lying on the floor.");
            Console.WriteLine("Do you want to check the bodies, Yes 'y' or No 'n'");
            char answer = char.Parse(Console.ReadLine());
            if (answer == 'y' || answer == 'Y')
            {
                Console.WriteLine("You check the bodies each of them have bite marks on different areas of their bodies");
                Console.WriteLine("They both start to move and swiftly attack you. \nYou died");
                Console.WriteLine("PRESS ENTER");
                Console.ReadLine();
                Died();

            }
            else if (answer == 'n' || answer == 'N')
            {
                Console.WriteLine("You move past them ignoring them and continue walking down the corridor");
                Console.WriteLine("You reach the end and see 2 signs with arrows pointing in different directions");
                Console.WriteLine("Left going to 'Library' and right going to 'Rec room'");
                Console.WriteLine("Which way do you want to go? \n Left (L) or Right (R)");
                char direction = char.Parse(Console.ReadLine());
                if (direction == 'L' || direction == 'l')
                {
                    Library();
                    Console.WriteLine("You check the bodies each of them have bite marks on different areas of their bodies");
                    Console.WriteLine("They both start to move and swiftly attack you. \nYou died");
                    Console.ReadLine();
                    Died();
                }
                else if (direction == 'R' || (direction == 'r'))
                {
                    Recroom();
                }
                else
                {
                    Console.WriteLine("Invalid input. Please choose 'L' for Library or 'R' for Rec Room ");
                }

            }
            else
            {
                Console.WriteLine("Invalid input. Please choose 'Y' for Yes or 'N' for No");
            }
        }

        static void Library()
        {
            string temp;
            bool explore = false;

            Console.WriteLine("You walk down corridor and open the library doors");
            Thread.Sleep(1000);
            Console.WriteLine("You see that the room is a mess. Books scattered every where on the ground it's a total mess");
            Thread.Sleep(1000);
            Console.WriteLine("You enter the room, closing the door behind you, would you like to explore? \nYes (y) or No (n)?");
            Thread.Sleep(1000);
            char answer = char.Parse(Console.ReadLine());
            if (answer == 'y' || answer == 'Y')

            {
                explore = true;
                {
                    Console.WriteLine("You search around the room, seeing piles and piles of books on the ground.");
                    Thread.Sleep(1000);
                    Console.WriteLine("Would you like to investigate? Yes 'y' or No 'n'?");
                    char investigate = char.Parse(Console.ReadLine());
                    if (investigate == 'y' || investigate == 'Y')
                    {
                        Console.WriteLine("You step on a piece of paper");
                        Thread.Sleep(1000);
                        Console.WriteLine("Pick up Yes (y) or No (n)?");
                        char pickUp = char.Parse(Console.ReadLine());
                        if (pickUp == 'y' || pickUp == 'Y')
                        {
                            Console.WriteLine("You pick up the piece of paper and examine it.");
                            Thread.Sleep(1000);
                            Console.WriteLine("On further examination you see that it is the map of the prison");
                            inventory[2] = "Map";
                            Thread.Sleep(1000);
                            Console.WriteLine("You examine the map carefully slowly growing board of it,\nyou scrunch up the map and put it into your pocket and move on");
                            Thread.Sleep(1000);


                        }
                        else if (pickUp == 'n' || pickUp == 'N')
                        {
                            Console.WriteLine("You leave the piece of paper on the ground.");
                            Thread.Sleep(1000);

                        }
                    }
                    else
                    {
                        Console.WriteLine("You decide not to look at the paper");
                        Thread.Sleep(1000);
                    }
                    Console.WriteLine("You venture deeper into the library walking into a maze of bookshelfs ");
                    Thread.Sleep(1000);
                    Console.WriteLine("You find yourself lost, each direction look unfimilliar \nYou look left, right, straight and up");
                    Thread.Sleep(1000);
                    Console.WriteLine("What is your next move?");

                    while (explore.Equals(true))
                    {
                        temp = Console.ReadLine().ToLower();
                        switch (temp)
                        {
                            case "left":
                                Console.WriteLine("You find yourself back at where you started");
                                Console.WriteLine("Everything seems to be the same, you walk back into the maze to try again");
                                break;
                            case "right":
                                Console.WriteLine("You exit the maze to see a door with the words 'Cafeteria' on it");
                                Console.WriteLine("You feel your stomach growling, it's been a while since you have eaten");
                                Console.WriteLine("YOu let your stomach control your actions \nYou walk towards the door and enter the cafeteria.");
                                explore = false;
                                Cafeteria();
                                break;
                            case "up":
                                Console.WriteLine("You are too indecisive to make a choice");
                                Thread.Sleep(1000);
                                Console.WriteLine("You look up and decide to get a better view");
                                Thread.Sleep(1000);
                                Console.WriteLine("You start to climb up the bookshelf \nYou feel it wobble and shake");
                                Thread.Sleep(1000);
                                Console.WriteLine("You lose your footing and fall to your death");
                                Thread.Sleep(1000);
                                Main();
                                break;
                            default:
                                Console.WriteLine("Invalid input. Please enter 'left', 'right', or 'up'.");
                                break;
                        }

                    }
                }
            }
            else
            {
                Console.WriteLine("You decide not to search the room.");
            }
        }

        static void Recroom()
        {
            Random rand = new Random();
            for (int i = 0; i < 10; i++)
            {
                int frequency = rand.Next(100, 2000);
                int duration = rand.Next(50, 400);
                Console.Beep(frequency, duration);
                System.Threading.Thread.Sleep(100);
            }
            Console.WriteLine("You reached the recreation room, a safe spot for now");
            Console.ForegroundColor = ConsoleColor.Red;
            Thread.Sleep(1000);
            Console.WriteLine("\r\n ██▀███  ▓█████  ▄████▄      ██▀███   ▒█████   ▒█████   ███▄ ▄███▓\r\n▓██ ▒ ██▒▓█   ▀ ▒██▀ ▀█     ▓██ ▒ ██▒▒██▒  ██▒▒██▒  ██▒▓██▒▀█▀ ██▒\r\n▓██ ░▄█ ▒▒███   ▒▓█    ▄    ▓██ ░▄█ ▒▒██░  ██▒▒██░  ██▒▓██    ▓██░\r\n▒██▀▀█▄  ▒▓█  ▄ ▒▓▓▄ ▄██▒   ▒██▀▀█▄  ▒██   ██░▒██   ██░▒██    ▒██ \r\n░██▓ ▒██▒░▒████▒▒ ▓███▀ ░   ░██▓ ▒██▒░ ████▓▒░░ ████▓▒░▒██▒   ░██▒\r\n░ ▒▓ ░▒▓░░░ ▒░ ░░ ░▒ ▒  ░   ░ ▒▓ ░▒▓░░ ▒░▒░▒░ ░ ▒░▒░▒░ ░ ▒░   ░  ░\r\n  ░▒ ░ ▒░ ░ ░  ░  ░  ▒        ░▒ ░ ▒░  ░ ▒ ▒░   ░ ▒ ▒░ ░  ░      ░\r\n  ░░   ░    ░   ░             ░░   ░ ░ ░ ░ ▒  ░ ░ ░ ▒  ░      ░   \r\n   ░        ░  ░░ ░            ░         ░ ░      ░ ░         ░   \r\n                ░                                                 \r\n");
            Console.WriteLine("   _______\r\n  | Rec   |\r\n  | Room  |\r\n  |       | \r\n  | o     | \r\n  |_______|\r\n");
            Console.ResetColor();
            Console.WriteLine("");
            Console.WriteLine("Your heart is racing as you look for a hiding spot. You find a strong table at the corner of the room");

            Console.WriteLine("");
            Console.WriteLine("HURRY TYPE    'HIDE'    IN ALL CAPS TO HIDE UNDER THE TABLE");
            string temp = "";
            temp = Console.ReadLine().ToUpper();
            if (temp != "HIDE")
            {
                Died();
            }


            Console.WriteLine("  \r\n ********  *********  ********* ********* ******** ******** *********  *  *********\r\n *         *       *  *         *             *    *        *       *  *  *       *\r\n *         *********  ********  *********     *    ******** ********   *  *********\r\n *         *       *  *         *             *    *        *   *      *  *       *\r\n ********  *       *  *         *********     *    ******** *      *   *  *       *     ");
            Console.WriteLine("  ( (\r\n   ) )\r\n   ____\r\n  |    |\r\n  |    |]|\r\n  |____| \r\n");
            Thread.Sleep(1000);
            Console.WriteLine("You push open the heavy doors and step into the cafeteria,");
            Thread.Sleep(400);
            Console.WriteLine("The dim lighting casting long shadows across the empty tables and benches.");
            Thread.Sleep(400);
            Console.WriteLine("As you scan the room, your eyes quickly adjust, and you begin to take note of your surroundings. ");
            Thread.Sleep(400);
            Console.WriteLine("The cafeteria is surprisingly well stocked, with several stainless steel counters lining the walls, each containing various supplies and equipment.");
            Thread.Sleep(400);
            Console.WriteLine("As you take a closer look, you notice that some of these counters have locks on them");
            Thread.Sleep(400);
            Console.WriteLine("Your gaze shifts to a stack of metal trays, their sharp edges glinting in the faint light.");
            Thread.Sleep(400);
            Console.WriteLine("Nearby, you spot a collection of dull-looking utensils forks, knives, and spoons  that could potentially be used again as makeshift weapons.");
            Thread.Sleep(400);
            Console.WriteLine("Further down, you notice a rack of plastic cups, each filled with a murky liquid that you assume is water.");
            Thread.Sleep(400);
            Console.WriteLine("The cups could be useful for carrying liquid or even as improvised tools but eww.");
            Thread.Sleep(400);
            Console.WriteLine("As you continue to explore the cafeteria, your attention is drawn to a set of shelves lining the back wall.");
            Thread.Sleep(400);
            Console.WriteLine("Upon closer inspection, you see that are some bags of dried food and a few sealed bottles of water.");
            Thread.Sleep(400);
            Console.WriteLine("These supplies could be valuable in your quest for survival in the WALKING JAIL.");
            Thread.Sleep(400);
            Console.WriteLine("Which of these items would you like to gather, (food or water)?");
            string choice = Console.ReadLine();
            if (choice == "food")
            {
                inventory[4] = "food";
                Console.WriteLine("You grab the bags of dried food and stuff them in your pockets");
                Console.WriteLine("Wold you like to grab any other item y/n?");
                char user = char.Parse(Console.ReadLine());
                if (user == 'y')

                {
                    temp = Console.ReadLine();

                    if (temp != "HIDE")
                    {
                        Console.WriteLine("TRY AGAIN HURRY!!!!!");
                        Console.WriteLine("HURRY TYPE    'HIDE'    IN ALL CAPS TO HIDE UNDER THE TABLE");
                    }
                }

                Console.WriteLine("You're hiding behind A table in the prison's game room.");
                Thread.Sleep(2000);
                Console.WriteLine("");
                Console.WriteLine("There are no lights and its hard to see but you can hear scary noises outside, like moans and shuffling. It's a zombie.");
                Thread.Sleep(5000);
                Console.WriteLine("");
                Console.WriteLine("Its skin was decaying, and its clothes were in tatters. But what truly made it horrifying were its gruesome injuries.");
                Console.WriteLine("         .-\"\"\"-.\r\n        /       \\\r\n       |  O   O  |\r\n       |    ∆    |\r\n        \\ ----- /\r\n     ____|_____|____\r\n   /  ______(O)______\\\r\n   \\_/  [_____]'   [___] \r\n");
                Thread.Sleep(5000);
                Console.WriteLine("");
                Console.WriteLine("You look around for a better hiding spot and see a dark corner. You try to sneak over there, but then you see the zombie stumble into the room.");
                Thread.Sleep(5000);
                Console.WriteLine("");
                Console.WriteLine("Its eyes are creepy and it's looking for someone to eat.");
                Thread.Sleep(3000);
                Console.WriteLine("");
                Console.WriteLine("You freeze as the zombie's gaze lands on you.");
                Thread.Sleep(3000);
                Console.WriteLine("");
                Console.WriteLine("You scan the room quickly, searching for a safer place to hide.");
                Thread.Sleep(4000);
                Console.WriteLine("");
                Console.WriteLine("You see a sturdy-looking cabinet against the far wall, it seems like a better option than your current spot behind the tables.");
                Thread.Sleep(5000);
                Console.WriteLine("");
                Console.WriteLine("You notice another possibility, you see a large overturned couch near the center of the room. It's bulky and could provide better cover.");
                Thread.Sleep(6000);
                Console.WriteLine("");
                Console.WriteLine("But the couch is closer to where the zombie is slowly making its way towards you.");
                Thread.Sleep(3000);
                Console.WriteLine("");

                for (int i = 0; i < 3; i++)
                {
                    Console.Beep(1000, 200);
                    System.Threading.Thread.Sleep(200);
                    Console.Beep(1500, 200);
                    System.Threading.Thread.Sleep(200);
                }

                Console.WriteLine("CHOOSE A PLACE TO HIDE: CLOSET(press 1)   COUCH(press 2)");
                temp = Console.ReadLine();
                int hide = Convert.ToInt32(temp);



                switch (hide)
                {
                    case 1:
                        Console.WriteLine("You see the closet door slightly open and think it's a good hiding spot. Without much thinking, you go inside and close the door.");
                        Console.WriteLine("");
                        Thread.Sleep(5000);
                        Console.WriteLine("Inside, it's dark and cramped. You start feeling scared. There's no way out.");
                        Console.WriteLine("");
                        Thread.Sleep(4000);
                        Console.WriteLine("Outside, the zombie gets closer. It finds you in the closet and opens the door. You try to hide, but there's nowhere to go.");
                        Console.WriteLine("");
                        Thread.Sleep(5000);
                        Console.WriteLine("The zombie grabs you, and you can't escape. Your mistake cost your life");
                        break;



                    case 2:
                        Console.WriteLine("You see the couch and think it might be a good hiding spot. It's closer and seems big enough to protect you.");
                        Console.WriteLine("");
                        Thread.Sleep(5000);
                        Console.WriteLine("You quickly hide behind the couch, holding your breath. The zombie's groans get quieter, and you start feeling safer.");
                        Console.WriteLine("");
                        Thread.Sleep(4000);
                        Console.WriteLine("You wait for a long time, but eventually, the zombie's sounds go away completely.");
                        Console.WriteLine("");
                        Thread.Sleep(5000);
                        Console.WriteLine("You peek out from behind the couch and see that it's safe.");
                        break;

                    case 3:
                    case 4:
                    case 5:
                    case 6:
                    case 7:
                    case 8:
                    case 9:
                        Console.WriteLine("You choose to stay under the table");
                        Console.WriteLine("");
                        Thread.Sleep(3000);
                        Console.WriteLine("You heard the zombie approaching, as it's groans grow louder.");
                        Thread.Sleep(3000);
                        Console.WriteLine("");
                        Console.WriteLine("IT");
                        Thread.Sleep(1500);
                        Console.WriteLine("");
                        Console.WriteLine("SEES");
                        Console.WriteLine("");
                        Thread.Sleep(1500);
                        Console.WriteLine("YOU");
                        Console.WriteLine("");
                        Thread.Sleep(2000);
                        Console.WriteLine("The zombie lunges forward, sinking its teeth into your leg. \n You scream in agony as the zombie's terrifying cry fills the room.");
                        break;

                    default:
                        Console.WriteLine("Invalid choice");
                        break;
                }

                Console.WriteLine("You're hiding behind the couch in the dim rec room jail. It's dark, and you can't see much.");
                Console.WriteLine("");
                Thread.Sleep(4000);
                Console.WriteLine("Your hand touches two things: a flashlight and a pool stick. You need to decide which one to pick.");
                Console.WriteLine("");
                Thread.Sleep(4000);
                Console.WriteLine("The flashlight can help you see better, but the pool stick could protect you if zombies come near. \n What will you choose?");
                Console.WriteLine("PRESS Q for  FLASHLIGHT OR PRESS W POOL StICK ");
                temp = Console.ReadLine();
                char item = Convert.ToChar(temp);

                if (item == 'q' || item == 'Q')
                {
                    Console.WriteLine("");
                    inventory[2] = "Flashlight";
                }
                else
                {
                    Console.WriteLine("");
                }
            }
        }

        static void Cafeteria()
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("\r\n ▄████▄   ▄▄▄        █████▒▓█████▄▄▄█████▓▓█████  ██▀███   ██▓ ▄▄▄      \r\n▒██▀ ▀█  ▒████▄    ▓██   ▒ ▓█   ▀▓  ██▒ ▓▒▓█   ▀ ▓██ ▒ ██▒▓██▒▒████▄    \r\n▒▓█    ▄ ▒██  ▀█▄  ▒████ ░ ▒███  ▒ ▓██░ ▒░▒███   ▓██ ░▄█ ▒▒██▒▒██  ▀█▄  \r\n▒▓▓▄ ▄██▒░██▄▄▄▄██ ░▓█▒  ░ ▒▓█  ▄░ ▓██▓ ░ ▒▓█  ▄ ▒██▀▀█▄  ░██░░██▄▄▄▄██ \r\n▒ ▓███▀ ░ ▓█   ▓██▒░▒█░    ░▒████▒ ▒██▒ ░ ░▒████▒░██▓ ▒██▒░██░ ▓█   ▓██▒\r\n░ ░▒ ▒  ░ ▒▒   ▓▒█░ ▒ ░    ░░ ▒░ ░ ▒ ░░   ░░ ▒░ ░░ ▒▓ ░▒▓░░▓   ▒▒   ▓▒█░\r\n  ░  ▒     ▒   ▒▒ ░ ░       ░ ░  ░   ░     ░ ░  ░  ░▒ ░ ▒░ ▒ ░  ▒   ▒▒ ░\r\n░          ░   ▒    ░ ░       ░    ░         ░     ░░   ░  ▒ ░  ░   ▒   \r\n░ ░            ░  ░           ░  ░           ░  ░   ░      ░        ░  ░\r\n░                                                                       \r\n                                                                        \r\n                                                                        \r\n                                                                        \r\n                                                                        \r\n                                                                        \r\n                                                                        \r\n                                                                        \r\n                                                                        \r\n                                                                        \r\n                                                                        \r\n");
            Console.ResetColor();
            Console.WriteLine("You push open the heavy doors and step into the cafeteria,");
            Thread.Sleep(400);
            Console.WriteLine("The dim lighting casting long shadows across the empty tables and benches.");
            Thread.Sleep(400);
            Console.WriteLine("As you scan the room, your eyes quickly adjust, and you begin to take note of your surroundings. ");
            Thread.Sleep(400);
            Console.WriteLine("The cafeteria is surprisingly well stocked, with several stainless steel counters lining the walls, each containing various supplies and equipment.");
            Thread.Sleep(400);
            Console.WriteLine("As you take a closer look, you notice that some of these counters have locks on them");
            Thread.Sleep(400);
            Console.WriteLine("Your gaze shifts to a stack of metal trays, their sharp edges glinting in the faint light.");
            Thread.Sleep(400);
            Console.WriteLine("Nearby, you spot a collection of dull-looking utensils forks, knives, and spoons  that could potentially be used again as makeshift weapons.");
            Thread.Sleep(400);
            Console.WriteLine("Further down, you notice a rack of plastic cups, each filled with a murky liquid that you assume is water.");
            Thread.Sleep(400);
            Console.WriteLine("The cups could be useful for carrying liquid or even as improvised tools but eww.");
            Thread.Sleep(400);
            Console.WriteLine("As you continue to explore the cafeteria, your attention is drawn to a set of shelves lining the back wall.");
            Thread.Sleep(400);
            Console.WriteLine("Upon closer inspection, you see that are some bags of dried food and a few sealed bottles of water.");
            Thread.Sleep(400);
            Console.WriteLine("These supplies could be valuable in your quest for survival in the WALKING JAIL.");
            Thread.Sleep(400);
            Console.WriteLine("Which of these items would you like to gather, (food or water)?");
            string choice = Console.ReadLine();
            if (choice == "food")
            {
                inventory[4] = "food";
                Console.WriteLine("You grab the bags of dried food and stuff them in your pockets");
                string food = "yes";
                Console.WriteLine("Wold you like to grab any other item y/n?");
                char user = char.Parse(Console.ReadLine());
                if (user == 'y')
                {
                    Console.WriteLine("Now that you know where you can find some supplies, you grab yourself a bottle of water and chug it. ");

                }
                else if (user == 'n')
                {
                    Console.WriteLine("You're satisfied with what you have and continue exploring the cafeteria");
                }
            }
            else if (choice == "water")
            {
                inventory[4] = "water";
                Console.WriteLine("You grab yourself a bottle of water and chug it");

                Console.WriteLine("Wold you like to grab any other item?");
                char user = char.Parse(Console.ReadLine());
                if (user == 'y')
                {
                    Console.WriteLine("Now that you know where you can find some supplies, you grab yourself a few packets of some dried food and stuff them in your pockets. ");

                }
                else if (user == 'n')
                {
                    Console.WriteLine("You're satisfied with what you have and continue exploring the cafeteria");
                    ////
                    Console.WriteLine("Whilst exploring do you decide to go to cell block 2 or keep expolering the cafeteria\n(Y)es to enter cell block 2 or (N)o to stay in the cafetria");

                    choice = Console.ReadLine().ToLower();
                    cellBlock2();


                }

            }



        }





        /**@cellBlock2()
             * The cell block 2 method. Intiates all cell block 2 events
             */
        static void cellBlock2()
        {
            string temp;
            bool check = false, zombieAwake = false;
            int zombieMemory = -1;

            if (!zombies.ContainsKey(1))
            {
                check = false;                    //Gun check
                zombieAwake = false;
                zombieMemory = -1;                 //Zombie zombieMemory or last location of zombie
                cellBlockZombies = 1;
                zombies.Add(1, true);                 //Appends cell block 2 zombie to 'zombies' key, pair 
                zombieHealth.Add(1, 100);             //Appends cell black 2 zombies health to 'zombieHealth' key, pair
                                                      //health = 100;
            }

            Console.WriteLine("You enter cellblock 2. The cell block is eriely quiet. The are 4 cells.\nDo you choose to continue towards the courtyard or enter one of the cells?\n (Y)es contine or (N)o stay?");
            temp = Console.ReadLine().ToLower();

            if (temp.Equals("y"))
            {
                Console.WriteLine("You continue towards the corridor");
                corridor();
            }
            else if (temp.Equals("n"))
            {
                Console.WriteLine("You stay and decide to investigate each cell");
                Console.WriteLine("What cell do you choose to enter first?\nCell 1, 2, 3 or 4?");

                temp = Console.ReadLine();

                switch (Convert.ToInt32(temp))
                {
                    case 1:
                        cellBlock2Options(Convert.ToInt32(temp), check, zombieAwake, zombieMemory);
                        break;
                    case 2:
                        cellBlock2Options(Convert.ToInt32(temp), check, zombieAwake, zombieMemory);
                        break;

                    case 3:
                        cellBlock2Options(Convert.ToInt32(temp), check, zombieAwake, zombieMemory);
                        break;

                    case 4:
                        cellBlock2Options(Convert.ToInt32(temp), check, zombieAwake, zombieMemory);
                        break;

                    default:
                        Console.WriteLine("Choose a number");
                        break;
                }
            }
        }

        /**
         * @cellBlock2Options controls the decisions the player makes and cell expolring
         */
        static void cellBlock2Options(int x, bool check, bool zombieAwake, int zombieMemory)
        {
            if (health <= 0)
            {
                Died();
            }

            zombies.TryGetValue(cellBlockZombies, out bool y);
            bool tempAlive = y;
            int bullets;
            Console.WriteLine($"Zombie zombieMemory {zombieMemory}");

            string temp = "";
            switch (x)
            {
                //Cell 1
                case 1:
                    if (zombies.ContainsKey(cellBlockZombies) && zombies[1].Equals(dead) || zombieAwake.Equals(false) && ((zombies.ContainsKey(1) && zombies[1].Equals(alive))))
                    {
                        Console.WriteLine("You decide to enter cell 2.1. Its cold and empty except for a metal closet, bunk bed, sink and toilet");
                        cellBlockEvent(temp, check, zombieAwake, zombieMemory);
                    }
                    else if (!zombieMemory.Equals(1) && zombieAwake.Equals(true) && ((zombies.ContainsKey(1) && zombies[1].Equals(alive))))
                    {
                        int chance;
                        Console.WriteLine("You run to cell 1");
                        Console.WriteLine("You see the closet. Do you choose to hide in it or exit to the center of the cell block again? (Y)es enter or (N)o re-enter cellblock");
                        temp = Console.ReadLine().ToLower();

                        if (temp.Equals("y"))
                        {
                            Console.WriteLine("You enter the closet close to the sink");
                            Console.WriteLine("You're safe momentarily. But you know given enough time the zombie will find and attack you. The sink is close by and you think up an idea.");
                            do
                            {
                                chance = rand.Next(1, 11);
                                Console.WriteLine("Do you decide to turn the sink on to distract the zombie and make a run for it or take your chances and wait for the zombie to leave. (Y)es or (N)o? (Choose quick)");
                                temp = Console.ReadLine().ToLower();

                                if (temp.Equals("y"))
                                {
                                    zombieMemory = 1;
                                    Console.WriteLine("You turn the sink on distracting the zombie and run to the center of the cell block again");
                                    cellBlockEvent(temp, check, zombieAwake, zombieMemory);
                                }
                                else if (temp.Equals("n"))
                                {
                                    Console.WriteLine("You take your chances and remain in hiding");

                                }
                                else if (chance >= 4 && chance <= 7)
                                {
                                    zombieMemory = 1;
                                    Console.WriteLine("You were too slow. The zombie has now blocked your escape from the cell");
                                    zombiesDamage(0, cellBlockZombies, chance);
                                    Console.WriteLine("Do you (f)ire back or (m)elee?");
                                    temp = Console.ReadLine().ToLower();
                                    weapons(temp, zombieAwake);
                                }
                            } while (!zombieMemory.Equals(1));
                        }
                        else if (temp.Equals("n"))
                        {
                            Console.WriteLine("You don't hide in the closet");
                            cellBlockEvent(temp, check, zombieAwake, zombieMemory);
                        }
                    }
                    else if (zombieMemory.Equals(1))
                    {
                        Console.WriteLine("You can't re-enter this cell the zombie will attack you");
                        cellBlockEvent(temp, check, zombieAwake, zombieMemory);
                    }
                    break;

                //Cell 2
                case 2:

                    if (zombies.ContainsKey(1) && zombies[1].Equals(dead) || zombieAwake.Equals(false) && ((zombies.ContainsKey(1) && zombies[1].Equals(alive))))
                    {
                        if (!inventory.Contains("Med kit") && medkitCheck.Equals(false))
                        {
                            Console.WriteLine("You decide to enter cell 2. You find a medkit maybe you can use it later");
                            for (int i = 0; i < inventory.Length; i++)
                            {
                                if (inventory[i] == null && !inventory.Contains("Med kit"))
                                {
                                    inventory[i] = "Med kit";
                                }
                            }
                            for (int i = 0; i < inventory.Length; i++)
                            {
                                if (inventory[i] != null)
                                {
                                    Console.WriteLine($"You now have {inventory[i]} in your inventory");
                                }
                            }
                            medkitCheck = true;
                            cellBlockEvent(temp, check, zombieAwake, zombieMemory);
                        }
                        else if (inventory.Contains("Med kit") || medkitCheck.Equals(true) && !inventory.Contains("Med kit"))
                        {
                            Console.WriteLine("You re-enter cell 2. Nothing... You leave");
                            cellBlockEvent(temp, check, zombieAwake, zombieMemory);
                        }
                    }
                    else if (!zombieMemory.Equals(2) && zombieAwake.Equals(true) && ((zombies.ContainsKey(1) && zombies[1].Equals(alive)))) ///Zombie follows into cell 2
                    {
                        zombieMemory = 2;
                        Console.WriteLine("You run to cell 2. Theres nothing there but a bunkbed layed down as a make shift baracade.");
                        Console.WriteLine("The zombie follows you in. You decide its your only option to attack the zombie or defend yourself.");
                        bool containsItem = inventory.Contains("9mm Handgun");

                        if (check.Equals(false))                                //If player didn't check handgun ammo prior
                        {
                            if (inventory.Contains("9mm Handgun"))
                            {
                                Console.WriteLine("You either (m)elee attack the zombie or (f)ire your handgun");

                                temp = Console.ReadLine().ToLower();
                                if (temp.Equals("m"))
                                {
                                    weapons(temp, zombieAwake);
                                    cellBlockEvent(temp, check, zombieAwake, zombieMemory);
                                }
                                else if (temp.Equals("f") && inventory.Any("9mm Handgun".Contains))
                                {
                                    if (ammo.ContainsKey("9mm Bullets"))
                                    {
                                        Console.WriteLine("You reload your handgun weapon...");
                                        check = true;
                                    }
                                    check = true;
                                    weapons(temp, zombieAwake);
                                    cellBlockEvent(temp, check, zombieAwake, zombieMemory);
                                }
                            }
                        }

                        if (check.Equals(true))
                        {
                            if (inventory.Contains("9mm Handgun"))
                            {
                                Console.WriteLine("You either choose a (m)elee option or (f)ire your handgun");
                                temp = Console.ReadLine().ToLower();
                                if (temp.Equals("m"))
                                {
                                    weapons(temp, zombieAwake);
                                    cellBlockEvent(temp, check, zombieAwake, zombieMemory);
                                }
                                else if (temp.Equals("f") && inventory.Any("9mm Handgun".Contains) && ammo.ContainsKey("9mm Bullets"))
                                {
                                    weapons(temp, zombieAwake);
                                    cellBlockEvent(temp, check, zombieAwake, zombieMemory);
                                }
                            }
                            else if (!inventory.Contains("9mm Handgun"))
                            {
                                Console.WriteLine("You can only choose a (m)elee option");
                                temp = Console.ReadLine().ToLower();
                                if (temp.Equals("m"))
                                {
                                    weapons(temp, zombieAwake);
                                    cellBlockEvent(temp, check, zombieAwake, zombieMemory);
                                }
                            }
                        }
                    }
                    else if (zombieMemory.Equals(2))
                    {
                        Console.WriteLine("You can't re-enter this cell while the zombie is in there. It'll attack you.");
                        cellBlockEvent(temp, check, zombieAwake, zombieMemory);
                    }
                    break;

                //Cell 3
                case 3:
                    if (zombies.ContainsKey(1) && zombies[1].Equals(alive) && zombieAwake.Equals(false))
                    {
                        Console.WriteLine("You decide to enter cell 3. The cells light is flickering and it looks like a slaughter house with blood covering majority of the cell.");
                        Console.WriteLine("You find a prison guard wearing riot gear or whats left of it - A pistol that is still in his holster.");
                        Console.WriteLine("Do you pick up the prison gaurds riot vest(Vest) or (Handgun) first?");
                        temp = Console.ReadLine().ToLower();
                        if (temp.Equals("vest"))
                        {
                            for (int i = 0; i < inventory.Length; i++)                  //Appends vest to inventory
                            {
                                if (inventory[i] == null && !inventory.Contains("vest"))
                                {
                                    inventory[i] = "vest";
                                }
                            }

                            zombieMemory = 3;
                            Console.WriteLine("You pick up the riot vest. You're supprised as to how it was the only gear left intact. As you take it off the guard you can't imagine the horrors he must've witnessed");
                            Console.WriteLine("You put the vest on and ponder what your next move will be. Whilst daydreaming you don't notice the prison guard begin to rise");
                            Console.WriteLine("You here a growl, turn around - You are now face to face with a grotesque spitting image of hell. The guards face is mangled in manner you've never seen before ");
                            Console.WriteLine("Whilst frozen in place from shock the guard now a zombie, lunges at you.\nDo you decide to lean left to dodge his attack or (l)ean (r)ight");
                            temp = Console.ReadLine().ToLower();

                            if (temp.Equals("l"))
                            {
                                Console.WriteLine("You lean left and dodge his attack and run out to the center of the cell block");
                                Console.WriteLine("The zombie guard now moving closer to you. You realise you have 2 options to either find some where to hide or try fight the zombie");
                                zombieAwake = true;
                                cellBlockEvent(temp, check, zombieAwake, zombieMemory);
                            }
                            else if (temp.Equals("r"))
                            {
                                Console.WriteLine("You lean right and dodge his attack and run out to the center of the cell block");
                                Console.WriteLine("The zombie guard now moving closer to you. You realise you have 2 options to either find some where to hide or try fight the zombie");
                                zombieAwake = true;
                                cellBlockEvent(temp, check, zombieAwake, zombieMemory);
                            }
                        }
                        if (temp.Equals("handgun"))
                        {
                            for (int i = 0; i < inventory.Length; i++)                       //Appends handgun to inventory
                            {
                                if (inventory[i] == null && !inventory.Contains("9mm Handgun"))
                                {
                                    inventory[i] = "9mm Handgun";
                                }
                            }
                            zombieMemory = 3;
                            Console.WriteLine("You pick up the handgun and inspect it. During your inspection You here a growl, you turn around slowly. Whilst frozen in place from shock the guard now a zombie, lunges at you.\nDo you decide to lean left to dodge his attack or (l)ean (r)ight");
                            temp = Console.ReadLine().ToLower();

                            if (temp.Equals("l"))
                            {
                                Console.WriteLine("You lean left and dodge his attack and run out to the center of the cell block");
                                Console.WriteLine("You've got a bit of time to assess the sitution. Do you check the handguns magazine for any remaining ammuination? (Y)es or (N)o?");
                                zombieAwake = true;
                                temp = Console.ReadLine().ToLower();

                                if (temp.Equals("y"))
                                {
                                    Console.WriteLine("You cock the slide back - lock it and release the magazine... Zero bullets...");
                                    check = true;
                                    cellBlockEvent(temp, check, zombieAwake, zombieMemory);
                                }
                                else if (temp.Equals("n"))
                                {
                                    Console.WriteLine("You decide this isn't the time to check");
                                    cellBlockEvent(temp, check, zombieAwake, zombieMemory);
                                }
                            }
                            else if (temp.Equals("r"))
                            {
                                Console.WriteLine("You lean right and dodge his attack and run out to the center of the cell block");
                                Console.WriteLine("You've got a bit of time to assess the sitution.\nDo you check the handguns magazine for any remaining ammuination? (Y)es or (N)o?");
                                zombieAwake = true;
                                temp = Console.ReadLine().ToLower();

                                if (temp.Equals("y"))
                                {
                                    Console.WriteLine("You cock the slide back - lock it and release the magazine... Zero bullets...");
                                    check = true;
                                    cellBlockEvent(temp, check, zombieAwake, zombieMemory);
                                }
                                else if (temp.Equals("n"))
                                {
                                    Console.WriteLine("You decide this isn't the time to check");
                                    cellBlockEvent(temp, check, zombieAwake, zombieMemory);
                                }
                            }
                        }
                    }
                    else if (!zombieMemory.Equals(3) && zombieAwake.Equals(true) && (zombies.ContainsKey(1) && zombies[1].Equals(alive)))
                    {
                        if (inventory.Contains("9mm Handgun"))
                        {
                            zombieMemory = 3;
                            Console.WriteLine("You run back to where the zombie rose. It walks in behind you.\nDo you (f)ire your handgun or (m)elee attack the zombie");
                            temp = Console.ReadLine().ToLower();
                            weapons(temp, zombieAwake);
                            cellBlockEvent(temp, check, zombieAwake, zombieMemory);
                        }
                        else if (!inventory.Contains("9mm Handgun"))
                        {
                            zombieMemory = 3;
                            Console.WriteLine("You run back to where the zombie rose. It walks in behind you.\nYou can only (m)elee attack");
                            temp = Console.ReadLine().ToLower();
                            weapons(temp, zombieAwake);
                            cellBlockEvent(temp, check, zombieAwake, zombieMemory);
                        }
                    }
                    else if (zombieMemory.Equals(3))
                    {
                        Console.WriteLine("You can't enter cell 3 while the zombie is in there. It'll attack you");
                        cellBlockEvent(temp, check, zombieAwake, zombieMemory);

                    }
                    else if (zombieAwake.Equals(false) && zombies.ContainsKey(1) && zombies[1].Equals(dead) && !inventory.Contains("Batton"))
                    {
                        Console.WriteLine("You explore cell 3 to see if you've missed anything. You find a batton and take it");
                        for (int i = 0; i < inventory.Length; i++)                              //Appends Batton to inventory
                        {
                            if (inventory[i] == null && !inventory.Contains("Batton"))
                            {
                                inventory[i] = "Batton";
                            }
                        }
                        cellBlockEvent(temp, check, zombieAwake, zombieMemory);
                    }
                    else if (zombieAwake.Equals(false) && zombies.ContainsKey(1) && zombies[1].Equals(dead) && inventory.Contains("Batton"))
                    {
                        Console.WriteLine("You explore cell 3 to see if you've missed anything. You find nothing... You've cleared the cell out");
                        cellBlockEvent(temp, check, zombieAwake, zombieMemory);
                    }
                    break;

                //Cell 4
                case 4:
                    if (zombieAwake.Equals(false) && (zombies.ContainsKey(1) && zombies[1].Equals(alive)) || zombieAwake.Equals(false) && ((zombies.ContainsKey(1) && zombies[1].Equals(dead))))
                    {
                        Console.WriteLine("You enter the cell. A body with its entrails gouged out");
                        if (ammoPickupCheck.Equals(false))
                        {
                            Console.WriteLine("To the left of him a pack of bullets.\nDo you choose to pick up the bullets. (Y)es or (N)o?");
                            temp = Console.ReadLine().ToLower();

                            if (temp.Equals("y"))
                            {
                                bullets = rand.Next(5, 15);

                                if (!ammo.ContainsKey("9mm Bullets"))
                                {
                                    ammo.Add("9mm Bullets", bullets);                                 //Appends 9mm Bullets to 'ammo' key pair                  
                                    Console.WriteLine("You pick up bullets. They're 9mm ammunition");
                                    foreach (KeyValuePair<string, int> ammo in ammo)
                                    {
                                        Console.WriteLine("You now have {0} {1} on you",
                                                  ammo.Value, ammo.Key);
                                    }
                                }
                                else if (ammoPickupCheck.Equals(false) && ammo.ContainsKey("9mm Bullets"))
                                {
                                    ammo.TryGetValue("9mm Bullets", out int currentValue);
                                    ammo["9mm Bullets"] = currentValue + bullets;
                                    Console.WriteLine("You pick up bullets. They're 9mm ammunition");
                                }

                                if (inventory != null)                                             //So inventory that you have already
                                {
                                    for (int i = 0; i < inventory.Length; i++)
                                    {
                                        if (inventory[i] != null)
                                        {
                                            Console.WriteLine($"You have {inventory[i]} in your inventory");
                                        }
                                    }
                                }
                                ammoPickupCheck = true;
                                cellBlockEvent(temp, check, zombieAwake, zombieMemory);
                            }
                            else if (temp.Equals("n"))
                            {
                                Console.WriteLine("You dont pick up the ammo...");
                                cellBlockEvent(temp, check, zombieAwake, zombieMemory);
                            }
                        }
                        else if (ammoPickupCheck.Equals(true))
                        {
                            Console.WriteLine("The cell is empty except for the dead body you first saw");
                            cellBlockEvent(temp, check, zombieAwake, zombieMemory);
                        }
                    }
                    else if (!zombieMemory.Equals(4) && zombieAwake.Equals(true) && (zombies.ContainsKey(1) && zombies[1].Equals(alive)))
                    {
                        zombieMemory = 4;
                        if (ammoPickupCheck.Equals(false))
                        {
                            Console.WriteLine("You enter the cell in a pace. A body with its entrails gouged out. To the left of him a pack of bullets");
                            Console.WriteLine("whilst you have a bit of time do you choose to pick up the bullets? (Y)es or (N)o");
                            temp = Console.ReadLine().ToLower();

                            if (temp.Equals("y"))
                            {
                                bullets = rand.Next(5, 15);
                                if (!ammo.ContainsKey("9mm Bullets"))
                                {
                                    ammo.Add("9mm Bullets", bullets);                                 //Appends 9mm Bullets to 'ammo' key pair                  
                                    Console.WriteLine("You pick up bullets. They're 9mm ammunition");
                                    foreach (KeyValuePair<string, int> ammo in ammo)
                                    {
                                        Console.WriteLine("You now have {0} {1} on you",
                                                  ammo.Value, ammo.Key);
                                    }
                                }
                                else if (ammoPickupCheck.Equals(false) && ammo.ContainsKey("9mm Bullets"))
                                {
                                    ammo.TryGetValue("9mm Bullets", out int currentValue);
                                    ammo["9mm Bullets"] = currentValue + bullets;
                                    Console.WriteLine("You pick up bullets. They're 9mm ammunition");
                                }

                                foreach (KeyValuePair<string, int> ammo in ammo)
                                {
                                    Console.WriteLine("You now have {1} {0} on you", ammo.Key, ammo.Value);
                                }

                                if (inventory != null)
                                {
                                    for (int i = 0; i < inventory.Length; i++)
                                    {
                                        if (inventory[i] != null)
                                        {
                                            Console.WriteLine($"You have {inventory[i]} in your inventory");
                                        }
                                    }
                                }
                                ammoPickupCheck = true;
                                Console.WriteLine("Now face to face with the zombie do you (f)ire your handgun or(m)elee the zombie?");
                                temp = Console.ReadLine().ToLower();

                                if (temp.Equals("f") && check.Equals(false) && ammo.ContainsKey("9mm Bullets"))
                                {
                                    int chance = rand.Next(1, 11);
                                    Console.WriteLine("You reload...");
                                    check = true;
                                    if (chance >= 3 && chance <= 8)
                                    {
                                        zombiesDamage(0, cellBlockZombies, chance);
                                        Console.WriteLine("...Shoud've checked the gun...");
                                    }
                                }
                                weapons(temp, zombieAwake);
                                Console.WriteLine("You run past the zombie doging its attatck again");
                                cellBlockEvent(temp, check, zombieAwake, zombieMemory);
                            }
                            else if (temp.Equals("n"))
                            {
                                Console.WriteLine("You dont pick up the ammo...");
                                Console.WriteLine("Now face to face with the zombie do you (f)ire your handgun or(m)elee the zombie?");
                                temp = Console.ReadLine().ToLower();

                                weapons(temp, zombieAwake);
                                Console.WriteLine("You run past the zombie doging its attatck again");
                                cellBlockEvent(temp, check, zombieAwake, zombieMemory);
                            }
                        }
                        else if (ammoPickupCheck.Equals(false) && !inventory.Contains("9mm Handgun"))
                        {
                            Console.WriteLine("The cell is empty except for the dead body already there. The zombie follows you in\nYou can only (m)elee attack");
                            temp = Console.ReadLine().ToLower();
                            weapons(temp, zombieAwake);
                            cellBlockEvent(temp, check, zombieAwake, zombieMemory);
                        }
                        else if (ammoPickupCheck.Equals(true) && inventory.Contains("9mm Handgun"))
                        {
                            Console.WriteLine("The cell is empty except for the dead body already there. The zombie follows you in.\nDo you (f)ire at the zombie or (m)elee it?");
                            temp = Console.ReadLine().ToLower();
                            weapons(temp, zombieAwake);
                            cellBlockEvent(temp, check, zombieAwake, zombieMemory);
                        }
                    }
                    else if (zombieMemory.Equals(4))
                    {
                        Console.WriteLine("You can't enter this cell while the zombie is in there. It'll attack you");
                        cellBlockEvent(temp, check, zombieAwake, zombieMemory);
                    }
                    break;

                default:
                    Console.WriteLine("Choose a cell");
                    break;
            }
        }

        /**@corridor()
         * The corridor method. Intiate corridor events
         */
        static void corridor()
        {
            bool check = false;
            string temp = null;
            int luck = 0, damage;
            if (fear > 100)
            {
                fear = 100;
            }
            fear += 25;
            if (!enteredCheck.Equals(true))
            {
                enteredCheck = true;
                Console.WriteLine("You enter the corridor. You're surrounded by a mob of zombies they all turn around and try grab you");
                Console.WriteLine("Do you go l or r?");
                do
                {
                    try
                    {
                        temp = Console.ReadLine().ToLower();

                        if (temp.Equals("r") || temp.Equals("l"))
                        {
                            switch (temp)
                            {
                                case "l":

                                    Console.WriteLine("You decide to run left. As you turn a zombie grabs you and tries to bite you neck");
                                    Console.WriteLine("Do you headbutt the zombie or try push it off you?");
                                    temp = Console.ReadLine().ToLower();
                                    if (temp.Equals("headbutt"))
                                    {
                                        fear -= rand.Next(4, 10);
                                        Console.WriteLine("You head butt the zombie holding you and make a run for it down the left side of the corridor");
                                    }
                                    else if (temp.Equals("push"))
                                    {
                                        luck = rand.Next(1, 13);
                                        if ((luck >= 6 && luck <= 9) && !luck.Equals(13))
                                        {
                                            damage = rand.Next(5, 8);
                                            playerHealth(damage);
                                            fear += rand.Next(3, 8);
                                            Console.WriteLine($"As you push past the zombie but it manages scratch you.\nYou take {-damage} damage\nYou enter the courtyard");
                                            courtyardCheck = true;
                                            courtyard();
                                            // corridor(); //For testing
                                        }
                                        else if (luck.Equals(13))
                                        {
                                            Console.WriteLine("The zombie bites into your neck");
                                            health = 0;
                                            playerHealth(health);
                                            // corridor(); //For testing
                                        }
                                        else
                                        {
                                            fear -= rand.Next(1, 5);
                                            Console.WriteLine("You push past the zombie and make a run for it down the left side of the corridor");
                                            Console.WriteLine("You see the door to the court yard a crawling zombie is blocking the door way.\nDo you try (jump) over the zombie or make a dive for it?");
                                            temp = Console.ReadLine().ToLower();
                                            if (temp.Equals("dive"))
                                            {
                                                if (luck >= 6 && luck <= 9)
                                                {
                                                    Console.WriteLine("You dive over the zombie and enter the courtyard. You look down and see that the zombie scratched you");
                                                    playerHealth(rand.Next(1, 5));
                                                    courtyardCheck = true;
                                                    courtyard();
                                                }
                                                else
                                                {
                                                    Console.WriteLine("You dive over the zombie and enter the courtyard");
                                                    courtyardCheck = true;
                                                    courtyard();
                                                }

                                            }
                                            else if (temp.Equals("jump"))
                                            {
                                                if ((luck >= 3 && luck <= 10) && !luck.Equals(13))
                                                {
                                                    Console.WriteLine("You jump over the zombie and enter the courtyard. You look down and see that the zombie scratched your leg");
                                                    playerHealth(rand.Next(1, 5));
                                                    courtyardCheck = true;
                                                    courtyard();
                                                }
                                                else if (luck.Equals(13))
                                                {
                                                    Console.WriteLine("You try jump the zombie but it grabs you leg and you fall. Before you can get up the mob of zombies pile on top of you and begin to devour you limb by limb.");
                                                    health = 0;
                                                    playerHealth(health);
                                                }
                                            }
                                        }
                                    }
                                    break;

                                case "r":

                                    Console.WriteLine("You decide to run right. As you turn a crawling zombie grabs your foot");
                                    Console.WriteLine("Do you kick the zombie or stomp on its head?");
                                    temp = Console.ReadLine().ToLower();
                                    if (temp.Equals("kick"))
                                    {
                                        fear -= rand.Next(5, 10);
                                        Console.WriteLine("You kick the zombie holding you foot and make a run for it down the right side of the corridor");
                                        Console.WriteLine("Whilst running you see the showers. Do you continue down the right of the corrider or go into the showers?\nContinue or showers");
                                        temp = Console.ReadLine().ToLower();
                                        if (temp.Equals("continue"))
                                        {
                                            //TODO
                                        }
                                        else if (temp.Equals("showers"))
                                        {
                                            Console.WriteLine("You decide to run to the showers");
                                            showerCheck = true;
                                            showers();
                                        }
                                    }
                                    else if (temp.Equals("stomp"))
                                    {
                                        luck = rand.Next(1, 13);
                                        if ((luck >= 6 && luck <= 9) && !luck.Equals(13))
                                        {
                                            damage = rand.Next(10, 18);
                                            playerHealth(damage);
                                            fear += rand.Next(3, 8);
                                            Console.WriteLine($"As you try stomp the zombie it moves its head... the ground shock to your leg hurts but you manage to limp past it. As you do the zombie gives you a deep scratch.\nYou take {-damage} damage");
                                            // corridor(); //For testing
                                        }
                                        else if (luck.Equals(13))
                                        {
                                            Console.WriteLine("You miss and the zombie bites your calf causing you to fall over. The other zombies pile up on top of you. You scream as you finally experience the tourment many of the others went through... ");
                                            health = 0;
                                            playerHealth(health);
                                            // corridor(); //For testing
                                        }
                                        else
                                        {
                                            fear -= rand.Next(5, 10);
                                            Console.WriteLine("You stomp on the zombies head killing it and make a run for it down the right side of the corrider");
                                            Console.WriteLine("Whilst running you see the showers. Do you continue down the right of the corrider or go into the showers?\nContinue or showers");
                                            temp = Console.ReadLine().ToLower();
                                            if (temp.Equals("continue"))
                                            {
                                                //TODO
                                            }
                                            else if (temp.Equals("showers"))
                                            {
                                                Console.WriteLine("You decide to run to the showers");
                                                showerCheck = true;
                                                showers();
                                            }
                                            // corridor(); //For testing
                                        }
                                    }
                                    break;
                            }
                        }
                        else
                        {
                            Console.WriteLine("Choose either r or l option");
                        }

                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                } while (!(temp.Equals("r") || temp.Equals("l")));
            }
            else if (enteredCheck.Equals(true))
            {
                Console.WriteLine("You re-enter the corridor and run past the mob of zombie");
                do
                {
                    try
                    {
                        if (courtyardCheck.Equals(true) && !showerCheck.Equals(true))
                        {
                            Console.WriteLine("Do you run to cellblock 2, the showers or down the rest of the corridor");
                        }
                        else if (showerCheck.Equals(true) && !courtyardCheck.Equals(true))
                        {
                            Console.WriteLine("Do you run to cellblock 2, the courtyard or down the rest of the corridor");
                        }
                        else if (showerCheck.Equals(true) && courtyardCheck.Equals(true))
                        {
                            Console.WriteLine("Do you run to cellblock 2 or go back?");
                        }
                        temp = Console.ReadLine().ToLower();
                        switch (temp)
                        {
                            case "cellblock 2":
                                Console.WriteLine("You decide to re-enter cell block 2");
                                check = true;
                                cellBlock2();
                                break;
                            case "showers":
                                Console.WriteLine("You decide to enter the showers");
                                showerCheck = true;
                                check = true;
                                showers();
                                break;
                            case "corridor":
                                Console.WriteLine("You run down the rest of the corridor and close the door behind you (or lock)\nYou see three rooms in front of you. Do you go into the (bathroom), (offices) or (vistors) room?");
                                Console.ReadLine().ToLower();

                                if (temp.Equals("bathroom"))
                                {
                                    Console.WriteLine("You decie to enter the bathroom");
                                    check = true;
                                    //bathroom();
                                }
                                else if (temp.Equals("offices"))
                                {
                                    Console.WriteLine("You decie to enter the offices area");
                                    check = true;
                                    //offices();
                                }
                                else if (temp.Equals("vistors"))
                                {
                                    Console.WriteLine("You decie to enter the vistors room");
                                    check = true;
                                    VisitorRoom();
                                }
                                break;
                            case "back":
                                check = true;
                                Console.WriteLine("You go back to where you saw the three rooms. Do you go into the (bathroom), (offices) or (vistors) room?");
                                Console.ReadLine().ToLower();

                                if (temp.Equals("bathroom"))
                                {
                                    Console.WriteLine("You decie to enter the bathroom");
                                    //bathroom();
                                }
                                else if (temp.Equals("offices"))
                                {
                                    Console.WriteLine("You decie to enter the offices area");
                                    //offices();
                                }
                                else if (temp.Equals("vistors"))
                                {
                                    Console.WriteLine("You decie to enter the vistors room");
                                    VisitorRoom();
                                }
                                break;
                            default:
                                Console.WriteLine("Enter an answer");
                                break;

                            case "courtyard":
                                check = true;
                                Console.WriteLine("You decide to enter the courtyard");
                                Console.WriteLine("You see the door to the court yard a crawling zombie is blocking the door way.\nDo you try (jump) over the zombie or make a dive for it?");
                                temp = Console.ReadLine().ToLower();

                                if (temp.Equals("dive"))
                                {
                                    if (luck >= 6 && luck <= 9)
                                    {
                                        Console.WriteLine("You dive over the zombie and enter the courtyard. You look down and see that the zombie scratched you");
                                        playerHealth(rand.Next(1, 5));
                                        enteredCheck = true;
                                        courtyardCheck = true;
                                        courtyard();
                                    }
                                    else
                                    {
                                        Console.WriteLine("You dive over the zombie and enter the courtyard");
                                        enteredCheck = true;
                                        courtyardCheck = true;
                                        courtyard();
                                    }

                                }
                                else if (temp.Equals("jump"))
                                {
                                    if ((luck >= 3 && luck <= 10) && !luck.Equals(13))
                                    {
                                        Console.WriteLine("You jump over the zombie and enter the courtyard. You look down and see that the zombie scratched your leg");
                                        playerHealth(rand.Next(1, 5));
                                        enteredCheck = true;
                                        courtyardCheck = true;
                                        courtyard();
                                    }
                                    else if (luck.Equals(13))
                                    {
                                        Console.WriteLine("You try jump the zombie but it grabs you leg and you fall. Before you can get up the mob of zombies pile on top of you and begin to devour you limb by limb.");
                                        health = 0;
                                        playerHealth(health);
                                    }
                                }
                                courtyardCheck = true;
                                courtyard();
                                break;
                        }

                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine();
                    }
                } while (!(temp.Equals("cellblock 2") || temp.Equals("showers") || temp.Equals("corridor") || temp.Equals("courtyard") || temp.Equals("back") || temp.Equals("vistors")));
            }

        }

        /**
         * @cellBlockEvent
         * Controls all cellblock events
         */
        static void cellBlockEvent(string option, bool check, bool zombieAwake, int zombieMemory)
        {
            if (health <= 0)
            {
                Died();
            }
            zombies.TryGetValue(cellBlockZombies, out bool x);                  //Finds zombie through key = cellBlockZombies and appends all changes to that select zombie
            bool tempAlive = x;

            if (x.Equals(false))
            {
                zombieAwake = false;

                if (!inventory.Contains("vest"))
                {
                    for (int i = 0; i < inventory.Length; i++)                  //Appends vest to inventory
                    {
                        if (inventory[i] == null && !inventory.Contains("vest"))
                        {
                            inventory[i] = "vest";
                        }
                    }
                    Console.WriteLine("You take the vest off of the zombie it'll help with protection later on");
                }

                if (!inventory.Contains("9mm Handgun"))
                {
                    for (int i = 0; i < inventory.Length; i++)                       //Appends handgun to inventory
                    {
                        if (inventory[i] == null && !inventory.Contains("9mm Handgun"))
                        {
                            inventory[i] = "9mm Handgun";
                        }
                    }
                    Console.WriteLine("You take the handgun off of the zombie");
                }
            }

            if (fear > 100)
            {
                fear = 100;
            }

            string temp;
            switch (cellBlockZombies)
            {
                case 1:
                    if (zombieAwake.Equals(true) && zombies.ContainsKey(cellBlockZombies) && zombies[cellBlockZombies].Equals(alive))
                    {
                        Console.WriteLine("You're in the centre of the cellblock. Which (c)ell do you decide to enter or do you try (r)un, use you med(k)it, check your (i)nventory or (f)ight the zombie?");
                    }
                    else if (zombieAwake.Equals(false) && zombies.ContainsKey(cellBlockZombies) && (x.Equals(false) || x.Equals(true)))
                    {
                        Console.WriteLine("You're in the centre of the cellblock. Which (c)ell do you decide to enter or do you try (r)un, use you med(k)it or check your (i)nventory?");
                    }
                    break;
            }

            temp = Console.ReadLine().ToLower();

            if (temp.Equals("c"))
            {
                Console.WriteLine("Cell 1, 2, 3 or 4");
                temp = Console.ReadLine();
                cellBlock2Options(Convert.ToInt32(temp), check, zombieAwake, zombieMemory);
            }
            else if (!temp.Equals("c"))
            {
                option = temp;
            }

            switch (option)
            {
                case "h":                                           //Hide option
                    if (cellBlockZombies.Equals(1) && zombieAwake.Equals(true) && (zombies.ContainsKey(cellBlockZombies) && zombies[cellBlockZombies].Equals(true)))
                    {
                        Console.WriteLine("You decide to hide. Which cell do you choose?\nCell 1, 2, 3 or 4");
                        temp = Console.ReadLine();
                        cellBlock2Options(Convert.ToInt32(temp), check, zombieAwake, zombieMemory);
                    }
                    break;

                case "f":                                           //Fight option within centre cell

                    if (zombies.ContainsKey(cellBlockZombies) && zombies[cellBlockZombies].Equals(true))
                    {
                        if (check.Equals(false))
                        {
                            if (inventory.Contains("9mm Handgun"))
                            {
                                Console.WriteLine("Do you (m)elee the zombie or try(f)ire your handgun at it?");
                                temp = Console.ReadLine().ToLower();
                                if (temp.Equals("f") && !ammo.ContainsKey("9mm Bullets"))
                                {
                                    weapons(temp, zombieAwake);
                                }
                                else if (temp.Equals("f") && ammo.ContainsKey("9mm Bullets"))
                                {
                                    Console.WriteLine("You reload...");
                                    check = true;
                                    weapons(temp, zombieAwake);
                                }
                                else if (temp.Equals("m"))
                                {
                                    weapons(temp, zombieAwake);
                                }

                                Console.WriteLine("Do you (h)ide or (r)un?");
                                temp = Console.ReadLine().ToLower();

                                if (temp.Equals("h"))
                                {
                                    Console.WriteLine("What cell do you choose to hide in?\nCell 1, 2, 3 or 4?");
                                    temp = Console.ReadLine().ToLower();
                                    cellBlock2Options(Convert.ToInt32(temp), check, zombieAwake, zombieMemory);
                                }
                                else if (temp.Equals("r"))
                                {
                                    Console.WriteLine("You make a run for it. Where do you decide to run to? Coorridor");
                                    temp = Console.ReadLine().ToLower();
                                    if (temp.Equals("Coorridor"))
                                    {
                                        corridor();
                                    }
                                }
                            }
                            else if (!inventory.Contains("9mm Handgun"))
                            {
                                Console.WriteLine("You can only (m)elee attack?");
                                temp = Console.ReadLine().ToLower();
                                weapons(temp, zombieAwake);
                                Console.WriteLine("Do you (h)ide or (r)un?");
                                temp = Console.ReadLine().ToLower();

                                if (temp.Equals("h"))
                                {
                                    Console.WriteLine("What cell do you choose to hide in?\nCell 1, 2, 3 or 4?");
                                    temp = Console.ReadLine().ToLower();
                                    cellBlock2Options(Convert.ToInt32(temp), check, zombieAwake, zombieMemory);
                                }
                            }
                        }
                        else if (check.Equals(true))
                        {
                            Console.WriteLine("Do you (m)elee the zombie or (f)ire at it?");
                            temp = Console.ReadLine().ToLower();

                            if (temp.Equals("f"))
                            {
                                weapons(temp, zombieAwake);
                            }
                            else if (temp.Equals("m"))
                            {
                                weapons(temp, zombieAwake);
                            }

                            Console.WriteLine("Do you (h)ide or (r)un?");
                            temp = Console.ReadLine().ToLower();

                            if (temp.Equals("h"))
                            {
                                Console.WriteLine("What cell do you choose to hide in?\nCell 1, 2, 3 or 4?");
                                temp = Console.ReadLine().ToLower();
                                cellBlock2Options(Convert.ToInt32(temp), check, zombieAwake, zombieMemory);
                            }
                            else if (temp.Equals("r"))
                            {
                                Console.WriteLine("You make a run for it. Where do you decide to run to?");
                            }
                        }
                    }
                    break;

                case "k":                                           //Med kit
                    if (inventory.Contains("Med kit") && !health.Equals(100))
                    {
                        Console.WriteLine("You use med kit");
                        health += 15;

                        inventory[0] = "";

                        if (health > 100)
                        {
                            health = 100;
                        }

                        Console.WriteLine($"You now have {health} health");
                        cellBlockEvent(option, check, zombieAwake, zombieMemory);
                    }
                    else if (!inventory.Contains("Med kit"))
                    {
                        Console.WriteLine("You don't have a health kit...");
                        cellBlockEvent(option, check, zombieAwake, zombieMemory);
                    }
                    else if (inventory.Contains("Med kit") && health.Equals(100))
                    {
                        Console.WriteLine("You don't need to use a med kit you already have full health");
                        cellBlockEvent(option, check, zombieAwake, zombieMemory);
                    }
                    break;

                case "i":                                           //Inventory check

                    if (!inventory.Any().Equals(""))                        //TO BE WORKED ON
                    {
                        Console.WriteLine("You check your inventory\nYou have ");
                        for (int i = 0; i < inventory.Length; i++)
                        {

                            if (inventory[i] != null)
                            {
                                Console.Write($"[{inventory[i]}]");
                            }
                        }
                    }
                    else
                    {
                        Console.Write("You have nothing in your inventory");
                    }

                    if (ammo.Any())
                    {
                        Console.Write(" and ");
                        foreach (KeyValuePair<string, int> ammo in ammo)
                        {
                            Console.Write("{0} {1}",
                                      ammo.Value, ammo.Key);
                        }
                        Console.WriteLine(" ammunition.");
                    }
                    else
                    {
                        Console.Write(" and zero ammunition.");
                    }

                    cellBlockEvent(option, check, zombieAwake, zombieMemory);
                    break;

                case "r":
                    Console.WriteLine("Do you choose to run to the corridor?");
                    temp = Console.ReadLine().ToLower();

                    if (temp.Equals("corridor"))
                    {
                        corridor();
                    }
                    break;
            }
        }


        /**@weapons
         * Lets player choose a weapon or melee option to use
         */
        static void weapons(string temp, bool awake)
        {
            int tempHold = 0, currentValue, count = 0, attack = 0, chance, inaccuracy;
            zombieHealth.TryGetValue(cellBlockZombies, out int x);
            int health = x;
            switch (temp)
            {
                case "f":
                    do
                    {
                        chance = rand.Next(1, 11);

                        if (ammo.TryGetValue("9mm Bullets", out currentValue))
                        {
                            tempHold = currentValue;
                            if (tempHold > 0)
                            {
                                Console.WriteLine($"You have {currentValue} 9mm bullets left");
                                Console.WriteLine("How many bullets do you decide to fire?");

                                temp = Console.ReadLine();

                                if (Convert.ToInt32(temp) > 15 && !count.Equals(15))
                                {
                                    Console.WriteLine("You can't shoot more then the mag can hold");
                                    Console.WriteLine("Do you fire again?\n(Y)es or (N)o?");
                                    temp = Console.ReadLine().ToLower();
                                }
                                else if (Convert.ToInt32(temp) > currentValue)
                                {
                                    Console.WriteLine("You don't have enough ammo to fire that many shoots");
                                    Console.WriteLine("Choose an ammout currently on you");
                                    Console.WriteLine("Do you fire again?\n(Y)es or (N)o?");
                                    temp = Console.ReadLine().ToLower();
                                }
                                else
                                {
                                    if (zombies[cellBlockZombies].Equals(true))
                                    {
                                        Console.WriteLine($"You fire {temp} bullets");
                                        count += Convert.ToInt32(temp);

                                        for (int i = 0; i < Convert.ToInt32(temp); i++)
                                        {
                                            inaccuracy = rand.Next(2, 7) / fear;
                                            if (inaccuracy > 15)
                                            {
                                                Console.WriteLine("Shot {i} fired. You miss and deal 0 damage");
                                                zombiesDamage(0, cellBlockZombies, chance);
                                            }
                                            else
                                            {
                                                attack = rand.Next(4, 10) * Convert.ToInt32(temp);
                                                Console.WriteLine($"Shot {i} fired. You deal -{attack} damage");
                                                zombiesDamage(attack, cellBlockZombies, chance);
                                            }
                                        }
                                    }
                                    else if (zombies[cellBlockZombies].Equals(false))
                                    {
                                        Console.WriteLine($"You fire {temp} bullets");
                                        count += Convert.ToInt32(temp);
                                        Console.WriteLine($"The zombie's already dead... you waste {temp} bullets");
                                    }

                                    ammo["9mm Bullets"] = currentValue - Convert.ToInt32(temp);
                                    tempHold = currentValue;

                                    Console.WriteLine("Do you fire again or (r)eload?\n(Y)es or (N)o");
                                    temp = Console.ReadLine().ToLower();
                                    if (count.Equals(15) && currentValue > 0)
                                    {
                                        Console.WriteLine("You reload first");
                                        zombiesDamage(0, cellBlockZombies, chance);
                                        count = 0;
                                        Console.WriteLine("Do you fire again?\n(Y)es or (N)o?");
                                        temp = Console.ReadLine().ToLower();
                                    }
                                    else if (temp.Equals("r") && !currentValue.Equals(0) && (count > 0 && count < 15))
                                    {
                                        if (currentValue <= 15)
                                        {
                                            Console.WriteLine("You cant reload");
                                            Console.WriteLine("Do you fire again?\n(Y)es or (N)o?");
                                            temp = Console.ReadLine().ToLower();
                                        }
                                        else
                                        {
                                            Console.WriteLine("You reload");
                                            zombiesDamage(0, cellBlockZombies, chance);
                                            count = 0;
                                            Console.WriteLine("Do you fire again?\n(Y)es or (N)o");
                                            temp = Console.ReadLine().ToLower();
                                        }
                                    }
                                }
                            }
                            else if (tempHold.Equals(0))
                            {
                                count.Equals(0);
                                Console.WriteLine("You can't fire you're out of ammo");
                                Console.WriteLine("Do you fire again? (Y)es or (N)o");
                                temp = Console.ReadLine().ToLower();

                                if (temp.Equals("n"))
                                {
                                    Console.WriteLine("You don't shoot");
                                    Console.WriteLine($"You have {currentValue} 9mm bullets left");
                                    zombiesDamage(0, cellBlockZombies, chance);
                                }
                            }
                        }
                        else if (!ammo.ContainsKey("9mm Bullets"))
                        {
                            Console.WriteLine("You don't have any ammo...");
                            Console.WriteLine("Do you fire again? (Y)es or (N)o");
                            temp = Console.ReadLine().ToLower();

                            if (temp.Equals("n"))
                            {
                                Console.WriteLine("You don't shoot");
                                Console.WriteLine($"You have {currentValue} 9mm bullets left");
                                zombiesDamage(0, cellBlockZombies, chance);
                            }
                        }

                    } while (currentValue.Equals(tempHold) && temp.Equals("y") && health > 0);
                    //LOOK INTO
                    if (health <= 0)
                    {
                        //playerHealth(0);
                        Died();
                    }
                    break;

                case "m":
                    chance = rand.Next(1, 4);
                    if (!inventory.Contains("Batton"))
                    {
                        Console.WriteLine("Do you (p)unch or (k)ick the Zombie?");
                    }
                    else if (inventory.Contains("Batton"))
                    {
                        Console.WriteLine("Do you (p)unch, (k)ick the Zombie or use your (b)atton?");
                    }
                    temp = Console.ReadLine().ToLower();
                    if (temp.Equals("p"))
                    {
                        attack = rand.Next(1, 11);

                        if (chance.Equals(2))
                        {
                            Console.WriteLine("You try punch the zombie and miss...");
                            zombiesDamage(0, cellBlockZombies, chance);
                        }
                        else
                        {
                            Console.WriteLine($"You punch the zombie");
                            zombiesDamage(attack, cellBlockZombies, chance);
                        }
                    }
                    else if (temp.Equals("k"))
                    {
                        attack = rand.Next(1, 11);

                        if (chance.Equals(2))
                        {
                            Console.WriteLine("You try kick the zombie and miss..");
                            zombiesDamage(0, cellBlockZombies, chance);
                        }
                        else
                        {
                            Console.WriteLine($"You kick the zombie");
                            zombiesDamage(attack, cellBlockZombies, chance);
                        }
                    }
                    else if (temp.Equals("b"))
                    {
                        attack = rand.Next(1, 11);

                        if (chance.Equals(2))
                        {
                            Console.WriteLine("You swing and miss...");
                            zombiesDamage(0, cellBlockZombies, chance);
                        }
                        else
                        {
                            Console.WriteLine($"You swing you batton at the zombie and hit it");
                            zombiesDamage(attack, cellBlockZombies, chance);
                        }

                    }
                    break;
            }
        }


        /*@zombiesDamge
         * Controls the health and attack of a select zombie through zombieNum = zombies.key(xyz)
         */
        static void zombiesDamage(int damage, int zombieNum, int chance)
        {
            int tempPDamage;
            switch (zombieNum)
            {
                case 1:
                    if (zombies.ContainsKey(zombieNum).Equals(zombieHealth.ContainsKey(zombieNum)))
                    {
                        tempPDamage = rand.Next(15, 35);
                        if (zombieHealth.TryGetValue(zombieNum, out int currentValue) && currentValue > 0 && !chance.Equals(2))
                        {
                            fear -= rand.Next(1, 3);
                            Console.WriteLine($"The zombie takes {damage} damage");
                            zombieHealth[zombieNum] = currentValue - damage;
                            Console.WriteLine($"Zombie now has {zombieHealth[zombieNum].ToString()}");
                        }
                        else if (currentValue > 0 && chance.Equals(2))
                        {
                            Console.WriteLine($"The zombie attacks you. You take {tempPDamage} damage");
                            if (tempPDamage > 20)
                            {
                                fear += rand.Next(5, 10);
                                playerHealth(tempPDamage);
                            }
                            else if (tempPDamage < 20)
                            {
                                fear += rand.Next(10, 20);
                                playerHealth(tempPDamage);
                            }
                            else if (tempPDamage.Equals(30))
                            {
                                Console.WriteLine("Critical hit");
                                fear += 30;
                                playerHealth(tempPDamage);
                            }
                        }
                        else if (zombieHealth.ContainsKey(1) && currentValue <= 0)
                        {
                            fear -= 10;
                            zombies[zombieNum] = false;
                            Console.WriteLine("You've killed the zombie\nZombie is dead");
                        }
                    }
                    break;
            }
        }


        /**@playerHealth controls player health
         */
        static void playerHealth(int value)
        {
            if (health > 0)
            {
                if (inventory.Contains("vest"))
                {
                    fear += rand.Next(5, 10);
                    health -= value / 2;
                    Console.WriteLine($"Ow...\nPlayer now has {health} health left");
                }
                else if (!inventory.Contains("vest"))
                {
                    fear += rand.Next(8, 200);
                    health -= value;
                    Console.WriteLine($"Ouch...\nPlayer now has {health} health left");
                }
            }

            if (health <= 0)
            {
                fear = 5;
                Died();
            }
        }

        static void courtyard()
        {
            Random Rand = new Random();
            Console.WriteLine("You walk into the courtyard. There are zombies everywhere.");
            Console.WriteLine("You realize that they are in a dull state and don't realize you're there yet. You might be able to sneak around them");
            Console.WriteLine("You notice the dead prison guard at the other end of the courtyard, his body has a gun and a key card.");
            Console.WriteLine("If you don't have a weapon on you, this could end badly.");
            /*CREATE VARIABLE CALLED WEAPONCHECK(); AND IF IT IS TICKED THEN CREATE DIFFERENT OPTIONS FOR PLAYER */
            Console.WriteLine("Do you sneak forward (Y) or do you leave the Courtyard (N)");
            string usersinputs = Console.ReadLine();
            if (usersinputs == "Y" || usersinputs == "y")
            {
                Console.WriteLine("You start sneaking towards the dead guard.");
                Console.WriteLine("You step as quietly as possible around the mumbling, walking dead bodies.");
                int randzombie = Rand.Next(1, 6);
                if (randzombie == 3)
                {
                    Console.WriteLine("You suddenly are thrown to the ground as a zombie jumps on top of you.");
                    Console.WriteLine("The last thing you hear is the crunching sound of your head being bitten");
                    Console.WriteLine("YOU DIED");
                    Console.Beep((int)294.2, 800);
                    Console.Beep((int)277.2, 800);
                    Console.Beep((int)262.2, 800);
                    Console.Beep((int)247.2, 1100);
                    Console.ReadLine();
                }
                else
                {
                    Console.WriteLine("You make it to the dead guard's body. You take the key card and the gun.");
                    /*INSERT INVENTORY GET GUN AND KEYCARD CODE HERE */
                    string guardgun = "yes";
                    string keycard = "yes";
                    Console.WriteLine("Do you try sneaking back through the crowd of zombies (Y) or shoot your way out? (N)");
                    string seconduserinput = Console.ReadLine();
                    int randzombie2 = Rand.Next(1, 6);
                    if (seconduserinput == "Y" || seconduserinput == "y")
                    {
                        Console.WriteLine("You try crawling your way past the zombies.");
                        if (randzombie >= 3)
                        {
                            Console.WriteLine("You bump into a zombie's leg. It yells and tries to bite you.");
                            Console.WriteLine("Push the zombie away (Y) or shoot it (N) ?");
                            string thirduserinput = Console.ReadLine();
                            if (thirduserinput == "Y" || thirduserinput == "y")
                            {
                                Console.WriteLine("You try to push the zombie away...");
                                if (randzombie2 == 5)
                                {
                                    Console.WriteLine("As you stretch out your arm, another zombie bites off your entire hand.");
                                    Console.WriteLine("The last thing you see is the zombies eating your flesh.");
                                    Console.WriteLine("YOU DIED");
                                    Console.Beep((int)294.2, 800);
                                    Console.Beep((int)277.2, 800);
                                    Console.Beep((int)262.2, 800);
                                    Console.Beep((int)247.2, 1100);
                                    Console.ReadLine();
                                }
                                else
                                {
                                    Console.WriteLine("You pushed the zombie back and ran to the exit.");
                                    Console.WriteLine("You escaped the courtyard.");
                                    Console.ReadLine();
                                }
                            }
                            else
                            {
                                Console.WriteLine("You aim the gun and fire it at the zombie");
                                Console.WriteLine("The rest of the zombies turn around and start moving towards you.");
                                Console.WriteLine("Run straight for the exit (Y) or shoot the zombies as you go (N)?");
                                string fourthuserinput = Console.ReadLine();
                                if (fourthuserinput == "Y" || fourthuserinput == "y")
                                {
                                    Console.WriteLine("You run straight for the exit and escape the Courtyard.");
                                    Console.ReadLine();
                                }
                                else
                                {
                                    Console.WriteLine("You try to shoot the zombies as you move towards the exit");
                                    if (randzombie2 == 5)
                                    {
                                        Console.WriteLine("You try to shoot the zombie in front of you by putting the barrel against the zombie's temple.");
                                        Console.WriteLine("Your gun jams.");
                                        Console.WriteLine("You are paralyzed in fear as the zombie proceeds to bite your fingers off the gun.");
                                        Console.WriteLine("The last thing you see is the zombies eating your flesh.");
                                        Console.WriteLine("YOU DIED");
                                        Console.Beep((int)294.2, 800);
                                        Console.Beep((int)277.2, 800);
                                        Console.Beep((int)262.2, 800);
                                        Console.Beep((int)247.2, 1100);
                                        Console.ReadLine();
                                    }
                                }
                            }
                        }
                        else
                        {
                            Console.WriteLine("You escaped the courtyard.");
                        }
                    }
                    else
                    {
                        Console.WriteLine("You aim the gun at the nearest zombie and fire");
                        Console.WriteLine("The rest of the zombies turn around and start moving towards you");
                        Console.WriteLine("You try to shoot the zombies as you move towards the exit");
                        if (randzombie2 == 5)
                        {
                            Console.WriteLine("You try to shoot the zombie in front of you by putting the barrel against the zombie's temple.");
                            Console.WriteLine("Your gun jams.");
                            Console.WriteLine("You are paralyzed in fear as the zombie proceeds to bite your fingers off the gun.");
                            Console.WriteLine("The last thing you see is the zombies eating your flesh.");
                            Console.WriteLine("YOU DIED");
                            Console.Beep((int)294.2, 800);
                            Console.Beep((int)277.2, 800);
                            Console.Beep((int)262.2, 800);
                            Console.Beep((int)247.2, 1100);
                            Console.ReadLine();
                        }
                        else
                        {
                            Console.WriteLine("You escaped the Courtyard.");
                            Console.ReadLine();
                        }
                        Console.WriteLine("You escape the courtyard, and left behind a pile of corpses.");
                    }

                }
            }
            else
            {
                Console.WriteLine("You leave the courtyard and instead go to the showers further down the corridor");
                /* showers(); */
                Console.ReadLine();
            }
            Console.ReadLine();
        }

        static void Died()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("▓██   ██▓ ▒█████   █    ██    ▓█████▄  ██▓▓█████ ▓█████▄ \r\n ▒██  ██▒▒██▒  ██▒ ██  ▓██▒   ▒██▀ ██▌▓██▒▓█   ▀ ▒██▀ ██▌\r\n  ▒██ ██░▒██░  ██▒▓██  ▒██░   ░██   █▌▒██▒▒███   ░██   █▌\r\n  ░ ▐██▓░▒██   ██░▓▓█  ░██░   ░▓█▄   ▌░██░▒▓█  ▄ ░▓█▄   ▌\r\n  ░ ██▒▓░░ ████▓▒░▒▒█████▓    ░▒████▓ ░██░░▒████▒░▒████▓ \r\n   ██▒▒▒ ░ ▒░▒░▒░ ░▒▓▒ ▒ ▒     ▒▒▓  ▒ ░▓  ░░ ▒░ ░ ▒▒▓  ▒ \r\n ▓██ ░▒░   ░ ▒ ▒░ ░░▒░ ░ ░     ░ ▒  ▒  ▒ ░ ░ ░  ░ ░ ▒  ▒ \r\n ▒ ▒ ░░  ░ ░ ░ ▒   ░░░ ░ ░     ░ ░  ░  ▒ ░   ░    ░ ░  ░ \r\n ░ ░         ░ ░     ░           ░     ░     ░  ░   ░    \r\n ░ ░                           ░                  ░     ");
            Console.WriteLine("                                                  PRESS ENTER TO GO BACK TO MAIN MENU");
            Console.ResetColor();
            Console.ReadLine();
            Console.Clear();
            Main();

        }
        static void showers()
        {
            Console.WriteLine("You sneak towards the showers, hoping to find a place to clean up and perhaps find some supplies.");
            Console.WriteLine("As you enter the showers, you hear the faint sound of running water.");
            Console.WriteLine("You turn a corner into one of the showers and see a bar of soap sitting on a bench.");
            Console.WriteLine("Would you like to pick up the bar of soap: (Y)es or (N)o?");
            string showier = Console.ReadLine().ToUpper();
            if (showier == "Y")
            {
                Console.WriteLine("You pick up the bar of soap....");
                Thread.Sleep(500);
                Console.WriteLine("     .-.\r\n    (o o) Boo!\r\n    | O |\r\n    |   | \r\n    '~~~'\r\n");
                Console.Beep(800, 200);
                Thread.Sleep(50);
                Console.Beep(900, 200);
                Thread.Sleep(50);
                Console.Beep(1000, 300);
                Console.WriteLine("You feel your heart racing as you just got a jumpscare from an old, bitten and dehydrated bar of soap \nYou silently laugh about it..(LOL) but (LOS) ");
                Console.WriteLine("As you Laughing Out Silently, you hear footsteps approaching from a distance \nYou turn around and see a zombie walking into the shower opposite yours");
                Console.WriteLine("You realise that this zombie is looking for the soap 0 0\n                                                      O");
                Console.WriteLine("In order to save yourself, you have to throw this bar of soap to the zombie");
                Console.WriteLine("PRESS ENTER TO CONTINUE");
                Console.ReadLine();
                soap();
                Console.WriteLine("The zombie then happily walks away with its' bar of soap and doesn't even notice you \nYou're SAFE!! ");
                Thread.Sleep(400);
                Console.WriteLine("Only for now \nYou then make your way back into the corridor and see some rooms ahead of you");
                corridor();

            }
            else if (showier == "N")
            {
                Console.WriteLine("Suddenly,  you hear footsteps approaching from a distance \nYou turn around and see a zombie walking into the shower room, your and slip on the wet floor.");
                Console.WriteLine("The zombie turns around and spots you");
                Console.WriteLine("Just before you know it, the zombie attacks you and you're dead");
                Died();
            }


            static void office(string food, string guardgun, string keycard)
            {
                Console.WriteLine("You open the door and quietly enter the office.");
                Console.WriteLine("The room is dark and you can clearly hear zombies mumbling.");
                Console.WriteLine("You turn on the lights and realize the room has some zombies.");
                Console.WriteLine("There is a door labelled 'Armory' in the corner, it has a keycard reader. If you have a keycard, you may be able to access it.");
                Console.WriteLine("The zombies will definitely notice you if you try to go for it though, if you have food, you could cause a distraction.");
                Console.WriteLine("Or if you have a weapon, you could fight the zombies");
                Console.WriteLine("Throw Food (t) or Leave Room (l)?");
                string usersinput = Console.ReadLine();
                if (usersinput == "t" || usersinput == "T")
                {
                    if (food == "yes")
                    {
                        Console.WriteLine("You throw the food into the corner of the room.");
                        Console.WriteLine("All the zombies are distracted and move to the corner of the room, you step up to the armory door.");
                        if (keycard == "yes")
                        {
                            Console.WriteLine("You grab a fully loaded rifle. and leave the office.");
                            string rifle = "yes";
                        }
                    }
                    else
                    {
                        Console.WriteLine("You do not have any food to throw.");
                        Console.WriteLine("You turn around and leave the office.");
                    }
                }
                else if (usersinput == "l" || usersinput == "L")
                {
                    Console.WriteLine("You turn around and leave the office");
                    Console.WriteLine("PRESS ENTER");
                    Console.ReadLine();
                }
                else
                {
                    Console.WriteLine("ERROR");
                }

            }



            Console.ReadLine();


            // }
            static void soap()
            {
                int screenWidth = 50;
                // Fixed screen width
                string[] soap =
                {
                                    "     .-.",
                                    "    (o o) HEHEHEHE",
                                    "    | O |",
                                    "    |   |",
                                    "    '~~~'"
                            };
                string[] zombie =
                {
                                    "      .-.",
                                    "     (o o)",
                                    "     | O |",
                                    "     |   |",
                                    "     '~~~'",
                                    "  .-''''''-.",
                                    " /  _.  ._  \\",
                                    " | (_)  (_)  |",
                                    " \\   __    //' ",
                                    "  '.-- '---_",
                                    " .||`-----'-||.",
                                    "/ ||        ||   \\"
                            };
                for (int x = 0; x < screenWidth - 10; x++)
                {
                    Console.Clear();

                    // Draw the soap
                    for (int i = 0; i < soap.Length; i++)
                    {
                        Console.SetCursorPosition(x, 5 + i);
                        // Fixed vertical position starting at row 5
                        Console.WriteLine(soap[i]);
                    }

                    // Draw the zombie at a different position
                    for (int i = 0; i < zombie.Length; i++)
                    {
                        Console.SetCursorPosition(screenWidth - x - 5, 5 + i);
                        // Another vertical position starting at row 5
                        Console.WriteLine(zombie[i]);
                    }

                    Thread.Sleep(100);
                }

                Console.SetCursorPosition(0, 10);
                Console.Clear();
            }


        }
        static void VisitorRoom()
        {
            Console.Clear();
            Console.WriteLine("You step into the visitor room, the dim lighting casts long shadows across the empty space.");
            Console.WriteLine("Your footsteps echo ominously as you move forward.");
            Console.WriteLine("All you see are rows of empty booths and your reflection in the dull bulletproof glass.\n");
            Console.WriteLine("PRESS ENTER TO CONTINUE");
            Console.ReadLine();
            Console.WriteLine("You scan the room, your eyes straining to make out any sign of movement or life, but the visitor room is completely deserted.");
            Console.WriteLine("Your heart pounds in your chest, the weight of your situation bearing down on you.");
            Console.WriteLine("You can't help but wonder where all the visitors have gone, and a chill runs down your spine at this thought.");
            Console.WriteLine("You carefully make your way towards the reception area.");
            Console.WriteLine("PRESS ENTER TO CONTINUE");
            Console.ReadLine();
            Console.WriteLine("The silence is deafening, broken only by the sound of your own breathing and the occasional creak of the old building settling around you.");
            Console.WriteLine("You can't afford to let your fear hold you back. The key to your freedom lies beyond these walls, and you know you have to keep moving, no matter the cost.");
            Console.WriteLine("As you reach the doorway leading to the reception area, you pause, your hand gripping the handle tightly. This is it, the moment of truth.");
            Console.WriteLine("With a steadfast resolve, you push the door open and step out into the unknown, ready to confront whatever challenges await you.");
            Console.WriteLine("PRESS ENTER TO CONTINUE");
            Console.ReadLine();
            Console.WriteLine("It's finally starting to look like you're close to an exit ");
            Console.WriteLine("You make you way into the reception");
            reception(true);
        }
        static void reception(bool calledFromVisitorRoom)
        {
            string temp;
            bool choice = true;
            bool receptionVisited = false;
            bool bodyVisited = false;
            bool chairVisited = false;
            if (calledFromVisitorRoom)
            {
                Console.WriteLine("You walk into the room, everything is dark, only lit up by a flickering light and the light coming from an adjacent door ");
                Console.WriteLine("Would you like to explore the room or go to the door?(type 'explore' or 'door')");
                temp = Console.ReadLine().ToLower();

                if (temp == "explore")
                {
                    Console.WriteLine("You decide to explore the room cautiously");
                    Console.WriteLine("The room is in shambles, lights a are flickering showing splatter of blood all over the floor and walls");
                    Thread.Sleep(2000);
                    Console.WriteLine("");
                    Console.WriteLine("The room is seems unintersting to you, just a normal looking reception room minus the blood you thought");
                    Console.WriteLine("You decided to go towards the open door");
                    Thread.Sleep(3000);
                    Console.WriteLine("As you reach the door you see a hoard of zombies lurking in the corridor");
                    Console.WriteLine("The amount there gives you immense fear, your heartbeat starts to increase");
                    Console.WriteLine("Your body freezes and you frantically think of your next move");
                    Console.WriteLine("You decided to close the door as quietly as possible");
                    Thread.Sleep(3000);
                    Console.WriteLine("You slowly begin to close the door but as you do a loud creak emits from the hinge");
                    Console.WriteLine("Iin an instance all the zombies look in your direction");
                    Console.WriteLine("You quickly slam the door shut");


                }
                else if (temp == "door")
                {
                    Console.WriteLine("You cautiously make your way to the door out of curiosity.");
                    Console.WriteLine("You decided to go towards the open door");
                    Thread.Sleep(3000);
                    Console.WriteLine("As you reach the door you see a hoard of zombies lurking in the corridor");
                    Console.WriteLine("The amount there gives you immense fear, your heartbeat starts to increase");
                    Console.WriteLine("Your body freezes and you frantically think of your next move");
                    Console.WriteLine("You decided to close the door as quietly as possible");
                    Thread.Sleep(3000);
                    Console.WriteLine("You slowly begin to close the door but as you do a loud creak emits from the hinge");
                    Console.WriteLine("In an instance all the zombies look in your direction");
                    Console.WriteLine("You quickly slam the door shut");

                    Console.WriteLine("PRESS ENTER TO CONTINUE");
                    Console.ReadLine();

                }
                else
                {
                    Console.WriteLine("Invalid choice: Please type 'explore' or 'door'.");

                    Console.WriteLine("You cautiously make you way to the door out of curiosity ");
                }

            }
            Console.WriteLine("You quickly take a moment regain your composure");

            Console.WriteLine("You take a look at your surroundings");
            Thread.Sleep(1500);
            Console.WriteLine("The room is in shambles, lights a are flickering showing splatter of blood all over the floor and walls");
            Console.WriteLine("Using the flickering light you see a a receptionist desk, a dead body and a ominous looking chair \n\nwhat would you like to do?");

            while (!(receptionVisited && bodyVisited && chairVisited))
            {



                temp = Console.ReadLine().ToLower();
                switch (temp)
                {
                    case "receptionist desk":
                        Console.WriteLine("You cautiously make your way to the desk");
                        Console.WriteLine("The desk looks very messy but something catches your eyes");
                        Console.WriteLine("A small note with the words 'Note 1', you see the numbers '112'");
                        Thread.Sleep(1500);
                        Console.WriteLine("You wonder what this means");
                        Thread.Sleep(1500);
                        Console.WriteLine("Where next?");
                        receptionVisited = true; if (!(receptionVisited && bodyVisited && chairVisited))
                        {
                            Console.WriteLine("Where next?");
                        }
                        break;
                    case "dead body":
                        Console.WriteLine("You make you way cautiously to the dead body");
                        Console.WriteLine("You see that it is the receptionist");
                        Thread.Sleep(1500);
                        Console.WriteLine("You check if they are alive but they don't seem to be");
                        Console.WriteLine("You check around in their pockets to see if they had anything useful, you find a wallet");
                        Thread.Sleep(1500);
                        Console.WriteLine("You open the wallet to find an ID with the name 'Rick A', the rest of the name is covered up with dried blood");
                        Console.WriteLine("Rummaging through the wallet more you find another small note that reads 'Note 2' it has the numbers '92'");
                        Thread.Sleep(1500);
                        Console.WriteLine("You wonder what this means");
                        Thread.Sleep(1500);
                        Console.WriteLine("Where next?");
                        bodyVisited = true;
                        if (!(receptionVisited && bodyVisited && chairVisited))
                        {
                            Console.WriteLine("Where next?");
                        }
                        break;
                    case "ominous chair":
                        Console.WriteLine("You make you way to the ominous chair");
                        Console.WriteLine("You don't know why you feel so uneasy, you prepare for the worst");
                        Thread.Sleep(1500);
                        Console.WriteLine("This feeling lingers till you make it to the chair");
                        Console.WriteLine("You wonder what makes this chair so ominous");
                        Thread.Sleep(1500);
                        Console.WriteLine("You see a small note with the words 'Note 3' written on it, the note has the numbers '25' ");
                        Thread.Sleep(1500);
                        Console.WriteLine("You wonder what this means");
                        Thread.Sleep(1500);
                        Console.WriteLine("Where next?");
                        chairVisited = true;
                        if (!(receptionVisited && bodyVisited && chairVisited))
                        {
                            Console.WriteLine("Where next?");
                        }
                        break;
                    default:
                        Console.WriteLine("Invalid input: Please enter 'receptionist desk', 'dead body' or 'ominous chair'.");
                        break;
                }

            }
            Thread.Sleep(3000);
            Console.WriteLine("You take a moment to look at all 3 notes 1129225");
            Console.WriteLine("What could these numbers mean?");
            Console.WriteLine("PRESS ENTER TO CONTINUE");
            Console.ReadLine();
            Console.WriteLine("Your short moment is interupted by a a loud bang and crash at the door");
            Console.WriteLine("You see that the door is starting to slowly hinge open due to the share amount of zombies pushing at it");
            Console.WriteLine("You start to panic and look for your next escape");
            Thread.Sleep(1500);
            Console.WriteLine("You scan the room in a rush to find and exit");
            Console.WriteLine("You being to panic as things start to set in");
            Console.WriteLine("The room is so dark that anything that is not lit up by the flickering light is not visable");
            Thread.Sleep(1500);
            Console.WriteLine("The banging continues but it gradually gets louder");
            Console.WriteLine("You panic more until you see a sign that is barely lit up by the light with the words exit");
            Console.WriteLine("Run for exit?\n Yes 'y' or NO 'n'");
            char Word = char.Parse(Console.ReadLine());
            if (Word == 'y' || Word == 'Y')
            {
                Console.WriteLine("You run towards the exit door and crash through");
                //exit();
            }


            // Console.WriteLine("You burst through the door and slam it shut");
            Console.WriteLine("You feel the zombies crash onto the door with tremendous force");
            Thread.Sleep(1500);
            Console.WriteLine("");
            Console.WriteLine("You look for something to wedge against the door");
            Console.WriteLine("On your right you see a bookshelf and on your left you see a chair \nWhat do you choose?");

            while (choice)
            {
                temp = Console.ReadLine().ToLower();
                switch (temp)
                {
                    case "chair":
                        Console.WriteLine("You grab the chair and wedge it under the door knob");
                        Thread.Sleep(1500);
                        Console.WriteLine("The banging continues but it seems that the door is holding");
                        Thread.Sleep(2000);
                        Console.WriteLine("You breath a sigh of relief");
                        choice = false;
                        Console.ReadLine();
                        break;
                    case "bookshelf":
                        Console.WriteLine("You grab ahold of the side of the bookshelf and with all your might pull it down against the door");
                        Console.WriteLine("The banging continues but it seems that the door is holding");
                        Console.ReadLine();
                        Console.WriteLine("You breath a sigh of relief");
                        choice = false;
                        Console.ReadLine();
                        break;
                    default:
                        Console.WriteLine("Invalid input: Please enter 'chair' or 'bookshelf'");
                        break;

                }

            }
            Console.WriteLine("You quickly take a moment regain your composure");

            Console.WriteLine("You take a look at your surroundings");
            Thread.Sleep(1500);
            Console.WriteLine("The room is in shambles, lights a are flickering showing splatter of blood all over the floor and walls");
            Console.WriteLine("Using the flickering light you see a a receptionist desk, a dead body and a ominous looking chair \n\nwhat would you like to do?");

            while (!(receptionVisited && bodyVisited && chairVisited))
            {
                temp = Console.ReadLine().ToLower();
                switch (temp)
                {
                    case "receptionist desk":
                        Console.WriteLine("You cautiously make your way to the desk");
                        Console.WriteLine("The desk looks very messy but something catches your eyes");
                        Console.WriteLine("A small note with the words 'Note 1', you see the numbers '112'");
                        Thread.Sleep(1500);
                        Console.WriteLine("You wonder what this means");
                        Thread.Sleep(1500);
                        Console.WriteLine("Where next?");
                        receptionVisited = true; if (!(receptionVisited && bodyVisited && chairVisited))
                        {
                            Console.WriteLine("Where next?");
                        }
                        break;
                    case "dead body":
                        Console.WriteLine("You make you way cautiously to the dead body");
                        Console.WriteLine("You see that it is the receptionist");
                        Thread.Sleep(1500);
                        Console.WriteLine("You check if they are alive but they don't seem to be");
                        Console.WriteLine("You check around in their pockets to see if they had anything useful, you find a wallet");
                        Thread.Sleep(1500);
                        Console.WriteLine("You open the wallet to find an ID with the name 'Rick A', the rest of the name is covered up with dried blood");
                        Console.WriteLine("Rummaging through the wallet more you find another small note that reads 'Note 2' it has the numbers '92'");
                        Thread.Sleep(1500);
                        Console.WriteLine("You wonder what this means");
                        Thread.Sleep(1500);
                        Console.WriteLine("Where next?");
                        bodyVisited = true;
                        if (!(receptionVisited && bodyVisited && chairVisited))
                        {
                            Console.WriteLine("Where next?");
                        }
                        break;
                    case "ominous chair":
                        Console.WriteLine("You make you way to the ominous chair");
                        Console.WriteLine("You don't know why you feel so uneasy, you prepare for the worst");
                        Thread.Sleep(1500);
                        Console.WriteLine("This feeling lingers till you make it to the chair");
                        Console.WriteLine("You wonder what makes this chair so ominous");
                        Thread.Sleep(1500);
                        Console.WriteLine("You see a small note with the words 'Note 3' written on it, the note has the numbers '25' ");
                        Thread.Sleep(1500);
                        Console.WriteLine("You wonder what this means");
                        Thread.Sleep(1500);
                        Console.WriteLine("Where next?");
                        chairVisited = true;
                        if (!(receptionVisited && bodyVisited && chairVisited))
                        {
                            Console.WriteLine("Where next?");
                        }
                        break;
                    default:
                        Console.WriteLine("Invalid input: Please enter 'receptionist desk', 'dead body' or 'ominous chair'.");
                        break;
                }

            }
            Thread.Sleep(3000);
            Console.WriteLine("You take a moment to look at all 3 notes 1129225");
            Console.WriteLine("What could these numbers mean?");
            Console.WriteLine("PRESS ENTER TO CONTINUE");
            Console.ReadLine();
            Console.WriteLine("Your short moment is interupted by a a loud bang and crash at the door");
            Console.WriteLine("You see that the door is starting to slowly hinge open due to the share amount of zombies pushing at it");
            Console.WriteLine("You start to panic and look for your next escape");
            Thread.Sleep(1500);
            Console.WriteLine("You scan the room in a rush to find and exit");
            Console.WriteLine("You being to panic as things start to set in");
            Console.WriteLine("The room is so dark that anything that is not lit up by the flickering light is not visable");
            Thread.Sleep(1500);
            Console.WriteLine("The banging continues but it gradually gets louder");
            Console.WriteLine("You panic more until you see a sign that is barely lit up by the light with the words exit");
            Console.WriteLine("Run for exit?\n Yes 'y' or NO 'n'");
            char answer = char.Parse(Console.ReadLine());
            if (answer == 'y' || answer == 'Y')
            {
                Console.WriteLine("You run towards the exit door and crash through");
                //exit();
            }
            else
            {
                {
                    Console.WriteLine("You decided to stay and accept your fate");
                    Console.WriteLine("The zombies come crashing thorugh the door and pile on you");
                    Console.WriteLine("Life begins to fade as you become one of them");
                    Console.ReadLine();
                    Died();
                }
            }

        }

    }
}







