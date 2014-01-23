

function Grid2(objRoot) {
    //Init
    //root element in DOM.module container.used in jquery selector to gain unique objects
    this._root=objRoot;
    this.news = new Array();
    this.baseHeight = 0;
    this.fillGrid();    
}

Grid2.prototype = {
    //call asp.net page method asynchronous (send and recives data in JSON format)
    PageMethod: function(fn, paramArray, successFn, errorFn) {
        var pagePath = window.location.pathname;
        var that = this;
        //Call the page method  
        $.ajax({
            type: "POST",
            url: pagePath + "?Callback=" + fn,
            contentType: "application/json; charset=utf-8",
            data: paramArray,
            dataType: "json",
            success: function(res) { successFn(res, that) },
            error: errorFn
        });
    },

    //override jquery selector to ensure selected DOM objects be unique in multiple instanse of module
    getElement: function(domElement) {
        return $(this._root).find(domElement);
    },

BindInboxRows: function() {
    //public var
    var myInbox;
    myInbox=this.Inbox;
    
    //create Grid rows and columns 
    for (var index = 0; index < this.Inbox.length; index++) { 
         this.getElement('.facebookTable').append($('<tr><td width=200></td><td width=480></td><td class="closeTD" width=20><div class="closeDiv">X</div></td></tr>'));
         this.getElement('.facebookTable tr:eq(' + index + ') td:first').append($('<img width=50 height=50 align=middle />').attr('src',this.Inbox[index].imgSrc));
         this.getElement('.facebookTable tr:eq(' + index + ') td:eq(0)').append($('<span></span>').text(' '));
         this.getElement('.facebookTable tr:eq(' + index + ') td:eq(0)').append($('<a id=titleLink target=_blank></a>').text(this.Inbox[index].Title));
         this.getElement('.facebookTable tr:eq(' + index + ') #titleLink').attr('href', this.Inbox[index].LinkSrc);
         this.getElement('.facebookTable tr:eq(' + index + ') td:eq(1)').append($('<div class="desc"></div>').text(this.Inbox[index].Summary.substring(0,70) + ' ...' ));
    }
       
    // add click event to desc column
    this.getElement('.desc').click(function(event){
        $(this).animate({height: '100px'}, 500); 
        var RowIndex =$(this).parent().parent().attr('sectionRowIndex'); 
        $(this).text(myInbox[RowIndex].Summary);  
    }); 
        
    //add click event to close button
    $('.closeDiv').click(function(){
       var RowIndex = $(this).parent().parent().attr('sectionRowIndex'); 
       myInbox.splice(RowIndex,1);
       $(this).fadeOut('slow', function() {$(this).parent().parent('tr').remove();});    
       
    });
        
    //add RollOver event to close button
    this.getElement('.closeDiv').hover(function(event){
        $(this).addClass('closeDivhover');
    }, function() {
        $(this).removeClass('closeDivhover');
    });
    },
   
    BindInbox: function() {
      this.BindInboxRows();
      var that = this;
    },

    SuccessFetchInbox: function(response, that) {
       that.Inbox = response;
       if (response.length == undefined)
          that.Inbox = new Array(response);
       that.BindInbox();
    },

    FailFetchInbox: function(err) {
       alert('Error!  ' + err.responseText);
    },

    fillGrid: function() {
      this.PageMethod('fillGrid',{},this.SuccessFetchInbox, this.FailFetchInbox);
    }
}
