using LibrarySystemDB.AsciiArt;
using LibrarySystemDB.Models;
using LibrarySystemDB.Repositories;
using Microsoft.Identity.Client;
using System.Data;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using static System.Reflection.Metadata.BlobBuilder;

namespace LibrarySystemDB
{

    public class Program
    {
        static void Main(string[] args)
        { //--------------------------------------SET UP------------------------------

            ApplicationDBContext applicationDbContext = new ApplicationDBContext();
            BooksRepo book = new BooksRepo(applicationDbContext);
            BorrowsRepo borrow = new BorrowsRepo(applicationDbContext);
            CategoriesRepo category = new CategoriesRepo(applicationDbContext);
            LibrarianRepo librarian = new LibrarianRepo(applicationDbContext);
            ReadersRepo reader = new ReadersRepo(applicationDbContext);


            Artworks art = new Artworks();

            //----------------------------------------------------------------------------

            Console.Clear();
            Console.WriteLine("\n\n- - - - - - - - - - - - - - - - - -W E L C O M E   T O   T H E   L I B R A R Y- - - - - - - - - - - - - - - - - - -\n\n");
            art.PrintCastle();
            Console.Write("\n\t\t\t\t\t     Press enter to continue...");
            Console.ReadKey();

            //LeaderBoard();
           // Console.Write("\t\t\t\t\t  Press enter to continue...");
            //Console.ReadKey();

            bool Authentication = false;
            do
            {
                Console.Clear();
                PrintTitle();
                Console.Write("\n\n\n\n\t\t\t\t\t\t   MAIN MENU:\n\n\n");
                Console.WriteLine("\t\t\t\t\t\t  1.  Reader Login\n");
                Console.WriteLine("\t\t\t\t\t\t  2.  Librarian Login\n");
                Console.WriteLine("\t\t\t\t\t\t  3.  Register\n");
                Console.WriteLine("\t\t\t\t\t\t  4.  Exit\n\n\n");
                int Option = 0;
                Console.Write("\t\t\t\t\t\t   Enter: ");


                try
                {
                    Option = int.Parse(Console.ReadLine());
                }
                catch (Exception ex) { Console.WriteLine(ex.Message); Console.WriteLine("\n Press enter to continue :("); Console.ReadKey(); }

                switch (Option)
                {
                    case 1:
                        Console.Clear();
                        PrintTitle();
                        Console.Write("\n\n\n\n\n\t\t\t\t\t\t   READER LOGIN:\n\n");
                        Console.Write("\t\t\t\t\tUsername: ");
                        string Usr = Console.ReadLine();
                        Console.Write("\t\t\t\t\tPassword: ");
                        string Pswd = Console.ReadLine();

                        //ADD USER AUTH IN READERS REPO AND THEN ACCESS FROM HERE 
                        int UsrAuth = reader.UserAuthentication(Usr, Pswd);

                        if (UsrAuth == 1)
                        {
                            Console.Clear();

                            UserPage(applicationDbContext, Usr, reader);

                        }

                        else if (UsrAuth == 2)
                        {
                            Console.WriteLine("\n\t\t\t\t\t<!>Incorrect password please try again :( <!>");
                            Console.WriteLine("\t\t\t\t\t<!>Press enter to try again<!>");
                            Console.ReadKey();
                        }

                        else
                        {
                            Console.WriteLine("<!>Login details were not found :(<!>");
                            Console.WriteLine("\n\t\t\t\t\t<!>This username is not in our system<!> \n\n\t\t\t\tWould you like to register? Yes to register, enter to exit");
                            Console.Write("\t\t\t\t\tEnter: ");
                            string NewRegistration = (Console.ReadLine()).ToLower();

                            if (NewRegistration == "yes")
                            {
                                Register();
                            }

                        }
                        break;

                    case 2:
                        //librarian side 
                        break;

                    case 3:
                       //new user 
                        break;

                    case 4:
                        Console.Clear();
                        Console.WriteLine("\n\n- - - - - - - - - - - - - - - - - -L E A V I N G   T H E   L I B R A R Y- - - - - - - - - - - - - - - - - - -\n\n");
                        art.PrintAlien();
                        Console.WriteLine("\t\t\t\tPress enter to continue...");
                        Console.ReadKey();
                        Authentication = true;
                        break;

                    default:
                        Console.WriteLine("\n\t\t\t\t\t<!>Invalid input :( <!> \n\t\t\t<!>Please try again, enter one of the given options<!>");
                        Console.WriteLine("\n\t\t\t\t\t<!>Press enter to continue<!>"); Console.ReadKey();
                        break;
                }
            } while (Authentication != true);
        



         }

