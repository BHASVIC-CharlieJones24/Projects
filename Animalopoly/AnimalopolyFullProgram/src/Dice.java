import java.util.Random;

public class Dice {

    private boolean diceRolled;

    public Dice() {

        this.diceRolled = false;
    }
    public int rollDice() {
        int dice1 = new Random().nextInt(6) + 1;
        return dice1;
    }
}
