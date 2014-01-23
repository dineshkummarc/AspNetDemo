using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Xml;
using System.Data;
using System.Xml.XPath;
using System.Collections;


namespace TechFestXmlSolution
{
    /// <summary>
    /// class ProccesRider 
    /// </summary>
    class ProcessRider
    {
        private static string _foundByIndex = System.Configuration.ConfigurationSettings.AppSettings["FoundByIndex"];
       

        #region Main
        /// <summary>
        /// main method which calls run method resposible for process
        /// </summary>
        /// <param name="args"></param>
       
        static void Main(string[] args)
        {
            run();
        }
        #endregion

       
        #region Run Method
        /// <summary>
        /// Run method creates the object GutenbergBookManager which have all function required 
        /// for book searching  
        /// </summary>
        public static void run()
        {
            // create the object of class GutenbergBookManager to get its functionality
            GutenbergBookManager objectGBM = new GutenbergBookManager();
            // use for continuation 
            bool _result = true;
            while (_result)
            {
                int _choice=0;
                Console.WriteLine(" Enter your choice to Search the book ");
               Console.WriteLine(" Press 1 - To search by id \n " + "Press 2 - To search by index \n" + " Press 3 - To search by Title/Subject/Auther/Publisher \n" + " Press 4 - To Exit \n");
               try
               {
                 _choice   = Convert.ToInt32(Console.ReadLine());
               }
               catch (Exception ex)
               { 
                
               }

                switch(_choice)
                {                    // seacrhing book by id 
                    case 1:  
                           
                                Console.WriteLine("Please enter the Id of book you want to search ");
                                string _idOfBook = Console.ReadLine();
                                // call get book function which takes id of book as argument
                                // and returns the book object if fails to find then return null
                                Book book1 = objectGBM.getBook(_idOfBook);
                                if (book1 == null)
                                {
                                    Console.WriteLine(" Sorry no book found with this id ");
                                }
                                Console.ReadLine();
                                // to make console clear
                                Console.Clear();
                    break;
                                // get number of books depending on indexes
                    case 2:
                               long _startIndex =0;
                                long _lastIndex=0;
                                // take lower and upper indexes from user
                                Console.WriteLine("Please give the start and last index to get books ");
                                try
                                {
                                    _startIndex= Convert.ToInt32(Console.ReadLine());
                                    _lastIndex = Convert.ToInt32(Console.ReadLine());
                                }
                                catch (Exception ex)
                                { }
                                // create the list of type book and capacity equals to difference between lower 
                                // upper index

                                List<Book> books = null;

                                // after validating it call the function getbooks function which returns list of type book
                                if (_startIndex < _lastIndex)
                                {
                                    books = new List<Book>(Convert.ToInt32(_lastIndex - _startIndex));
                                    books = objectGBM.getBooks(_startIndex, _lastIndex);
                                }
                                else
                                {
                                    Console.WriteLine(" Expecting proper input start index must me less than last index");
                                }
                                // As it is console application out put can't be shown on properly sp write a text file
                                // by calling write text file which contains list of type book and name of text file
                                if(books!=null)
                                if (books.Count > 0)
                                {
                                   writeTextFile(books, _foundByIndex,false);
                                }
                                Console.ReadLine();
                                Console.Clear();
                    break;
                                // search books by giving title/subject/Publisher
                    case 3:                     
                                // calls the function getByTitle which returns list of books 
                                // if fails to find then returns null
                                List<Book> booksList = objectGBM.getByTitle();

                                // check if booklist is null or not 
                                if (booksList == null)
                                {
                                    Console.WriteLine(" No book found of this type  ");
                                }
                               
                                Console.ReadLine();
                                Console.Clear();
                     break;
                     case  4:    // Exit from the application
                               _result = false;
                                break;
                    default:
                        Console.WriteLine("Please Enter proper choice ");
                        break;
                    }

          }
        }
        #endregion


        #region writing to text file
        /// <summary>
        /// Finction takes list of type book and file name as string as argument and writes the text file 
        /// which contains detail inforamation of books found 
        /// </summary>
        /// <param name="books"></param>
        /// <param name="_fileName"></param>
        
        public static void writeTextFile(List<Book> books, string _fileName,bool _appnedMode)
        {
            TextWriter tw=null;
            try
            {
                tw = new StreamWriter(_fileName, _appnedMode);
                // loop to print each book
                for (int count = 0; count < books.Count; count++)
                {
                    tw.WriteLine();
                    tw.WriteLine("  Book Id -> " + books[count].Id);
                    tw.WriteLine("  Subject    -> " + books[count].Subject);
                    tw.WriteLine("  Book's  Friendly  Title -> " + books[count].FriendlyTitle);
                    tw.WriteLine("  Created By   -> " + books[count].Created);
                    tw.WriteLine("  Title      -> " + books[count].Title);
                    tw.WriteLine("  Language   -> " + books[count].Language);
                    tw.WriteLine("  Publisher  -> " + books[count].Publisher);
                    tw.WriteLine("  Rights     -> " + books[count].Rights);
                    tw.WriteLine("  Other Information -> " + books[count].OtherInfo);
                }
                
            }
            catch (Exception ex)
            {
                Console.WriteLine(" Error occured " + ex.Message);
            }
            finally 
            {   // close the textWriter;
                tw.Close();
            }
     
        }
        #endregion


        #region writing to text file
        /// <summary>
        /// Finction takes list of type book and file name as string as argument and writes the text file 
        /// which contains detail inforamation of books found 
        /// </summary>
        /// <param name="books"></param>
        /// <param name="_fileName"></param>

        public static void writeSingleBook(Book books, string _fileName, bool _appnedMode)
        {
            TextWriter tw = null;
            try
            {
                tw = new StreamWriter(_fileName, _appnedMode);
                // loop to print each book
                
                
                    tw.WriteLine();
                    tw.WriteLine("  Book Id -> " + books.Id);
                    tw.WriteLine("  Subject    -> " + books.Subject);
                    tw.WriteLine("  Book's  Friendly  Title -> " + books.FriendlyTitle);
                    tw.WriteLine("  Created By   -> " + books.Created);
                    tw.WriteLine("  Title      -> " + books.Title);
                    tw.WriteLine("  Language   -> " + books.Language);
                    tw.WriteLine("  Publisher  -> " + books.Publisher);
                    tw.WriteLine("  Rights     -> " + books.Rights);
                    tw.WriteLine("  Other Information -> " + books.OtherInfo);
                

            }
            catch (Exception ex)
            {
                Console.WriteLine(" Error occured " + ex.Message);
            }
            finally
            {   // close the textWriter;
                tw.Close();
            }

        }
        #endregion




        
    }
}