        static void PrintTitle()
        { Console.WriteLine("\n\n- - - - - - - - - - - - - - - - - - - - - - - -C I T Y   L I B R A R Y- - - - - - - - - - - - - - - - - - - - - - - - -\n\n"); }


        //--------------------------------SHARED FUNCTIONS----------------------------- 
        //METHOD TO EXIT THE LIBRARY  
        static bool LeaveLibrary(bool ExitFlag)
        {
            Console.WriteLine("\n\nAre you sure you want to leave? \n\nYes to leave anything else to stay.");
            Console.Write("Enter: ");
            string Leave = (Console.ReadLine()).ToLower();

            if (Leave != "yes")
            {
                return false;
            }
            else
            {
                ExitFlag = true;
                Console.Clear();
                Console.WriteLine("\n\n- - - - - - - - - - - - - - - - - - - - - - - -C I T Y   L I B R A R Y- - - - - - - - - - - - - - - - - - - - - - - - -\n\n");
                Console.WriteLine("\n\nThank you for visiting the library :) \nCome again soon!\n\n");
                return true;
            }
        }



        //--------------------------------USER PAGE----------------------------- 
        static void UserPage(ApplicationDBContext applicationDbContext, string usr, ReadersRepo readerRepo)
        {
            Artworks art = new Artworks();
            BorrowsRepo borrow = new BorrowsRepo(applicationDbContext);


            var currentReader = readerRepo.GetReaderByUserName(usr);
            var OverdueBooks = borrow.OverdueFinder(currentReader);

            //Overdue books 
            if (OverdueBooks != null) 
            {
                Console.Clear();
                PrintTitle();
                Console.WriteLine("! ! ! ! ! ! ! ! ! ! ! ! ! ! ! ! ! ! ! ! ! ! ! ! ACCOUNT SUSPENDED :( ! ! ! ! ! ! ! ! ! ! ! ! ! ! ! ! ! ! ! ! ! ! ! !");
                art.PrintSpaceShip();

                Console.WriteLine("Please return the following overdue books to unlock your account\n");
                for (int j = 0; j < OverdueBooks.Count; j++)
                {
                    Console.WriteLine($"Book ID: {OverdueBooks[j].BBID} \nReturn Date: {OverdueBooks[j].PredictedReturn} \nCurrentDate: {DateOnly.FromDateTime(DateTime.Now)}\n\n");
                }

                bool ReturnLoop = true;
                do
                {
                    Console.WriteLine("\n\n\n\n\t\t\t\t\t\t   READER OPTIONS:\n");
                    Console.WriteLine("\t\t\t\t\t1.  Return A Book\n");
                    Console.WriteLine("\t\t\t\t\t2.  Logout\n\n");
                    Console.Write("\t\t\t\t\tEnter: ");
                    int choice = 0;

                    try
                    {
                        choice = int.Parse(Console.ReadLine());
                    }
                    catch (Exception ex) { Console.WriteLine(ex.Message); }

                    switch (choice)
                    {
                        case 1:
                            Console.Clear();
                            ReturnBook();
                            break;

                        case 2:
                            Console.WriteLine("\n\n- - - - - - - - - - - - - - - - - - - - - -L O G I N G   O U T- - - - - - - - - - - - - - - - - - - - - - -\n\n");
                            art.PrintMonkey();
                            Console.Write("\t\t\t\t\tPress enter to continue...");
                            Console.ReadKey();
                            ReturnLoop = false;
                            break;

                        default:
                            Console.WriteLine("\n\t\t\t\t\t<!>Invalid choice :( <!>");
                            break;

                    }
                } while (ReturnLoop == true);
            }
        

            //No overdue books 
            else 
            {
                bool ExitFlag = false;
                do
                {
                    Console.Clear();
                    Console.WriteLine("\n\n- - - - - - - - - - - - - - - - - - - - - - - -C I T Y   L I B R A R Y- - - - - - - - - - - - - - - - - - - - - - - - -\n\n");
                    Console.Write("\n\n\n\n\t\t\t\t\t\tREADER OPTIONS:\n\n\n");
                    Console.WriteLine("\t\t\t\t\t1. View All Books\n");
                    Console.WriteLine("\t\t\t\t\t2. Search by Book Name or Author\n");
                    Console.WriteLine("\t\t\t\t\t3. View Profile\n");
                    Console.WriteLine("\t\t\t\t\t4. Borrow A Book\n");
                    Console.WriteLine("\t\t\t\t\t5. Return A Book\n");
                    //Console.WriteLine("\t\t\t\t\t6. View Leader Board\n");
                    Console.WriteLine("\t\t\t\t\t7. Log out\n\n\n");
                    Console.Write("\t\t\t\t\tEnter: ");
                    int choice = 0;

                    try
                    {
                        choice = int.Parse(Console.ReadLine());
                    }
                    catch (Exception ex) { Console.WriteLine(ex.Message); }

                    switch (choice)
                    {
                        case 1:
                            Console.Clear();
                            PrintTitle();
                            ViewBook(applicationDbContext);
                            break;

                        case 2:
                            Console.Clear();
                            UserSearchForBook();
                            break;

                        case 3:
                            Console.Clear();
                            ViewUsrProfile();
                            break;

                        case 4:
                            Console.Clear();
                            BorrowBook();
                            break;

                        case 5:
                            Console.Clear();
                            ReturnBook();
                            break;

                        //case 6:
                        //    LeaderBoard();
                        //    Console.Write("Press enter to continue...");
                        //    Console.ReadKey();
                        //    break;

                        case 7:
                            Console.Clear();
                            bool Response = LeaveLibrary(ExitFlag);
                            if (Response == true)
                            {
                                ExitFlag = true;
                            }
                            break;

                        default:
                            Console.WriteLine("\n\t\t\t\t\t<!>Sorry your choice was wrong<!>");
                            break;

                    }
                    Console.Clear();
                    Console.WriteLine("\n\n- - - - - - - - - - - - - - - - - - - - - -L O G I N G   O U T- - - - - - - - - - - - - - - - - - - - - - -\n\n");
                    art.PrintMonkey();
                    Console.WriteLine("\t\t\t\t\tPress enter to continue...");
                    Console.ReadKey();
                    string cont = Console.ReadLine();
                    Console.Clear();

                } while (ExitFlag != true);
            }
        }


