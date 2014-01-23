using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Xml.XPath;
using System.Collections;

namespace TechFestXmlSolution
{
  /// <summary>
  /// Book is class which will treat as single entity of book as it conatins many
  /// identiies as name title,subject,id etc all this are declared and iniltialised in this class
  /// </summary>
    public class Book 
   {

              #region private Varaibles 
       private string _id = "Not Given";
       private string _title = "Not Given";
       private string _publisher = "Not Given";
       private string _subject = "Not Given";
       private string _created = "Not Given";
       private string _language = "Not Given";
       private string _rights="Not Given";
       private string _friendlyTitle = "Not Given";
       private string _otherInfo ;
       #endregion


              #region public properties
       public string Id
       {
           get { return _id; }
           set { _id = value; }
       }
       public string Title
       {
           get { return _title; }
           set { _title = value; }
       }

       public string Publisher
       {
           get { return _publisher; }
           set { _publisher = value; }
       }

       public string Subject
       {
           get { return _subject; }
           set { _subject = value; }
       }

       public string Created
       {
           get { return _created; }
           set { _created = value; }
       }

       public string Language
       {
           get { return _language; }
           set { _language = value; }
       }

       public string Rights
       {
           get { return _rights; }
           set { _rights = value; }
       }

       public string FriendlyTitle
       {
           get { return _friendlyTitle; }
           set { _friendlyTitle = value; }
       }

       public string OtherInfo
       {
           get { return _otherInfo; }
           set { _otherInfo = value; }
       }
       #endregion

        // counter to get cpu usage 
        CounterClass sampleCounter = new CounterClass();

       #region Intialise book varaibles 
       public void Initialise(XmlNode xnode)
       {
           XmlNode xnode1;
           XmlNodeList list = xnode.ChildNodes;
           if (xnode.Attributes.Count > 0)
           {
               this.Id = xnode.Attributes[0].Value;
           }
           if (xnode.HasChildNodes)
           {
               for (int c = 0; c < list.Count; c++)
               {
                   xnode1 = xnode.ChildNodes[c];
                   string property = xnode1.LocalName;
                   switch(property)
                   {
                       case "ID":
                           this.Id = xnode1.InnerText;
                           break;
                       
                       case "publisher" :
                                   this.Publisher = xnode1.InnerText;
                       break;

                       case "title" :
                           this.Title = xnode1.InnerText;
                       break;

                       case "subject":
                           this.Subject = xnode1.InnerText; 
                       break;
                       
                       case "rights" :
                           this.Rights = xnode1.InnerText;
                       break;
                           
                       case "language":
                           this.Language = xnode1.InnerText;
                       break;

                       case "friendlytitle" :
                           this.FriendlyTitle = xnode1.InnerText;
                       break;
                           
                       case "created" :
                           this.Created = xnode1.InnerText;
                       break;
                       default :
                           this.OtherInfo +=" "+ xnode1.InnerText;
                           break;
                       }

                   }   
               }
           }
       #endregion


       

             #region dispose book
       public void dispose()
       {
           this.dispose();
       
       
       }
#endregion

      }
}
