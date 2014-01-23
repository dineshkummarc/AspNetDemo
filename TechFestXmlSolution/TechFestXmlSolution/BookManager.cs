using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace TechFestXmlSolution
{
  /// <summary>
  /// Abstract class Book Manager having all function required to parse XML
  /// </summary>
    
    public abstract  class BookManager
  {
      public abstract XmlReader openXmlFile(string FileName);
      public abstract Book getBook(string name);
      public abstract List<Book> getByTitle();
      public abstract List<Book> getBooks(long startIndex, long lastIndex);
     
  
  
  }
}