        //ALL BOOKS
        static void ViewBook(ApplicationDBContext applicationDbContext)
        {
            BooksRepo book = new BooksRepo(applicationDbContext);
            var AllBooks = book.GetAll();
            if (AllBooks.Count != 0)
            {
                Console.Write("\n\n\n\n\t\t\t\t\t\t   AVAILABLE BOOKS:\n\n");

                var BooksTable = new DataTable("Books");
                BooksTable.Columns.Add("ID", typeof(int));
                BooksTable.Columns.Add("NAME", typeof(string));
                BooksTable.Columns.Add("AUTHOR FNAME", typeof(string));
                BooksTable.Columns.Add("AUTHOR LNAME", typeof(string));
                BooksTable.Columns.Add("CATEGORY", typeof(string));
                BooksTable.Columns.Add("AVAILABLE QTY", typeof(int));

                for (int i = 0; i < AllBooks.Count; i++)
                {
                    BooksTable.Rows.Add(AllBooks[i].BookID, AllBooks[i].Title.Trim(), AllBooks[i].AuthFName.Trim(), AllBooks[i].AuthLName.Trim(), AllBooks[i].Categories.CatName, AllBooks[i].BorrowPeriod);
                }

                foreach (DataColumn column in BooksTable.Columns)
                {
                    Console.Write($"{column.ColumnName,-25}");
                }
                Console.WriteLine();


                foreach (DataRow row in BooksTable.Rows)
                {
                    foreach (var item in row.ItemArray)
                    {
                        Console.Write($"{item,-25}");
                    }
                    Console.WriteLine();
                }
                Console.WriteLine();

                Console.WriteLine("Would you like to borrow a book? \n\nYes to continue enter to leave.");
                Console.Write("Enter: ");
                string BorrowNow = Console.ReadLine().ToLower();
                if (BorrowNow != "yes")
                {
                    Console.WriteLine("Exiting...");
                }

                else;
                {
                    BorrowBook();
                }
            }
            else { Console.WriteLine("Sorry it looks like we don't have any books available :( \nPlease come again another time.\n"); }

        }


