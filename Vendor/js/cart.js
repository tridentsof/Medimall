$(document).ready(function(){

    var quantitiy=1;
       $('.quantity-right-plus').click(function(e){
           // Stop acting like a button
            e.preventDefault();
            // Get the field name
            var quantity = parseInt($('#quantity').val());    
                $('#quantity').val(quantity + 1);
       
        });
    
         $('.quantity-left-minus').click(function(e){
            // Stop acting like a button
            e.preventDefault();
            // Get the field name
            var quantity = parseInt($('#quantity').val());
            // Increment
            if(quantity>1){
              $('#quantity').val(quantity - 1);
            }
            else if(quantitiy == 1)
            {
                $('#popup2').addClass("active");
                $('#cartbox').addClass("blur");
            }
        });
        $('#back2').click(function(e){
            $('#popup2').removeClass("active");
            $('#cartbox').removeClass("blur");
            var quantity = parseInt($('#quantity').val());
            $('#quantity').val(quantity);
        });
        
    });

