public class Card {
    private String description;
    private int value;
    public Card(String descriptionInput, int valueInput) {
        this.description = descriptionInput;
        this.value = valueInput;
    }
    public void useCard(Player player){
        player.updateMoney(value);//Adjust players money
        System.out.println(description);//outputs description

    }
}