        //ALLOWS USER TO SEARCH FOR BOOK WITH OUTPUTS SUITED FOR USER 
        static void UserSearchForBook(ApplicationDBContext applicationDbContext)
        {
            BooksRepo book = new BooksRepo(applicationDbContext);
            var allbooks = book.GetAll();
            Console.Clear();
            PrintTitle();
            Console.Write("\n\n\n\n\t\t\t\t\t\t   SEARCH LIBRARY:\n\n");
            Console.Write("\t\t\t\t\tBook name or author: ");
            string name = (Console.ReadLine().Trim()).ToLower();
            string SearchPattern = Regex.Escape(name);
            Regex regex = new Regex(SearchPattern, RegexOptions.IgnoreCase);

            bool flag = false;

            for (int i = 0; i < allbooks.Count; i++)
            {

                if (regex.IsMatch(allbooks[i].Title) || regex.IsMatch(allbooks[i].AuthLName) || regex.IsMatch(allbooks[i].AuthFName))
                {
                    Console.WriteLine($"\nBook Title: {allbooks[i].Title} \nBook Author: {allbooks[i].AuthFName} {allbooks[i].AuthLName} \nID: {allbooks[i].BookID} \nCategory: {allbooks[i].Categories.CatName} \nPrice: {allbooks[i].Price} \nBorrow Period: {allbooks[i].BorrowPeriod} days\n");
                    flag = true;
                }

            }

            if (flag != true)
            { Console.WriteLine("\t\t\t\t\t<!>Book not found :( <!>"); }

            Console.Write("\n\t\t\t\t\tPress enter to continue");
            Console.ReadKey();
        }

