using LibrarySystemDB.AsciiArt;
using LibrarySystemDB.Models;
using LibrarySystemDB.Repositories;
using Microsoft.Identity.Client;
using System.ComponentModel.Design;
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
                        var CurrentReader = reader.GetReaderByName(Usr);

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
                            ReturnBook(borrow, currentReader, applicationDbContext, art);
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
                            ViewBook(applicationDbContext, art, currentReader);
                            break;

                        case 2:
                            Console.Clear();
                            UserSearchForBook(applicationDbContext);
                            break;

                        case 3:
                            Console.Clear();
                            ViewUsrProfile(currentReader, art, applicationDbContext);
                            break;

                        case 4:
                            Console.Clear();
                            ViewBook(applicationDbContext, art, currentReader);
                            BorrowBook(applicationDbContext, art, currentReader);
                            break;

                        case 5:
                            Console.Clear();
                            ReturnBook(borrow, currentReader, applicationDbContext, art);
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
        static void ViewBook(ApplicationDBContext applicationDbContext, Artworks art, Reader reader)
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
                    BorrowBook(applicationDbContext, art, reader);
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

        //ALLOWS USER TO SEARCH FOR BOOK WITH ID
        static bool UserSearchByID(ApplicationDBContext applicationDbContext, int ID)
        {
            BooksRepo book = new BooksRepo(applicationDbContext);
            var allbooks = book.GetAll();
            bool found = false;

            for (int i = 0; i < allbooks.Count; i++)
            {

                if (allbooks[i].BookID == ID)
                {
                    found = true;
                }

            }

            if (!found)
            {
                Console.WriteLine("<!>This book does not exist<!>");
            }

            return found;

        }

        //BORROW BOOK
        static void BorrowBook(ApplicationDBContext applicationDbContext, Artworks art, Reader reader )
        {

            Console.Write("\n\n\n\n\t\t\t\t\t\t   BORROWING A BOOK:\n\n");
            Console.Write("Enter ID: ");
            int BorrowID = 0;

            try
            {
                BorrowID = int.Parse(Console.ReadLine());
            }
            catch (Exception ex) { Console.WriteLine(ex.Message + "\n"); }

            bool Exists = UserSearchByID(applicationDbContext, BorrowID);

            if (Exists)
            {
                BorrowsRepo borrows = new BorrowsRepo(applicationDbContext);

                var allBorrows = borrows.GetAll();

                foreach (Borrow borrow in allBorrows)
                {
                    if (borrow.BRID == reader.RID && borrow.BBID == BorrowID && borrow.IsReturned == IsReturnedType.NotReturned) //checks if user has this book borrowed currently 
                    {
                        Console.WriteLine("Looks like you already borrowed this book, sorry you can't borrow more than one copy :(");
                        Console.WriteLine("Press enter...");
                        Console.ReadKey();
                    }

                    else
                    {
                        //Adding Borrow 
                        bool complete = borrows.Add(BorrowID, reader.RID, applicationDbContext);


                        if (complete)
                        {
                            PrintRecipt(art, borrow.BBID, applicationDbContext);
                        }

                        else { Console.WriteLine("<!> ERROR occured please try again<!>"); }
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
                {
                    ViewBook(applicationDbContext, art, reader);
                }
            }

        }

        //PRINT BORROW RECIPT
        static void PrintRecipt(Artworks art, int bookID, ApplicationDBContext applicationDBContext)
        {
            BooksRepo Books = new BooksRepo(applicationDBContext);
            var book = Books.GetBookByID(bookID);

            DateOnly Return = DateOnly.FromDateTime(DateTime.Now);
            Return.AddDays(book.BorrowPeriod);

            //Printing recipt 
            Console.Clear();
            Console.WriteLine("\n\n- - - - - - - - - - - - - - - - - - - - - - - -C I T Y   L I B R A R Y- - - - - - - - - - - - - - - - - - - - - - - - -\n\n");
            Console.WriteLine("* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *  \n\n");
            Console.WriteLine("\t\t\t\t\t" + DateTime.Now);
            Console.WriteLine("\n\n* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * \n\n");

            //Getting related ASCII art
            Random random = new Random();
            int RandomArt = random.Next(1, 11);
            switch (RandomArt)
            {
                case 1:
                    art.PrintBear();
                    break;

                case 2:
                    art.PrintPie();
                    break;

                case 3:
                    art.PrintScroll();
                    break;

                case 4:
                    art.PrintComputer();
                    break;

                case 5:
                    art.PrintPerson();
                    break;

                case 6:
                    art.PrintBooks();
                    break;

                case 7:
                    art.PrintMoon();
                    break;

                case 8:
                    art.PrintWindowsLogo();
                    break;

                case 9:
                    art.PrintSherlock();
                    break;

                case 10:
                    art.PrintPacMan();
                    break;

            }

            Console.WriteLine($"BOOK: ID - {book.BookID} \nNAME - {book.Title} \nAUTHOR - {book.AuthFName} {book.AuthFName}\nRETURN BY - {Return}");
            Console.WriteLine("\n\n\n* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * \n\n\n");
            Console.WriteLine("\n\t\t\tThank you for visiting the library come again soon!");
            Console.WriteLine("\t\t\tHappy Reading :)");
            Console.WriteLine("\n\n* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * \n ");
            Console.WriteLine("Press enter to continue");
            Console.ReadKey();

        }


        //RETURN BOOK
        static void ReturnBook(BorrowsRepo borrow, Reader CurrentReader, ApplicationDBContext applicationDbContext, Artworks art)
        {
            int returnBook = 0;
            Console.Clear();

            var borrowedBooks = borrow.GetBorrowByReaderID(CurrentReader.RID);

            PrintTitle();
            Console.Write("\n\n\n\n\t\t\t\t\t\t   RETURN BOOK:\n\n");

            Console.WriteLine("BORROWED BOOKS: ");
            foreach(var book in borrowedBooks)
            {
                Console.WriteLine($"Book ID: {book.BBID} | Borrowed Date: {book.BorrowedDate} | Should be returned by: {book.PredictedReturn}");
            }

            Console.Write("\nEnter Book ID: ");

            try
            {
                returnBook = int.Parse(Console.ReadLine());
            }
            catch (Exception ex) { Console.WriteLine(ex.Message); }

            bool Found = false;

            foreach (var book in borrowedBooks)
            {
                if (book.BBID == returnBook)
                {
                    RatingTypes rating = RatingTypes.Okay;
                    bool Error = false;
                    int Response = 0;

                    do
                    {
                        Error = false;
                        Console.WriteLine($"Please rate the book:");
                        Console.WriteLine("1. Excellent");
                        Console.WriteLine("2. Great");
                        Console.WriteLine("3. Good");
                        Console.WriteLine("4. Okay");
                        Console.WriteLine("5. Bad");
                        Console.WriteLine("6. Very Bad");

                        Console.Write("\nRating: ");

                        try
                        {
                            Response = int.Parse(Console.ReadLine());
                        }
                        catch (Exception e) { Console.WriteLine(e.Message); }


                        switch (Response)
                        {
                            case 1:
                                rating = RatingTypes.Excellent;
                                break;
                            case 2:
                                rating = RatingTypes.Great;
                                break;
                            case 3:
                                rating = RatingTypes.Good;
                                break;
                            case 4:
                                rating = RatingTypes.Okay;
                                break;
                            case 5:
                                rating = RatingTypes.Bad;
                                break;
                            case 6:
                                rating = RatingTypes.VeryBad;
                                break;
                            default:
                                Console.WriteLine("<!>Please choose a valid option<!>");
                                Error = true;
                                break;
                        }
                    } while (Error == true);

                    bool complete = borrow.Return(CurrentReader.RID, returnBook, applicationDbContext, rating);

                    if (complete)
                    {
                        Console.WriteLine("Book successfully returned!");
                        Console.WriteLine($"Thank you for returning book ID - {returnBook} :) \nPress enter to print your recipt");
                        Console.ReadKey();
                        ReturnRecipt(art, returnBook, applicationDbContext);

                    }

                    else
                    {
                        Console.WriteLine("<!> ERROR book could not be returned<!>");
                    }
                    
                    Found = true;
                    break;
                }
            }

           if (!Found) { Console.WriteLine("<!>You haven't borrowed this book :(<!>"); }
        }


        //PRINTS USER DETAILS
        static void ViewUsrProfile(Reader CurrentReader, Artworks art, ApplicationDBContext applicationDbContext)
        {
            BorrowsRepo borrow = new BorrowsRepo(applicationDbContext);

            double CountDown;

            int UserRead = borrow.GetTotalBorrowedBooksByReader(CurrentReader);
            var borrowedBooks = borrow.GetBorrowByReaderID(CurrentReader.RID);

            Console.Clear();
            Console.WriteLine("\n\n- - - - - - - - - - - - - - - - - - - - - - - -C I T Y   L I B R A R Y- - - - - - - - - - - - - - - - - - - - - - - - -\n\n");
            Console.WriteLine("^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^\n");
            art.PrintOwl();
            Console.WriteLine($"\n\t\t\t\t\t\t {CurrentReader.RFName}'s Home Page :) \n ");
            Console.WriteLine($"MY DETAILS: \nUser ID: {CurrentReader.RID} \nUser Name: {CurrentReader.RUserName} \nEmail: {CurrentReader.REmail} \nBooks Read: {UserRead}\n");
            Console.WriteLine("^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^\n");
            Console.WriteLine($"CURRENTLY BORROWED:");
            foreach (var i in borrowedBooks)
            {
                if (i.IsReturned == IsReturnedType.NotReturned)
                {
                    Console.WriteLine($"Book ID: {i.BBID} \nReturn Date: {i.PredictedReturn}");
                }
            }

            //count currently borrowed using get borrowed by id

            Console.WriteLine("\n");
            Console.WriteLine("^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^\n");
            Console.WriteLine("RETURNED BOOKS:");
            foreach (var i in borrowedBooks)
            {
                if (i.IsReturned == IsReturnedType.Returned)
                {
                    Console.WriteLine($"Book ID: {i.BBID} \nReturned Date: {i.ActualReturn}");
                }
            }

            Console.WriteLine("\n");
            Console.WriteLine("^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^\n");
            Console.WriteLine("\n\t\t\t\t\t Press enter to continue...");
            Console.ReadKey();

        }


        //PRINT RETRUN RECIPT
        static void ReturnRecipt(Artworks art,int bookID, ApplicationDBContext applicationDBContext)
        {
            DateTime Now = DateTime.Now;
            BooksRepo book = new BooksRepo(applicationDBContext);
            var ThisBook = book.GetBookByID(bookID);

            Console.Clear();
            Console.WriteLine("\n\n- - - - - - - - - - - - - - - - - - - - - - - -C I T Y   L I B R A R Y- - - - - - - - - - - - - - - - - - - - - - - - -\n\n");
            Console.WriteLine("* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *  \n\n");
            Console.WriteLine("\t\t\t\t\t Returned: " + Now);
            Console.WriteLine("* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *  \n\n");
            art.PrintDucks();
            Console.WriteLine($"\t\t\t\t\tBOOK: \nID - {ThisBook.BookID} \nNAME - {ThisBook.Title} \nAUTHOR - {ThisBook.AuthFName} {ThisBook.AuthLName}");
            Console.WriteLine("* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *  \n\n");
            Console.WriteLine($"\t\t\t\t\tThank you for returning {ThisBook.Title} :)\n\n");
            Console.WriteLine("\t\t\t\t\t\tCome again soon!");
            Console.WriteLine("* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *  \n\n");

            Console.Write("Press enter to continue");
            Console.ReadKey();
        }


        //BOOK RECCOMENDATION GENERATOR
        static void Reccomend(string Author, BooksRepo book)
        {

            //Book author find other books with book author and suggest 
            Console.Clear();
            var recommendations = book.GetAllByAuthor(Author);

            Console.WriteLine("You might also like: ");
            foreach (var r in recommendations) 
            {
                Console.WriteLine($"|Book ID: {r.BookID} | Book Title: {r.Title} | Book Author {r.AuthFName} {r.AuthLName}|");
            }
        }
    }
}
