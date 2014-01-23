using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Xml.XPath;
using System.Drawing;
using System.Data;
using System.IO;
using System.Diagnostics;
using System.Security;

namespace TechFestXmlSolution
{
    class GutenbergBookManager : BookManager
    {
        #region private variables 
        // get the path and file name from Application setting confog file
        private  string _fileName = System.Configuration.ConfigurationSettings.AppSettings[0];
        private static string _foundByTitle = System.Configuration.ConfigurationSettings.AppSettings["FoundByTitle"]; 
        
        #endregion

   
        XmlDocument doc;
         // Object of CounterClass required for get Cpu Usage and time calcuations
        CounterClass sampleCounter = new CounterClass();

        #region openXmlFile function 
        /// <summary>
        /// Function openXmlfile opens the xml file and retunrs the Xml reader for reading the 
        /// xml file 
        /// </summary>
        /// <param name="FileName"></param>
        /// <returns></returns>
        public override XmlReader openXmlFile(string FileName)
        {
            XmlReader _xReader=null;
            
            // Xml Reader settings to make efficent use od reader 
            XmlReaderSettings XRSettings = new XmlReaderSettings();
            XRSettings.CheckCharacters = true;
            XRSettings.ConformanceLevel = ConformanceLevel.Document;
            XRSettings.IgnoreComments = true;
            XRSettings.ProhibitDtd = false;
            XRSettings.ValidationType = ValidationType.Schema;
            XRSettings.IgnoreWhitespace = true;

            try
            {    // It repesents the readre that provides fast/,noncashed/forward only
                _xReader = XmlNodeReader.Create(_fileName, XRSettings);
            }
            catch (Exception ex)
            {
                Console.WriteLine("File cann't be open"+ex.Message);
            }
                // return the xml reader 
            return _xReader;

        }
        #endregion

        
       
            
        #region getbook by id function
        /// <summary>
        /// Function getBook takes book id as argument search the book in the xml using 
        /// xmlreader and retunrs the book of type Book 
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public override Book getBook(string name)
        {
           // XmlDocument doc;
            Book book1 = new Book();
            XmlReader _xReader = openXmlFile(_fileName);

            //initialise the timer counter start time
            sampleCounter.StartTime=DateTime.Now;

            #region try
            try
            {  // loop continue till reaches end of file
               while (!_xReader.EOF)
                {  // check Node type,node name ,match attribute which is id to search, RDF is root element
                    if((_xReader.MoveToContent() == XmlNodeType.Element && _xReader.Name == "pgterms:etext" && _xReader.GetAttribute(0) == name) || _xReader.Name == "rdf:RDF")
                    {
                       
                      // if the node name is rdf the it is root element don't skip it and continue to read            
                        if (_xReader.Name == "rdf:RDF")
                        {
                            Console.BackgroundColor = System.ConsoleColor.Red;
                            Console.WriteLine(" Before finding the book  CPU usage  ->" + sampleCounter.CpuUsage);
                            Console.BackgroundColor = System.ConsoleColor.Black;
                            _xReader.Read();
                        }
                        else
                        {   // when the node containing Id is found
                            // create the document object to load the node only
                            // here we are loading the node in document not whole file
                            // for getting the data of node faster
                            Console.BackgroundColor = System.ConsoleColor.Red;
                            Console.WriteLine(" After finding the book CPU usage  ->" + sampleCounter.CpuUsage);
                            Console.BackgroundColor = System.ConsoleColor.Black;
                            doc = new XmlDocument();
                            // from get node in memory 
                            XmlNode xnode = doc.ReadNode(_xReader);
                            // check if element contains any attribute 
                            if (xnode.Attributes.Count > 0)
                            { // call the Initialise method of class Book which intializes whole variables
                               book1.Initialise(xnode);
                            }

                            // Print the whole book description after initialising 
                            Console.BackgroundColor = System.ConsoleColor.Red;
                            Console.WriteLine(" Time Taken ->" + sampleCounter.TimeTaken);
                            Console.BackgroundColor = System.ConsoleColor.Blue;
                            Console.WriteLine("\n\t ID -> " + book1.Id + "\t \t ");
                            Console.WriteLine("\t Created -> " + book1.Created + "\t \t \t");
                            Console.WriteLine("\t FriendlyTitle -> " + book1.FriendlyTitle);
                            Console.WriteLine("\t Language -> " + book1.Language + "\t");
                            Console.WriteLine("\t Publisher -> " + book1.Publisher);
                            Console.WriteLine("\t Rights -> " + book1.Rights);
                            Console.WriteLine("\t Subject -> " + book1.Subject);
                            Console.WriteLine("\t Title -> " + book1.Title);
                            Console.BackgroundColor = System.ConsoleColor.Black;
                            // book is found and it assume that Id of book is unique 

                            break;
                         }
                    }
                    else
                    {    // skip the whole node as it is not of use
                        _xReader.Skip();
                        _xReader.MoveToContent();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message + "" + ex.Source);
            }
            finally
            {
                _xReader.Close();
            }
            #endregion
            // check book id whether it is found or not
            // if book which is found have id Not Given then book is not found return null

            if (book1.Id.Equals("Not Given"))
            {
                return null ;
            }
                // if id is present then return book
            else return book1;

        }
        #endregion

        
        #region Get Books List
        /// <summary>
        /// Function getBooks takes start and end index as argument to find that number of books
        /// and retunrns the books in list of type Book 
        /// </summary>
        /// <param name="startIndex"></param>
        /// <param name="lastIndex"></param>
        /// <returns></returns>
        public override List<Book> getBooks(long _startIndex, long _lastIndex)
        {
            // create the List object of capacity number of books needed
            List<Book> bookList = new List<Book>(Convert.ToInt32(_lastIndex - _startIndex + 1));
            // local variable to count number of book found
            int _index = 0;
            

            sampleCounter.StartTime = DateTime.Now;
            XmlReader _xReader = openXmlFile(_fileName);
            //   Console.WriteLine(DateTime.Now.TimeOfDay.ToString());
            #region try
            try
            {   // loop till end of the file 
                while (!_xReader.EOF)
                {  // check the conditions that untill end file 
                    if ((_xReader.MoveToContent() == XmlNodeType.Element && _xReader.Name == "pgterms:etext" && _index > _startIndex - 2 && _index < _lastIndex) || _xReader.Name == "rdf:RDF")
                    {
                        if (_xReader.Name == "rdf:RDF")
                        {
                            _xReader.Read();
                        }
                        else
                        {
                            // index is greater than start index and less than lastIndex
                            // get whole node in the memory so that searching become easy and productive
                            doc = new XmlDocument();
                            XmlNode xnode = doc.ReadNode(_xReader);
                            // create the instance of the book as an container 
                            Book book1 = new Book();
                            if (xnode.Attributes.Count > 0)
                            { // Intialise and add to the list 
                                book1.Initialise(xnode);
                                bookList.Add(book1);
                             }
                            // increment as book is found
                           _index++;
                         }
                    } // check whether the found books are equal to last index or that 
                    else if (_index == _lastIndex)
                    {  // last index is reached break 
                        break;
                    }
                    else  // check until start index is not reach 
                    {
                        if (_xReader.MoveToContent() == XmlNodeType.Element && _xReader.Name == "pgterms:etext")
                        {
                            _index++;
                            _xReader.Skip();
                            _xReader.MoveToContent();

                        }// skip unwanted nodes to make efficent searching
                        else
                        {
                            _xReader.Skip();
                            _xReader.MoveToContent();
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                Console.Write(ex.Message + "" + ex.Source);
            }
            finally
            {
                _xReader.Close();
            }
            #endregion

            if (bookList.Count > 0)
            {
                Console.BackgroundColor = System.ConsoleColor.Blue;
                Console.WriteLine("Total number of books are " + bookList.Count);
                Console.WriteLine(" CPU usage is " +sampleCounter.CpuUsage);
                Console.WriteLine("Time taken " + sampleCounter.TimeTaken);
                Console.BackgroundColor = System.ConsoleColor.Black;
                return bookList;
            }
            else
                return null;
        }
        #endregion


        #region Search Book List
        /// <summary>
        /// Function getByTitle retunrs the list of books in collection of type list
        /// it searches the book title/subject/publisher/.. for given string 
        /// so can be used efficiently
        /// </summary>
        /// <returns></returns>

        public override List<Book> getByTitle()
        {
            float _counter;
            
            Book book1 = new Book();
            // get input from console as string to search
            Console.WriteLine("Enter the book Subject to be search");
            string title = Console.ReadLine().ToLower();
            // split that input string with spaces 
            string[] subtitle = title.Split(' ');
            int len1 = subtitle.Length;
            // to get intialise the timer counter
            sampleCounter.StartTime=DateTime.Now;
            #region XML reader and document
            // list of books and liat of counter which will contain the cpu usage to get average cpu usage time
            List<Book> listBooks = new List<Book>(100);
            List<float> cpuCounter = new List<float>(100);
           try
            {  // open the xml file and get Xml reader
                XmlReader _xReader = openXmlFile(_fileName);
               // read untill end of the file
                while (!_xReader.EOF)
                {   // check the conditions              
                    if ((_xReader.MoveToContent() == XmlNodeType.Element && _xReader.Name == "pgterms:etext" || _xReader.Name == "rdf:RDF"))
                    {   // if current node is root element then read next 
                        if (_xReader.Name == "rdf:RDF")
                        {
                            _xReader.Read();
                        }
                        else
                        {  // use of xml and xnode to get node into memory and get output effectivly
                            doc = new XmlDocument();
                            XmlNode xnode = doc.ReadNode(_xReader);
                            // check the capacity of both book list and count list should not greater than capacity
                            if (listBooks.Count == listBooks.Capacity)
                            { // capacity is reached then increase the capacity
                                cpuCounter.Capacity += 25;
                                listBooks.Capacity += 25;
                            }// search the book by giving title ,node and returning counter as out
                            Book book = new Book();
                            book = searchBook(xnode, title, out _counter);
                            if (book != null)
                            { // check wether book contins null or not 
                                cpuCounter.Add(_counter);
                                listBooks.Add(book);
                                ProcessRider.writeSingleBook(book,_foundByTitle,true);
                             }
                        }
                    }
                    else
                    { // skip all others node to make effcient searching 
                        _xReader.Skip();
                        _xReader.MoveToContent();
                    }
                }


            }
            catch (Exception ex)
            {
                Console.WriteLine(" Some Error occured" + ex.Message);
            }
            finally
            {

            }

            #endregion

            float multiplication = 0;
            
            if (listBooks.Count > 0)
            {
                Console.BackgroundColor = System.ConsoleColor.Blue;
                Console.WriteLine("\n Total number of books found are " +listBooks.Count);
                Console.BackgroundColor = System.ConsoleColor.Black;
                for (int k = 0; k < cpuCounter.Count; k++)
                {
                    multiplication += cpuCounter[k];
                }

                
                Console.BackgroundColor = System.ConsoleColor.Blue;
                Console.WriteLine("\n Avg CPU usage is " + (multiplication / cpuCounter.Count));
                Console.WriteLine("\n Total time taken " + sampleCounter.TimeTaken);
                Console.BackgroundColor = System.ConsoleColor.Black;
                return listBooks;
            }

            return null;


        }
        #endregion
       
     
        #region search Book
        /// <summary>
        /// Function is used by this class to get book using the title
        /// now the each selected node id sent to this function along with 
        /// string to search and performance counter as out parameter
        /// function first seacrh using whole string but if not found then 
        /// will search by using single single word from search string
        /// </summary>
        /// <param name="xnode"></param>
        /// <param name="toSearch"></param>
        /// <param name="counter"></param>
        /// <returns></returns>

        public Book searchBook(XmlNode xnode, string toSearch, out float counter)
        {

            string[] _singleWord = toSearch.Split(' ');
            XmlNodeList list = xnode.ChildNodes;
            Book book = new Book();
            string innerText = xnode.InnerText.ToLower();
            // check whether the string to serach is in xnode inner text which contains whole text of current node
            if (innerText.Contains(toSearch))
            {
                book.Initialise(xnode);
                // get the cpu usage during the call
                counter = sampleCounter.CpuUsage;
               // return the searched book
                return book;

            }
            else
            { // make counter =0 as it must be initilise before returning 
                // search seprate words from the input string to search
                counter = 0;
                for (int k = 0; k < _singleWord.Length; k++)
                {
                    if (innerText.Contains(_singleWord[k]))
                    {
                        book.Initialise(xnode);
                        counter = sampleCounter.CpuUsage;
                        return book;
                    }
                    else
                    {
                        counter = 0;
                        return null;
                    }
                }
            }
            return book;
        }
        # endregion



       

    }
}
