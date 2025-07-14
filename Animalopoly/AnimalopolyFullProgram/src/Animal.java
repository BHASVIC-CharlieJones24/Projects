public class Animal {
    private int owner = -1;
    private int level = 0;
    private String name = "";
    private int stopCost;
    private int cost;


    public Animal(String name, int cost, int stopCost) {
        this.name = name;
        this.cost = cost;
        this.stopCost = stopCost;
    }

    public int getOwner() {
        return owner;
    }

    public void setOwner(int owner) {
        this.owner = owner;
    }

    public int getLevel() {
        return level;
    }

    public void upgrade() {
        level += 1;
    }

    public String getName() {
        return name;
    }

    public int getCost() {
        return cost;
    }

    public int getStopCost() {
        return stopCost;
    }

}
