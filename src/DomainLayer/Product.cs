using System;
using System.Collections.Generic;


public class Product
{
    private const int MaxReviewLength = 250;
    private string name;
    private string category;
    private double price;
    private double rate;
    private int sumOfRates;
    private int numberOfRates;
    private Dictionary<User, string> reviews;


     public Product(string name, string category, double price)
    {
        this.name=name;
        this.category= category;
        this.price= price;
        this.reviews =new Dictionary<User, string>();
        this.rate=0;
    }


    public void addReview(User user, string review)
    {
        if(canRateProduct(user) && review.Length <= MaxReviewLength)
            reviews.Add(user,review);
    }

    public void rateProduct(User user, int rate){
        if(canRateProduct(user) && rate>0 && rate <= 5){
            sumOfRates+=rate;
            numberOfRates++;
            rate=sumOfRates/numberOfRates;
        }
    }

    private bool canRateProduct(User user){
        return user.isLogged() && user.hasPurchasedProduct(this);
    }
    // Method that overrides the base class (System.Object) implementation.
    public override string ToString()
    {
        return "Name: "+ name+ "\nCategory: "+category+ "\nRating: "+ rate+ "\nPrice"+price;
    }
}

