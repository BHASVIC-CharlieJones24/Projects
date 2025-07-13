//imports
import java.util.ArrayList;
import java.util.Scanner;

public class Board{
    //attributes
    private ArrayList<Player> players = new ArrayList<Player>();
    private ArrayList<Animal> animals = new ArrayList<Animal>();
    private int prevPosition = 0;
    private int newPosition = 0;
    private int spaceStatus;
    private boolean startPassed = false;
    private int animalStatus;
    private int paidCost;
    private int animalCost;
    private String animalName;
    private String playerResponse;
    private char playerName;
    private int animalLevel;

    //constructor
    Board(){
        animals.add(new Animal("start dummy",0,0)); //space 0 - start
        animals.add(new Animal("Lion",340,170)); //space 1
        animals.add(new Animal("Tiger",330,165)); //space 2
        animals.add(new Animal("Cheetah",320,160)); //space 3
        animals.add(new Animal("Leopard",310,155)); //space 4
        animals.add(new Animal("Dog",150,75)); //space 5
        animals.add(new Animal("Cat",120,60)); //space 6
        animals.add(new Animal("Hamster",40,20)); //space 7
        animals.add(new Animal("Fox",180,90)); //space 8
        animals.add(new Animal("Elephant",350,175)); //space 9
        animals.add(new Animal("Giraffe",340,170)); //space 10
        animals.add(new Animal("Zebra",300,150)); //space 11
        animals.add(new Animal("Hyena",280,140)); //space 12
        animals.add(new Animal("miss dummy",0,0)); //space 13 - miss
        animals.add(new Animal("Dingo",230,115)); //space 14
        animals.add(new Animal("Monkey",280,140)); //space 15
        animals.add(new Animal("Otter",200,100)); //space 16
        animals.add(new Animal("Deer",300,150)); //space 17
        animals.add(new Animal("Owl",100,50)); //space 18
        animals.add(new Animal("Bald Eagle",320,160)); //space 19
        animals.add(new Animal("Penguin",240,120)); //space 20
        animals.add(new Animal("Crocodile",330,165)); //space 21
        animals.add(new Animal("Turtle",150,75)); //space 22
        animals.add(new Animal("Narwhale",340,170)); //space 23
        animals.add(new Animal("Blue Whale",350,175)); //space 24
        animals.add(new Animal("Orca",340,170)); //space 25
        // this is definitely inefficient.
    }

    //used to display the board. displays the players' positions seperately because i couldn't figure out a neat way to display players on the board
    public void displayBoard(){
        System.out.printf("\n\n=================================================================\n| START |   1   |   2   |   3   |   4   |   5   |   6   |   7   |\n=================================================================\n|   25  |                                               |   8   |\n=========                                               =========\n|   24  |                                               |   9   |\n=========                                               =========\n|   23  |                                               |  10   |\n=========                                               ========= \n|   22  |                                               |  11   |\n=========                                               =========\n|   21  |                                               |  12   |\n=================================================================\n|   20  |  19   |  18   |  17   |  16   |  15   |  14   | MISS  |\n=================================================================\n");
        listPlayers();
        System.out.println(); //readability
    }

    //used to update the position of a player. whether the position should be stored here or in player might be a bit subject to change
    public void movePlayer(int playerNumber, int spaceCount){
        Player player = players.get(playerNumber);
        prevPosition = player.getPosition();
        if(player.getMiss() == 1){ //missing a turn - stuff for it might not be what's actually used in player
            newPosition = prevPosition;
            player.setMiss(2);
            System.out.printf("%c missed this turn.\n",player.getSymbol());
        }
        else {
            newPosition = prevPosition + spaceCount;
            if (newPosition > 25) { //prevents positions greater than 25 (which would be invalid)
                newPosition -= 26;
                startPassed = true; //checking if both newPosition and prevPosition are greater than 0 would also be true if e.g. moved 1 space from 7 to 8, so something like this is useful
            }
        }
        player.setPosition(newPosition); //might not be the method used in player

        //money stuff from start is included here because otherwise all of the position checking would have to be done in two places
        if(newPosition==0){ //increasing money by 1000 when landing on start
            player.updateMoney(1000);
        }
        else if(newPosition>0 && startPassed == true){ //increasing money by 500 when passing start. startPassed is also set to true if start is landed on, so still need to check newPosition
            player.updateMoney(500);
            System.out.printf("%c passed Start and collected £500.\n",player.getSymbol());
        }
        if(newPosition==13 && player.getMiss()==0){ //missing a turn - stuff for it might not be what's actually used in player
            player.setMiss(1);
            System.out.printf("%c landed on Miss and will miss their next turn.\n", player.getSymbol());
        }
        if(player.getMiss()!=2){
            System.out.printf("%c moved %d space(s) to space %d\n",player.getSymbol(),spaceCount,newPosition);
        }
        if(player.getMiss() == 2){
            player.setMiss(0);
        }
        displayInstructions(playerNumber,newPosition);
    }

