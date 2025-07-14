//TIP To <b>Run</b> code, press <shortcut actionId="Run"/> or
// click the <icon src="AllIcons.Actions.Execute"/> icon in the gutter.
public class Main {
    public static void main(String[] args) {
        Player player = new Player("Name",'@');
        System.out.println(player.getMoney());
        Deck deck = new Deck();
        deck.drawCard(player);
        System.out.println(player.getMoney());
    }
}