        //BORROW BOOK-
        static void BorrowBook()
        {
            bool CanBorrow = true;
            if (Books.Count != 0)
            {
                ViewAllBooks();

                Console.Write("\n\n\n\n\t\t\t\t\t\t   BORROWING A BOOK:\n\n");
                Console.Write("Enter ID: ");
                int BorrowID = 0;

                try
                {
                    BorrowID = int.Parse(Console.ReadLine());
                }
                catch (Exception ex) { Console.WriteLine(ex.Message + "\n"); }

                for (int i = 0; i < Borrowing.Count; i++)
                {
                    if (Borrowing[i].UserID == CurrentUser && Borrowing[i].BookID == BorrowID && Borrowing[i].IsReturned == false) //checks if user has this book borrowed currently 
                    {
                        CanBorrow = false;
                        Console.WriteLine("Looks like you already borrowed this book, sorry you can't borrow more than one copy :(");
                        Console.WriteLine("Press enter...");
                        Console.ReadKey();
                    }
                }

                if (CanBorrow == true)
                {
                    int Location = -1;

                    for (int i = 0; i < Books.Count; i++)
                    {
                        if (Books[i].BookID == BorrowID)
                        {
                            Location = i;
                            break;
                        }
                    }

                    if (Location != -1) //Book found
                    {
                        Console.WriteLine($"Request to borrow: {Books[Location].BookName}");
                        if (Books[Location].BookQuantity > 0)
                        {
                            Console.WriteLine("We've got this in stock!\n");
                            Console.Write("Would you like to proceed? Yes or No: ");
                            string Checkout = Console.ReadLine().ToLower();

                            if (Checkout != "no")
                            {

                                DateTime Now = DateTime.Now;

                                //Decreasing book quantity 
                                int NewQuantity = (Books[Location].BookQuantity - 1);
                                int NewBorrowed = (Books[Location].Borrowed + 1);
                                Books[Location] = ((Books[Location].BookID, Books[Location].BookName, Books[Location].BookAuthor, Quantity: NewQuantity, Borrowed: NewBorrowed, Books[Location].Price, Books[Location].Category, Books[Location].BorrowPeriod));

                                //Appending data to borrow tuple list
                                //System.TimeSpan timeSpan = new System.TimeSpan(Books[Location].BorrowPeriod);
                                DateTime Return = Now.AddDays(Books[Location].BorrowPeriod);

                                //DEFUALT: actual return is the return by date | rating -1 | isReturned false
                                Borrowing.Add((UserID: CurrentUser, BorrowID: Books[Location].BookID, BorrowedOn: Now, ReturnBy: Return, ActualReturn: Return, Rating: -1, IsReturned: false));

                                SaveBorrowInfo();
                                SaveBooksToFile();

                                Invoices.Add((CurrentUser, DateTime.Now, Books[Location].BookID, Books[Location].BookName, Books[Location].BookAuthor, 1));
                                SaveInvoice();

                                //Finding book category to print related ASCII art
                                string CurrentCategory = Books[Location].Category.Trim();


                                //Printing recipt 
                                Console.Clear();
                                Console.WriteLine("\n\n- - - - - - - - - - - - - - - - - - - - - - - -C I T Y   L I B R A R Y- - - - - - - - - - - - - - - - - - - - - - - - -\n\n");
                                Console.WriteLine("* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *  \n\n");
                                Console.WriteLine("\t\t\t\t\t" + Now);
                                Console.WriteLine("\n\n* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * \n\n");

                                //Getting related ASCII art
                                switch (CurrentCategory)
                                {
                                    case "Children":
                                        PrintBear();
                                        break;

                                    case "Cooking":
                                        PrintPie();
                                        break;

                                    case "History":
                                        PrintScroll();
                                        break;

                                    case "IT":
                                        PrintComputer();
                                        break;

                                    case "Non-Fiction":
                                        PrintPerson();
                                        break;

                                    case "Science":
                                        PrintBooks();
                                        break;

                                    case "Self Help":
                                        PrintMoon();
                                        break;

                                    case "Software":
                                        PrintWindowsLogo();
                                        break;

                                    case "Stories":
                                        PrintSherlock();
                                        break;

                                    case "Young Adult":
                                        PrintPacMan();
                                        break;

                                }

                                Console.WriteLine($"BOOK: ID - {Books[Location].BookID} \nNAME - {Books[Location].BookName} \nAUTHOR - {Books[Location].BookAuthor} \nRETURN BY - {Return}");
                                Console.WriteLine("\n\n\n* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * \n\n\n");
                                Console.WriteLine("\n\t\t\tThank you for visiting the library come again soon!");
                                Console.WriteLine("\t\t\tHappy Reading :)");
                                Console.WriteLine("\n\n* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * \n ");
                                Console.WriteLine("Press enter to continue");
                                Console.ReadKey();

                                Reccomend(Books[Location].BookAuthor);

                                Console.WriteLine("\n\nWould you like to borrow another book? Yes or No.");
                                Console.Write("Enter: ");
                                string Response = Console.ReadLine().ToLower();

                                if (Response != "no") //Will repeat borrowing process
                                {
                                    BorrowBook();
                                }

                            }
                        }
                        else
                        {
                            Console.WriteLine("Sorry this book is out of stock :(");
                            Console.WriteLine("We might have something else that you might like! \n\nWould you like to see what we have in stock? Yes or No");
                            string ViewOtherBooks = Console.ReadLine().ToLower();

                            if (ViewOtherBooks != "no")
                            {
                                ViewAllBooks();
                            }
                        }
                    }

                    else
                    {
                        Console.WriteLine("Sorry we don't have this book :(");
                        Console.WriteLine("We might have something else that you might like! \n\nWould you like to see what we have in stock? \nYes to continue anything else to leave.");
                        string ViewOtherBooks = Console.ReadLine().ToLower();

                        if (ViewOtherBooks != "yes")
                        {
                            Console.WriteLine("Exiting...");
                        }
                        else
                        { ViewAllBooks(); }
                    }
                }
            }
            else { Console.WriteLine("Sorry it looks like we don't have any books available right now :( \n"); }
        }


