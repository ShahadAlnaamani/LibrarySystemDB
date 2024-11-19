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

                            UserPage(applicationDbContext, Usr, reader, CurrentReader);

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
                                RegisterReader(reader);
                            }

                        }
                        break;


                    case 2:
                        Console.Clear();
                        PrintTitle();
                        Console.Write("\n\n\n\n\n\t\t\t\t\t\t   LIBRARIAN LOGIN:\n\n");
                        Console.Write("\t\t\t\t\tUsername: ");
                        string admin = Console.ReadLine();
                        Console.Write("\t\t\t\t\tPassword: ");
                        string adminPass = Console.ReadLine();


                        int adminAuth = librarian.AdminAuthentication(admin, adminPass);
                        var CurrentAdmin = librarian.GetLibrarianByName(admin);

                        if (adminAuth == 1)
                        {
                            Console.Clear();
                            LibrarianPage(applicationDbContext, admin, CurrentAdmin, art, book);
                        }

                        else if (adminAuth == 2)
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
                                RegisterLibrary(librarian);
                            }

                        }
                        break;

                    case 3:

                        Console.Clear();
                        PrintTitle();
                        Console.Write("\n\n\n\n\t\t\t\t\t\t   REGISTRATION:\n\n\n");
                        Console.WriteLine("\t\t\t\t\t\t  1.  Reader Registration\n");
                        Console.WriteLine("\t\t\t\t\t\t  2.  Librarian Registration\n");

                        int Choice = 0;
                        Console.Write("\t\t\t\t\t\t   Enter: ");


                        try
                        {
                            Choice = int.Parse(Console.ReadLine());
                        }
                        catch (Exception ex) { Console.WriteLine(ex.Message); Console.WriteLine("\n Press enter to continue :("); Console.ReadKey(); }

                        switch (Choice)
                        {
                            case 1:
                                RegisterReader(reader);
                                break;

                            case 2:
                                RegisterLibrary(librarian);
                                break;

                            default:
                                Console.WriteLine("<!>Please choose a valid option<!>");
                                break;
                        }
                        break;

                    case 4:
                        Console.Clear();
                        PrintTitle();
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
       
        static void PrintTitle()
        { Console.WriteLine("\n\n- - - - - - - - - - - - - - - - - - - - - - - -C I T Y   L I B R A R Y- - - - - - - - - - - - - - - - - - - - - - - - -\n\n"); }


        //--------------------------------USER FUNCTIONS----------------------------- 
        //USER MAIN PAGE 

        static void RegisterReader(ReadersRepo reader)
        {
            Console.Clear();
            PrintTitle();
            Console.WriteLine("\n\n\n\n\t\t\t\t\t\t   READER REGISTRATION:\n");

            Console.WriteLine("First Name "); //check criteria 
            Console.Write("Enter: ");
            string Fname = Console.ReadLine();

            Console.WriteLine("Last Name ");//check criteria 
            Console.Write("Enter: ");
            string Lname = Console.ReadLine();

            Console.WriteLine("Email "); //check criteria 
            Console.Write("Enter: ");
            string Email = Console.ReadLine();

            bool Continue = false;
            GenderType gender = GenderType.Male;

            do {
                Continue = false;
                Console.WriteLine("Gender \n1) Male \n2) Female \n");
                Console.Write("Enter: ");
                int GSelection = 0;
                try
                {
                    GSelection = int.Parse(Console.ReadLine());
                } catch (Exception e) { Console.WriteLine(e.Message); }

                switch (GSelection)
                {
                    case 1:
                        gender = GenderType.Male;
                        break;

                    case 2:
                        gender = GenderType.Female;
                        break;

                    default:
                        Continue = true;
                        break;
                }
            } while (Continue);

            Console.WriteLine("Phone ");
            Console.Write("Enter: ");
            string Phone = Console.ReadLine();

            Console.WriteLine("User Name "); // check not repeated
            Console.Write("Enter: ");
            string UsrName = Console.ReadLine();

            Console.WriteLine("Password "); //check criteria 
            Console.Write("Enter: ");
            string Password = Console.ReadLine();

            reader.Add(Fname, Lname, Email, gender, Phone, UsrName, Password);
        }
        
        static void UserPage(ApplicationDBContext applicationDbContext, string usr, ReadersRepo readerRepo, Reader currentReader)
        {
            Artworks art = new Artworks();
            BorrowsRepo borrow = new BorrowsRepo(applicationDbContext);


            //var currentReader = readerRepo.GetReaderByUserName(usr);
            var OverdueBooks = borrow.OverdueFinder(currentReader);

            //Overdue books 
            if (OverdueBooks.Count() !=  0) 
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
                BooksTable.Columns.Add("TITLE", typeof(string));
                BooksTable.Columns.Add("AUTHOR FNAME", typeof(string));
                BooksTable.Columns.Add("AUTHOR LNAME", typeof(string));
                BooksTable.Columns.Add("CATEGORY", typeof(string));
                BooksTable.Columns.Add("BORROW PERIOD", typeof(int));

                for (int i = 0; i < AllBooks.Count; i++)
                {
                    BooksTable.Rows.Add(AllBooks[i].BookID, AllBooks[i].Title.Trim(), AllBooks[i].AuthFName.Trim(), AllBooks[i].AuthLName.Trim(), AllBooks[i].Categories.CategoryTypes, AllBooks[i].BorrowPeriod);
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

                else
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
                    Console.WriteLine($"\nBook Title: {allbooks[i].Title} \nBook Author: {allbooks[i].AuthFName} {allbooks[i].AuthLName} \nID: {allbooks[i].BookID} \nCategory: {allbooks[i].Categories.CategoryTypes} \nPrice: {allbooks[i].Price} \nBorrow Period: {allbooks[i].BorrowPeriod} days\n");
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
            PrintTitle();
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
            PrintTitle();
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
            PrintTitle();
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


        ////BOOK RECCOMENDATION GENERATOR
        //static void Reccomend(string Author, BooksRepo book)
        //{

        //    //Book author find other books with book author and suggest 
        //    Console.Clear();
        //    var recommendations = book.GetAllByAuthor(Author);

        //    Console.WriteLine("You might also like: ");
        //    foreach (var r in recommendations) 
        //    {
        //        Console.WriteLine($"|Book ID: {r.BookID} | Book Title: {r.Title} | Book Author {r.AuthFName} {r.AuthLName}|");
        //    }
        //}




        //- - - - - - - - - - - - - - - - - - - - ADMIN FUNCTIONS  - - - - - - - - - - - - - - - - - - //
        //ADMIN PAGE  

        static void RegisterLibrary(LibrarianRepo librarian)
        {
            Console.Clear();
            PrintTitle();
            Console.WriteLine("\n\n\n\n\t\t\t\t\t\t   LIBRARIAN REGISTRATION:\n");

            Console.WriteLine("First Name: "); //check criteria 
            Console.Write("Enter: ");
            string Fname = Console.ReadLine();

            Console.WriteLine("Last Name: ");//check criteria 
            Console.Write("Enter: ");
            string Lname = Console.ReadLine();

            Console.WriteLine("Email: "); //check criteria 
            Console.Write("Enter: ");
            string Email = Console.ReadLine();

            Console.WriteLine("User Name: "); // check not repeated
            Console.Write("Enter: ");
            string UsrName = Console.ReadLine();

            Console.WriteLine("Password: "); //check criteria 
            Console.Write("Enter: ");
            string Password = Console.ReadLine();

            librarian.Add(Fname, Lname, Email, UsrName, Password);
        }

        //ADMIN PAGE-
        static void LibrarianPage(ApplicationDBContext applicationDbContext, string adminName, Librarian CurrentLibrarian, Artworks art, BooksRepo book)
        {
            CategoriesRepo categories = new CategoriesRepo(applicationDbContext);
            bool ExitFlag = false;
            do
            {
                Console.Clear();
                PrintTitle();
                Console.Write("\n\n\n\n\t\t\t\t\t\t   LIBRARIAN OPTIONS:\n\n\n");
                Console.WriteLine("\t\t\t\t\t1. Add New Book\n");
                Console.WriteLine("\t\t\t\t\t2. Display All Books\n");
                Console.WriteLine("\t\t\t\t\t3. Search by Book Name or Author\n");
                //Console.WriteLine("\t\t\t\t\t4. Edit Book\n");
                //Console.WriteLine("\t\t\t\t\t5. Delete Book\n");
                //Console.WriteLine("\t\t\t\t\t6. Show Reports\n");
                Console.WriteLine("\t\t\t\t\t7. Add Category\n");
                Console.WriteLine("\t\t\t\t\t8. Update Category\n");
                Console.WriteLine("\t\t\t\t\t9. Log out\n\n\n");
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
                        AddNewBook(book, applicationDbContext);
                        break;

                    case 2:
                        Console.Clear();
                        ViewBooksLibrarian(applicationDbContext);
                        break;

                    case 3:
                        Console.Clear();
                        SearchForBook(applicationDbContext);
                        break;

                    case 4:
                        Console.Clear();
                        //EditBooks();
                        break;

                    case 5:
                        Console.Clear();
                        //DeleteBook();
                        break;

                    case 6:
                        Console.Clear();
                        //Reports();
                        break;

                    case 7:
                        Console.Clear();
                        AddCategory(categories);
                        break;

                    case 8:
                        Console.Clear();
                        UpdateCategory(categories, applicationDbContext);
                        break;

                    case 9:
                        Console.Clear();
                        Console.WriteLine("\n\n- - - - - - - - - - - - - - - - - - - - - -L O G I N G   O U T- - - - - - - - - - - - - - - - - - - - - - -\n\n");
                        art.PrintFish();
                        ExitFlag = true;
                        break;

                    default:
                        Console.WriteLine("\t\t\t\t\t<!>Sorry your choice was wrong<!>");
                        break;

                }
                Console.WriteLine("\t\t\t\t\tPress enter to continue...");
                string cont = Console.ReadLine();
                Console.Clear();

            } while (ExitFlag != true);

        }


        //ALL BOOKS
        static void ViewBooksLibrarian(ApplicationDBContext applicationDbContext)
        {
            PrintTitle();
            Book book = new Book();
            BooksRepo Books = new BooksRepo(applicationDbContext);
            var AllBooks = Books.GetAll();
            if (AllBooks.Count != 0)
            {
                Console.Write("\n\n\n\n\t\t\t\t\t\t   AVAILABLE BOOKS:\n\n");

                var BooksTable = new DataTable("Books");
                BooksTable.Columns.Add("ID", typeof(int));
                BooksTable.Columns.Add("TITLE", typeof(string));
                BooksTable.Columns.Add("AUTHOR FNAME", typeof(string));
                BooksTable.Columns.Add("AUTHOR LNAME", typeof(string));
                BooksTable.Columns.Add("CATEGORY", typeof(string));
                BooksTable.Columns.Add("PRICE", typeof(decimal));
                BooksTable.Columns.Add("BORROW PERIOD", typeof(int));
                BooksTable.Columns.Add("TOTAL", typeof(int));
                BooksTable.Columns.Add("BORROWED", typeof(int));

                for (int i = 0; i < AllBooks.Count; i++)
                {
                    BooksTable.Rows.Add(AllBooks[i].BookID, AllBooks[i].Title.Trim(), AllBooks[i].AuthFName.Trim(), AllBooks[i].AuthLName.Trim(), AllBooks[i].Categories.CategoryTypes, AllBooks[i].Price, AllBooks[i].BorrowPeriod, AllBooks[i].TotalCopies, AllBooks[i].BorrowedCopies);
                }

                foreach (DataColumn column in BooksTable.Columns)
                {
                    Console.Write($"{column.ColumnName,-14}");
                }
                Console.WriteLine();


                foreach (DataRow row in BooksTable.Rows)
                {
                    foreach (var item in row.ItemArray)
                    {
                        Console.Write($"{item,-14}");
                    }
                    Console.WriteLine();
                }
                Console.WriteLine();
            }
        }

        //ADDS NEW CATEGORIES
        static void AddCategory(CategoriesRepo categories)
        {
            Console.Clear();
            PrintTitle();
            Console.Write("\n\n\n\n\t\t\t\t\t\t   ADDING NEW CATEGORY:\n\n");

            Console.WriteLine("Category Name: "); // check not repeated
            Console.Write("Enter: ");
            string CatName = Console.ReadLine();

            bool completed = categories.Add(CatName);


            if (completed)
            {
                Console.WriteLine("Successfully added category!");
            }

            else { Console.WriteLine("<!>Error occured when adding category<!>"); }
        }


        //UPDATES CATEGORY NAME 
        static void UpdateCategory(CategoriesRepo categories, ApplicationDBContext applicationDbContext)
        {
            Console.Clear();
            PrintTitle();
            Console.Write("\n\n\n\n\t\t\t\t\t\t   UPDATE CATEGORY:\n\n");

            PrintCategories(applicationDbContext);


            Console.WriteLine("Category ID "); 
            Console.Write("Enter: ");
            int CatID = int.Parse(Console.ReadLine()); //check parsing 


            Console.WriteLine("New name: "); 
            Console.Write("Enter: ");
            string NewName = Console.ReadLine();

            int Change = categories.Update(NewName, CatID);

            if (Change == 0)
            { Console.WriteLine("<!>Category ID not valid<!>"); }

            else if (Change == 1)
            { Console.WriteLine("Updated successfully!"); }

            else { Console.WriteLine("<!> Category name is already used :( <!>"); }
        }


        //PRINT CATEGORIES 
        static void PrintCategories(ApplicationDBContext applicationDbContext)
        {
            CategoriesRepo categories = new CategoriesRepo(applicationDbContext);
            var AllCategories = categories.GetAll();
            if (AllCategories.Count != 0)
            {
                Console.Write("\n\n\n\n\t\t\t\t\t\t   AVAILABLE CATEGORIES:\n\n");

                var CategoriessTable = new DataTable("Category");
                CategoriessTable.Columns.Add("ID", typeof(int));
                CategoriessTable.Columns.Add("CATEGORY TYPES", typeof(string));
                CategoriessTable.Columns.Add("NO OF BOOKS", typeof(int));

                for (int i = 0; i < AllCategories.Count; i++)
                {
                    CategoriessTable.Rows.Add(AllCategories[i].CatID, AllCategories[i].CategoryTypes.Trim(), AllCategories[i].NoBooks);
                }

                foreach (DataColumn column in CategoriessTable.Columns)
                {
                    Console.Write($"{column.ColumnName,-25}");
                }
                Console.WriteLine();


                foreach (DataRow row in CategoriessTable.Rows)
                {
                    foreach (var item in row.ItemArray)
                    {
                        Console.Write($"{item,-25}");
                    }
                    Console.WriteLine();
                }
                Console.WriteLine();
            }
        }

        //ADDS BOOK
        static void AddNewBook(BooksRepo book, ApplicationDBContext applicationDbContext0)
        {
            Console.Clear();
            PrintTitle();
            Console.Write("\n\n\n\n\t\t\t\t\t\t   ADDING NEW BOOK:\n\n");

            Console.WriteLine("Book Title: "); // check not repeated
            Console.Write("Enter: ");
            string Title = Console.ReadLine();

            Console.WriteLine("Borrow Period: "); //check int parsing  
            Console.Write("Enter: ");
            int Period = int.Parse(Console.ReadLine());

            Console.WriteLine("Author First Name: ");
            Console.Write("Enter: ");
            string FName = Console.ReadLine();

            Console.WriteLine("Author Last Title: ");
            Console.Write("Enter: ");
            string LName = Console.ReadLine();

            Console.WriteLine("Price: "); //check int parsing  
            Console.Write("Enter: ");
            decimal Price = decimal.Parse(Console.ReadLine());

            Console.WriteLine("Available copies: ");
            Console.Write("Enter: ");
            int Copies = int.Parse(Console.ReadLine());

            Console.Clear();
            PrintTitle();
            PrintCategories(applicationDbContext0);   

            Console.WriteLine("Category ID: "); //make sure valid 
            Console.Write("Enter: ");
            int CatID = int.Parse(Console.ReadLine());

            bool added = book.Add(Title, Period, FName, LName, Price, Copies, CatID, applicationDbContext0);

            if (added)
            { Console.WriteLine("Successfully added book!"); }

            else { Console.WriteLine("<!>Error occured when adding book, check that category is valid<!>"); }
        }


        //ALLOWS USER TO SEARCH FOR BOOK - SPECIAL ADMIN OUTPUT
        static void SearchForBook(ApplicationDBContext applicationDBContext)
        {
            BooksRepo book = new BooksRepo(applicationDBContext);
            var allbooks = book.GetAll();
            Console.Clear();
            PrintTitle();
            Console.Write("\n\n\n\n\t\t\t\t\t   LIBRARIAN SEARCH LIBRARY:\n\n");
            Console.Write("\t\t\t\t\tBook name or author: ");
            string name = (Console.ReadLine().Trim()).ToLower();
            string SearchPattern = Regex.Escape(name);
            Regex regex = new Regex(SearchPattern, RegexOptions.IgnoreCase);

            bool flag = false;

            for (int i = 0; i < allbooks.Count; i++)
            {

                if (regex.IsMatch(allbooks[i].Title) || regex.IsMatch(allbooks[i].AuthLName) || regex.IsMatch(allbooks[i].AuthFName))
                {
                    Console.WriteLine($"\nBook Title: {allbooks[i].Title} \nBook Author: {allbooks[i].AuthFName} {allbooks[i].AuthLName} \nID: {allbooks[i].BookID} \nCategory: {allbooks[i].Categories.CategoryTypes} \nPrice: {allbooks[i].Price} \nBorrow Period: {allbooks[i].BorrowPeriod} days \nTotal Copies: {allbooks[i].TotalCopies} \nBorrowed Copies {allbooks[i].BorrowedCopies}");
                    flag = true;
                }

            }

            if (flag != true)
            { Console.WriteLine("\t\t\t\t\t<!>Book not found :( <!>"); }

            Console.Write("\n\t\t\t\t\tPress enter to continue");
            Console.ReadKey();
        }


        ////ALLOWS LIBRARIAN TO EDIT BOOK INFO-
        //static void EditBooks()
        //{
        //    Console.WriteLine("\n\n\n\n\t\t\t\t\t\t   EDIT BOOKS:\n\n\n");
        //    Console.WriteLine("\t\t\t\t\t\t1.  Edit Book Title\n");
        //    Console.WriteLine("\t\t\t\t\t\t2.  Edit Author Name\n");
        //    Console.WriteLine("\t\t\t\t\t\t3.  Add More Copies of Available Books\n");
        //    Console.WriteLine("\t\t\t\t\t\t4.  Save and exit\n\n\n");
        //    Console.Write("\t\t\t\t\t\tEnter Option: ");
        //    int Choice = 0;

        //    try
        //    {
        //        Choice = int.Parse(Console.ReadLine());
        //    }
        //    catch (Exception ex) { Console.WriteLine(ex.Message); }

        //    Console.Clear();
        //    PrintTitle();

        //    bool ChooseOption = true;
        //    do
        //    {
        //        ChooseOption = true;
        //        switch (Choice)
        //        {
        //            //Editing book title
        //            case 1:
        //                int Location = GetInformation();
        //                if (Location != -1)
        //                {
        //                    Console.WriteLine("\n\n\n\n\t\t\t\t\t\t   EDIT BOOK TITLE:\n");
        //                    string NewBookName;
        //                    bool Repeated;

        //                    do
        //                    {
        //                        Repeated = false;
        //                        Console.Write("\t\t\t\t\t\tEnter Book Name: ");
        //                        NewBookName = Console.ReadLine().Trim(); //Trim added for more accurate search  

        //                        for (int i = 0; i < Books.Count; i++)
        //                        {
        //                            if ((Books[i].BookName).Trim() == NewBookName)
        //                            {
        //                                Repeated = true;
        //                                break;
        //                            }
        //                        }

        //                        if (Repeated != false)
        //                        {
        //                            Console.WriteLine("\n\t\t\t\t<!>This book already exists please enter a new book name<!>");
        //                            Repeated = true;
        //                        }

        //                    } while (Repeated != false);

        //                    Books[Location] = ((Books[Location].BookID, BookName: NewBookName, Books[Location].BookAuthor, Books[Location].BookQuantity, Books[Location].Borrowed, Books[Location].Price, Books[Location].Category, Books[Location].BorrowPeriod));
        //                    Console.WriteLine($"\n\nUPDATED DETAILS:  \nName: {Books[Location].BookName}  Author: {Books[Location].BookAuthor}  ID: {Books[Location].BookID}  x{Books[Location].BookQuantity}  Issues Borrowed: {Books[Location].Borrowed}\n ");
        //                    SaveBooksToFile();
        //                }

        //                break;


        //            //Editing author name 
        //            case 2:
        //                int Position = GetInformation();
        //                if (Position != -1)
        //                {
        //                    Console.WriteLine("\n\n\n\n\t\t\t\t\t\t   EDIT AUTHOR NAME:\n");
        //                    Console.Write("\n\t\t\t\t\t\tNew author name: ");
        //                    string NewAuthName = Console.ReadLine();
        //                    Books[Position] = ((Books[Position].BookID, Books[Position].BookName, BookAuthor: NewAuthName, Books[Position].BookQuantity, Books[Position].Borrowed, Books[Position].Price, Books[Position].Category, Books[Position].BorrowPeriod));
        //                    Console.WriteLine($"\n\nUPDATED DETAILS:  \nName: {Books[Position].BookName}  Author: {Books[Position].BookAuthor}  ID: {Books[Position].BookID}  x{Books[Position].BookQuantity}  Issues Borrowed: {Books[Position].Borrowed}\n ");
        //                    SaveBooksToFile();
        //                }
        //                break;


        //            //Adding book copies 
        //            case 3:
        //                int Index = GetInformation();
        //                if (Index != -1)
        //                {
        //                    Console.WriteLine("\n\n\n\n\t\t\t\t\t\t   EDIT BOOK QUANTITY:\n");
        //                    Console.Write("\n\t\t\t\t\t\tHow many would you like to add: ");
        //                    int Add = 0;

        //                    try
        //                    {
        //                        Add = int.Parse(Console.ReadLine());
        //                    }
        //                    catch (Exception ex) { Console.WriteLine(ex.Message); }

        //                    //Checking the positive number inputted so that books aren't minused 
        //                    if (Add > 0)
        //                    {
        //                        Add = Books[Index].BookQuantity + Add;
        //                        Books[Index] = ((Books[Index].BookID, Books[Index].BookName, Books[Index].BookAuthor, BookQuantity: Add, Books[Index].Borrowed, Books[Index].Price, Books[Index].Category, Books[Index].BorrowPeriod));
        //                        Console.WriteLine($"\n\nUPDATED DETAILS:  \nName: {Books[Index].BookName}  Author: {Books[Index].BookAuthor}  ID: {Books[Index].BookID}  x{Books[Index].BookQuantity}  Issues Borrowed: {Books[Index].Borrowed}\n ");
        //                        SaveBooksToFile();
        //                    }
        //                    else { Console.WriteLine("\t\t\t\t<!>Improper input please input a number greater than 0 :( <!>"); }
        //                }
        //                break;


        //            case 4:
        //                SaveBooksToFile();
        //                break;


        //            default:
        //                Console.WriteLine("\t\t\t\t<!>Improper input, please choose one of the given options :( <!>");
        //                break;
        //        }
        //    } while (ChooseOption != false);


        //}


        ////DELETE BOOKS -
        //static void DeleteBook()
        //{
        //    Console.Clear();
        //    PrintTitle();
        //    Console.WriteLine("\n\n\n\n\t\t\t\t\t\t   DELETE A BOOK:\n");
        //    ViewAllBooks();

        //    int DeleteIndex = GetInformation();

        //    if (DeleteIndex != -1)
        //    {
        //        if (Books[DeleteIndex].Borrowed > 0)  //Book currently borrowed 
        //        {
        //            Console.WriteLine("\n\t\t\t\t<!>Sorry you can't delete this book as someone currently has it borrowed :( <!>\n");
        //            for (int i = 0; i < Borrowing.Count; i++)
        //            {
        //                if (Books[DeleteIndex].BookID == Borrowing[i].BookID)
        //                {
        //                    Console.WriteLine($"User {Borrowing[i].UserID} is currently borrowing this book \nThey should return the book by {Borrowing[i].ReturnBy} ");
        //                }
        //            }

        //        }

        //        else
        //        {
        //            Console.WriteLine($"DELETING: Name: {Books[DeleteIndex].BookName}  Author: {Books[DeleteIndex].BookAuthor}  ID: {Books[DeleteIndex].BookID}  x{Books[DeleteIndex].BookQuantity}  Issues Borrowed: {Books[DeleteIndex].Borrowed} ");
        //            Console.WriteLine("\n\t\t\t\t\t\tTo delete press X:");
        //            string Delete = Console.ReadLine().ToLower();

        //            if (Delete != "x")
        //            { Console.WriteLine("\n\t\t\t\t\t\tThe book was not deleted :)"); }
        //            else
        //            {
        //                Books.Remove((Books[DeleteIndex] = (Books[DeleteIndex].BookID, Books[DeleteIndex].BookName, Books[DeleteIndex].BookAuthor, Books[DeleteIndex].BookQuantity, Books[DeleteIndex].Borrowed, Books[DeleteIndex].Price, Books[DeleteIndex].Category, Books[DeleteIndex].BorrowPeriod)));
        //                Console.WriteLine("\n\t\t\t\t\t\tThe book was deleted sucessfully :)");
        //            }
        //            SaveBooksToFile();
        //        }
        //    }
        //}


        ////SHOWS STATISTICS ON BORROWED AND AVAILABLE BOOKS-
        //static public void Reports()
        //{
        //    Console.Clear();

        //    //List available books 
        //    ViewAllBooks();

        //    Console.WriteLine("- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -");
        //    Console.WriteLine("\t\tREPORTS:\n");
        //    Console.WriteLine("- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -");



        //    //Breaking down tuple list so we can carry out calculations on info
        //    List<string> BookNames = new List<string>();
        //    List<string> BookAuthors = new List<string>();
        //    List<int> BookIDs = new List<int>();
        //    List<int> BookQuantities = new List<int>();
        //    List<int> BorrowedBooks = new List<int>();
        //    List<string> MostBorrowedAuth = new List<string>();
        //    List<string> LeastBorrowedAuth = new List<string>();
        //    List<int> ReaderIDs = new List<int>();
        //    List<int> TransactionType = new List<int>();

        //    for (int i = 0; i < Books.Count; i++)
        //    {
        //        var (BookID, bookNames, bookAuthors, BookQuantity, Borrowed, Price, Category, BorrowPeriod) = Books[i];
        //        BookNames.Add(bookNames);
        //        BookAuthors.Add(bookAuthors);
        //        BookIDs.Add(BookID);
        //        BookQuantities.Add(BookQuantity);
        //        BorrowedBooks.Add(Borrowed);
        //    }

        //    //Total books borrowed
        //    int SumOfBorrowed = BorrowedBooks.Sum();
        //    Console.WriteLine("Number of Borrowed Books: " + SumOfBorrowed);


        //    //Total available books
        //    int SumOfAvailable = BookQuantities.Sum();
        //    Console.WriteLine("Number of Available Books: " + SumOfAvailable);

        //    //Number of categories
        //    Console.WriteLine("Number of Available Categories: " + Categories.Count);

        //    //Category information 
        //    var CategoriesTable = new DataTable("Categories");
        //    CategoriesTable.Columns.Add("ID", typeof(int));
        //    CategoriesTable.Columns.Add("Name", typeof(string));
        //    CategoriesTable.Columns.Add("No. Books", typeof(int));

        //    for (int i = 0; i < Categories.Count; i++)
        //    {
        //        CategoriesTable.Rows.Add(Categories[i].CategoryID, Categories[i].CategoryName, Categories[i].NoOfBooks);
        //    }

        //    foreach (DataColumn column in CategoriesTable.Columns)
        //    {
        //        Console.Write($"{column.ColumnName,-25}");
        //    }
        //    Console.WriteLine();


        //    foreach (DataRow row in CategoriesTable.Rows)
        //    {
        //        foreach (var item in row.ItemArray)
        //        {
        //            Console.Write($"{item,-25}");
        //        }
        //        Console.WriteLine();
        //    }
        //    Console.WriteLine();



        //    //Most borrowed book
        //    Console.WriteLine("- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -");
        //    Console.WriteLine("\n\n\tMOST BORROWED BOOK:\n");
        //    int MostBorrowedBook;

        //    MostBorrowedBook = BorrowedBooks.IndexOf(BorrowedBooks.Max());

        //    //To ensure that if more than one book have the maximum borrowed index they are included 
        //    for (int i = 0; i < Books.Count; i++)
        //    {
        //        if (Books[i].Borrowed == BorrowedBooks[MostBorrowedBook])
        //        {
        //            Console.WriteLine($"BOOK TITLE: {BookNames[i]} \nBOOK AUTHOR: {BookAuthors[i]} \nNUMBER OF COPIES BORROWED: {BorrowedBooks[i]}\n");
        //            MostBorrowedAuth.Add(BookAuthors[i]);
        //        }
        //    }


        //    //Least borrowed book
        //    Console.WriteLine("\n\n\tLEAST BORROWED BOOK:\n");
        //    int LeastBorrowedBook;

        //    LeastBorrowedBook = BorrowedBooks.IndexOf(BorrowedBooks.Min());
        //    //To ensure that if more than one book have the minimum borrowed index they are included 
        //    for (int i = 0; i < Books.Count; i++)
        //    {
        //        if (Books[i].Borrowed == BorrowedBooks[LeastBorrowedBook])
        //        {
        //            Console.WriteLine($"BOOK TITLE: {BookNames[i]} \nBOOK AUTHOR: {BookAuthors[i]} \nNUMBER OF COPIES BORROWED: {BorrowedBooks[i]}\n");
        //            LeastBorrowedAuth.Add(BookAuthors[i]);
        //        }
        //    }

        //    //Most borrowed author
        //    Console.WriteLine("- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -");
        //    Console.WriteLine("\n\n\tMOST POPULAR AUTHOR:\n");
        //    for (int i = 0; i < MostBorrowedAuth.Count; i++)
        //    {
        //        Console.WriteLine(MostBorrowedAuth[i]);
        //    }

        //    //Least borrowed author
        //    Console.WriteLine("\n\n\tLEAST POPULAR AUTHOR:\n");
        //    for (int i = 0; i < LeastBorrowedAuth.Count; i++)
        //    {
        //        Console.WriteLine(LeastBorrowedAuth[i]);
        //    };
        //    // Console.WriteLine("- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -");
        //    /*
        //    //Most active reader
        //    Console.WriteLine("- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -");
        //    Console.WriteLine("\n\n\tMOST ACTIVE READER:\n");
        //    LoadInvoices();
        //    //CustomerID, DateTime BorrowedOn, int BookID, string BookName, string BookAuthor, int Borrow
        //    for (int i = 0; i < Invoices.Count; i++)
        //    {
        //        var(CustomerID, BorrowedOn, BookID, BookName,BookAuthor, Borrow) = Invoices[i];
        //        // BookNames.Add(BookName);
        //        ReaderIDs.Add(CustomerID);
        //        TransactionType.Add(Borrow); //Will be 1 if Borrow transaction 0 if return transaction

        //    }

        //    //Finding recurrences of each ID and choosing maximum
        //    int Occurances = 0;
        //    int CompareID = 0;
        //    int HighestID = 0;


        //    for (int i = 0; i < ReaderIDs.Count; i++)
        //    {
        //        /*
        //        if (ReaderIDs.Contains(ReaderIDs[i])) //Counting how many times ID repeats
        //        {
        //            Occurances++;

        //        }

        //        for (int j = 0; j < ReaderIDs.Count; j++)
        //        {
        //            if (ReaderIDs[i] == ReaderIDs[j])
        //            {
        //                Occurances++;
        //            }
        //        }






        //        int b = ReaderIDs[i].Count();

        //        if (Occurances > CompareID)
        //        {
        //            HighestID = ReaderIDs[i];
        //        }
        //        Occurances = 0;
        //    }

        //    Console.WriteLine("Reader ID: " + HighestID);

        //    Console.WriteLine("- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -\n");
        //    */

        //}

    }
}
