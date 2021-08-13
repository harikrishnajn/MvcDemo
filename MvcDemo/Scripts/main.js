

const actualBtn = document.getElementById('actual-btn');

const fileChosen = document.getElementById('file-chosen');

actualBtn.addEventListener('change', function(){
  fileChosen.textContent = this.files[0].name
})


$('#downstreamworkitem').change(function () {
  if (!this.checked) 
  //  ^
     $('.drownstream ').hide();
  else 
      $('.drownstream').show();
});

$('#updates').change(function () {
  if (!this.checked) 
  //  ^
     $('.updateSection ').hide();
  else 
      $('.updateSection').show();
});

$('#taskhistories').change(function () {
  if (!this.checked) 
  //  ^
     $('.taskhistory ').hide();
  else 
      $('.taskhistory').show();
});



$(function () {

   $(".chatIcon").click(function(e) {
     $("#chat-content").slideToggle();
     event.stopPropagation();
 });
 $(".close").click(function(e) {
   $("#chat-content").hide();
 });

  

   $(window).scroll(function(){ 
       if ($(this).scrollTop() > 100) { 
           $('#scroll').fadeIn(); 
       } else { 
           $('#scroll').fadeOut(); 
       } 
   }); 
   $('#scroll').click(function(){ 
       $("html, body").animate({ scrollTop: 0 }, 600); 
       return false; 
   }); 

});