        //RETURN BOOK-
        static void ReturnBook()
        {
            List<int> BorrowedBookIDs = new List<int>();
            double CountDown;
            bool Found = false;
            int ReturnBook = 0;
            Console.Clear();
            Console.WriteLine("\n\n- - - - - - - - - - - - - - - - - - - - - - - -C I T Y   L I B R A R Y- - - - - - - - - - - - - - - - - - - - - - - - -\n\n");
            Console.Write("\n\n\n\n\t\t\t\t\t\t   RETURN BOOK:\n\n");

            Console.WriteLine("BORROWED BOOKS: ");
            for (int i = 0; i < Borrowing.Count; i++)
            {
                if (Borrowing[i].UserID == CurrentUser && Borrowing[i].IsReturned != true)
                {
                    CountDown = (Borrowing[i].ReturnBy - DateTime.Now).TotalDays;
                    CountDown = Math.Round(CountDown, 0);
                    Console.WriteLine($"Book ID: {Borrowing[i].BookID} \nReturn Date: {Borrowing[i].ReturnBy} \nDays remaining: {CountDown}\n");
                    BorrowedBookIDs.Add(Borrowing[i].BookID);

                }
            }


            Console.Write("Enter Book ID: ");

            try
            {
                ReturnBook = int.Parse(Console.ReadLine());
            }
            catch (Exception ex) { Console.WriteLine(ex.Message); }


            if (BorrowedBookIDs.Contains(ReturnBook)) //Checking if the user has borrowed the book they are trying to return
            {
                for (int i = 0; i < Books.Count; i++)
                {
                    if (Books[i].BookID == ReturnBook)
                    {
                        //Checking if this book has been borrowed -> handels case of books being returned without being borrowed (ie new books added)
                        if (Books[i].Borrowed > 0)
                        {
                            DateTime Now = DateTime.Now;
                            int NewBorrowCount = (Books[i].Borrowed - 1);
                            int NewBookQuantity = (Books[i].BookQuantity + 1);
                            Books[i] = ((Books[i].BookID, Books[i].BookName, Books[i].BookAuthor, Quantity: NewBookQuantity, Borrowed: NewBorrowCount, Books[i].Price, Books[i].Category, Books[i].BorrowPeriod));

                            // Borrowing.Add((UserID: CurrentUser, BorrowID: NewBID, BorrowedOn:Now, ReturnBy: Return, ActualReturn: Return, Rating:  -1, IsReturned: false));
                            Console.WriteLine($"Please rate {Books[i].BookName} out of 5");
                            Console.Write("Rating: ");

                            float UserRate;
                            while (!float.TryParse(Console.ReadLine(), out UserRate) || UserRate < 0)
                            {
                                Console.WriteLine("Invalid input please enter a number greater than 0.");
                                while (UserRate < 0 || UserRate > 6)
                                {
                                    Console.WriteLine("Invalid input please enter a number between 0 and 5.");
                                    Console.Write("Rating: ");
                                    UserRate = float.Parse(Console.ReadLine());
                                }
                            }

                            int Location = -1;
                            for (int j = 0; j < Borrowing.Count; j++)
                            {
                                if (Borrowing[j].BookID == ReturnBook)
                                {
                                    Location = j;
                                    break;
                                }
                            }

                            Borrowing[Location] = ((Borrowing[Location].UserID, Borrowing[Location].BookID, Borrowing[Location].BorrowedOn, Borrowing[Location].ReturnBy, ActualRetrun: Now, Rating: UserRate, IsReturned: true));

                            Console.WriteLine($"Thank you for returning {Books[i].BookName} :) \nPress enter to print your recipt");
                            Console.ReadKey();
                            SaveBooksToFile();
                            SaveBorrowInfo();
                            Console.Clear();
                            ReturnRecipt(i);
                            Found = true;
                        }
                        else
                        {
                            Console.WriteLine("This book has not been borrowed. \nPress enter to continue.");
                            Console.ReadKey();
                            Found = true;

                        }
                        break;
                    }
                    Found = true;


                }
                if (Found != true) { Console.WriteLine("Invalid Book ID :("); }

                Console.WriteLine("Press enter to continue...");
                Console.ReadKey();

            }
            else { Console.WriteLine("You have not taken out this book :) \nPlease check your recipt for book ID"); }
        }


