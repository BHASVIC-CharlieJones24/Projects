//https://www.w3schools.com/java/java_strings.asp
// I used this link to find out how to create a sting variable in java

public class Player {
    private String name;
    private int money;
    private char symbol;
    private int isBankrupt     ;
    private int position;
    private boolean missNextTurn;
    public Player(String nameInput, char symbolInput) {
        name = nameInput;
        symbol = symbolInput;
        money = 1500;
    }
    public void setPosition(int pos){
        position = pos;
    }
    public int getPosition(){
        return position;
    }
    public void updateMoney(int value){
        money = money + value;
        if (money <= 0)
        {
            isBankrupt = 1;
        }
    }
    public String getName(){
        return name;
    }
    public int getMoney(){
        return money;
    }
    public char getSymbol(){
        return symbol;
    }
    public int getIsBankrupt(){
        return isBankrupt;
    }
}