    //displays the instructions associated with a space. playerChar is used to address instructions to a player, and may become unnessecary if phrasing is altered from pseudocode
    public void displayInstructions(int playerNumber, int space){
        spaceStatus = spaceCheck(space);
        Player player = players.get(playerNumber);
        if(spaceStatus == -3) { //start
            System.out.printf("%c landed on Start and colleted £1000.\n",player.getSymbol());
        }
        else if(spaceStatus == -2){ //miss
            //System.out.printf("%c landed on Miss and will miss their next turn.\n", player.getSymbol());
        }
        else if(spaceStatus == -1){ //available animal
            Animal animal = animals.get(space);
            //animalCost = animal.getCost();
            //animalName = animal.getName();
            System.out.printf("----\n%s:\nLevel: 0\nL0 stop cost: £%d\nCost: £%d\nOwner: None\n----\n",animal.getName(),animal.getStopCost(),animal.getCost());
            animalSpend(0, playerNumber, space);
        }
        else if(playerNumber==spaceStatus){ //animal owned by landing player
            Animal animal = animals.get(space);
            int stopCost = animal.getStopCost();
            System.out.printf("----\n%s:\nLevel: %d\n",animal.getName(),animal.getLevel());
            System.out.printf("L0 stop cost: £%d\nL1 stop cost: £%d\nL2 stop cost: £%d\nL3 stop cost: £%d\n",stopCost,stopCost*2,stopCost*3,stopCost*4);
            System.out.printf("Cost: £%d\nOwner: %c\n----\n",animal.getCost(),player.getSymbol());
            if(animal.getLevel()>=3){
                System.out.printf("%s is already at its maximum level, and cannot be upgraded further.",animal.getName());
            }
            else{
                animalSpend(1, playerNumber, space);
            }
        }
        else{ //animal owned by a player other than the landing player
            Animal animal = animals.get(space);
            Player animalOwner = players.get(animal.getOwner());
            animalLevel = animal.getLevel();
            paidCost = animal.getStopCost()*(animalLevel+1); //this is kind of temporary
            System.out.printf("----\n%s:\nLevel: %d\nStop cost: £%d\nOwner: %c\n----\n",animal.getName(),animal.getLevel(),paidCost,animalOwner.getSymbol());
            player.updateMoney(0-paidCost);
            animalOwner.updateMoney(paidCost);
            System.out.printf("%c has paid £%d to %c\n",player.getSymbol(),paidCost,animalOwner.getSymbol());
        }
    }

    public int spaceCheck(int space){ //subject to change depending on how animal ownership will work
        if(space == 0){ //start
            return -3;
        }
        else if(space == 13){ //miss
            return -2;
        }
        else{
            animalStatus = animals.get(space).getOwner();
            return animalStatus; //currently working on the assumption that -1 will mean no owner and if it is owned it'll be the index of the owner in players
        }
    }

    private void animalSpend(int mode, int playerNumber, int space){ //used for purchasing and upgrading
        Animal animal = animals.get(space);
        Player player = players.get(playerNumber);
        animalCost = animal.getCost();
        animalName = animal.getName();
        playerName = player.getSymbol();
        //if else instead of switch because switch would get more indented and that'd look annoying
        if(mode==0){ //purchasing
            Scanner purchaseScanner = new Scanner(System.in);
            if(animalCost<=player.getMoney()){
                System.out.printf("Purchase %s for £%d? (y/n): ",animalName,animalCost);
                playerResponse = purchaseScanner.nextLine();
                playerResponse = playerResponse.toLowerCase();
                if(playerResponse.charAt(0) == 'y'){
                    animal.setOwner(playerNumber);
                    System.out.printf("%c has purchased %s for £%d\n",playerName,animalName,animalCost);
                    player.updateMoney(0-animalCost);
                }
                else{
                    System.out.printf("%c did not purchase %s\n",playerName,animalName);
                }
            }
            else{
                System.out.printf("%c lacks the funds to purchase %s\n",playerName,animalName);
            }
        }
        else if(mode==1){ //upgrading
            Scanner upgradeScanner = new Scanner(System.in);
            if(animalCost<=player.getMoney()){
                System.out.printf("Upgrade %s for £%d? (y/n): ",animalName,animalCost);
                playerResponse = upgradeScanner.nextLine();
                playerResponse = playerResponse.toLowerCase();
                if(playerResponse.charAt(0) == 'y'){
                    animal.upgrade();
                    System.out.printf("%c has upgraded %s to %d for £%d\n",playerName,animalName,animal.getLevel(),animalCost);
                    player.updateMoney(0-animalCost);
                }
                else{
                    System.out.printf("%c did not upgrade %s\n",playerName,animalName);
                }
            }
            else{
                System.out.printf("%c lacks the funds to upgrade %s\n",playerName,animalName);
            }
        }
        else{
            System.out.println("Error: unknown mode in animalSpend");
        }
    }

    public void addPlayer(Player player){
        this.players.add(player);
    }

    //method to list the characters used to represent each player (all objects in players)
    public void listPlayers(){
        System.out.printf("PLAYERS\n");
        for(int i=0;i<players.size();i++){
            Player player = players.get(i);
            if(player.getPosition() == 0){
                System.out.printf("%c (Player %d): Start with £%d\n", player.getSymbol(), i+1, player.getMoney());
            }
            else if(player.getPosition() == 13){
                System.out.printf("%c (Player %d): Miss with £%d\n", player.getSymbol(), i+1, player.getMoney());
            }
            else {
                System.out.printf("%c (Player %d): Space %d with £%d\n", player.getSymbol(), i+1, player.getPosition(), player.getMoney());
            }
        }
    }

    public Player getPlayer(int playerNumber){
        Player player = players.get(playerNumber);
        return player;
    }
}
