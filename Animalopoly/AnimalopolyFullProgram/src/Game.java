import java.util.ArrayList;
import java.util.Scanner;

public class Game{
    private Board board = new Board();
    private ArrayList<Boolean> playerBankrupt = new ArrayList<Boolean>();
    private int playerCount = 0;
    private Dice dice1 = new Dice();
    private Dice dice2 = new Dice();
    private Deck deck = new Deck();
    private int bankruptcyCheck;

    Game(){
    }

    public void fullGame(){
        boolean looping = true;
        while(looping){
            int turnCount = 1;
            boolean playingGame = true;
            while(playingGame){
                playerEntry();
                bankruptcyCheck = fullBankruptCheck();
                while(bankruptcyCheck==-1){
                    for(int i=0;i<playerCount;i++){
                        if(bankruptcyCheck==-1){ //this degree of nested statements is probably overcomplicated
                            playerTurn(i);
                            bankruptcyCheck = fullBankruptCheck();
                        }
                    }
                    turnCount++;
                }
                playingGame = false;
            }
            if(bankruptcyCheck>=0){
                System.out.printf("%s won!\n",board.getPlayer(bankruptcyCheck).getName());
            }
            else{ //doesn't need to be an else if because it should only happen if the game is over
                System.out.printf("Nobody won\n");
            }
            Scanner playerContScanner = new Scanner(System.in);
            System.out.printf("Play another game? (y/n) ");
            char playerContResponse = playerContScanner.nextLine().toLowerCase().charAt(0);
            if(playerContResponse == 'y'){
                //System.out.printf("test statement\n"); //testing
                board = new Board(); //need to reset everything in it
                playerBankrupt.clear(); //it's first populated during player entry, so just clearing it should work
                playerCount = 0;
                playingGame = true;
            }
            else{
                looping = false;
            }
        }
    }

    public void playerEntry(){
        boolean addingPlayers = true;
        String playerName = "";
        char playerSymbol = ' ';
        ArrayList<Character> playerSymbols = new ArrayList<Character>();
        playerSymbols.add('*');
        playerSymbols.add('@');
        playerSymbols.add('#');
        playerSymbols.add('!');
        playerSymbols.add('?');
        playerSymbols.add('|'); //possibly inefficient, i feel like the last three might not be clear enough
        while(playerCount<6 && addingPlayers == true){
            //scanner stuff that i can't be bothered to do yet
            Scanner playerNameScanner = new Scanner(System.in);
            System.out.printf("Enter player %d's name: ",playerCount+1);
            playerName = playerNameScanner.nextLine();
            Scanner playerSymbolScanner = new Scanner(System.in); //possibly redundant with other scanners
            boolean selectingSymbol = true;
            while(selectingSymbol){
                System.out.printf("Select one of the following symbols:\n");
                for(int i=0;i<playerSymbols.size();i++){
                    System.out.printf("%c\n",playerSymbols.get(i));
                }
                playerSymbol = playerSymbolScanner.nextLine().charAt(0);
                if(playerSymbols.contains(playerSymbol)){
                    System.out.printf("Selected %c\n",playerSymbol);
                    playerSymbols.remove(Character.valueOf(playerSymbol));
                    selectingSymbol = false;
                }
                else{
                    System.out.printf("Invalid symbol selected.\n");
                    playerSymbol = ' ';
                }
            }
            board.addPlayer(new Player(playerName, playerSymbol));
            playerBankrupt.add(false);
            playerCount+=1;
            if(playerCount>=2){ //can't have less than 2 players
                Scanner playerCountScanner = new Scanner(System.in); //possibly redundant with other scanners
                System.out.printf("Add another player? (y/n) ");
                char playerCountResponse = playerCountScanner.nextLine().toLowerCase().charAt(0);
                if(playerCountResponse != 'y'){
                    addingPlayers = false;
                }
            }
        }
        if(playerCount==6){
            System.out.println("Maximum player count reached.");
        }
        System.out.println("Finished adding players");
        //board.displayBoard(); - testing
    }

    public void playerTurn(int playerIndex){
        Player player = board.getPlayer(playerIndex);
        //player.updateMoney(-9999); //testing
        //board.getPlayer(1).updateMoney(-9999); //testing
        //player.updateMoney(-250); //testing
        int dice1Val = dice1.rollDice(); //these are here so that moveplayer doesn't break if the player is missing their turn
        int dice2Val = dice2.rollDice();
        System.out.printf("=======%s'S TURN=======\n",player.getName());
        if(player.getIsBankrupt() == 1){ //needs to be checked so that a bankrupt player can't
            System.out.println("This player is bankrupt.");
            playerBankrupt.set(playerIndex,true); //failsafe?
            //playerBankrupt.set(1,true); //testing
        }
        else{
            if(player.getMiss() != 1){ //this is just so that a player who's missing a turn can't still draw a card
                board.displayBoard();
                System.out.printf("First dice rolled a %d\n",dice1Val);
                System.out.printf("Second dice rolled a %d\n",dice2Val);
                if(dice1Val == dice2Val){
                    System.out.printf("A double was rolled and a card was drawn from the deck.\n");
                    deck.drawCard(player);
                }
            }
            board.movePlayer(playerIndex,dice1Val+dice2Val);
            //board.movePlayer(playerIndex,13); //testing
            if(player.getIsBankrupt() == 1){
                playerBankrupt.set(playerIndex,true);
            }
        }
    }

    public int fullBankruptCheck(){
        int moneyPlayer = -1;
        boolean onlyOneLeft = false;
        for(int i=0;i<playerBankrupt.size();i++){
            if(playerBankrupt.get(i) == false){
                if(moneyPlayer == -1){
                    moneyPlayer = i;
                    onlyOneLeft = true;
                }
                else if(onlyOneLeft == true){
                    onlyOneLeft = false;
                }
            }
        }
        if(onlyOneLeft == true){
            return moneyPlayer;
        }
        else if(onlyOneLeft == false && moneyPlayer == -1){
            return -2;
        }
        else{
            return -1;
        }
    }
}