        //PRINTS USER DETAILS-
        static void ViewUsrProfile()
        {
            List<int> SearchIDs = new List<int>();
            List<int> BookID = new List<int>();
            List<int> BorrowedBookIDs = new List<int>();
            List<int> ReaderIDs = new List<int>();

            double CountDown;
            DateTime Now = DateTime.Now;

            for (int i = 0; i < Users.Count; i++)
            {
                SearchIDs.Add(Users[i].UserID);
            }

            for (int i = 0; i < Books.Count; i++)
            {
                BookID.Add(Books[i].BookID);
            }

            int CurrentIndex = SearchIDs.IndexOf(CurrentUser);

            //Getting user reading ranking across all readers 

            int UserCounter = 0;
            int Counter = 0;
            int CurrentCount = 0;
            int HighScore = 0;
            int UserRead = 0;

            for (int i = 0; i < Borrowing.Count; i++)
            {
                if (Borrowing[i].IsReturned) //We only want values of books completed by reader (so borrowed and returned as well)
                {
                    ReaderIDs.Add(Borrowing[i].UserID);
                }
            }
            int UserRank = ReaderIDs.Count;

            for (int i = 0; i < ReaderIDs.Count; i++)
            {
                Counter = 0;
                for (int j = 0; j < ReaderIDs.Count; j++)
                {
                    if (ReaderIDs[i] == ReaderIDs[j])
                    {
                        Counter++;
                    }
                }

                if (ReaderIDs[i] != CurrentUser) //Ensures that user is not being compared to themself
                {
                    if (CurrentCount < UserCounter) //Moves users rank down one as if the counter is higher than they have a higher rank 
                    {
                        UserRank--;
                    }
                }
            }


            for (int i = 0; i < ReaderIDs.Count; i++)
            {
                if (ReaderIDs[i] == CurrentUser)
                {
                    UserRead++;
                }
            }


            Console.Clear();
            Console.WriteLine("\n\n- - - - - - - - - - - - - - - - - - - - - - - -C I T Y   L I B R A R Y- - - - - - - - - - - - - - - - - - - - - - - - -\n\n");
            Console.WriteLine("^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^\n");
            PrintOwl();
            Console.WriteLine($"\n\t\t\t\t\t\t {Users[CurrentIndex].UserUserName}'s Home Page :) \n ");
            Console.WriteLine($"MY DETAILS: \nUser ID: {Users[CurrentIndex].UserID} \nUser Name: {Users[CurrentIndex].UserUserName} \nEmail: {Users[CurrentIndex].UserEmail} \nUser Ranking: #{UserRank}, Books Read: {UserRead}\n");
            Console.WriteLine("^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^\n");
            Console.WriteLine($"CURRENTLY BORROWED:");
            for (int i = 0; i < Borrowing.Count; i++)
            {
                if (Borrowing[i].UserID == CurrentUser && Borrowing[i].IsReturned != true)
                {
                    CountDown = (Borrowing[i].ReturnBy - DateTime.Now).TotalDays;
                    CountDown = Math.Round(CountDown, 0);
                    Console.WriteLine($"Book ID: {Borrowing[i].BookID} \nReturn Date: {Borrowing[i].ReturnBy} \nDays remaining: {CountDown}\n");
                    BorrowedBookIDs.Add(Borrowing[i].BookID);

                }
            }
            Console.WriteLine("\n");
            Console.WriteLine("^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^\n");
            Console.WriteLine("RETURNED BOOKS:");
            for (int i = 0; i < Borrowing.Count; i++)
            {
                if (Borrowing[i].UserID == CurrentUser && Borrowing[i].IsReturned != false)
                {
                    Console.WriteLine($"Book ID: {Borrowing[i].BookID} \nReturn Date: {Borrowing[i].ReturnBy} \nActual Return: {Borrowing[i].ActualReturn}\n");
                    BorrowedBookIDs.Add(Borrowing[i].BookID);

                }
            }
            Console.WriteLine("\n");
            Console.WriteLine("^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^\n");
            Console.WriteLine("\n\t\t\t\t\t Press enter to continue...");
            Console.ReadKey();

        }


