//https://www.geeksforgeeks.org/how-to-convert-a-string-to-an-int-in-java/
//Used this link to find out how to convert string to int in java on line 10

//https://www.w3schools.com/java/java_arraylist.asp
//Used this link to find out how to create an array list

//https://www.w3schools.com/java/java_howto_random_number.asp
//I used this link to find out how to create a random number

import java.util.ArrayList;
public class Deck {
    private ArrayList<Card> cards = new ArrayList<Card>();
    private String[][] deck = {
            {"You got attacked by a snake and have to pay £10 to the hospital","-10"},
            {"You just found a £20 note in the zoo","20"},
            {"You went to feed the monkeys and they stole your money. You lost £50","-50"},
            {"The fox dug up a treasure chest worth £100","100"},
            {"The vet bills have increased. You loose £40","-40"},
            {"The vet bills have decreased. You gain £40","40"},
            {"All your animals escaped and you had to pay to get people to find them. You loose £30","-30"},
            {"You ate dog food by accident and have food poisoning. You loose £10","-10"},
            {"Your bank account got hacked and they took £100","-100"},{"The bank gave you £100 for being a loyal customer","100"},
            {"You won the lottery jackpot and got £50","50"},
            {"Your house set on fire and it cost £100 to repair","-100"},
            {"Your got your monthly interest and gained £10","10"},
            {"Its your birthday you got £30","30"},
            {"You tripped on a log and broke your knee. You pay the hospital £20","-20"},
            {"You just got a raise at your job. You gained £30","30"},
            {"You went on a luxurious holiday and spent £100","-100"},
            {"You won the competition for the best animal owner in your local town and got £60","60"},
            {"You came runner up for the best animal owner in your local town and got £15","15"},
            {"You ran out of animal food and have to buy more. You spent £30","-30"},
            {"The zoo gave you a free meal and you saved £15","15"}};
    public Deck() {
        for (int i = 0;i < 20;i++){
            cards.add(new Card(deck[i][0],Integer.parseInt(deck[i][1]))); //Adds cards to deck
        }
    }
    public void drawCard(Player player){
        int randomNumber = (int)(Math.random() * 20); //Generates random number
        randomNumber-=1;
        Card card = cards.get(randomNumber);
        card.useCard(player); //Uses card
    }
}