        //PRINT RETRUN RECIPT-
        static void ReturnRecipt(int i)
        {
            DateTime Now = DateTime.Now;

            Console.Clear();
            Console.WriteLine("\n\n- - - - - - - - - - - - - - - - - - - - - - - -C I T Y   L I B R A R Y- - - - - - - - - - - - - - - - - - - - - - - - -\n\n");
            Console.WriteLine("* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *  \n\n");
            Console.WriteLine("\t\t\t\t\t Returned: " + Now);
            Console.WriteLine("* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *  \n\n");
            PrintDucks();
            Console.WriteLine($"\t\t\t\t\tBOOK: \nID - {Books[i].BookID} \nNAME - {Books[i].BookName} \nAUTHOR - {Books[i].BookAuthor}");
            Console.WriteLine("* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *  \n\n");
            Console.WriteLine($"\t\t\t\t\tThank you for returning {Books[i].BookName} :)\n\n");
            Console.WriteLine("\t\t\t\t\t\tCome again soon!");
            Console.WriteLine("* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *  \n\n");

            Console.Write("Press enter to continue");
            Console.ReadKey();
        }


        //SAVES USER INFO TO FILE-
        static void SaveUsers()
        {
            try
            {//Info saved -> ID|UserName|Password|Email
                using (StreamWriter writer = new StreamWriter(UserPath))
                {
                    foreach (var user in Users)
                    {
                        writer.WriteLine($"{user.UserID}| {user.UserUserName.Trim()} | {user.UserEmail.Trim()} | {user.UserPswd.Trim()}");
                    }
                }
                Console.WriteLine("User details saved to file successfully! :)");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving to file: {ex.Message}");
            }
        }


        //READS USER INFO FROM FILE-
        static void LoadUsers()
        {
            try
            {
                if (File.Exists(UserPath))
                {
                    using (StreamReader reader = new StreamReader(UserPath))
                    {
                        string line;
                        while ((line = reader.ReadLine()) != null)
                        {
                            var parts = line.Split('|');
                            if (parts.Length == 4)
                            {
                                Users.Add((int.Parse(parts[0]), parts[1], parts[2], parts[3]));
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading users from file: {ex.Message}");
            }
        }


        //RECORDS INVOICES ON FILE-
        static void SaveInvoice()
        {
            try
            {
                using (StreamWriter writer = new StreamWriter(InvoicePath, true))
                {
                    foreach (var invoice in Invoices)
                    {
                        writer.WriteLine($"{invoice.CustomerID}|{invoice.BorrowedOn}|{invoice.BookID}|{invoice.BookName}|{invoice.BookAuthor}|{invoice.Borrow}");
                    }
                }
                Console.WriteLine("Invoices saved to file successfully! :)");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving to file: {ex.Message}");
            }
        }


        //BOOK RECCOMENDATION GENERATOR-
        static void Reccomend(string Author)
        {
            //Book author find other books with book author and suggest 
            Console.Clear();
            Console.WriteLine("You might also like: ");
            for (int i = 0; i < Books.Count; i++)
            {
                if (Books[i].BookAuthor == Author)
                {
                    Console.WriteLine($"Book name: {Books[i].BookName}");
                }
            }

            //Most popular book -> suggest 

        }
    }
}
